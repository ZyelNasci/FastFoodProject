using Godot;
using System;

public partial class Drag : Node
{
    private bool isDragging = false;
    private Vector2 dragOffset;
    private Rect2 screenBounds;

    public override void _Ready()
    {
        var viewport = GetViewport();
        screenBounds = new Rect2(Vector2.Zero, viewport.GetVisibleRect().Size);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (mouseButton.Pressed)
            {
                isDragging = true;
                dragOffset = GetViewport().GetMousePosition() - GetParent<Node2D>().GlobalPosition;
            }
            else
            {
                isDragging = false;
            }
        }

        if (@event is InputEventMouseMotion && isDragging)
        {
            Vector2 target = GetViewport().GetMousePosition() - dragOffset;
            GetParent<Node2D>().GlobalPosition = ClampToScreen(target);
        }
    }

    private Vector2 ClampToScreen(Vector2 pos)
    {
        var sprite = GetParent().GetNode<Sprite2D>("Sprite2D");
        Vector2 size = sprite.Texture.GetSize() * sprite.GlobalScale;
        float hw = size.X / 2f;
        float hh = size.Y / 2f;

        pos.X = Mathf.Clamp(pos.X, screenBounds.Position.X + hw, screenBounds.End.X - hw);
        pos.Y = Mathf.Clamp(pos.Y, screenBounds.Position.Y + hh, screenBounds.End.Y - hh);

        return pos;
    }
}