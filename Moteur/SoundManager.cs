using System.Windows.Forms.VisualStyles;
using NAudio.Wave;
namespace Moteur;


public class SoundManager
{
    private static string path = Directory.GetCurrentDirectory().Split("bin")[0] + @"Assets\Sounds\";
    private static string ActualMusic = "";
    private static (Mp3FileReader reader ,WaveOut waveOut)  LevelStream; 
    private static void garbage(Mp3FileReader reader, WaveOut waveout)
    {
        waveout.Dispose();
        reader.Dispose();
    }

    private  static void playUntilEnd(WaveOut waveOut, Mp3FileReader reader)
    {
        waveOut.PlaybackStopped += (sender, eventArgs) =>
        {
            waveOut.Stop();
            garbage(reader, waveOut);
        };
        waveOut.Play();
    }

    public static void MusicLevel(string SongName)
    {
        if( SongName == "")
            return;
        if (LevelStream.reader == null || SongName != ActualMusic)
        {
            ActualMusic = SongName;
            LevelStream.reader = new Mp3FileReader( path + SongName);
            if(LevelStream.waveOut == null)
                LevelStream.waveOut = new WaveOut();
            LevelStream.waveOut.Init(LevelStream.reader);
            LevelStream.waveOut.Play();
        }
    }
    public void jumpSong()
    {
        Mp3FileReader reader =
            new Mp3FileReader(path + "Jump.mp3");
        
        WaveOut waveout = new WaveOut();
        
        waveout.Init(reader);
        
        playUntilEnd(waveout,reader);
    }

    public void doorSong()
    {
        Mp3FileReader reader =
            new Mp3FileReader(path + "Door_opening.mp3");

        WaveOut waveOut = new WaveOut();
        
        waveOut.Init(reader);
        
        playUntilEnd(waveOut,reader);
    }

    public void touchElectricitySong()
    {
        Mp3FileReader reader =
            new Mp3FileReader(path + "Electricity.mp3");

        WaveOut waveOut = new WaveOut();
        
        waveOut.Init(reader);
        
        playUntilEnd(waveOut,reader);
    }
}