using Godot;
using System;

public partial class Idle_BigRed : State
{
    [Export] RayCast2D raycast;
    [Export] Enemy enemy;

    public override void Enter() {
        raycast.Enabled = true;
        enemy.animatedSprite.Play("idle");
        Timer timer = GetNodeOrNull<Timer>("Timer");
        if (timer != null)
        {
            timer.Start();
        }
        else
        {
            fsm.TransitionTo("Idle"); // Or an error state
            return;
        }

    }
    public override void Exit() {
    }

    public override void Update(float delta) {

    }
    public override void PhysicsUpdate(float delta) {
        if(raycast.IsColliding() && raycast.GetCollider() is Player player)
        {
            fsm.TransitionTo("Attack");
        }
        
    }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout(){
        if(fsm.currentState == this)
        {
            fsm.TransitionTo("Run");
        }
        
    }
}
