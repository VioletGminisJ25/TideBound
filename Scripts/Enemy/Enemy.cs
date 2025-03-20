using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	 double health;
	[Export]
	 double damage;

	[Export] public Player player;

	[Export]
	CollisionShape2D collisionShape;

	[Export]
	public AnimatedSprite2D animatedSprite;

    public Enemy()
    {

    }
   
}
