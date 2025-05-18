using Godot;
using System;

public partial class BigRed : Enemy
{
    public Vector2 velocity;
    private HealthComponent _healthComponent;

    private Enemy _enemy;

    private bool died = false;

    public BigRed() : base()
    {
    }

    public override void ApplyPushback(Vector2 sourcePosition, float force, float duration)
    {
        base.ApplyPushback(sourcePosition, force, duration);
        GetNode<StateMachine>("FSM").TransitionTo("Hit");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddToGroup("Enemy");
        _healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
        if (_healthComponent == null)
        {
            GD.PushError("HealthComponent not found in BigRed node.");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (died)
        {
            animatedSprite.GetNode<AnimationPlayer>("AnimationPlayer").Play("dead");
            return;
        }
        base._PhysicsProcess(delta);
    }


    public void _on_health_component_object_destroyed()
    {
        // GetNode("FSM").QueueFree();
        // GetNode("HurtBox").QueueFree();
        died = true;
    }




}

