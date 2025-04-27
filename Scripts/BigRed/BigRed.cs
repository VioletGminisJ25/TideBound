using Godot;
using System;

public partial class BigRed : Enemy
{
    public Vector2 velocity;
    private HealthComponent _healthComponent;
    public BigRed():base()
    {
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
        base._PhysicsProcess(delta);
    }

    private void _on_body_entered(Node2D body)
    {
        if (body is Player player)
        {
            GD.Print("ENEMY: BigRed - Player Entered");
            // Verificar si el jugador tiene un DamageComponent (en su área de ataque)
            if (player.HasNode("SwordHitbox") && player.GetNode("SwordHitbox") is Area2D swordHitboxArea)
            {
                if (swordHitboxArea.HasNode("DamageComponent") && swordHitboxArea.GetNode("DamageComponent") is DamageComponent damageComponent)
                {
                    GD.Print("ENEMY: BigRed - Player's attack detected.");
                    _healthComponent?.TakeDamage(damageComponent.DamageAmount);
                    ApplyPushback(player.GlobalPosition, damageComponent.PushForce, damageComponent.PushDuration);
                }
            }
            // También podrías tener un DamageComponent directamente en el Player si el contacto cuerpo a cuerpo daña
            else if (player.HasNode("DamageComponent") && player.GetNode("DamageComponent") is DamageComponent playerDamageComponent)
            {
                GD.Print("ENEMY: BigRed - Player's body contact damage.");
                _healthComponent?.TakeDamage(playerDamageComponent.DamageAmount);
                ApplyPushback(player.GlobalPosition, playerDamageComponent.PushForce, playerDamageComponent.PushDuration);
            }
        }
    }

}

