using Godot;
using System;

public partial class Attack_BigRed : State
{
    [Export] public RayCast2D raycast; // Keep if used elsewhere, otherwise remove
    [Export] public Enemy enemy; // Ensure this is assigned in the inspector

    public override void Enter() {
        enemy.animatedSprite.Play("attack");
        GetChild<Timer>(0).Start(); // Start the timer for the attack animation
    }
    public override void Exit() { }

    public override void Update(float delta) {
        
    }
    public override void PhysicsUpdate(float delta) {
    //     if(raycast.GetCollider() is not Player player)
    //     {
    //         GD.Print("ENEMY: Attack State - Raycast Collided with Player");
    //         fsm.TransitionTo("Idle");
    //     }
        if(raycast.IsColliding() && raycast.GetCollider() is Player player)
        {
            GD.Print("ENEMY: Attack State - Raycast Collided with Player");
            player.CallDeferred("ApplyPushback", enemy.GlobalPosition, enemy.PushForce, enemy.PushDuration);
        }
      }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout()
    {
        if(fsm.currentState == this)
        {
            GD.Print("ENEMY: Attack State - Timer Timeout");
            fsm.TransitionTo("Idle");
        }
    }

    public void _on_animated_sprite_2d_animation_finished(string animationName)
    {
        // if (animationName == "attack")
        // {
        //     GD.Print("ENEMY: Attack State - Animation Finished");
        //     fsm.TransitionTo("Idle");
        // }
    }

}
