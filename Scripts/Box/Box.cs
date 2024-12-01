using Godot;
using System;

public partial class Box : RigidBody2D
{
    private int heath;
    AnimatedSprite2D sprite;
    AnimationPlayer player;
    Area2D area;
    bool destroyed = false;
    public int Health
    {
        get
        {
            return heath;
        }
        set
        {

            heath = value;
        }
    }


    public Box()
    {
        heath = 3;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print($"INIT: {heath}");
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            if (this.GetChild(i) is AnimatedSprite2D)
            {
                sprite = ((AnimatedSprite2D)this.GetChild(i));
                for (int j = 0; j < sprite.GetChildCount(); j++)
                {
                    if (sprite.GetChild(j) is AnimationPlayer)
                    {
                        player = ((AnimationPlayer)sprite.GetChild(j));
                    }
                }
            }
        }
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            if (this.GetChild(i) is Area2D)
            {
                area = ((Area2D)this.GetChild(i));
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (heath < 0)
        {
          
            destroyed = true;
        }
        if (destroyed)
        {
            if (heath <= 0)
            {
                destroyed = false;
                player.Play("destroyed");

            }
        }


    }
    public void _on_animation_player_animation_finished(String anim)
    {
        if (anim == "destroyed")
        {
            area.Monitorable = false;
            area.Monitoring= false;
            this.QueueFree();
        }
    }


}