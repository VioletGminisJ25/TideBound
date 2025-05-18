using Godot;
using System;

public partial class PikyStar_Attack : State
{
    [Export] public RayCast2D raycast; // Keep if used elsewhere, otherwise remove
    [Export] public Enemy enemy; // Ensure this is assigned in the inspector

    [Export] public float velocity;
    public AnimationNodeStateMachinePlayback animationMachine;


    public override void Enter()
    {
        animationMachine = (AnimationNodeStateMachinePlayback)enemy.animationTree.Get("parameters/playback");

        animationMachine.Travel("attack");
        GetChild<Timer>(0).Start(); // Start the timer for the attack animation

        velocity = velocity == 0 ? 10.0f : velocity;
    }
    public override void Exit() { }

    public override void Update(float delta)
    {

    }
    public override void PhysicsUpdate(float delta)
    {

        Vector2 movementDirection = (enemy.player.GlobalPosition - enemy.GlobalPosition).Normalized();
        enemy.Velocity = new Vector2(movementDirection.X * velocity, enemy.Velocity.Y);
        enemy.MoveAndSlide(); 
        // if (raycast.IsColliding() && raycast.GetCollider() is Player player)
        // {
        //     GD.Print("ENEMY: Attack State - Raycast Collided with Player");
        //     player.CallDeferred("ApplyPushback", enemy.GlobalPosition, enemy.PushForce, enemy.PushDuration);
        // }
    }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout()
    {
        if (fsm.currentState == this)
        {
            fsm.TransitionTo("Idle");
        }
    }
}
