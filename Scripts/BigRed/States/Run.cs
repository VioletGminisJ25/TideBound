using Godot;
using System;
using static Godot.TextServer;

public partial class Run : State
{
    [Export] NavigationAgent2D navigationAgent2D;
    [Export] Enemy enemy;
    [Export] RayCast2D raycast;
    [Export] float speed;
    private Godot.Vector2 velocity;
    private int JumpVelocity;
    private bool colliding = false;
    public override void Enter()
    {
        GD.Print("ENEMY: Run State");
        velocity = new Godot.Vector2(0, 0);
        JumpVelocity = 310;

    }
    public override void Exit() { }

    public override void Update(float delta)
    {


    }

    public void MakePath()
    {
        navigationAgent2D.TargetPosition = enemy.player.GlobalPosition;
    }

    public void _on_timer_timeout()
    {
        MakePath();
    }
    public override void PhysicsUpdate(float delta)
    {
        Motion(delta);


    }

    private void Motion(float delta)
    {
        Godot.Vector2 dir = enemy.ToLocal(navigationAgent2D.GetNextPathPosition()).Normalized();

        if (dir != Godot.Vector2.Zero)
        {
            velocity.X = dir.X * speed;

            if (velocity.X < 0)
            {
                enemy.animatedSprite.FlipH = true;
                double rotation = 90 * (Math.PI / 180);
                raycast.Rotation = (float)rotation;



            }
            if (velocity.X > 0)
            {
                enemy.animatedSprite.FlipH = false;
                double rotation = 270 * (Math.PI / 180);
                raycast.Rotation = (float)rotation;

            }
        }
        else
        {
            velocity.X = Mathf.MoveToward(enemy.Velocity.X, 0, speed);
        }

        if (!enemy.IsOnFloor())
        {
            velocity += enemy.GetGravity() * (float)delta;
        }
        else
        {
            velocity.Y = 0;
            if (raycast.IsColliding())
            {
                if (!(raycast.GetCollider() is Player))
                {
                    velocity.Y = -JumpVelocity;
                }
            }
        }
        enemy.Velocity = velocity;
        enemy.MoveAndSlide();
        Animations();

        //if (raycast.IsColliding())
        //{
        //    if (raycast.GetCollider() is Player)
        //    {
        //       fsm.TransitionTo("Attack");
        //    }
        //}
    }

    private void Animations()
    {
        if (enemy.IsOnFloor())
        {

            if (velocity.X != 0)
            {
                enemy.animatedSprite.Play("run");
            }
            else
            {
                enemy.animatedSprite.Play("idle");
            }
        }
        else
        {

            if (velocity.Y > 0)
            {
                enemy.animatedSprite.Play("jump");
            }
            else
            {
                enemy.animatedSprite.Play("jump");
            }
        }
    }

    public override void HandleInput(InputEvent @event) { }

}
