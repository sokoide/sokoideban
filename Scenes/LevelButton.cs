using Godot;
using System;
using System.Diagnostics;

public class LevelButton : Button
{
    public int ButtonID { get; set; }
    public void _on_LevelButton_button_up()
    {
        Debug.WriteLine("Button{0} pressed", ButtonID);
        Global.CurrentLevel = ButtonID;
        GetTree().ChangeScene("res://Scenes/Game.tscn");
    }
}
