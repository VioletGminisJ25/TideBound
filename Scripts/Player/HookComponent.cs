using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class HookComponent : Node2D
{
	[Export] public RayCast2D raycast;
	private float hookStopDistance = 10f;
	private float hookSpeed = 300f;
	private bool isHooked = false;
	private CharacterBody2D parent;
	private Vector2 velocity;

	private Rope line;
	private StaticBody2D cursor;

	public override void _Ready()
	{
		parent = (CharacterBody2D)this.GetParent();
		if(parent is IHook parentHook)
		{
			line = parentHook.Line;
			cursor = parentHook.Cursor;
		}
	}
	public override void _PhysicsProcess(double delta){
		// springJoint.GlobalPosition = GlobalPosition;
		raycast.LookAt(GetGlobalMousePosition());
		raycast.Rotation = raycast.Rotation + 80;
		if (Input.IsActionJustPressed("hook"))
		{
			if (raycast.IsColliding())
			{
				line.ClearPoints();
				cursor.Position = raycast.GetCollisionPoint();
				float distance = this.Position.DistanceTo(cursor.Position);
				// springJoint.Length = distance;
				// springJoint.RotationDegrees = raycast.RotationDegrees;
				// springJoint.RestLength = distance*tension;
				line.start = cursor.GlobalPosition;
				// springJoint.NodeB =cursor.GetPath();
				isHooked = true;
			}
			else
			{
				isHooked = false;
			}
		}
		if (isHooked)
		{

			if (Input.IsActionPressed("hook"))
			{
				line.Visible = true;
				line.end = this.GlobalPosition;

				Vector2 hookDirection = (cursor.GlobalPosition - GlobalPosition).Normalized();
				float distance = GlobalPosition.DistanceTo(cursor.GlobalPosition);

				if (distance > hookStopDistance)
				{
					velocity = hookDirection * hookSpeed;
				}
				else
				{
					velocity = Vector2.Zero;
				}
				parent.Velocity = velocity;
				// if(springJoint.NodeA != springJoint.NodeB){
				// }

			}
			else
			{
				line.Visible = false;
				line.ClearPoints();
				// springJoint.NodeB = springJoint.NodeA;
			}

		}
	}
}
