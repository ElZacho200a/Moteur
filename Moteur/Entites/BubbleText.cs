namespace Moteur.Entites;

internal class BubbleText:ActiveEntity
{
    private int time = 0;
    private String Text = "";
    private Rectangle origin;
    public  BubbleText(String Text ,Rectangle rect)
    {
        var filename = Form1.RootDirectory + "Assets\\Textures\\DialogBox.png";
        spriteManager = new SpriteManager( filename, 94, 100);
        Sprite = spriteManager.GetImage(0, sensX);
        origin = rect;
        Hitbox = new Rectangle(origin.X - Sprite.Width , origin.Y - Sprite.Height, Sprite.Width, Sprite.Height);
        this.Text = Text;
        
        
        
    }

  
      
    public override void Update()
    {
        time = (time + 1) % 60;
        
        Hitbox.X = origin.X - Sprite.Width ;
        Hitbox.Y = origin.Y - Sprite.Height;
    }

    public void destroy()
    {
        Camera.OnTenTick -= UpdateAnimation;
        Level.currentLevel.RemoveEntity(this);
    }
    protected override bool Moove()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateAnimation()
    {
       return;
    }
}