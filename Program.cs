namespace Tetris;

class Program
{

    [STAThread]
    static void Main(string[] args)
    {
        Controller controller = new();
        controller.Start();
    }
}
