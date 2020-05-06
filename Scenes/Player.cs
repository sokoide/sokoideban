using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        Game game = (Game)GetParent();
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
        else if (@event.IsActionPressed("undo"))
        {
            List<UndoBuffer.UndoBufferItem> items = Global.UndoBuffer.Pop();
            if (items.Count == 0) return;

            // undo
            foreach (UndoBuffer.UndoBufferItem item in items)
            {
                Debug.WriteLine("Undo: {0}, {1}, {2}", item.t, item.dir, item.r);
                if (item.t == UndoBuffer.UndoBufferItemType.UNDO_PLAYER)
                {
                    // UNDO_PLAYER
                    this.Position -= item.dir * Const.GRID_SIZE;
                }
                else
                {
                    // UNDO_BOX
                    item.r.Position -= item.dir * Const.GRID_SIZE;
                }
            }
            // decrement
            game.DecrementMoves();
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
            // add into undo buffer
            Global.UndoBuffer.Push(new List<UndoBuffer.UndoBufferItem>(){
                new UndoBuffer.UndoBufferItem(UndoBuffer.UndoBufferItemType.UNDO_PLAYER, inputs[dir]),
            });
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
                    // add into undo buffer
                    Global.UndoBuffer.Push(new List<UndoBuffer.UndoBufferItem>(){
                            new UndoBuffer.UndoBufferItem(UndoBuffer.UndoBufferItemType.UNDO_PLAYER, inputs[dir]),
                            new UndoBuffer.UndoBufferItem(UndoBuffer.UndoBufferItemType.UNDO_BOX, inputs[dir], (Box)collider),
                    });
                }
            }
        }
    }
}
