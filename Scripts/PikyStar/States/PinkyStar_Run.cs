using Godot;
using System;

public partial class PinkyStar_Run : State
{
    [Export] public Enemy enemy; // Ensure this is assigned in the inspector
    // [Export] public RayCast2D raycast; // Keep if used elsewhere, otherwise remove
    [Export] public float speed = 10f; // Pixels per second

    [Export] public RayCast2D raycast; // Keep if used elsewhere, otherwise remove

    private bool movingForward = true;
    private PathFollow2D pathFollow;
    private Path2D path;

    // Small tolerance to prevent issues with floating point comparisons at path ends
    private const float PATH_END_TOLERANCE = 0.005f; // Adjust if needed
    public AnimationNodeStateMachinePlayback animationMachine;

    public override void Enter()
    {
        animationMachine = (AnimationNodeStateMachinePlayback)enemy.animationTree.Get("parameters/playback");

        GD.Print("ENEMY: Run State - Enter");

        Timer timer = GetNodeOrNull<Timer>("Timer");
        if (timer != null)
        {
            timer.Start();
        }
        else
        {
            GD.PushError($"Enemy '{enemy.Name}' requires a Timer node named 'Timer' for the Run state.");
            fsm.TransitionTo("Idle"); // Or an error state
            return;
        }


        pathFollow = enemy.GetParent<PathFollow2D>();
        if (pathFollow == null)
        {
            GD.PushError($"Enemy '{enemy.Name}' requires a PathFollow2D as its direct parent.");
            fsm.TransitionTo("Idle"); // Or an error state
            return;
        }

        path = pathFollow.GetParent<Path2D>();
        if (path == null || path.Curve == null)
        {
            GD.PushError($"PathFollow2D for enemy '{enemy.Name}' requires a Path2D with a valid Curve resource as its parent.");
            fsm.TransitionTo("Idle"); // Or an error state
            return;
        }

        if (path.Curve.GetBakedLength() <= 0f)
        {
            GD.PushWarning($"Path for enemy '{enemy.Name}' has zero length.");
            // Decide how to handle this - maybe stay idle?
            fsm.TransitionTo("Idle");
            return;
        }

        movingForward = true; // Default to forward

        // Set initial visual direction
        SetVisualDirection(movingForward);

        if (enemy.animatedSprite != null)
        {
            animationMachine.Travel("run");
        }
        else
        {
            GD.PushWarning($"Enemy '{enemy.Name}' has no AnimatedSprite2D assigned in its Enemy script.");
        }
    }

    public override void Exit()
    {
        GD.Print("ENEMY: Run State - Exit");
        if (enemy is CharacterBody2D character)
        {
            character.Velocity = Vector2.Zero;

        }
    }

    public override void Update(float delta)
    {
    }

    public void _on_timer_timeout()
    {
        if (fsm.currentState == this)
        {
            GD.Print("ENEMY: Run State - Timer Timeout");
            fsm.TransitionTo("Idle");
        }
    }

    public override void PhysicsUpdate(float delta)
    {
        if (raycast.IsColliding() && raycast.GetCollider() is Player player)
        {
            fsm.TransitionTo("Attack");
        }
        if (pathFollow == null || path == null || path.Curve == null || !(enemy is CharacterBody2D character))
            return;

        MoveOnPath(character, delta);
    }

    private void MoveOnPath(CharacterBody2D character, float delta)
    {
        float pathLength = path.Curve.GetBakedLength();
        if (pathLength <= 0f)
        {
            GD.Print("Path length is zero or negative, cannot move on path.");
            return; // Avoid division by zero
        }

        float progressIncrement = speed / pathLength * delta;
        float directionMultiplier = movingForward ? 1f : -1f;
        if (fsm.currentState is PinkyStar_Run)
        {
            pathFollow.ProgressRatio += directionMultiplier * progressIncrement;
            pathFollow.ProgressRatio = Mathf.Clamp(pathFollow.ProgressRatio, 0f, 1f);
        }

        // 2. Get the *target* world position from the PathFollow2D
        Vector2 targetPosition = pathFollow.GlobalPosition;

        // 3. Calculate the direction from the character's *current* position to the target
        Vector2 currentPosition = character.GlobalPosition;
        Vector2 directionToTarget = targetPosition - currentPosition;

        // 4. Set the character's velocity towards the target
        if (directionToTarget.Length() > 1.0f) // Only move if not already very close to the target point
        {
            // Move towards the target at the defined speed
            character.Velocity = directionToTarget.Normalized() * speed;
        }
        else
        {
            character.Velocity = Vector2.Zero;
        }


        character.MoveAndSlide(); // This handles collisions and actual movement

        if (movingForward && pathFollow.ProgressRatio >= (1f - PATH_END_TOLERANCE))
        {
            movingForward = false;
            pathFollow.ProgressRatio = 1f; // Snap to exact end
            SetVisualDirection(movingForward);
        }
        else if (!movingForward && pathFollow.ProgressRatio <= PATH_END_TOLERANCE)
        {
            movingForward = true;
            pathFollow.ProgressRatio = 0f; // Snap to exact start
            SetVisualDirection(movingForward);
        }
    }

    // Helper function to set sprite/scale direction
    private void SetVisualDirection(bool isMovingForward)
    {
        GD.Print(enemy.animatedSprite);
        if (enemy.animatedSprite != null)
        {
            enemy.animatedSprite.GetParent<Node2D>().Scale = new Vector2(!isMovingForward ? 1 : -1, -1);
        }


    }
}
