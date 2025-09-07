using Godot;
using System;

public partial class DragCharacter : CharacterBody2D
{
    private bool isDragging = false;
    private Vector2 dragOffset;

    public override void _Ready()
    {
        SetProcessInput(true);
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (mouseButton.Pressed)
            {
                isDragging = true;
                dragOffset = GetViewport().GetMousePosition() - GlobalPosition;
            }
            else
            {
                isDragging = false;
            }
        }
    }

    public override void _Process(double delta)
    {
        if (isDragging)
        {
            GlobalPosition = GetViewport().GetMousePosition() - dragOffset;
        }
    }
}