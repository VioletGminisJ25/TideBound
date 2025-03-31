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
    }
    public override void Exit() {
    }

    public override void Update(float delta) {
        if (raycast.IsColliding())
        {
            if(raycast.GetCollider() is Player)
            {
                GD.Print("ENEMY: Detected player!!");
                fsm.TransitionTo("Run");
            }
        }
    }
    public override void PhysicsUpdate(float delta) {
        
    }
    public override void HandleInput(InputEvent @event) { }
}
