using Godot;
using System;
using System.Collections.Generic;

public class Box : KinematicBody2D
{
    private RayCast2D ray;
    private Dictionary<string, Vector2> inputs = new Dictionary<string, Vector2>(){
        {"ui_up", Vector2.Up},
        {"ui_down", Vector2.Down},
        {"ui_left", Vector2.Left},
        {"ui_right", Vector2.Right},
    };

    public override void _Ready()
    {
        ray = (RayCast2D)FindNode("RayCast2D");
    }
    public bool Move(string dir)
    {
        var vectorPos = inputs[dir] * Const.GRID_SIZE;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (!ray.IsColliding())
        {
            this.Position += vectorPos;
            return true;
        }
        return false;
    }
}
