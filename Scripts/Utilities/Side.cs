
public enum Side
{
    Left, 
    Right, 
    Forward, 
    Backward
}

public static class SideExtensions
{
    public static Side Invert(this Side side)
    {
        switch (side)
        {
            case Side.Left:
                return Side.Right;
            case Side.Right:
                return Side.Left;
            case Side.Forward:
                return Side.Backward;
            default:
                return Side.Forward;
        }
    }
}
