using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NAudio.CoreAudioApi;

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
    private static Dictionary<string, SpriteManager> Managers;
    private delegate void Loop();

    public static string Role = "Local";
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

        Loop loop = LocalLoop;
        switch (Role)
        {
            case("Host"):
                loop = HostLoop;
                HostInit();
               break;
            case("Client"):
                Level.EntityEnable = false;
                loop = ClientLoop;
                ClientInit();
                break;
        }
       
        while (!Raylib.WindowShouldClose())
        {
            loop();
        }

    }

    private static void LocalLoop()
    {
      
       
       drawCamera();
       for (int i = 0; i < Cameras.Length; i++)
        {
            var camera = Cameras[i];
            camera.rayControl(i);
            camera.Update(i);
        }

      UpdateScope();
    }

    private static void drawCamera()
    {
        BeginDrawing();
        for (int i = 0; i < Cameras.Length; i++)
        {
            var camera = Cameras[i];
            if( i != 0)
                Raylib.DrawRectangle(Widht / 2 - 25 , 0 , 50 , Heigt , Raylib_cs.Color.GOLD);
           
            camera.rayDraw(i);
        }
        EndDrawing();
    }
    
    private static void drawCamera(Dictionary<string, List<string>>Entities)
    {
        BeginDrawing();
        for (int i = 0; i < Cameras.Length; i++)
        {
            var camera = Cameras[i];
            if( i != 0)
                Raylib.DrawRectangle(Widht / 2 - 25 , 0 , 50 , Heigt , Raylib_cs.Color.GOLD);
           
            camera.rayDraw(i , Entities , Managers);
        }
        EndDrawing();
    }
    private static void UpdateScope()
    {
        if (Level.currentLevel.Update())
        {

            for (int i = 0; i < Cameras.Length; i++)
            {
                var camera = Cameras[i];
                if(camera.gameState == 0)
                    camera.player.Update();
                // ajustement de la cam 
                camera.UpdateScope();
            }
        }
    }

    public static void init()
    {
        InitWindow(0,0 , "G.O.A.T");
        //ToggleFullscreen();
        Widht = Raylib.GetMonitorWidth(0);
        Heigt = Raylib.GetMonitorHeight(0);
        //back = new Bitmap(Widht, Height);
        SetTargetFPS(60);
    }


    public static void ClientInit()
    {

        Managers = new Dictionary<string, SpriteManager>();
    }
    public static void ClientLoop()
    {
        Cameras[0].Update(0);
        Cameras[0].player.Update();

        var level = OnlinePass.Ask("Level :" + Level.currentLevel.ID );
        if (level != "Same")
        {
            Level.currentLevel.destroy();
            Level.currentLevel = new Level(Convert.ToInt32(level) );
        }

        var Sentities = OnlinePass.Ask("Entities");
        var Entities = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(Sentities);
       drawCamera(Entities);

    }

    public static void HostLoop()
    {

        var dictEnt = new Dictionary<string, List<string>>();
        foreach (var entity in Level.currentLevel.GetEntities().Where(ent => ent is ActiveEntity).Select(ent => ent as ActiveEntity))
        {
            var att = entity.GetSpriteManagerAtt();
            if(att == "")
                continue;
            var currentSprite = entity.getCurrentSprite();
            
            if (!dictEnt.ContainsKey(att))
                dictEnt.Add(att , new List<string>());
            dictEnt[att].Add(entity[0] + "x" + entity[1]+"x"+entity.getCurrentSprite());
        }


        var f = JsonSerializer.Serialize(dictEnt);
        OnlinePass.Ask(f);
        LocalLoop();
        

    }

    public static  void HostInit()
    {
        
    }
    
}