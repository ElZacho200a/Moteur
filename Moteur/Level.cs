using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using Moteur.Entites;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace Moteur;

public class Level
{
    public static Level? currentLevel; //  l'accès à tout niveau ( ou salle ) doit se faire à travers cette variable .
    public static int LevelLoaded;
    public  XmlSerializer serializer;
    private Raylib_cs.Image Background;
    protected Raylib_cs.Texture2D? [,] BackgroundMatrice = {{}};
    protected bool[,] CollisionMatrice;
    public bool Dark;
    private ConcurrentBag<Entity> entities = new();
    private bool fullLoaded;
    public int ID;
    public VoidArea? VoidArea;
    public WaterArea? WaterArea;
    protected Raylib_cs.Texture2D? [,]  levelMatrice;
    protected bool[,] backgroundNeedded;
    public static List<Player> Players;
    
   
    public bool[,] BackgroundNeedded
    {
        get => backgroundNeedded;
       
    }

    public Level(int id)
    {
        LevelLoaded++;

        ID = id;
        if (currentLevel == null)
            currentLevel = this;
        setupMatrice(findFilenameByID(id));


        fullLoaded = true;
    }

    public Level(int id, Palette palette)
    {
        if (currentLevel.ID == id)
            throw new Exception();

        LevelLoaded++;
        this.getPalette = palette;
        ID = id;
        if (currentLevel == null)
            currentLevel = this;
        setupMatrice(findFilenameByID(id));
    }

    public static int blocH => Camera.blocH;
    public Raylib_cs.Texture2D ?[,] BackGroundMatrice => BackgroundMatrice;
    public Palette getPalette { get; private set; }

    public (int w, int h ) GetRealSize => (levelMatrice.GetLength(0), levelMatrice.GetLength(1));

    public Raylib_cs.Image? getBackground()
    {
        return Background;
    }

    private string findFilenameByID(int id)
    {
        return Program.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.png";
    }

    private string findDataFilenamebyID(int id)
    {
        return Program.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.ROOM";
    }

    private string findXMLFilenamebyID(int id)
    {
        return Program.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.xml";
    }

    public Raylib_cs.Texture2D ?[,] getLevelMatrice()
    {
        return levelMatrice;
    }

    public bool[,] getCollisonMatrice()
    {
        return CollisionMatrice;
    }

    public ConcurrentBag<Entity> GetEntities()
    {
        return entities;
    }

    private void setupMatrice(string filename)
    {
        var rawLevel = new Bitmap(Image.FromFile(filename));
        if (serializer == null)
            serializer = new XmlSerializer(typeof(List<RawEntity>));
        levelMatrice = new Raylib_cs.Texture2D?[rawLevel.Width, rawLevel.Height];
        CollisionMatrice = new bool[rawLevel.Width, rawLevel.Height];
        backgroundNeedded = new bool[rawLevel.Width, rawLevel.Height];
        // Construction à partir du ROOM_ID.ROOM

        if (levelMatrice.GetLength(0) * blocH < Camera.Width)
            while (levelMatrice.GetLength(0) * blocH < Camera.Width)
                Camera.FOV--;
        else
            Camera.FOV = 30;
        foreach (var player in Players)
            player.ResetSprite();
        getPalette = new Palette(blocH);

        try
        {
            LoadFromXML();
        }
        catch (Exception e)
        {
          LoadFromRoomFile();
        }
      
         
        
       


        VoidArea = new VoidArea(rawLevel.Width, rawLevel.Height , this);

        //Construction à partir de l'image ROOM_ID.png
        for (var i = 0; i < rawLevel.Width; i++)
        for (var j = 0; j < rawLevel.Height; j++)
        {
            var color = rawLevel.GetPixel(i, j);
            switch (color.R)
            {
                case 1:
                case 2:
                   
                    getPalette.loadBloc(color);
                    CollisionMatrice[i, j] = color.R == 1; // setup de la Matrice de Collision
                    levelMatrice[i, j] = getPalette.getImageByColor(color); // setup des Images
                    backgroundNeedded[i, j] = getPalette.isOpaque(levelMatrice[i,j]);
                    VoidArea[i, j] = color.R == 2 && color.G == 28; // Setup DangerousArea
                    break;

                case 5:
                    if (color.G == 0)
                    {
                        entities.Add(new Sortie(color.B, i * blocH, j * blocH));
                    }
                    else
                    {
                        getPalette.loadBloc(color);
                        entities.Add(new Porte(color.B, i * blocH, j * blocH,  (Texture2D)getPalette.getImageByColor(color)));
                    }
                    break;
                case 6 :
                    if (this.WaterArea is null)
                    {
                        WaterArea = new WaterArea(rawLevel.Width, rawLevel.Height, this);
                       
                    }
                    WaterArea[i, j] = true;
                    break;
            }
            //Setup des Opti Background
        }

       
        if(haveBackground())
            BackgroundMatrice = SliceImage(Background);
    }

