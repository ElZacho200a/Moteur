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
       
        var filename = @"C:\Users\zache\source\repos\Moteur\Moteur\Assets\BlocsImage\";
        var allBlockFile = Directory.EnumerateFiles(filename);
        foreach (var file in allBlockFile)
        {
            var name = Path.GetFileName(file);
            var chrominance = name.Split(".p");
            chrominance = chrominance[0].Split(",");
            var chrominanceBY = new []{byte.Parse(chrominance[0]),byte.Parse(chrominance[1]),byte.Parse(chrominance[2])};
            Bitmap img = new Bitmap(file);
            img = new Bitmap(img, new Size(blocH, blocH));
            ColorIndex.Add( Color.FromArgb(chrominanceBY[0],chrominanceBY[1],chrominanceBY[2]) ,img );
        }
    }

    public Bitmap getImageByColor(Color color)
    {
        if (color == Color.White)
            return null;
        try
        {
            var img = ColorIndex[color];
            return img;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}