using System.Windows.Forms.VisualStyles;

namespace Moteur;
using NAudio.Wave;

public class SoundManager
{
    private string path = Directory.GetCurrentDirectory().Split("bin")[0] + @"Assets\Sounds\";

    private void garbage(Mp3FileReader reader, WaveOut waveout)
    {
        waveout.Dispose();
        reader.Dispose();
    }

    private void playUntilEnd(WaveOut waveOut, Mp3FileReader reader)
    {
        waveOut.PlaybackStopped += (sender, eventArgs) =>
        {
            waveOut.Stop();
            garbage(reader, waveOut);
        };
        waveOut.Play();
    }
    public void jumpSong()
    {
        Mp3FileReader reader =
            new Mp3FileReader(path + "Jump.mp3");
        
        WaveOut waveout = new WaveOut();
        
        waveout.Init(reader);
        
        playUntilEnd(waveout,reader);
    }
}