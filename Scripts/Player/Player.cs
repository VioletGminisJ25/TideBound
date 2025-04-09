using System;
using Godot;

public partial class Player : CharacterBody2D, DamageableObject, AttackInterface,IHook
{
	private int health = 100;
    public int Health { get => health; set => health = value; }
	private bool isAttacking = false;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
	private Rope line;
	[Export] public Rope Line { get => line; set => line = value; }
	private StaticBody2D cursor;
	[Export] public StaticBody2D Cursor { get => cursor; set => cursor = value; }
	private bool isHooked = false;
    public bool IsHooked { get => isHooked; set => isHooked = value; }
	private bool skipGravityFrame = false;
    public bool SkipGravityFrame { get => skipGravityFrame; set => skipGravityFrame = value; }


    public override void _Ready() {}
	public override void _PhysicsProcess(double delta){
		MoveAndSlide();
	}

	public void _on_sword_hit_area_entered(Area2D area)
	{
		if (area is DamageComponent)
		{
			DamageComponent hurt = (DamageComponent)area;
			hurt.dameage();
		}
	}
}











