using Godot;
using System;

public partial class Gravidade : Node
{
    [Export] public float Gravity = 500f;
    private Vector2 velocity = Vector2.Zero;
    private bool isOnGround = false;

    private bool isDragging = false;
    
    private Rect2 screenBounds;

    public override void _Ready()
    {
        var viewport = GetViewport();
        screenBounds = new Rect2(Vector2.Zero, viewport.GetVisibleRect().Size);
    }

    public void SetIsDraggin(bool _value)
    {
        isDragging = _value;
        if (_value)
        {
            velocity.Y = 0;
            isOnGround = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isOnGround || isDragging) return;

        velocity.Y += Gravity * (float)delta;

        var parent = GetParent<Node2D>();
        parent.Position += velocity * (float)delta;
        CheckGround(parent);
    }

    private void CheckGround(Node2D parent)
    {
        var spaceState = parent.GetWorld2D().DirectSpaceState;

        //if(parent.GlobalPosition > screenbou)

        var collisionShape = parent.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        if (collisionShape == null || collisionShape.Shape == null)
        {
            GD.PrintErr("CollisionShape2D não encontrado ou sem forma.");
            return;
        }

        var shape = collisionShape.Shape;
        var transform = Transform2D.Identity.Translated(parent.GlobalPosition + new Vector2(0, velocity.Y * (float)GetPhysicsProcessDeltaTime()));

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
}