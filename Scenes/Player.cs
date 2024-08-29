using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -300.0f;
	private static AnimatedSprite2D animationPlayer;
	private static CharacterBody2D player;
	private Vector2 velocity;

	public override void _PhysicsProcess(double delta)
	{
		// Vector2 position = Position;
		animationPlayer = GetTree().GetNodesInGroup("Player")[0].GetChild(1) as AnimatedSprite2D;
		velocity = Velocity;
		Animations();

		if (!IsOnFloor())
		{
			if (velocity.Y > 0)
			{
				animationPlayer.Play("fall");
			}
			velocity += GetGravity() * (float)delta;

		}

		if (Input.IsActionJustPressed("ui_space") && IsOnFloor())
		{
			animationPlayer.Play("jump");
			velocity.Y = JumpVelocity;
		}


		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (velocity.X > 0)
			{
				animationPlayer.FlipH = false;
			}
			else
			{
				animationPlayer.FlipH = true;
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		}

		Velocity = velocity;
		MoveAndSlide();
	}
	private void Animations()
	{

		if (velocity.X != 0)
		{
			animationPlayer.Play("run");
		}
		else
		{
			animationPlayer.Play("idle");
		}
	}



}
