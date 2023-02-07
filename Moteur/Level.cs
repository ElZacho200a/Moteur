using System.Drawing;
using System.IO;

using  System.Drawing;
using System.Windows.Forms.VisualStyles;
using Moteur.Entites;

namespace Moteur;

public class Level
{
    protected Bitmap[,] levelMatrice ;
    protected bool[,] CollisionMatrice;
    
    public  int ID; 
    public static Level? currentLevel; //  l'accès à tout niveau ( ou salle ) doit se faire à travers cette variable .
    private Palette palette;
    public static int blocH;
    private List<Entity> entities = new List<Entity>();
    private Bitmap? Background;
    public Level(int id)
    {
        palette = new Palette(blocH);
        this.ID = id;
        if (currentLevel == null)
            currentLevel = this;
        setupMatrice(findFilenameByID(id));
     
    }

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

    public Bitmap[,]getLevelMatrice()
    {
        return levelMatrice;
    }
    public bool[,] getCollisonMatrice()
    {
        return CollisionMatrice;
    }
    public List<Entity> GetEntities()
    {
        return entities;
    }
    
    private void setupMatrice(string filename)
    {
        Bitmap rawLevel = new Bitmap(Image.FromFile(filename));
       
        levelMatrice = new Bitmap[rawLevel.Width, rawLevel.Height];
        CollisionMatrice = new bool[rawLevel.Width, rawLevel.Height];
        //Construction à partir de l'image ROOM_ID.png
        for (int i = 0; i < rawLevel.Width; i++)
            for (int j = 0; j < rawLevel.Height; j++)
            {
                Color color = rawLevel.GetPixel(i, j);
                switch (color.R)
                {
                    case 1 : case 2 :
                        palette.loadBloc(color);
                        CollisionMatrice[i, j] = color.R == 1;// setup de la Matrice de Collision
                        levelMatrice[i, j] = palette.getImageByColor(color); // setup des Images
                        break;
                    case 3 :
                        entities.Add(GetActiveEntityFromGreen(color.G,color.B,i*blocH, j*blocH));
                        break;
                    case 5:
                        if(color.G == 0)
                            entities.Add(new Sortie(color.B, i * blocH, j * blocH));
                        else
                        {
                            palette.loadBloc(color);
                            entities.Add((new Porte(color.B, i * blocH, j * blocH, palette.getImageByColor(color))));
                        }
                        break;
                }
                

            }
        // Construction à partir du ROOM_ID.ROOM

        try
        {
            String[] lines = File.ReadAllLines(findDataFilenamebyID(ID));
            lines[0] = lines[0].Split("..")[1];
            lines[0] = Form1.RootDirectory + "Assets" + lines[0];
           
            for (int i = 1; i < lines.Length; i++)
            {
                entities.Add(getEncodedEntity(lines[i]));
            }
    
            Background = new Bitmap(lines[0]);
            var size = new Size(Background.Width * rawLevel.Height * blocH / Background.Height, rawLevel.Height * blocH);
            Background = new Bitmap(Background,size);
        }
        catch (Exception e)
        {
           //No Data Pack Found or Data Pack is corrupted
        }
           
            
        
        
    }

    public bool Update()
    {
      
        try
        {
            foreach (var entity in entities)
        {
            if(entity.IsDead)
                {
                    entities.Remove(entity);
                    continue;
                }
            if(Camera.isInScope(entity.Hitbox))
                entity.Update();
        }

        }
        catch (Exception e)
        {
            return false;

        }
        return true;
    }

    public void addEntity(Entity entity)
    {
        entities.Add(entity);
    }
    
    public  Entity GetActiveEntityFromGreen(int green, int blue, int x, int y)
    {
        switch (green)
        {

            case 0:
                 return  new Pigeon(x,y); // 0 -> Pigeon
            case 1:
                return new ElRatz(x, y); // 1 -> Rat
            case 2:
                return new Zombie(x, y); // 2 -> Zombie
            case 3:
                return new Skeletton(x, y); // 3 -> Squelette





            default:
                throw new InvalidDataException();
                return null;
        }

    }

    public void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
    }

    //ElRatz|748!517!:
    private Entity getEncodedEntity(string line)
    {
        
        var split1 = line.Split('|');
        Type t = Type.GetType("Moteur.Entites." + split1[0]);
        object[] argument;
        var split2 = split1[1].Split('!');
        var coord = new int[]
        {
            Int32.Parse(split2[0]) * blocH,
            Int32.Parse(split2[1]) * blocH
        };

        object[] arg = new object[0];
        if (split2.Length > 2 && split2[2] != "")
        {
            string[] split3 = split2[2].Split(',');
             arg = new object[split3.Length];
            for (int i = 0; i < split3.Length; i++)
                try
                {
                    arg[i] = Int32.Parse(split3[i]);
                }
                catch (Exception e)
                {
                    arg[i] = split3[i];
                }
        }

        argument = new object[2 + arg.Length];
        for (int i = 0; i < coord.Length; i++)
            argument[i] = coord[i];
        for (int i = 2; i < 2 + arg.Length; i++)
            argument[i] = arg[i - 2];

        return Activator.CreateInstance(t, argument) as Entity; 
    }

}