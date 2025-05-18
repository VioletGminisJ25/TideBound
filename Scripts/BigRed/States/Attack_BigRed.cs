using Godot;
using System;
using System.Numerics;

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
        if (raycast.IsColliding() && raycast.GetCollider() is Player player)
        {
            player.CallDeferred("ApplyPushback", enemy.GlobalPosition, enemy.PushForce, enemy.PushDuration);
            HealthComponent targetHealth = player.GetNodeOrNull<HealthComponent>("HealthComponent");
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(int.Parse(enemy.damage.ToString()));
            }
        }
      }
    public override void HandleInput(InputEvent @event) { }

    public void _on_timer_timeout()
    {
        if(fsm.currentState == this)
        {
            fsm.TransitionTo("Idle");
        }
    }

    public void _on_animated_sprite_2d_animation_finished(string animationName)
    {}

}
