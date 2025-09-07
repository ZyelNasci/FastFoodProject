using Godot; 
using System;

public partial class DragManager : Node2D 
{
     private DragArea objectSelected = null;
    private DragArea lastObjectSelected = null;
    private Vector2 dragOffset;
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                Vector2 mousePos = GetViewport().GetMousePosition();
                var spaceState = GetWorld2D().DirectSpaceState;

                var query = new PhysicsPointQueryParameters2D
                {
                    Position = mousePos,
                    CollisionMask = 1,
                    CollideWithAreas = true,
                    CollideWithBodies = true
                };

                var results = spaceState.IntersectPoint(query, 32);
                DragArea newObject = null;
                int maiorZ = int.MinValue;

                foreach (var result in results)
                {
                    var collider = result["collider"];

                    DragArea area = collider.As<DragArea>();
                    if (area != null && area.ZIndex > maiorZ)
                    {
                        maiorZ = area.ZIndex;
                        newObject = area;
                    }
                }

                if (newObject != null)
                {
                    lastObjectSelected?.ResetIndex();
                    newObject?.FronIndex();
                    objectSelected = newObject;
                    dragOffset = mousePos - objectSelected.GlobalPosition;
                    objectSelected.StartDragging();
                }
            }
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (objectSelected != null)
                {
                    objectSelected.StopDragging();
                    lastObjectSelected = objectSelected;
                    objectSelected = null;
                }
            }
        }

        if (@event is InputEventMouseMotion mouseMotion && objectSelected != null)
        {
            //dragOffset = GetViewport().GetMousePosition() - objetoSelecionado.GlobalPosition;                
            objectSelected.DragTo(mouseMotion.Position - dragOffset);
        }
    }
}