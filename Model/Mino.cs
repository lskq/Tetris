namespace Tetris.Model;

public class Mino(int x, int y, MinoColor minoColor)
{
    public int XRelative { get; set; } = x;
    public int YRelative { get; set; } = y;

    public MinoColor MinoColor { get; } = minoColor;
}