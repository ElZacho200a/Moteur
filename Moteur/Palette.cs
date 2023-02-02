using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Moteur;

public class Palette
{
    protected Dictionary<Color, Bitmap> ColorIndex;

    public Palette(int blocH)
    {
        ColorIndex = new Dictionary<Color, Bitmap>();
       
        var filename = Form1.RootDirectory + @"Assets\BlocsImage\";
        var allBlockFile = Directory.EnumerateFiles(filename);
        foreach (var file in allBlockFile)
        {
            var name = Path.GetFileName(file);
            var chrominance = name.Split(".p");
            chrominance = chrominance[0].Split(",");
            var chrominanceBY = new []{byte.Parse(chrominance[0]),byte.Parse(chrominance[1]),byte.Parse(chrominance[2])};
            Bitmap img = new Bitmap(file);
            img = new Bitmap(img, new Size(blocH  + blocH/50, blocH + blocH / 50));
            ColorIndex.Add( Color.FromArgb(chrominanceBY[0],chrominanceBY[1],chrominanceBY[2]) ,img );
        }
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
                return turnMultipleTime( ColorIndex[color] , blue);
                
                    
                
            

            
        }
        catch (Exception e)
        {
            return null;
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
