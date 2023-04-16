using NAudio.Wave;
namespace Moteur;

public static class Music
{
    private static string path = Directory.GetCurrentDirectory().Split("bin")[0] + @"Assets\Sounds\";
    
    private static void garbage(Mp3FileReader reader, WaveOut waveout)
    {
        waveout.Dispose();
        reader.Dispose();
    }

    private static void playLoop(WaveOut waveOut, Mp3FileReader reader)
    {
        waveOut.PlaybackStopped += (sender, eventArgs) =>
        {
            waveOut.Play();
        };
        waveOut.Play();
    }
}