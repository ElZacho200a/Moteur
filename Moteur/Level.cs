using System.Drawing;
using System.IO;

using  System.Drawing;



namespace Moteur;

public class Level
{
    protected Bitmap[,] levelMatrice ;
    protected int[,] CollisionMatrice;
    
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
        return @$"C:/Users/zache/source/repos/Moteur/Moteur/Assets/ROOMS/ROOM_{id}.png";
    }

    public Bitmap[,]getLevelMatrice()
    {
        return levelMatrice;
    }
    public int[,] getCollisonMatrice()
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
        CollisionMatrice = new int[rawLevel.Width, rawLevel.Height];
        for (int i = 0; i < rawLevel.Width; i++)
            for (int j = 0; j < rawLevel.Height; j++)
            {
                Color color = rawLevel.GetPixel(i, j);
                if (color.R == 1)
                    CollisionMatrice[i, j] = 1;// setup de la Matrice de Collision
                if (color.R <= 2)
                    levelMatrice[i, j] = palette.getImageByColor(color); // setup des Images
                if (color.R == 5)
                {
                    entities.Add(new Sortie(color.B, i * blocH, j * blocH));
                   
                }
            }
    }

    public void Update()
    {
        try
        {
            foreach (var entity in entities)
        {
            entity.Update();
        }

        }
        catch (Exception)
        {

            return;
        }
        
    }
    
}