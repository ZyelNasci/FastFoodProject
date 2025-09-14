using Godot;
using System;

public partial class FoodIndividual : DragArea
{

    [Export]
    public FoodType type { get; private set; }

}
