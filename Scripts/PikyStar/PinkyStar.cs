using Godot;
using System;

public partial class PinkyStar : Enemy
{
    public Vector2 velocity;
    private HealthComponent _healthComponent;
    private bool died;
    public AnimationNodeStateMachinePlayback animationMachine;


    public PinkyStar() : base()
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
        animationMachine = (AnimationNodeStateMachinePlayback) animationTree.Get("parameters/playback");
        AddToGroup("Enemy");
        _healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
        if (_healthComponent == null)
        {
            GD.PushError("HealthComponent not found in PikyStar node.");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (died)
        {
            animationMachine.Travel("dead");  
            return;
        }
        base._PhysicsProcess(delta);
    }

    public void _on_health_component_object_destroyed()
    {
        died = true;
    }
}
