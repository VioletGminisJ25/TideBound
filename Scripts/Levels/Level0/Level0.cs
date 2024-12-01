using Godot;
using System;

public partial class Level0 : Node2D
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public PathFollow2D pathFollow2D;
	private double mov;
	public override void _Ready()
	{
		mov = 0.1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		pathFollow2D.ProgressRatio += (float)(mov * delta);
	}
}
