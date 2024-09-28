using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed = 170.0f;
    [Export]
    public float JumpVelocity = -300.0f;
    private static AnimatedSprite2D animationPlayer;
    private Godot.Vector2 velocity;
    private Godot.Vector2 particlesScale;
    private Godot.Vector2 particlesPosition;
    private Godot.Vector2 lightPosition;
    private static PointLight2D playerlight;
    private static CpuParticles2D particles;

    public override void _PhysicsProcess(double delta)
    {
        // Vector2 position = Position;
        animationPlayer = this.GetChild(1) as AnimatedSprite2D;
        particles = animationPlayer.GetChild(0) as CpuParticles2D;
        playerlight = this.GetChild(2) as PointLight2D;
        particles.Emitting = false;
        velocity = Velocity;

        //Vertical movement
        if (!IsOnFloor())
        {

            velocity += GetGravity() * (float)delta;

        }
        else
        {
            Particles();
            if (Input.IsActionJustPressed("ui_space"))
            {
                velocity.Y = JumpVelocity;

            }
        }
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

        MoveAndSlide();
        Animations();
    }
    private void Animations()
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
    public void Particles()
    {

        if (velocity.X > 0)
        {
            particles.Emitting = true;
            particlesScale.X = (float)0.6;
            particlesPosition.X = -8;
            particles.Scale = particlesScale;
            particles.Position = particlesPosition;
        }
        if (velocity.X < 0)
        {
            particles.Emitting = true;
            particlesScale.X = (float)-0.6;
            particlesPosition.X = 8;
            particles.Scale = particlesScale;
            particles.Position = particlesPosition;
        }
        if (velocity.X == 0)
        {
            particles.Emitting = false;
        }

    }
    //public static void _on_cpu_particles_2d_finished()
    //{
    //    particles.Emitting = false;
    //}
}





