using Godot;
using System;
using System.Diagnostics;
using System.Text;

public class Level
{
    public int Width { get; set; }
    public int Height { get; set; }

    private char[,] chip;

    public Level(int w, int h)
    {
        Width = w;
        Height = h;
        chip = new char[Height, Width];
    }

    public char Chip(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return ' ';
        return chip[y, x];
    }

    public void SetChip(int x, int y, char c)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;
        chip[y, x] = c;
    }

    public void Dump()
    {
        Debug.WriteLine("W:{0}, H:{1}", Width, Height);
        for (int y = 0; y < Height; y++)
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < Width; x++)
            {
                sb.AppendFormat("{0}", Chip(x, y));
            }
            Debug.WriteLine(sb.ToString());
        }
    }
}
