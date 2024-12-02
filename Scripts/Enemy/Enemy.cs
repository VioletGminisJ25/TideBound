using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	 double health;
	[Export]
	 double damage;

	[Export]
	CollisionShape2D collisionShape;

	[Export]
	AnimatedSprite2D animatedSprite;

    public Enemy()
    {
    }
    public override void _PhysicsProcess(double delta)
    { 
        MoveAndSlide();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
