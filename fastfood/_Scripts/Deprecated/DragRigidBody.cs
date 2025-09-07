using Godot;
using System;

public partial class DragRigidBody : RigidBody2D
{
    private bool isDragging = false;
    private Vector2 dragOffset;
    public override void _Ready()
    {
        InputPickable = true; // Permite que o nó receba eventos de input
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed)
                {
                    isDragging = true;
                    dragOffset = GetViewport().GetMousePosition() - GlobalPosition;
                    Freeze = true; // Congela a física enquanto arrasta
                }
                else
                {
                    isDragging = false;
                    Freeze = false; // Libera a física ao soltar
                }
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