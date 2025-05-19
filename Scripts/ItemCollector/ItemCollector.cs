using Godot;
using System;

public partial class ItemCollector : Area2D
{
    private userInterface ui;
    public override void _Ready()
    {
        ui = GetTree().Root.GetNode("Level1/Menu/UI") as userInterface;
    }
    public void _on_body_entered(Node2D body)
    {
        if (body.IsInGroup("Key"))
        {
            if (!ui.getKey())
            {
                if (body.HasMethod("destroy"))
                {
                    body.CallDeferred("destroy");
                }

                ui.setKey(true);
            }
        }
        if (body.IsInGroup("Chest"))
        {
            if (ui.getKey())
            {
                if (body.HasMethod("open"))
                {
                    ui.setKey(((Chest)body).open());
                }

            }
        }
        if (body.IsInGroup("Coin"))
        {
            body.CallDeferred("destroy");
            ui.addCoins(1);
        }
    }
}
