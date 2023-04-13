using Raylib_cs;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Image = Raylib_cs.Image;
using Rectangle = System.Drawing.Rectangle;

namespace Moteur;

public class Palette
{
    protected Dictionary<Color, Texture2D> ColorIndex;
    private string filename;
    private int blocH;
    private static Bitmap Alphabet = new Bitmap(Program.RootDirectory +"Assets/Textures/Alphabet.png");
    
    public Palette(int blocH)
    {
        
        this.blocH = blocH;
        ColorIndex = new Dictionary<Color, Texture2D>();
        filename = Program.RootDirectory + @"Assets\BlocsImage\";
    }

    public Texture2D? getImageByColor(Color color)
    {
        if (color == Color.White)
            return null;
        try
        {
            Bitmap img;
            int blue = color.B;
                color = Color.FromArgb(color.R, color.G, 0);
                if (color.R <= 2)
                    return turnMultipleTime(ColorIndex[color], blue);
                else
                    return ColorIndex[color];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void loadBloc(Color color)
    {
        if (color.R == 255)
            return;
        color = Color.FromArgb(color.R, color.G, 0);
        if(ColorIndex.Keys.Contains(color))
            return;
        string file = $"{color.R},{color.G},0.png";
        try
        {
            Image img = Raylib.LoadImage(filename +file);
            Raylib.ImageResize(ref img, blocH  + Level.blocH / 50  , blocH+ Level.blocH / 50  );
            ColorIndex.Add(color,Raylib.LoadTextureFromImage(img));
            Raylib.UnloadImage(img);
        }
        catch (Exception e)
        {
        }
       
    }

    public Color simplify(Color color) => Color.FromArgb(color.A, color.R, color.G, 0);
    public bool isOpaque(Texture2D ? texture)
    {
        if (texture is null)
            return true;
        var img = Raylib.LoadImageFromTexture((Texture2D)texture );
        if (img.width != null)
            return true;
        for (int i = 0; i < img.width; i+=3 )
            for (int j = 0; j < img.height; j+= 3)
                if (Raylib.GetImageColor(img, i, j).a <= 200)
                {
                    Raylib.UnloadImage(img);
                    return true;
                }
        Raylib.UnloadImage(img);           
        return false;

    }
   
    public Texture2D turnMultipleTime(Texture2D img, int n)
    {
        if (n == 0)
            return img;
        n = n % 4;
        Image ne = Raylib.LoadImageFromTexture(img);
        for (int i = 0; i < n; i++)
        {
            Raylib.ImageRotateCCW(ref ne);
        }
        var toRet =  Raylib.LoadTextureFromImage(ne);
        Raylib.UnloadImage(ne);
        return toRet;
    }

    public static Rectangle getRectFromBitmap(Bitmap img)
    {
        return new Rectangle(0, 0, img.Width, img.Height);
    }

    private static Bitmap getLineImage(String s ,int charSize = 10)
    {
     
        Font font = new Font("Runescape UF", charSize);
        Bitmap b = new Bitmap(charSize * s.Length,charSize);
        using (var g = Graphics.FromImage(b))
        {
            g.DrawString(s,font,Brushes.Black, 0,0);
            
        }

        return b;
    }

    public void Destroy()
    {
        foreach (var texture in ColorIndex.Values)
        {
            Raylib.UnloadTexture(texture);
        }
    }
}
