using Raylib_cs;
using Color = Raylib_cs.Color;
using Image = Raylib_cs.Image;

namespace Moteur;

public class PauseMenu
{
    //L'image de base du menu au format 1920X1080
    private Image baseImage;
    //L'image redimensionné pour l'affichage ( taille définis par la Caméra )
    public Texture2D resizedImage;
    // Points en haut a gauche de chaque Slot de l'inventaire
    private Point[] InventorySlot = new []{ 
        new Point(39,500) ,// Les Point sont trié dans l'ordre de lecteur
        new Point(224,500),
        new Point(408,500),
        new Point(39,684) ,
        new Point(224,684),
        new Point(408,684),
        new Point(39,869) ,
        new Point(224,869),
        new Point(408,869),
    };
    //Point en haut à gauche du menu pause;
    public Point Origin;
    //Taille d'un Slot ( c'est un carré donc pas besoin de deux dimension )
    private int itemSize = 171;
   // Le joueur dont on affichera l'inventaire
   private Player player;
    
    public PauseMenu( int Width,int Height  , Player player)
    {
        this.player = player;
        //On récupère l'image de base
        baseImage = Raylib.LoadImage(Program.RootDirectory + "Assets/Textures/PauseMenu.png");
        // On trouve le ratio entre les deux images afin de setup les tailles et point pour l'affichage
        double ratio =  Width / (double)(baseImage.width);
       
        itemSize = (int)(itemSize * ratio); // La taille des Slot
        
        //On défini l'origin de manière à ce que le menu soit centré
        Origin = new Point((Camera.Width - Width) / 2, (Camera.Height - Height) / 2);
        //On la redimensionne
        Raylib.ImageResize(ref baseImage , Width,Height);
        resizedImage = Raylib.LoadTextureFromImage(baseImage);
        for (int i = 0; i < InventorySlot.Length; i++)
            scalaire(ref InventorySlot[i], ratio);




        }

    public void Draw() 
    {
       
        Raylib.DrawTexture(resizedImage,Origin.X,Origin.Y,Color.WHITE);
        for (int i = 0; i < InventorySlot.Length && i < player.Inventory.Count ; i++)
            Raylib.DrawTexture(player.Inventory[i].GetResizedImage(itemSize) ,Origin.X + InventorySlot[i].X ,Origin.Y +InventorySlot[i].Y , Color.WHITE );
        
    }
    private void scalaire( ref Point point, double scalaire) // Sers juste à fludifier le code 
    {
        point.X = (int)(point.X * scalaire) ;
        point.Y = (int)(point.Y * scalaire);
    }

    private void translate(Point point, Point vector)
    {
        point.X += vector.X;
        point.Y += vector.Y;
    }
    
    
}