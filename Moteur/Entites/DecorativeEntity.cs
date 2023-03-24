namespace Moteur.Entites;

public class DecorativeEntity : ActiveEntity
{
    private int temp = 60;
    private int animation = 1;
    private string imageFile;
    private string arguments;
    private int time = 0;
    private bool animated;
    public override string getArgument => arguments;
    // Tout les argument sont dans la string nommée "ImageFile_nbAnimation_Temp" et séparés par des |
    // ( on appel ça des pipes alors faites pas les étonné)
    public DecorativeEntity(int x, int y, string ImageFile_nbAnimation_Temp)
    {
        //on les sauvegardes pour reconstruire l'entité après save de la Room
        arguments = ImageFile_nbAnimation_Temp;
        var args = ImageFile_nbAnimation_Temp.Split("|");

        if (args.Length <= 3)
        {
            imageFile = Program.RootDirectory +"Assets/DecorativeSprite/" +args[0] +".png";
            if (args.Length > 1)
            {
                animation = Int32.Parse(args[1]);
                if (args.Length > 2)
                {
                    temp = Int32.Parse(args[2]);
                }
            }
        }
        else
        {
            throw new ArgumentException("Les arguments donné à cette entité ne sont du bon format");
        }

        animated = animation > 1;
        spriteManager = new SpriteManager(imageFile, animation, false);
        Hitbox = new Rectangle(x, y, spriteManager.Width, spriteManager.Height);
        Sprite = spriteManager.GetImage(0);
        Coordonates = (x, y);
    }
    public override void Update()
    {
        
        
        if(!animated)
            return;
        time = (time + 1 % temp);
        if (time == 0)
        {
            Sprite = spriteManager.nextCursor();
        }
        
    }

    
    
    
    protected override bool Moove()
    {
        return false;
    }

    protected override void UpdateAnimation()
    {
      
    }
}