using Godot;
using System;

public partial class Rope : Line2D
{
	[Export] public Vector2 start;
	[Export] public Vector2 end;

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(this.Visible){
			AddPoint(start);
			AddPoint(end,1);
			RemovePoint(0);
		}else{
			ClearPoints();
		}

	}

}
