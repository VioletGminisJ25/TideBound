using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
   
    [Export]
    private float Speed;
    [Export]
    private float JumpVelocity;
    [Export]
    private AnimatedSprite2D animatedSprite;
    [Export]
    public CpuParticles2D moveEffects;
    [Export]
    private PointLight2D playerlight;
    [Export]
    private AnimationPlayer animationPlayer;

    private Godot.Vector2 velocity;
    private Godot.Vector2 particlesScale;
    private Godot.Vector2 particlesPosition;
    private Godot.Vector2 lightPosition;

    //private static CpuParticles2D particles;
    private bool animationFinished;
    private Godot.Vector2 push_force = new Godot.Vector2(20, 20);
    Random random = new Random();

    private bool attack;
    public bool hit;

    public bool Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Animations();
        MoveAndSlide();
        push();

    }
    private void push()
    {
        for (int i = 0; i < this.GetSlideCollisionCount(); i++)
        {
            var c = this.GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D || c.GetCollider() is CharacterBody2D)
            {
                //((RigidBody2D)c.GetCollider()).ApplyCentralForce(-c.GetNormal() * push_force);
                ((RigidBody2D)c.GetCollider()).ApplyCentralImpulse(-c.GetNormal() * push_force);
            }
        }
    }

    
    private void Movement(double delta)
    {

        //Vertical movement
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        else
        {
            velocity.Y = 0;
            if (Input.IsActionJustPressed("ui_space"))
            {
                if (random.Next(0, 2) == 1)
                {
                    moveEffects.Emitting = true;

                }

                velocity.Y = JumpVelocity;

            }
        }

        //TODO IF DIRECOTION KEY IS JUST PRESSED PARTICLE ON
        //Horizontal movement
        Godot.Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Godot.Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            //Flip sprite direction
            if (velocity.X > 0)
            {
                animatedSprite.Scale = new Godot.Vector2(1, 1);

            }
            else
            {
                animatedSprite.Scale = new Godot.Vector2(-1, 1);
            }
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

        }

        Velocity = velocity; //Actualizar velocidad mediante una variable no a pelo??

    }
    private void Animations()
    {
        if (Input.IsActionJustPressed("attack"))
        {
            int num = random.Next(1, 4);
            animationPlayer.Play($"attack{num}");
            attack = true;
            hit = true;
        }
        else
        {
            hit = false;
        }

        if (!attack)
        {

            if (IsOnFloor())
            {

                if (velocity.X != 0)
                {
                    animationPlayer.Play("run");
                }
                else
                {
                    animationPlayer.Play("idle");
                }
            }
            else
            {

                if (velocity.Y > 0)
                {
                    animationPlayer.Play("fall");
                }
                else
                {
                    animationPlayer.Play("jump");
                }
            }


        }
    }


    private void _on_animation_player_animation_finished(String anim)
    {
        if (anim == "attack1" || anim == "attack2" || anim == "attack3")
        {
            attack = false;
        }
    }
    public void _on_sword_hit_area_entered(Area2D area)
    {
        if (area is Hurt)
        {
            Hurt hurt = (Hurt)area;
            hurt.dameage();
        }
    }
}








