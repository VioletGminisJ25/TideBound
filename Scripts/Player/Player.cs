using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public AnimationTree animationTree;
	[Export] public AnimatedSprite2D animatedSprite;

	private Vector2 direction;
	private float speed = 170f;
	private float jumpVelocity = -400f;
	private float fallMultiplier = 1.5f;
	private float lowJumpMultiplier = 3f;
	private float coyoteTime = 0.1f;
	private float jumpBufferTime = 0.1f;

	private float coyoteTimeCounter = 0f;
	private float jumpBufferCounter = 0f;
	private Vector2 velocity;
	private bool isAttacking = false;


	public AnimationNodeStateMachinePlayback fsm;



	public override void _Ready()
	{
		animationTree.Active = true;
		fsm = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		
	}



	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		Movement(dt);
		MoveAndSlide();
		if (Input.IsActionPressed("attack") && !isAttacking)
		{
			isAttacking = true;
			fsm.Travel("attack1");
		}
	}

	private void Movement(float delta)
	{
		direction = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"), 0);
		velocity.X = Mathf.Lerp(velocity.X, direction.X * speed, 10f * delta);
		if (!IsOnFloor())
		{
			if (velocity.Y > 0) // Cayendo
			{
				// 🔹 Lerp para suavizar la caída
				velocity.Y = Mathf.Lerp(velocity.Y, velocity.Y + GetGravity().Y * fallMultiplier, 0.75f * delta);
				if (fsm.GetCurrentNode() != "fall")
				{
					fsm.Travel("fall");
				}
			}
			else if (!Input.IsActionPressed("jump")) // Salto cortado
			{
				velocity.Y = Mathf.Lerp(velocity.Y, velocity.Y + GetGravity().Y * lowJumpMultiplier, 1.7f * delta);
			}
			else // Salto normal
			{
				velocity.Y += GetGravity().Y * delta;
			}
		}
		else
		{
			velocity.Y = 0;
			if (direction.X != 0 && !isAttacking)
			{
				
				fsm.Travel("run");
			}
			else
			{
				fsm.Travel("idle");
				isAttacking = false;
			}
		}
		if (IsOnFloor())
			coyoteTimeCounter = coyoteTime;
		else
			coyoteTimeCounter -= delta;

		if (Input.IsActionJustPressed("jump"))
			jumpBufferCounter = jumpBufferTime;
		else
			jumpBufferCounter -= delta;

		// **Salto**
		if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
		{
			// Interpolamos la velocidad del salto para que sea más progresivo
			velocity.Y = Mathf.Lerp(velocity.Y, jumpVelocity, 65f * delta);
			coyoteTimeCounter = 0;
			jumpBufferCounter = 0;
			fsm.Travel("jump");
			System.Console.WriteLine("Jumping");
		}

		if (direction.X != 0)
			animatedSprite.Scale = new Vector2(Mathf.Sign(direction.X), 1);

		Velocity = velocity;
	}

	public void _on_sword_hit_area_entered(Area2D area)
	{
		if (area is DamageComponent)
		{
			DamageComponent hurt = (DamageComponent)area;
			hurt.dameage();
		}
	}
}