    private Raylib_cs.Texture2D?[,] SliceImage(Raylib_cs.Image backImage)
    {
      
        var imgMatrice = new Raylib_cs.Texture2D?[backImage.width / 50, backImage.height / 50];
        var size = new Size(blocH + Level.blocH / 50 , blocH + Level.blocH / 50 );
        var rect = new Raylib_cs.Rectangle(0, 0, 50, 50);
        var w = Math.Min(imgMatrice.GetLength(0) , levelMatrice.GetLength(0));
        var h = Math.Min(imgMatrice.GetLength(1) , levelMatrice.GetLength(1));
        for (var i = 0; i  < w ; i ++)
        {
            for (var j = 0; j  < h; j ++)
            {
                var img = Raylib.ImageCopy(backImage);
                Raylib.ImageCrop(ref img , rect);
                Raylib.ImageResize(ref img , size.Height,size.Width);
                imgMatrice[i, j] = LoadTextureFromImage(img);
                UnloadImage(img);
                rect.y += 50;
            }

            rect.y = 0;
            rect.x += 50;
        }

        return imgMatrice;
    }

    private void LoadFromXML()
    {
        var filename = findXMLFilenamebyID(ID);
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        var rawEntities = xmlDocument.GetElementsByTagName("Entity");
        if (!File.Exists(Program.RootDirectory + $"Save/Save_{ID}.xml"))
        {
            
            foreach (XmlNode rawEntity in rawEntities)
                entities.Add(getEntityFromXML(rawEntity));
        }
        else
        {
            FileStream stream = new FileStream(Program.RootDirectory + $"Save/Save_{ID}.xml", FileMode.Open);
            List<RawEntity> list = (List<RawEntity>)serializer.Deserialize(stream);
            foreach (var rawEntity in list)
                try
                {
                    entities.Add(rawEntity.Recover());
                }
                catch (Exception e)
                {
                  
                }
                   
                
             
               
               
               
        }

        var RoomSave = xmlDocument.SelectSingleNode("RoomSave");
        Dark = bool.Parse(RoomSave.SelectSingleNode("isDark").InnerText);
        var BackGroundPath = RoomSave.SelectSingleNode("BackgroundPath").InnerText;
        BackGroundPath = Program.RootDirectory + "Assets" + BackGroundPath.Split("..")[1];
        Background = LoadImage(BackGroundPath);
        try
        {
            var MusicName = RoomSave.SelectSingleNode("MusicPath").InnerText;
            SoundManager.MusicLevel(MusicName);
        }
        catch (Exception e)
        {
          
        }
       
    }

    private void LoadFromRoomFile()
    {
        try
        {
            var lines = File.ReadAllLines(findDataFilenamebyID(ID));
            lines[0] = lines[0].Split("..")[1];
            lines[0] = Program.RootDirectory + "Assets" + lines[0];
            for (var i = 1; i < lines.Length; i++) entities.Add(getEncodedEntity(lines[i]));
            Background = Raylib.LoadImage(lines[0]);
            //var size = new Size(Background.Width * rawLevel.Height * blocH / Background.Height, rawLevel.Height * blocH);
            //Background = new Bitmap(Background,size);
        }
        catch (Exception e)
        {
            //No Data Pack Found or Data Pack is corrupted
        }
    }

    public void Activate()
    {
        fullLoaded = true;
    }

    public void Deactivate()
    {
        fullLoaded = false;
    }

    public bool Update()
    {
        if (!fullLoaded)
            return false;
        if(VoidArea != null)
            VoidArea.UpdateAnimation();
       try
        {
            foreach (var entity in entities)
            {
                if (entity.IsDead)
                {
                    RemoveEntity(entity);
                    continue;
                }

                foreach (var player in Players)
                {
                    var cam = player.Camera;
                    var hit = entity.Hitbox;
                    if (cam.isInScope(hit))
                    {
                        entity.Update();
                       break;
                    }
                }
               
            }
        }
        catch (Exception e)
        {
            //throw e;
            return false;
        }

        return true;
    }

