using Godot;
using System;

public partial class Hurt : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    public void dameage()
    {
        if (this.GetParent() is Box)
        {

            Box parent = (Box)this.GetParent();
            for (int i = 0; i < parent.GetChildCount(); i++)
            {
                if (parent.GetChild(i) is AnimatedSprite2D)
                {
                    AnimatedSprite2D anim = (AnimatedSprite2D)parent.GetChild(i);
                    for (int j = 0; j < anim.GetChildCount(); j++)
                    {
                        if (anim.GetChild(j) is AnimationPlayer)
                        {
                            AnimationPlayer player = (AnimationPlayer)anim.GetChild(j);

                            player.Stop();
                            player.Play("hit");
                        }
                    }
                }
            }
            parent.Health--;
        }
    }
}
