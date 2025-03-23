using Godot;
using System;

public partial class DamageComponent : Area2D
{
	[Export] public AnimationPlayer AnimationPlayer;

	[Signal]
	public delegate bool mysignalEventHandler();
	private DamageableObject parent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.SetDeferred("monitorable", true);
		parent = (DamageableObject)this.GetParent();
	}

	public void dameage()
	{
		AnimationPlayer.Stop();
		AnimationPlayer.Play("hit");

		if (parent.Health <= 0)
		{
			this.SetDeferred("monitorable", false);
			EmitSignal(SignalName.mysignal);
		}
		else
		{
			parent.Health--;
		}
	}
}

