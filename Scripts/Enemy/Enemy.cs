using Godot;
using System;

public partial class Enemy : CharacterBody2D,DamageableObject
{
	private int health;

	[Export]
	public int Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
		}
	}

	[Export]
	double damage;

	[Export] public Player player;

	[Export]
	CollisionShape2D collisionShape;

	[Export]
	public AnimatedSprite2D animatedSprite;

	public Enemy()
	{
		health = health != 0 ? health : 5;
	}

}
