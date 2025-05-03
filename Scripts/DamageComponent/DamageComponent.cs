using Godot;
using System;

[Tool]
[GlobalClass]
public partial class DamageComponent : Area2D
{
	[Export] public int DamageAmount = 1;
	[Export] public float PushForce = 100.0f;
	[Export] public float PushDuration = 0.2f;
	[Signal] public delegate void TakeDamageEventHandler(float DamageAmount);


	private void _on_body_entered(Node2D body)
	{
		if(body.IsInGroup("Spikes")){
			GD.Print("Spikes: Entity Entered");
			GetParent<Node2D>().GlobalPosition = body.GetParent().GetNode<Marker2D>("Marker2D").GlobalPosition;
			return;
		}
		if (body.IsInGroup("Enemy"))
		{
			GD.Print("DamageComponent: Entity Entered");
			// Verificar si el cuerpo tiene un HealthComponent
			if (GetParent().HasNode("HealthComponent") && GetParent().GetNode("HealthComponent") is HealthComponent targetHealth)
			{
				targetHealth.TakeDamage(DamageAmount);
				EmitSignal(SignalName.TakeDamage, DamageAmount);
				
				if(GetParent().HasMethod("ApplyPushback"))
				{
					GetParent().CallDeferred("ApplyPushback", body.GlobalPosition, PushForce, PushDuration);
				}
			}
		}
		
		
	}
}