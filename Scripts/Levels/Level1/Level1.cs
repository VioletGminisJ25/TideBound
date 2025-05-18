using Godot;
using System;

public partial class Level1 : Node2D
{
    public override void _Ready()
    {
        GetTree().Root.GetNode<MouseWithJoystick>("MouseWithJoystick").inmenu = false;
        GetTree().Root.GetNode<MouseWithJoystick>("MouseWithJoystick").Initialize();
    }
}
