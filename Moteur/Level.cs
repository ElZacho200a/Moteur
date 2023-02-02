using System.Drawing;
using System.IO;

using  System.Drawing;
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
    public Level(int id)
    {
        palette = new Palette(blocH);
        this.ID = id;
        if (currentLevel == null)
            currentLevel = this;
        setupMatrice(findFilenameByID(id));
     
    }

    private string findFilenameByID(int id)
    {
        return Form1.RootDirectory + @$"Assets/ROOMS/ROOM_{id}.png";
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
        for (int i = 0; i < rawLevel.Width; i++)
            for (int j = 0; j < rawLevel.Height; j++)
            {
                Color color = rawLevel.GetPixel(i, j);
               
                    CollisionMatrice[i, j] = color.R == 1;// setup de la Matrice de Collision
                if (color.R <= 2)
                    levelMatrice[i, j] = palette.getImageByColor(color); // setup des Images
                if (color.R == 5)
                {
                    entities.Add(new Sortie(color.B, i * blocH, j * blocH));
                   
                }else if(color.R == 3)
                {
                   
                    entities.Add(GetActiveEntityFromGreen(color.G,color.B,i*blocH, j*blocH));
                }

            }
    }

    public void Update()
    {
      
        try
        {
            foreach (var entity in entities)
        {
            if(Camera.isInScope(entity.Hitbox))
                entity.Update();
        }

        }
        catch (Exception)
        {
           
            return;
        }
        
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

}