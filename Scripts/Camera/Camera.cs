using Godot;
using Godot.NativeInterop;
using System;

public partial class Camera : Camera2D
{
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		CharacterBody2D player = GetTree().GetNodesInGroup("Player")[0] as CharacterBody2D;
		Vector2 position =  GlobalPosition;
		position = player.GlobalPosition;
		position.Y = player.GlobalPosition.Y-20;
		GlobalPosition = position;
	}
}
