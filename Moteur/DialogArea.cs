using Raylib_cs;
using Color = Raylib_cs.Color;
using Font = System.Drawing.Font;

namespace Moteur;

public class DialogArea
{
    private Raylib_cs.Image basImage;
    private Texture2D resizedImage;
    private Font font;
    private String toSay;
    private int sayed =0 ;
    private int CamerHeight;
    private bool finish = false;

    public bool Finish
    {
        get => finish;
        set => finish = value;
    }

    public string ToSay
    {
        get => toSay;
        set { toSay = value;
            finish = false;
        }
    }

    public DialogArea(int Width,int Height)
    {
        basImage = Raylib.LoadImage(Program.RootDirectory + "Assets/Textures/DialogArea.png");
        Raylib.ImageResize(ref basImage ,Width, Height / 4);
        resizedImage = Raylib.LoadTextureFromImage(basImage);
       
        font = new Font("Runescape UF", resizedImage.height / 10);
        CamerHeight = Height - resizedImage.height;
    }



    public bool ShowAndDraw( )
    {
        
        Raylib.DrawTexture(resizedImage,0,CamerHeight , Color.WHITE);
        if (finish)
        {
            Raylib.DrawText(toSay, (int)font.Size,(int)(CamerHeight + font.Size),(int)font.Size,Color.BLACK);
            return true;
        }
        var cut = "";
        sayed++;
        
        for (int i = 0; i < sayed; i++)
            cut += toSay[i];
        //Raylib.DrawText( toSay , font ,Brushes.Black, font.Size,CamerHeight + font.Size);
        Raylib.DrawText(cut, (int)font.Size,(int)(CamerHeight + font.Size),(int)font.Size,Color.BLACK);
       finish =  sayed == toSay.Length;
       return finish;
    }

    public void Reset()
    {
        sayed = 0;
        finish = false;
    }
}