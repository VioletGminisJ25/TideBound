using Godot;
using System;


public partial class Box : RigidBody2D, DamageableObject
{
	[Export] AnimationPlayer player;
	private int health;
	[Export] public int Health
	{
		get
		{
			return health;
		}
		set
		{

			health = value;
		}
	}
	private bool destroyed = false;


	public Box()
	{
		health = health != 0 ? health : 1; 
		destroyed = false;
	}

	public override void _Process(double delta)
	{
		
	}

	public void Destroy()
	{
		player.Play("destroyed");
	}

	public void _on_animation_player_animation_finished(string anim_name){
		if (anim_name == "destroyed")
		{
			this.QueueFree();
			destroyed = true;
		}
	}



}
