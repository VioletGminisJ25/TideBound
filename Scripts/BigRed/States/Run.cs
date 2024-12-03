using Godot;
using System;
using static Godot.TextServer;

public partial class Run : State
{
    [Export] NavigationAgent2D navigationAgent2D;
    [Export] Enemy enemy;
    [Export] float speed;
    private Godot.Vector2 velocity;
    public override void Enter()
    {
        GD.Print("ENEMY: Run State");
        velocity = new Godot.Vector2(0,0);

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
    public override void PhysicsUpdate(float delta) {
        Godot.Vector2 dir = enemy.ToLocal(navigationAgent2D.GetNextPathPosition()).Normalized();

        if (dir != Godot.Vector2.Zero)
        {
            velocity.X = dir.X * speed;
            
        }
        else
        {
            velocity.X = Mathf.MoveToward(enemy.Velocity.X, 0, speed);

        }
        velocity.Y = enemy.GetGravity().Y * (float)delta;
        GD.Print(velocity.ToString());

        if (velocity.X < 0)
        {
            enemy.Scale = new Godot.Vector2(1, 1);

        }
        else
        {
            enemy.Scale = new Godot.Vector2(-1, 1);
        }

        enemy.Velocity = velocity ;
        enemy.MoveAndSlide();
    }
    public override void HandleInput(InputEvent @event) { }

}
