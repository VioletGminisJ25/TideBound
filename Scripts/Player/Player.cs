using System;
using Godot;


public partial class Player : CharacterBody2D, AttackInterface,IHook
{
	private bool isAttacking = false;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
	private Rope line;
	[Export] public Rope Line { get => line; set => line = value; }
	private StaticBody2D cursor;
	[Export] public StaticBody2D Cursor { get => cursor; set => cursor = value; }
	private bool isHooked = false;
    public bool IsHooked { get => isHooked; set => isHooked = value; }
	private bool skipGravityFrame = false;
    public bool SkipGravityFrame { get => skipGravityFrame; set => skipGravityFrame = value; }

	[Export] public float PushForce = 200.0f; // Fuerza del tirón hacia atrás
	[Export] public float PushDuration = 0.2f; // Duración del tirón en segundos

	private bool _isBeingPushed = false;
	private Vector2 _pushDirection = Vector2.Zero;
	private float _pushTimer = 0.0f;
	private Vector2 _velocity = Vector2.Zero; // Mantén un registro de la velocidad normal del jugador
	[Export] public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback fsm;
	private HealthComponent _healthComponent;
	private float _currentPushForce = 0.0f;
	private float _currentPushDuration = 0.0f;



	public override void _Ready() {
		fsm = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		_healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
		if (_healthComponent == null)
		{
			GD.PushError("HealthComponent not found in Player node.");
		}
	}

	/**
	 * Called when the player's sword trigger collides with an object.
	 * @param area The area that the sword trigger collided with.
	 * This is usually a DamageComponent that will take damage.
	 */
	public void _on_sword_hit_area_entered(Area2D area)
	{
		if (area is DamageComponent)
		{
			DamageComponent hurt = (DamageComponent)area;
			// hurt.dameage();
		}
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
				_velocity = Vector2.Zero;
			}
			Velocity = _velocity;
		}
		MoveAndSlide();
	}

	public void ApplyPushback(Vector2 sourcePosition, float force, float duration)
	{
		_pushDirection = (GlobalPosition - sourcePosition).Normalized();
		PushForce = force; // Sobreescribe la fuerza base con la del impacto
		PushDuration = duration; // Sobreescribe la duración base con la del impacto
		_isBeingPushed = true;
		_pushTimer = 0.0f;
	}

}











