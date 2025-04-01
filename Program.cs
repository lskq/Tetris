using Tetris.Controller;

namespace Tetris;

class Program
{

    [STAThread]
    static void Main(string[] args)
    {
        ConsoleController controller = new();
        controller.Start();
    }
}
