using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Moteur;

public class Palette
{
    protected Dictionary<Color, Bitmap> ColorIndex;
    private string filename;
    private int blocH;

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
        color = Color.FromArgb(color.R, color.G, 0);
        if(ColorIndex.Keys.Contains(color))
            return;
        string file = $"{color.R},{color.G},0.png";
       
        try
        {
            Bitmap img = new Bitmap(filename +file);
            img = new Bitmap(img, new Size(blocH  + blocH/50, blocH + blocH / 50));
            ColorIndex.Add(color,img);
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw ;
        }
       
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
        n = n % 4;
        if (n == 0)
            return img;
        Bitmap ne = img;
        for (int i = 0; i < n; i++)
        {
            ne = turn(ne);
        }
        return ne;
    }

}
