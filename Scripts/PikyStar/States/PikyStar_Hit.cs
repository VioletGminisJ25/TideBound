using Godot;
using System;

public partial class PikyStar_Hit : State
{

    [Export] Enemy enemy;
    public AnimationNodeStateMachinePlayback animationMachine;


    public override void Enter()
    {
        animationMachine = (AnimationNodeStateMachinePlayback)enemy.animationTree.Get("parameters/playback");


        GD.Print("ENEMY: Hit State");
        animationMachine.Travel("hit");
        Timer timer = GetNodeOrNull<Timer>("Timer");
        if (timer != null)
        {
            timer.Start();
        }
    }
    public override void Exit()
    {
    }

    public override void Update(float delta)
    {

    }
    public override void PhysicsUpdate(float delta)
    {


    }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout()
    {
        GD.Print("ENEMY: Hit State - Timer Timeout");
        fsm.TransitionTo("Run");
    }
}
