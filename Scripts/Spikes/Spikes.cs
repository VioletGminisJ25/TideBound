using Godot;
using System;

public partial class Spikes : Sprite2D
{
    [Export]
    public CollisionShape2D CollisionShape;

    public override void _Ready()
    {
        // Set the collision shape to be the same size as the sprite
        RectangleShape2D rectangleShape = new RectangleShape2D();
        rectangleShape.Size = this.RegionRect.Size;
        CollisionShape.Shape = rectangleShape;
    }
}
