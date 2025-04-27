using Godot;
using System;

public partial class Idle_BigRed : State
{
    [Export] RayCast2D raycast;
    [Export] Enemy enemy;

    public override void Enter() {
        raycast.Enabled = true;
        GD.Print("ENEMY: Idle State");
        enemy.animatedSprite.Play("idle");
        Timer timer = GetNodeOrNull<Timer>("Timer");
        if (timer != null)
        {
            timer.Start();
        }
        else
        {
            GD.PushError($"Enemy '{enemy.Name}' requires a Timer node named 'Timer' for the Run state.");
            fsm.TransitionTo("Idle"); // Or an error state
            return;
        }

    }
    public override void Exit() {
    }

    public override void Update(float delta) {

    }
    public override void PhysicsUpdate(float delta) {
        
    }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout(){
        GD.Print("ENEMY: Idle State - Timer Timeout");
        fsm.TransitionTo("Run");
    }
}
