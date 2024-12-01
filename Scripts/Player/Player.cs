using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed = 170.0f;
    [Export]
    public float JumpVelocity = -340.0f;
    private static AnimatedSprite2D animatedSprite;
    private Godot.Vector2 velocity;
    private Godot.Vector2 particlesScale;
    private Godot.Vector2 particlesPosition;
    private Godot.Vector2 lightPosition;
    private static CpuParticles2D moveEffects;
    private static PointLight2D playerlight;
    //private static CpuParticles2D particles;
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
    private bool animationFinished;
    private Godot.Vector2 push_force = new Godot.Vector2(40, 40);
    Random random = new Random();
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        animatedSprite = this.GetChild(1) as AnimatedSprite2D;
        for (int i = 0; i < animatedSprite.GetChildCount(); i++)
        {
            if (animatedSprite.GetChild(i) is CpuParticles2D)
            {
                moveEffects = (CpuParticles2D)animatedSprite.GetChild(i);
            }
            if (animatedSprite.GetChild(i) is AnimationPlayer)
            {
                animationPlayer = (AnimationPlayer)animatedSprite.GetChild(i);
            }
        }
        //particles = animationPlayer.GetChild(0) as CpuParticles2D;
        Movement(delta);
        //Attack();
        Animations();
        MoveAndSlide();
        push();

    }
    private void push()
    {
        for (int i = 0; i < this.GetSlideCollisionCount(); i++)
        {
            var c = this.GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D)
            {
                //((RigidBody2D)c.GetCollider()).ApplyCentralForce(-c.GetNormal() * push_force);
                ((RigidBody2D)c.GetCollider()).ApplyCentralImpulse(-c.GetNormal() * push_force);
            }
        }
    }
    private void Movement(double delta)
    {
        playerlight = this.GetChild(2) as PointLight2D;

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
            else
            {

                //Particles();
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
            //animatedSprite.Play($"attack{num}");
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

    //private void Particles()
    //{

    //    if (Convert.ToInt32(GetRealVelocity().Y) == 0)
    //    {

    //        if (velocity.X > 0)
    //        {
    //            particles.Emitting = true;
    //            particles.Gravity = new Godot.Vector2(0, 0);
    //        }
    //        if (velocity.X < 0)
    //        {
    //            particles.Emitting = true;
    //            particles.Gravity = new Godot.Vector2(0, 0);

    //        }
    //        if (velocity.X == 0)
    //        {
    //            particles.Emitting = false;
    //        }
    //    }
    //}
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








