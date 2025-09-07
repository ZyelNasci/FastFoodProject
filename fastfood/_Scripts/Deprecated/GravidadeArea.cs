using Godot;
using System;

public partial class GravidadeArea : Area2D
{
    [Export] public float Gravity = 500f;

    private Vector2 velocity = Vector2.Zero;
    private bool isOnGround = false;
    public bool IsDragging = false;

    private CollisionShape2D collisionShape;
    private Vector2 spriteSize;
    private Rect2 screenBounds;

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        InputPickable = true;

        // Pega os limites da tela
        var viewport = GetViewport();
        screenBounds = new Rect2(Vector2.Zero, viewport.GetVisibleRect().Size);

        // Pega o tamanho da imagem (sprite) com escala global
        var sprite = GetNode<Sprite2D>("Sprite2D");
        if (sprite.Texture != null)
        {
            spriteSize = sprite.Texture.GetSize() * sprite.Scale;
        }
        else
        {
            GD.PrintErr("Sprite sem textura! Verifique se a imagem foi carregada.");
            spriteSize = Vector2.Zero;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsDragging && !isOnGround)
        {
            velocity.Y += Gravity * (float)delta;
            Position += velocity * (float)delta;
            ClampToScreen(); // mantém dentro da tela durante a queda
        }

        if (!IsDragging)
        {
            CheckCollisionWithGround();
        }
    }

    public void StartDragging()
    {
        IsDragging = true;
        velocity = Vector2.Zero;
    }

    public void StopDragging()
    {
        IsDragging = false;
    }

    public void DragTo(Vector2 globalPosition)
    {
        float halfWidth = spriteSize.X / 2f;

        // Mantém a altura do mouse, trava só horizontalmente
        globalPosition.X = Mathf.Clamp(globalPosition.X, screenBounds.Position.X + halfWidth, screenBounds.End.X - halfWidth);

        GlobalPosition = globalPosition;
    }

    private void CheckCollisionWithGround()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var shape = collisionShape.Shape;
        var transform = Transform2D.Identity.Translated(GlobalPosition + new Vector2(0, velocity.Y * (float)GetPhysicsProcessDeltaTime()));

        var result = spaceState.IntersectShape(new PhysicsShapeQueryParameters2D
        {
            Shape = shape,
            Transform = transform,
            CollisionMask = 1,
            CollideWithBodies = true,
            CollideWithAreas = false
        }, 1);

        isOnGround = result.Count > 0;

        if (isOnGround)
            velocity.Y = 0;
    }

    private void ClampToScreen()
    {
        Vector2 pos = GlobalPosition;

        float halfWidth = spriteSize.X / 2f;
        float halfHeight = spriteSize.Y / 2f;

        // Limites horizontais
        pos.X = Mathf.Clamp(pos.X, screenBounds.Position.X + halfWidth, screenBounds.End.X - halfWidth);

        // Limite inferior (chão)
        pos.Y = Mathf.Clamp(pos.Y, screenBounds.Position.Y + halfHeight, screenBounds.End.Y - halfHeight);

        GlobalPosition = pos;
    }
}