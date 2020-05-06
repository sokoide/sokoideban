using System.Collections.Generic;
using Godot;
public class UndoBuffer
{
    private const int MAX_BUFFER = 128;

    public enum UndoBufferItemType
    {
        UNDO_PLAYER = 1,
        UNDO_BOX
    }
    public class UndoBufferItem
    {
        public UndoBufferItemType t;
        public Vector2 dir;
        public Box r;

        public UndoBufferItem(UndoBufferItemType t, Vector2 dir, Box box = null)
        {
            this.t = t; this.dir = dir; this.r = box;
        }
    }

    private List<List<UndoBufferItem>> buffer = new List<List<UndoBufferItem>>();

    public void Push(List<UndoBufferItem> items)
    {
        buffer.Add(items);
        while (buffer.Count > MAX_BUFFER)
        {
            buffer.RemoveAt(0);
        }
    }

    public List<UndoBufferItem> Pop()
    {
        if (buffer.Count == 0) return new List<UndoBufferItem>();
        else
        {
            int idx = buffer.Count - 1;
            List<UndoBufferItem> items = buffer[idx];
            buffer.RemoveAt(idx);
            return items;
        }
    }

    public int Count { get { return buffer.Count; } }
}
