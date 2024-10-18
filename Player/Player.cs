using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed = 170.0f;
    [Export]
    public float JumpVelocity = -340.0f;
    private static AnimatedSprite2D animationPlayer;
    private Godot.Vector2 velocity;
    private Godot.Vector2 particlesScale;
    private Godot.Vector2 particlesPosition;
    private Godot.Vector2 lightPosition;
    private static PointLight2D playerlight;
    private static CpuParticles2D particles;
    private bool attack;
    private bool animationFinished;
    Random random = new Random();

    public override void _PhysicsProcess(double delta)
    {
        animationPlayer = this.GetChild(1) as AnimatedSprite2D;
        particles = animationPlayer.GetChild(0) as CpuParticles2D;
        Movement(delta);
        //Attack();
        Animations();
        MoveAndSlide();
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
                velocity.Y = JumpVelocity;
            }
            else
            {

                Particles();
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
                animationPlayer.FlipH = false;
                lightPosition.X = 7;
                playerlight.Position = lightPosition;

            }
            else
            {
                animationPlayer.FlipH = true;
                lightPosition.X = -7;
                playerlight.Position = lightPosition;
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

    private void Particles()
    {

        if (Convert.ToInt32(GetRealVelocity().Y) == 0)
        {

            if (velocity.X > 0)
            {
                particles.Emitting = true;
                particles.Gravity = new Godot.Vector2(0, 0);
            }
            if (velocity.X < 0)
            {
                particles.Emitting = true;
                particles.Gravity = new Godot.Vector2(0, 0);

            }
            if (velocity.X == 0)
            {
                particles.Emitting = false;
            }
        }
    }
    private void _animation_finished()
    {
        if (animationPlayer.Animation == "attack1" || animationPlayer.Animation == "attack2" || animationPlayer.Animation == "attack3")
        {
            attack = false;
        }
    }

}






