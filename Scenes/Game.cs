using Godot;
using System;
using System.Diagnostics;

public class Game : Node2D
{
    bool gameEnded = false;
    int moves = 0;
    int currentLevel = 1;
    int displayCounter = 0;
    Timer timer = new Timer();

    public override void _Ready()
    {
        AddChild(timer);
        Debug.WriteLine("Ready called");
        ResetLevel();
    }
    public override void _Process(float delta)
    {
        ((Label)FindNode("LabelMoves")).Text = "Moves: " + moves.ToString();
        if (displayCounter++ > 60)
        {
            displayCounter = 0;
        }

        if (gameEnded == false)
        {
            int spots = FindNode("Spots").GetChildCount();

            foreach (Spot spot in FindNode("Spots").GetChildren())
            {
                if (spot.Occupied) spots--;
            }
            if (spots == 0)
            {
                // level cleared
                // ((AcceptDialog)FindNode("AcceptDialog")).Popup_();
                gameEnded = true;
                ShowLevelCleared();
                GoToNextLevel();
            }
            else if (displayCounter == 0)
            {
                // Debug.WriteLine("Total: {0}, Remaiing: {1}", FindNode("Spots").GetChildCount(), spots);
            }
        }
    }

    private async void ShowLevelCleared()
    {
        PopupPanel panel = ((PopupPanel)FindNode("LevelClearedPanel"));
        Label label = (Label)panel.FindNode("Message");

        if (currentLevel < Const.MAX_LEVELS)
        {
            label.Text = string.Format("Level {0} cleared.\n\nLet's go to\nthe next level!", currentLevel);
            timer.WaitTime = 2.0f;
        }
        else
        {
            label.Text = string.Format("Congratulations!\n\nAll levels cleared\nGoing back to level 1");
            timer.WaitTime = 5.0f;
        }
        panel.ShowModal();

        timer.OneShot = true;
        var awaiter = ToSignal(timer, "timeout");
        timer.Start();
        await awaiter;
        panel.Hide();
    }
    public void _on_AcceptDialog_confirmed()
    {
        GoToNextLevel();
        // GetTree().ReloadCurrentScene();
    }

    public void GoToNextLevel()
    {
        currentLevel++;
        if (currentLevel > Const.MAX_LEVELS)
        {
            currentLevel = 1;
        }
        ResetLevel();
    }

    public void IncrementMoves() { moves++; }

    public void ResetLevel()
    {
        Level l = GetLevel(currentLevel);
        // l.Dump();
        SetLevel(l);
        moves = 0;
        ((Label)FindNode("LabelLevel")).Text = "Level: " + currentLevel.ToString();
        gameEnded = false;
    }
    private Level GetLevel(int level)
    {
        File file = new File();
        file.Open(string.Format("res://Assets/Levels/{0:000}.txt", level), File.ModeFlags.Read);
        string content = file.GetAsText();
        file.Close();

        string[] comps = content.Split("\n");
        Level l = new Level(int.Parse(comps[0]), int.Parse(comps[1]));

        int x = 0;
        int y = 0;
        for (int i = 2; i < comps.Length; i++)
        {
            x = 0;
            foreach (char c in comps[i].ToCharArray())
            {
                // Debug.WriteLine("{0}, {1}, {2}", x, y, string.Format("{0}", c));
                l.SetChip(x++, y, c);
            }
            y++;
        }
        return l;
    }

    private void SetLevel(Level l)
    {
        Hide();

        Node wallsNode = GetNode("/root/Game/Walls");
        Node boxesNode = GetNode("/root/Game/Boxes");
        Node spotsNode = GetNode("/root/Game/Spots");
        Node[] parentNodes = { boxesNode, wallsNode, spotsNode };

        foreach (Node p in parentNodes)
        {
            foreach (Node node in p.GetChildren())
            {
                p.RemoveChild(node);
            }
        }

        var boxScene = (PackedScene)GD.Load("res://Scenes/Box.tscn");
        var wallScene = (PackedScene)GD.Load("res://Scenes/Wall.tscn");
        var spotScene = (PackedScene)GD.Load("res://Scenes/Spot.tscn");
        Node2D n;

        for (int y = 0; y < l.Height; y++)
        {
            for (int x = 0; x < l.Width; x++)
            {
                if (l.Chip(x, y) == '_') continue;
                switch (l.Chip(x, y))
                {
                    case 'B':
                        n = (Node2D)boxScene.Instance();
                        n.Position = new Vector2(Const.X_OFFSET + x * Const.GRID_SIZE, Const.Y_OFFSET + y * Const.GRID_SIZE);
                        n.AddToGroup(Const.BoxGroup);
                        boxesNode.AddChild(n);
                        break;
                    case 'W':
                        n = (Node2D)wallScene.Instance();
                        n.Position = new Vector2(Const.X_OFFSET + x * Const.GRID_SIZE, Const.Y_OFFSET + y * Const.GRID_SIZE);
                        wallsNode.AddChild(n);
                        break;
                    case 'S':
                        n = (Node2D)spotScene.Instance();
                        n.Position = new Vector2(Const.X_OFFSET + x * Const.GRID_SIZE, Const.Y_OFFSET + y * Const.GRID_SIZE);
                        spotsNode.AddChild(n);
                        break;
                    case 'P':
                        n = (Node2D)FindNode("Player");
                        n.Position = new Vector2(Const.X_OFFSET + x * Const.GRID_SIZE, Const.Y_OFFSET + y * Const.GRID_SIZE);
                        break;
                }
            }
        }

        Show();
    }
}