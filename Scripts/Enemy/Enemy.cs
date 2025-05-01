using Godot;
using System;

public partial class Enemy : CharacterBody2D,DamageableObject
{
	[Export] public float PushForce = 100.0f; // Fuerza base de retroceso
	[Export] public float PushDuration = 0.1f; // Duración base del retroceso

	private bool _isBeingPushed = false;
	private Vector2 _pushDirection = Vector2.Zero;
	private float _pushTimer = 0.0f;
	private Vector2 _velocity = Vector2.Zero;
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

	public void ApplyPushback(Vector2 sourcePosition, float force, float duration)
	{
		_pushDirection = (GlobalPosition - sourcePosition).Normalized();
		PushForce = force; // Sobreescribe la fuerza base con la del impacto
		PushDuration = duration; // Sobreescribe la duración base con la del impacto
		_isBeingPushed = true;
		_pushTimer = 0.0f;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (_isBeingPushed)
		{
			_velocity = _pushDirection * PushForce; // Usa la fuerza base del jugador aquí
			_pushTimer += (float)delta;
			if (_pushTimer >= PushDuration) // Usa la duración base del jugador aquí
			{
				_isBeingPushed = false;
				_pushTimer = 0.0f;
				_velocity = Vector2.Zero;
				GD.Print("Push ended");
			}
			Velocity += _velocity;
		}
		if (!IsOnFloor())
		{
			Velocity += new Vector2(0, GetGravity().Y);
		}
		else
		{
			Velocity += new Vector2(0, 0);
		}
		// Aquí iría la lógica de movimiento normal del enemigo
	}

}
