using Godot;
using System;

[Tool]
[GlobalClass]
public partial class DamageComponent : Area2D
{
	[Export] public int DamageAmount = 1;
	[Export] public float PushForce = 100.0f;
	[Export] public float PushDuration = 0.1f;

	private void _on_body_entered(Node2D body)
	{
		if (body.HasNode("HealthComponent"))
		{
			HealthComponent targetHealth = body.GetNode<HealthComponent>("HealthComponent");
			targetHealth.TakeDamage(DamageAmount);

			// Intentamos aplicar el retroceso si el cuerpo tiene una función ApplyPushback
			if (body.HasMethod("ApplyPushback"))
			{
				Vector2 direction = (body.GlobalPosition - GlobalPosition).Normalized();
				body.Call("ApplyPushback", GlobalPosition, PushForce, PushDuration);
			}
		}
	}
}