using Godot;
using System;

[Tool]
[GlobalClass]
public partial class DamageComponent : Area2D
{
	[Export] public double DamageAmount = 1;
	[Export] public float PushForce = 100.0f;
	[Export] public float PushDuration = 0.2f;
	[Signal] public delegate void TakeDamageEventHandler(float DamageAmount);
	private FadeEffect _fadeEffect;
	private Marker2D _respawnMarker;


	public override void _Ready()
	{
		_fadeEffect = GetNode<FadeEffect>("/root/Level1/CanvasLayer/ColorRect");
		if (_fadeEffect == null)
		{
			GD.PushError("FadeEffect not found in DamageComponent node.");
		}
	}

	private void _on_body_entered(Node2D body)
	{
		if(body.IsInGroup("Spikes")){
			_respawnMarker =body.GetParent().GetNode<Marker2D>("Marker2D");
			if (_fadeEffect != null)
			{
				_fadeEffect.FadedToBlack += OnFadedToBlack;
				_fadeEffect.FadeToBlack(); // Iniciar fundido
			}
			return;
		}
		if (body.IsInGroup("Enemy"))
		{
			if (body is Enemy enemy)
			{
				DamageAmount = enemy.damage;
			}
			// Verificar si el cuerpo tiene un HealthComponent
			if (GetParent().HasNode("HealthComponent") && GetParent().GetNode("HealthComponent") is HealthComponent targetHealth)
			{
				targetHealth.TakeDamage(int.Parse(DamageAmount.ToString()));
				EmitSignal(SignalName.TakeDamage, DamageAmount);
				
				if(GetParent().HasMethod("ApplyPushback"))
				{
					GetParent().CallDeferred("ApplyPushback", body.GlobalPosition, PushForce, PushDuration);
				}
			}
		}
		
		
	}

	public void _on_area_exited(Area2D area)
	{
		if (area.IsInGroup("Water"))
		{
			if (GetParent().HasMethod("switchToWater"))
			{
				GetParent<Node2D>().CallDeferred("switchToWater");
				GetParent<Player>()._waterCooldown.Stop();
			}
			else
			{
				GD.PushError("Parent does not have switchToWater method.");
			}
		}
	}
	public void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Water"))
		{
			if (GetParent().HasMethod("switchToWater"))
			{
				GetParent<Node2D>().CallDeferred("switchToWater");
			}
			else
			{
				GD.PushError("Parent does not have switchToWater method.");
			}
		}
	}
	private void OnFadedToBlack()
	{
		// Reposicionar jugador al marker
		if (_respawnMarker != null)
		{
			GetParent<Node2D>().GlobalPosition = _respawnMarker.GlobalPosition;
			GD.Print("DamageComponent: Player repositioned after fade");
		}

		// Desconectar señal y hacer fundido de regreso
		_fadeEffect.FadedToBlack -= OnFadedToBlack;
		_fadeEffect.FadeFromBlack();
	}
}