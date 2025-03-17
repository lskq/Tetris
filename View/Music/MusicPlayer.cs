namespace Tetris.View.Music;

public static class MusicPlayer
{
    public static async Task Play(Melody melody, CancellationToken token)
    {
        var tune = melody.Tune;
        int mpsb = melody.Mpsb;

        int i = 0;
        int len = melody.Tune.Length;

        while (!token.IsCancellationRequested)
        {
            var tone = tune[i];

            int freq = (int)tone.Item1;
            freq = freq >= 37 ? freq : 0;
            int time = (int)((int)tone.Item2 * mpsb * 1.0);

            if (freq != 0)
            {
                Console.Beep(freq, time);
            }
            else
                await Task.Delay(time);

            if (i + 1 < len)
                i++;
            else
                i = 0;
        }
    }
}