    public void addEntity(Entity entity)
    {
        entities.Add(entity);
    }
    public void RemoveEntity(Entity entity)
    {
        entity.Destroy();
        entities = new ConcurrentBag<Entity>(entities.Except(new[] { entity }));
    }

    public void destroy()
    {
        //Sauvegarde Des entités 
              SaveEntities();
        //Effacement des Object Potentiellement Persistant

        foreach (var entity in entities)
        {
            RemoveEntity(entity);
        }
        
       UnloadImage(Background);
      
      //Unloading des bloc et matrice
      for (int i = 0; i < levelMatrice.GetLength(0); i++)
      for (int j = 0; j < levelMatrice.GetLength(1); j++)
          {
              var texture = levelMatrice[i, j];
              if (texture != null)
                  Raylib.UnloadTexture((Texture2D)texture);
              levelMatrice[i, j] = null;
          }
      
      for (int i = 0; i < BackgroundMatrice.GetLength(0); i++)
      for (int j = 0; j < BackgroundMatrice.GetLength(1); j++)
      {
          var texture = BackgroundMatrice[i, j];
          if (texture != null)
              Raylib.UnloadTexture((Texture2D)texture);
          BackgroundMatrice[i, j] = null;
      }

      
      getPalette.Destroy();
      VoidArea.Destroy();

      
    }

    private void SaveEntities()
    {
        var Actives = new List<RawEntity>();
        foreach (var entity in entities)
            if(entity is ActiveEntity && !(entity is BubbleText) && !(entity is Itemholder.Helper))
                Actives.Add((entity as ActiveEntity).CreateRawEntity());

       
        if(serializer == null)
         serializer = new XmlSerializer(Actives.GetType());
        try
        {
            using (StreamWriter writer = new StreamWriter(Program.RootDirectory + $"Save/Save_{ID}.xml"))
                serializer.Serialize(writer,Actives);
        }
        catch (Exception e)
        {
            
        }
       
        
    }

   

    

    //ElRatz|748!517!:
    private Entity getEntityFromXML(XmlNode node)
    {
        var t = Type.GetType("Moteur.Entites." + node.SelectSingleNode("type").InnerText);
        var argumentList = new List<object>();
        argumentList.Add(int.Parse(node.SelectSingleNode("x").InnerText) * blocH);
        argumentList.Add(int.Parse(node.SelectSingleNode("y").InnerText) * blocH);
        var argSup = node.SelectSingleNode("argument").InnerText;
        if (argSup != "")
            argumentList.Add(argSup);

        var entity = Activator.CreateInstance(t, argumentList.ToArray()) as Entity;
        entity.name = node.SelectSingleNode("name").InnerText;
        return entity;
    }

    private Entity getEncodedEntity(string line)
    {
        var split1 = line.Split('|');
        var t = Type.GetType("Moteur.Entites." + split1[0]);
        object[] argument;
        var split2 = split1[1].Split('!');
        var coord = new[]
        {
            int.Parse(split2[0]) * blocH,
            int.Parse(split2[1]) * blocH
        };

        var arg = new object[0];
        if (split2.Length > 2 && split2[2] != "")
        {
            var split3 = split2[2].Split(',');
            arg = new object[split3.Length];
            for (var i = 0; i < split3.Length; i++)
                try
                {
                    arg[i] = int.Parse(split3[i]);
                }
                catch (Exception e)
                {
                    arg[i] = split3[i];
                }
        }

        argument = new object[2 + arg.Length];
        for (var i = 0; i < coord.Length; i++)
            argument[i] = coord[i];
        for (var i = 2; i < 2 + arg.Length; i++)
            argument[i] = arg[i - 2];

        return Activator.CreateInstance(t, argument) as Entity;
    }

    public IEnumerable<CollidedEntity> getCollidedEntity()
    {
        foreach (var entity in entities)
        {
            if (entity is CollidedEntity)
                yield return (entity as CollidedEntity);
        }
    }
    public bool haveBackground()
    {
        return Background.width != null && Background.width != 0 ;
    }

    public void IsZombie()
    {
        SoundManager soundManager = new SoundManager();
        foreach (var entity in entities)
        {
            if (entity.GetType == Enum.EntityType.Zombie)
                soundManager.BruitDeZombie();
        }
    }
}