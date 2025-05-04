using Godot;
using System;

public partial class Hit_BigRed : State
{
    [Export] Enemy enemy;

    public override void Enter() {
        GD.Print("ENEMY: Hit State");
        enemy.animatedSprite.Play("hit");
        Timer timer = GetNodeOrNull<Timer>("Timer");
        if (timer != null)
        {
            timer.Start();
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
        GD.Print("ENEMY: Hit State - Timer Timeout");
        fsm.TransitionTo("Run");
    }
}
