﻿using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Moteur;

public class WaterArea : VoidArea
{
    private Color WaterColor = new Color(0, 0, 255, 150);
    public WaterArea(int w, int h, Level level) : base(w, h, level)
    {

      

      
    }

    protected override void setupAnim()
    {
        return;
    }

    public void draw()
    {

        for (int i = 0; i < this.DangerousMatrice.GetLength(0); i++)
        {
            for (int j = 0; j < DangerousMatrice.GetLength(1); j++)
            {
                if(DangerousMatrice[i,j])
                    Raylib.DrawRectangle(i * Level.blocH , j * Level.blocH ,Level.blocH ,Level.blocH , WaterColor);
            }
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        //Raylib.UnloadTexture(WaterSprite);
    }
    
    public  bool this[int i , int j ]
    {
        get
        {
            
            return i < DangerousMatrice.GetLength(0) && j < DangerousMatrice.GetLength(1) && DangerousMatrice[i, j];
        }
        set
        {
            if (!value)
                return;
            DangerousMatrice[i, j] = value;

        }
    }
}