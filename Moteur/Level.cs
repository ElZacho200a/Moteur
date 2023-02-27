using System.Collections.Concurrent;
using System.Xml;
using Moteur.Entites;

namespace Moteur;

public class Level
{
    public static Level? currentLevel; //  l'accès à tout niveau ( ou salle ) doit se faire à travers cette variable .
    public static int LevelLoaded;
    private Bitmap? Background;
    protected Bitmap[,] BackgroundMatrice = {{null}};
    protected bool[,] CollisionMatrice;
    public bool Dark;
    private ConcurrentBag<Entity> entities = new();
    private bool fullLoaded;
    public int ID;
    public VoidArea? VoidArea;
    protected Bitmap[,] levelMatrice;
    protected bool[,] backgroundNeedded;

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
    public Bitmap[,] BackGroundMatrice => BackgroundMatrice;
    public Palette getPalette { get; private set; }

    public (int w, int h ) GetRealSize => (levelMatrice.GetLength(0), levelMatrice.GetLength(1));

    public Bitmap getBackground()
    {
        return Background;
    }

    private string findFilenameByID(int id)
    {
        return Form1.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.png";
    }

    private string findDataFilenamebyID(int id)
    {
        return Form1.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.ROOM";
    }

    private string findXMLFilenamebyID(int id)
    {
        return Form1.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.xml";
    }

    public Bitmap[,] getLevelMatrice()
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

        levelMatrice = new Bitmap[rawLevel.Width, rawLevel.Height];
        CollisionMatrice = new bool[rawLevel.Width, rawLevel.Height];
        backgroundNeedded = new bool[rawLevel.Width, rawLevel.Height];
        // Construction à partir du ROOM_ID.ROOM

        if (levelMatrice.GetLength(0) * blocH < Camera.Width)
            while (levelMatrice.GetLength(0) * blocH < Camera.Width)
                Camera.FOV--;
        else
            Camera.FOV = 30;
        Camera.player.ResetSprite();
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
                        entities.Add(new Porte(color.B, i * blocH, j * blocH, getPalette.getImageByColor(color)));
                    }

                    break;
            }
            //Setup des Opti Background
        }

       
        if(haveBackground())
            BackgroundMatrice = SliceImage(Background);
    }

    private Bitmap[,] SliceImage(Bitmap bitmap)
    {
        var imgMatrice = new Bitmap[bitmap.Width / 50, bitmap.Height / 50];
        var size = new Size(blocH + Level.blocH / 50 , blocH + Level.blocH / 50 );
        var rect = new Rectangle(0, 0, 50, 50);
        var w = Math.Min(imgMatrice.GetLength(0) , levelMatrice.GetLength(0));
        var h = Math.Min(imgMatrice.GetLength(1) , levelMatrice.GetLength(1));
        for (var i = 0; i  < w ; i ++)
        {
            for (var j = 0; j  < h; j ++)
            {
                var img = bitmap.Clone(rect, bitmap.PixelFormat);
                imgMatrice[i, j] = new Bitmap(img, size);
                rect.Y += 50;
            }

            rect.Y = 0;
            rect.X += 50;
        }

        return imgMatrice;
    }

    private void LoadFromXML()
    {
        var filename = findXMLFilenamebyID(ID);
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        var rawEntities = xmlDocument.GetElementsByTagName("Entity");
        foreach (XmlNode rawEntity in rawEntities) entities.Add(getEntityFromXML(rawEntity));

        var RoomSave = xmlDocument.SelectSingleNode("RoomSave");
        Dark = bool.Parse(RoomSave.SelectSingleNode("isDark").InnerText);
        var BackGroundPath = RoomSave.SelectSingleNode("BackgroundPath").InnerText;
        BackGroundPath = Form1.RootDirectory + "Assets" + BackGroundPath.Split("..")[1];
        Background = new Bitmap(BackGroundPath);
    }

    private void LoadFromRoomFile()
    {
        try
        {
            var lines = File.ReadAllLines(findDataFilenamebyID(ID));
            lines[0] = lines[0].Split("..")[1];
            lines[0] = Form1.RootDirectory + "Assets" + lines[0];
            for (var i = 1; i < lines.Length; i++) entities.Add(getEncodedEntity(lines[i]));
            Background = new Bitmap(lines[0]);
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
                    entities = new ConcurrentBag<Entity>(entities.Except(new[] { entity }));
                    continue;
                }

                if (Camera.isInScope(entity.Hitbox))
                    entity.Update();
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
        
        entities = new ConcurrentBag<Entity>(entities.Except(new[] { entity }));
    }

    public void destroy()
    {
        entities.Clear();
        Background = null;
    }

    public Entity GetActiveEntityFromGreen(int green, int blue, int x, int y)
    {
        switch (green)
        {
            case 0:
                return new Pigeon(x, y); // 0 -> Pigeon
            case 1:
                return new ElRatz(x, y); // 1 -> Rat
            case 2:
                return new Zombie(x, y); // 2 -> Zombie
            case 3:
                return new Skeletton(x, y); // 3 -> Squelette
            case 4:
                return new Bullet(x, y);


            default:
                throw new InvalidDataException();
                return null;
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

    public bool haveBackground()
    {
        return Background != null;
    }
}