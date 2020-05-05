using Godot;
using System;

public class Title : Node
{
    private int tick = 0;
    private const int K = 60;
    Node2D player;
    Vector2 playerDefaultPos;
    Node2D box;
    Vector2 boxDefaultPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (Node2D)FindNode("Player");
        box = (Node2D)FindNode("Box");
        playerDefaultPos = player.Position;
        boxDefaultPos = box.Position;
        SetupButtons();
    }

    public override void _Process(float delta)
    {
        int frame = 0;
        tick++;
        if (tick <= 0 || tick % K != 0) return;
        frame = tick / K;
        if (frame <= 4)
        {
            player.Position += new Vector2(Const.GRID_SIZE, 0);
        }
        else if (frame <= 9)
        {
            player.Position += new Vector2(Const.GRID_SIZE, 0);
            box.Position += new Vector2(Const.GRID_SIZE, 0);
        }
        else if (frame <= 12)
        {
        }
        else
        {
            tick = -180;
            player.Position = playerDefaultPos;
            box.Position = boxDefaultPos;
        }
    }

    private void SetupButtons()
    {
        HBoxContainer h = (HBoxContainer)FindNode("HBoxContainer");
        LevelButton b1 = (LevelButton)FindNode("LevelButton");
        b1.ButtonID = 1;

        for (int i = 2; i <= Const.MAX_LEVELS; i++)
        {
            LevelButton b = (LevelButton)b1.Duplicate();
            b.Name = string.Format("Button{0}", i);
            b.Text = i.ToString();
            b.ButtonID = i;
            h.AddChild(b);
        }
    }
}
