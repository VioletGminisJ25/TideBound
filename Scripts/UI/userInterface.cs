using Godot;
using System;

public partial class userInterface : Control
{

    TextureRect key;
    private bool haveKey = false;

    public Label coinsLabel;
    private int coins = 0;
    public int getCoins()
    {
        return coins;
    }
    public void setCoins(int value)
    {
        coins = value;
        coinsLabel.Text = coins.ToString();

    }
    public void addCoins(int value)
    {
        coins += value;
        coinsLabel.Text = coins.ToString();
    }
    public void setKey(bool value)
    {
        haveKey = value;
        key.Visible = value;
    }
    public bool getKey()
    {
        return haveKey;
    }

    public override void _Ready()
    {
        key = GetNode<TextureRect>("Key");
        coinsLabel = GetNode<Label>("CoinsLabel");
        setKey(false);
    }



}
