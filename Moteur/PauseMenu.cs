namespace Moteur;

internal class PauseMenu
{
    //L'image de base du menu au format 1920X1080
    private Bitmap baseImage;
    //L'image redimensionné pour l'affichage ( taille définis par la Caméra )
    public Bitmap resizedImage;
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
        baseImage = new Bitmap(Form1.RootDirectory + "Assets/Textures/PauseMenu.png");
        //On la redimensionne
        resizedImage = new Bitmap(baseImage, Width, Height);
        // On trouve le ratio entre les deux images afin de setup les tailles et point pour l'affichage
        double ratio =  resizedImage.Width / (double)(baseImage.Width);
       
        itemSize = (int)(itemSize * ratio); // La taille des Slot
        
        //On défini l'origin de manière à ce que le menu soit centré
        Origin = new Point((Camera.Width - Width) / 2, (Camera.Height - Height) / 2);

        for (int i = 0; i < InventorySlot.Length; i++)
            scalaire(ref InventorySlot[i], ratio);




        }

    public void Draw(Graphics g) 
    {
        g.DrawImage(resizedImage,Origin);
        g.TranslateTransform(Origin.X , Origin.Y);
        for (int i = 0; i < InventorySlot.Length && i < player.Inventory.Count ; i++)
            g.DrawImage(player.Inventory[i].GetResizedImage(itemSize) , InventorySlot[i]);
        
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