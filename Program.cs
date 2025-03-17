using Tetris.View.Music;

namespace Tetris;

class Program
{

    [STAThread]
    static async Task Main(string[] args)
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        try
        {
            Controller controller = new(args);

            var musicTask = MusicPlayer.Play(Melody.GetTetrisA(), token);
            var controllerTask = Task.Run(() => controller.Start());

            await musicTask;
            await controllerTask;
        }
        catch (Exception e)
        {
            // For testing
            source.Cancel();
            Console.Write("\x1b[0m");
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine(e.ToString());
        }
    }
}
