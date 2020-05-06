using Godot;
using System;
using System.Collections.Generic;

public class Box : KinematicBody2D
{
    private RayCast2D ray;
    private Sprite onSprite;


    // private Dictionary<string, Vector2> inputs = new Dictionary<string, Vector2>(){
    //     {"ui_up", Vector2.Up},
    //     {"ui_down", Vector2.Down},
    //     {"ui_left", Vector2.Left},
    //     {"ui_right", Vector2.Right},
    // };

    public override void _Ready()
    {
        ray = (RayCast2D)FindNode("RayCast2D");
        onSprite = (Sprite)FindNode("On");
    }
    // public bool Move(string dir)
    // {
    //     var vectorPos = inputs[dir] * Const.GRID_SIZE;
    //     ray.CastTo = vectorPos;
    //     ray.ForceRaycastUpdate();
    //     if (!ray.IsColliding())
    //     {
    //         this.Position += vectorPos;
    //         int x = ((int)this.Position.x - Const.X_OFFSET) / Const.GRID_SIZE;
    //         int y = ((int)this.Position.y - Const.Y_OFFSET) / Const.GRID_SIZE;
    //         char t = Global.CurrentLevelMap.Chip(x, y);

    //         if (t == 'S' || t == '.') On(true);
    //         else On(false);
    //         return true;
    //     }
    //     return false;
    // }

    public bool Move(Vector2 unitvec, bool forceMove = false)
    {
        var vectorPos = unitvec * Const.GRID_SIZE;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (forceMove || !ray.IsColliding())
        {
            this.Position += vectorPos;
            int x = ((int)this.Position.x - Const.X_OFFSET) / Const.GRID_SIZE;
            int y = ((int)this.Position.y - Const.Y_OFFSET) / Const.GRID_SIZE;
            char t = Global.CurrentLevelMap.Chip(x, y);

            if (t == 'S' || t == '.') On(true);
            else On(false);
            return true;
        }
        return false;
    }

    public void On(bool on)
    {
        if (on)
        {
            onSprite.Show();
        }
        else
        {
            onSprite.Hide();
        }
    }
}
