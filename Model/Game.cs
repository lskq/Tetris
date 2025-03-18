namespace Tetris.Model;

public class Game
{
    public const int Width = 10;
    public const int Height = 20;
    public const int XInit = Width / 2;
    public const int YInit = 2;
    public const double GravityConstant = (double)Height / 50;

    public const int TickRate = 50;

    public Random Random { get; }

    public int Score { get; set; }
    public bool GameOver { get; set; }
    public bool Switching { get; set; }
    public Mino[][] Grid { get; set; }
    public Tetromino CurrentTetromino { get; set; }
    public Tetromino NextTetromino { get; set; }

    public Game()
    {
        Random = new();

        Score = 0;
        GameOver = false;
        Switching = false;

        Grid = new Mino[Height][];
        for (int i = 0; i < Height; i++)
        {
            Grid[i] = new Mino[Width];
        }

        CurrentTetromino = GetRandomTetromino();
        NextTetromino = GetRandomTetromino();

        UpdateGrid(true);
    }

    public void Step(int xInput, int yInput, int rotation)
    {
        if (GameOver)
            return;

        if (Switching)
            Switching = false;

        UpdateGrid(false);

        if (rotation != 0)
        {
            bool right = rotation > 0;

            CurrentTetromino.Rotate(right);
            if (IsColliding())
            {
                CurrentTetromino.Rotate(!right);
            }
        }

        if (xInput != 0)
        {
            CurrentTetromino.XAbsolute += xInput;
            if (IsColliding())
                CurrentTetromino.XAbsolute -= xInput;
        }

        double yMovement = GravityConstant + yInput * GravityConstant * 4;
        if (yMovement != 0)
        {
            CurrentTetromino.YAbsolute += yMovement;
            if (IsColliding())
            {
                do
                {
                    CurrentTetromino.YAbsolute -= 1;
                } while (IsColliding());

                UpdateGrid(true);
                NewTetromino();
                Switching = true;
            }
        }

        CheckScore();

        UpdateGrid(true);
    }

    public void NewTetromino()
    {
        CurrentTetromino = NextTetromino;

        if (IsColliding())
        {
            GameOver = true;
            return;
        }

        NextTetromino = GetRandomTetromino();
    }

    public void CheckScore()
    {
        int numScores = 0;

        for (int y = 0; y < Height; y++)
        {
            bool scored = true;

            for (int x = 0; x < Width; x++)
            {
                Mino mino = Grid[y][x];
                if (mino == null || CurrentTetromino.Minoes.Contains(mino))
                {
                    scored = false;
                    break;
                }
            }

            if (scored)
            {
                for (int i = y; i > 0; i--)
                {
                    Grid[i] = Grid[i - 1];
                }
                Grid[0] = new Mino[Width];

                numScores++;
            }
        }

        Score += (numScores > 0) ? (int)Math.Pow(2, numScores - 1) * 100 : 0;
    }

    public bool IsColliding()
    {
        foreach (Mino mino in CurrentTetromino.Minoes)
        {
            // Wall collision
            int x = (int)CurrentTetromino.XAbsolute + mino.XRelative;
            if (x < 0 || x >= Width)
                return true;

            // Floor/Ceiling collision
            int y = (int)CurrentTetromino.YAbsolute + mino.YRelative;
            if (y < 0 || y >= Height)
                return true;

            // Mino collision
            if (Grid[y][x] != null)
                return true;
        }

        return false;
    }

    public void UpdateGrid(bool draw)
    {
        foreach (Mino mino in CurrentTetromino.Minoes)
        {
            int x = (int)CurrentTetromino.XAbsolute + mino.XRelative;
            int y = (int)CurrentTetromino.YAbsolute + mino.YRelative;

            if (draw)
            {
                Grid[y][x] = mino;
            }
            else
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                Grid[y][x] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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