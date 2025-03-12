namespace Tetris.Model;

public class Mino(int x, int y, Color color)
{
    public int XRelative { get; set; } = x;
    public int YRelative { get; set; } = y;

    public Color Color { get; } = color;
}