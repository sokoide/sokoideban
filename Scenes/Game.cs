using Godot;
using System;

public class Game : Node2D
{
    bool gameEnded = false;
    int moves = 0;
    public override void _Process(float delta)
    {
        ((Label)FindNode("LabelMoves")).Text = "Moves: " + moves.ToString();

        if (gameEnded == false)
        {
            int spots = FindNode("Spots").GetChildCount();

            foreach (Spot spot in FindNode("Spots").GetChildren())
            {
                if (spot.Occupied) spots--;
            }
            if (spots == 0)
            {
                ((AcceptDialog)FindNode("AcceptDialog")).Popup_();
                gameEnded = true;
            }
        }
    }

    public void _on_AcceptDialog_confirmed()
    {
        GetTree().ReloadCurrentScene();
    }

    public void IncrementMoves() { moves++; }
}
