using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
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

    public override void _UnhandledInput(InputEvent @event)
    {
        foreach (var dir in inputs.Keys)
        {
            if (@event.IsActionPressed(dir))
            {
                Move(dir);
            }
        }
        if (@event.IsActionPressed("reset"))
        {
            // GetParent().GetTree().ReloadCurrentScene();
            ((Game)GetParent()).ResetLevel();
        }
        else if (@event.IsActionPressed("title"))
        {
            GetTree().ChangeScene("res://Scenes/Title.tscn");
        }
    }

    private void Move(string dir)
    {
        Game game = (Game)GetParent();

        var vectorPos = inputs[dir] * Const.GRID_SIZE;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (!ray.IsColliding())
        {

            this.Position += vectorPos;
            game.IncrementMoves();
        }
        else
        {
            var collider = ray.GetCollider();
            if (((Node)collider).IsInGroup(Const.BoxGroup))
            // if (collider is Box)
            {
                if (((Box)collider).Move(dir) == true)
                {
                    this.Position += vectorPos;
                    game.IncrementMoves();
                }
            }
        }
    }
}
