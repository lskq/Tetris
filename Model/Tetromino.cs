namespace Tetris.Model;

public class Tetromino
{
    public int RootX { get; set; }
    public int RootY { get; set; }
    public Color Color { get; set; }
    public (int, int)[] Blocks { get; set; }

    public Tetromino(int x, int y, Shape shape)
    {
        RootX = x;
        RootY = y;

        Blocks = new (int, int)[4];

        switch (shape)
        {
            case Shape.I:
                Color = Color.Cyan;
                for (int i = 0; i < 4; i++)
                {
                    Blocks[i] = (RootX - 2 + i, RootY);
                }
                break;
        }
    }

}