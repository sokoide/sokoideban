using Godot;
using System;

public class Spot : Area2D
{
    public bool Occupied
    {
        get { return occupied; }
    }
    private bool occupied = false;

    public override void _Ready()
    {
        occupied = false;
    }

    public void _on_Spot_body_entered(Node body)
    {
        if (body.IsInGroup(Const.BoxGroup))
        {
            occupied = true;
            ((Box)body).On(true);
        }
    }

    public void _on_Spot_body_exited(Node body)
    {
        if (body.IsInGroup(Const.BoxGroup))
        {
            occupied = false;
            ((Box)body).On(false);
        }
    }
}
