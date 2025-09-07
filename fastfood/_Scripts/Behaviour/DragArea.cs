using Godot;
using System;

public partial class DragArea : Area2D
{
    public bool isDragging { get; private set; }
    private Vector2 dragOffset;
    private Vector2 spriteSize;
    private Rect2 screenBounds;
    private Gravidade gravity;

    private int originalZIndex;

    public override void _Ready()
    {
        var viewport = GetViewport();
        screenBounds = new Rect2(Vector2.Zero, viewport.GetVisibleRect().Size);

        // Pega o tamanho da imagem (sprite) com escala global
        var sprite = GetNode<Sprite2D>("Sprite2D");
        if (sprite.Texture != null)
        {
            spriteSize = sprite.Texture.GetSize() * Scale;
        }
        else
        {
            GD.PrintErr("Sprite sem textura! Verifique se a imagem foi carregada.");
            spriteSize = Vector2.Zero;
        }

        if (HasNode("Gravity"))
            gravity = GetNode<Gravidade>("Gravity");

        originalZIndex = ZIndex;
    }

    public void ResetIndex()
    {
        ZIndex = originalZIndex;
    }

    public void FronIndex()
    {   
        ZIndex++;
    }

    public void StartDragging()
    {
        isDragging = true;
        if (gravity != null) gravity.SetIsDraggin(true);
    }

    public void StopDragging()
    {
        isDragging = false;
        if (gravity != null) gravity.SetIsDraggin(false);
    }

    public void DragTo(Vector2 globalPosition)
    {
        float halfWidth = spriteSize.X * .5f;
        float halfHeight = spriteSize.Y * .5f;

        // Mantém a altura do mouse, trava só horizontalmente
        globalPosition.X = Mathf.Clamp(globalPosition.X, screenBounds.Position.X + halfWidth, screenBounds.End.X - halfWidth);
        globalPosition.Y = Mathf.Clamp(globalPosition.Y, screenBounds.Position.Y + halfHeight, screenBounds.End.Y - halfHeight);

        GlobalPosition = globalPosition;        
    }
}