using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Moteur.OnlineClass.Serveur;
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
        var save = LoadSave();
        
        
        init();
         Cameras = new Camera[nbPlayer];
        // Setup des Caméras
        for (int i = 0; i < nbPlayer; i++)
        {
            Cameras[i] = new Camera((int)(Widht / nbPlayer) , Heigt , i ,save.Item2, save.level);
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
        SaveGame();
    }

    public  struct gameSave
    {
        public int level;
        public (int x , int y ) playerPos;
    }
    public static void SaveGame()
    {
        
        var save = new gameSave() { level = Level.currentLevel.ID, playerPos = Level.Players[0].Coordonates };
        var serializer = new XmlSerializer(typeof(gameSave));
        try
        {
           // if(!File.Exists(Program.RootDirectory + $"Save/PlayerSave.xml"))
                
            using (StreamWriter writer = new StreamWriter(Program.RootDirectory + $"Save/PlayerSave.xml"))
                serializer.Serialize(writer, save);
        }
        catch (Exception e)
        {
            
        }
    }

    public static (int level, (int x , int y)) LoadSave()
    {
        var filename = Program.RootDirectory + $"Save/PlayerSave.xml";
        if (!File.Exists(filename))
            return (0, (200, 200));
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        var serializer = new XmlSerializer(typeof(gameSave));
        var saveGame = xmlDocument.SelectSingleNode("gameSave");
        var level = Convert.ToInt32(saveGame.SelectSingleNode("level").InnerText);
        var pos = saveGame.SelectSingleNode("playerPos");
        var x = Convert.ToInt32(pos.SelectSingleNode("Item1").InnerText);
        var y = Convert.ToInt32(pos.SelectSingleNode("Item2").InnerText);
        
        return (level, (x, y));

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
       
        for (int i = 0; i < Cameras.Length; i++)
        {
            BeginDrawing();
            var camera = Cameras[i];
            if( i != 0)
                Raylib.DrawRectangle(Widht / 2 - 25 , 0 , 50 , Heigt , Raylib_cs.Color.GOLD);
           
            camera.rayDraw(i , Entities , Managers);
           
            EndDrawing();
        }
        
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
        OnlinePass.RoomCode = "";
        OnlinePass.start();
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

        var Sentities = OnlinePass.Ask(Level.Players[0].getForOnlineData());
        var Entities = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(Sentities);
       drawCamera(Entities);

    }

    public static void HostLoop()
    {

        var dictEnt = new Dictionary<string, List<string>>();
        var d = Level.currentLevel.GetEntities().
                Where(ent => ent is ActiveEntity).
                Select(ent => ent as ActiveEntity)
                .ToList();
        foreach (var player in Level.Players)
        {
            d.Add(player as ActiveEntity);
        }
        
        foreach (var entity in d)
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
        
        var players  = OnlinePass.Ask(f);
        try
        {
            List<string> onPlayers = JsonSerializer.Deserialize<List<string>>(players);
            for (int i = 0; i < onPlayers.Count; i++)
            {
                if (i > OnlinePass.OnlinePlayers.Count)
                    OnlinePass.OnlinePlayers.Add(new OnlinePlayer(null , i));
                OnlinePass.OnlinePlayers[i].setState(onPlayers[i]);
            }
            for(int i = 0 ; i < OnlinePass.OnlinePlayers.Count - onPlayers.Count; i++)
                OnlinePass.OnlinePlayers.RemoveAt(OnlinePass.OnlinePlayers.Count -1);
        }
        catch (Exception e)
        {
           
        }
        
        

        LocalLoop();
        

    }

    public static  void HostInit()
    {
        
        OnlinePass.start();
        
    }
    
}