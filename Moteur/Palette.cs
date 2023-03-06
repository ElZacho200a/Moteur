namespace Moteur;

public class Palette
{
    protected Dictionary<Color, Bitmap> ColorIndex;
    private string filename;
    private int blocH;
    private static Bitmap Alphabet = new Bitmap(Form1.RootDirectory +"Assets/Textures/Alphabet.png");
    
    public Palette(int blocH)
    {
        
        this.blocH = blocH;
        ColorIndex = new Dictionary<Color, Bitmap>();
        filename = Form1.RootDirectory + @"Assets\BlocsImage\";
    }

    public Bitmap getImageByColor(Color color)
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
            Bitmap img = new Bitmap(filename +file);
            img = new Bitmap(img, new Size(blocH  + Level.blocH / 50  , blocH+ Level.blocH / 50  ));
            ColorIndex.Add(color,img);
        }
        catch (Exception e)
        {
        }
       
    }

    public Color simplify(Color color) => Color.FromArgb(color.A, color.R, color.G, 0);
    public bool isOpaque(Bitmap img)
    {
        if (img == null)
            return true;
        for (int i = 0; i < img.Width; i+=3 )
            for (int j = 0; j < img.Height; j+= 3)
                if (img.GetPixel(i, j).A ==0)
                    return false;
        return true;

    }
    public Bitmap turn(Bitmap img)
    {
        Bitmap ne = new Bitmap(img);
        for (int i = 0; i < img.Width; i++)
        {
            for (int j = 0; j < img.Height; j++)
            {
                ne.SetPixel(j, i, img.GetPixel(img.Width -1- i, j));
            }
        }
        return ne;
    }
    public Bitmap turnMultipleTime(Bitmap img, int n)
    {
        if (n == 0)
            return img;
        n = n % 4;
        Bitmap ne = img;
        for (int i = 0; i < n; i++)
        {
            ne = turn(ne);
        }
        return ne;
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
    
    
}
