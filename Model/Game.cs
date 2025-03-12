namespace Tetris.Model;

public class Game
{
    public const int Width = 10;
    public const int Height = 10;
    public const int XInit = Width / 2;
    public const int YInit = Height * 1 / 4;
    public const int Gravity = 0;

    public Random Random { get; }

    public Mino[,] Grid { get; set; }
    public Tetromino Tetromino { get; set; }

    public Game()
    {
        Random = new();

        Grid = new Mino[Width, Height];
        Tetromino = GetRandomTetromino();

        UpdateGrid(true);
    }

    public void Step(int xInput, int yInput, int rotation)
    {

        UpdateGrid(false);

        if (xInput != 0)
        {
            Tetromino.XAbsolute += xInput;
            if (IsColliding())
                Tetromino.XAbsolute -= xInput;
        }
        if (yInput != 0)
        {
            Tetromino.YAbsolute += yInput;
            if (IsColliding())
                Tetromino.YAbsolute -= yInput;
        }
        if (rotation != 0)
        {
            bool right = rotation > 0;

            Tetromino.Rotate(right);
            if (IsColliding())
            {
                Tetromino.Rotate(!right);
            }
        }

        UpdateGrid(true);
    }

    public bool IsColliding()
    {
        foreach (Mino mino in Tetromino.Minoes)
        {
            int x = Tetromino.XAbsolute + mino.XRelative;
            if (x < 0)
                return true;
            else if (x >= Width)
                return true;

            int y = Tetromino.YAbsolute + mino.YRelative;
            if (y >= Height)
                return true;
        }

        return false;
    }

    public void UpdateGrid(bool draw)
    {
        foreach (Mino mino in Tetromino.Minoes)
        {
            int x = Tetromino.XAbsolute + mino.XRelative;
            int y = Tetromino.YAbsolute + mino.YRelative;

            if (draw)
            {
                Grid[x, y] = mino;
            }
            else
            {
                Grid[x, y] = null;
            }
        }
    }

    public Tetromino GetRandomTetromino()
    {
        int numShapes = Enum.GetNames(typeof(Shape)).Length;

        int rand = Random.Next(numShapes);

#pragma warning disable CS8605 // Unboxing a possibly null value.
        Shape shape = (Shape)Enum.GetValues(typeof(Shape)).GetValue(rand);
#pragma warning restore CS8605 // Unboxing a possibly null value.

        return new Tetromino(XInit, YInit, shape);
    }
}