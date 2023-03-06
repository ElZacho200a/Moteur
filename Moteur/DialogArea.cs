namespace Moteur;

public class DialogArea
{
    private Bitmap basImage, resizedImage;
    private Font font;
    private String toSay;
    private int sayed =0 ;
    private int CamerHeight;
    public string ToSay
    {
        get => toSay;
        set => toSay = value;
    }

    public DialogArea(int Width,int Height)
    {
        basImage = new Bitmap(Form1.RootDirectory + "Assets/Textures/DialogArea.png");
        resizedImage = new Bitmap(basImage ,Width, Height / 4);
       
        font = new Font("Runescape UF", resizedImage.Height / 10);
        CamerHeight = Height - resizedImage.Height;
    }



    public bool ShowAndDraw(Graphics g )
    {
        var cut = "";
        sayed++;
        
        for (int i = 0; i < sayed; i++)
            cut += toSay[i];
        g.DrawImage(resizedImage,0,CamerHeight);
        g.DrawString( cut , font ,Brushes.Black, font.Size,CamerHeight + font.Size);
        return sayed == toSay.Length;
    }

    public void Reset()
    {
        sayed = 0;
    }
}