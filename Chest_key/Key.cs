using Godot;
using System;

public partial class Key : RigidBody2D
{
    AnimatedSprite2D animatedSprite;
    AnimationPlayer player;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            if (this.GetChild(i) is AnimatedSprite2D)
            {
                animatedSprite = (AnimatedSprite2D)(this.GetChild(i));
            }

        }
        for (int j = 0; j < animatedSprite.GetChildCount(); j++)
        {
            if (animatedSprite.GetChild(j) is AnimationPlayer)
            {
                {
                    player = (AnimationPlayer)(animatedSprite.GetChild(j));
                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        for (int i = 0 ; i < this.GetCollidingBodies().Count; i++)
        {

            if (this.GetCollidingBodies()[i] is Player)
            {
                animatedSprite.Stop();
                player.Stop();
            }
        }

    }

    public void _on_body_entered(Node2D body)
    {
        if (body is Player)
        {
            
        }
    }
}
