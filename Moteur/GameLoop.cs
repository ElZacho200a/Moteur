namespace Moteur;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
public static class GameLoop
{

    public static int Widht, Heigt;
    public static Camera[] Cameras;
    public static void start(uint nbPlayer)
    {
        
        if (nbPlayer == 0)
            throw new ArgumentException("Il ne peut y avoir 0 joueurs");
        init();
         Cameras = new Camera[nbPlayer];
        // Setup des Caméras
        for (int i = 0; i < nbPlayer; i++)
        {
            Cameras[i] = new Camera((int)(Widht / nbPlayer) , Heigt , i);
        }
        
        while (!Raylib.WindowShouldClose())
        {
          Loop();
        }

    }

    private static void Loop()
    {
       BeginDrawing();
        for (int i = 0; i < Cameras.Length; i++)
        {
            var camera = Cameras[i];
           camera.rayDraw(i);
        }
       // DrawRectangle(Widht/2  -50, 0 ,50, Heigt, Raylib_cs.Color.DARKBROWN);
        EndDrawing();
        for (int i = 0; i < Cameras.Length; i++)
        {
            var camera = Cameras[i];
            camera.rayControl(i);
            camera.Update(i);
        }

        if (Level.currentLevel.Update())
        {

            for (int i = 0; i < Cameras.Length; i++)
            {
                var camera = Cameras[i];
                camera.player.Update();
                // ajustement de la cam 
                camera.UpdateScope();
            }
        }
    }
    

    public static void init()
    {
        InitWindow(0,0 , "Test de la manette");
       // ToggleFullscreen();
        Widht = Raylib.GetMonitorWidth(0);
        Heigt = Raylib.GetMonitorHeight(0);
        //back = new Bitmap(Widht, Height);
        SetTargetFPS(60);
    }
    
    
}