using Godot;
using System;

[Tool]
[GlobalClass]
public partial class DamageComponent : Area2D
{
	[Export] public AnimationPlayer AnimationPlayer;

	[Signal]
	public delegate bool mysignalEventHandler();
	private HealthComponent healthComponent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.SetDeferred("monitorable", true);
		Node parent = GetParent();
		if (parent == null)
		{
			GD.PushError("Parent node not found during runtime.");
			return;
		}

		healthComponent = parent.GetNodeOrNull<HealthComponent>("HealthComponent");
		if (healthComponent == null)
		{
			GD.PushWarning("HealthComponent not found in parent node during runtime.");
		}
	}

    public override void _EnterTree()
    {
		if (Engine.IsEditorHint())
		{
			Node parent = GetParent();
			if (parent == null)
			{
				GD.PushWarning("Parent node not found in editor.");
				return;
			}

			if (parent.GetNodeOrNull<HealthComponent>("HealthComponent") == null)
			{
				GD.PushWarning("HealthComponent not found in parent node in editor.");
			}
			return;
		}
	}





	public void dameage()
	{
		AnimationPlayer.Stop();
		AnimationPlayer.Play("hit");

		if (healthComponent.Health <= 0)
		{
			this.SetDeferred("monitorable", false);
			EmitSignal(SignalName.mysignal);
		}
		else
		{
			healthComponent.Health--;
		}
	}
}

