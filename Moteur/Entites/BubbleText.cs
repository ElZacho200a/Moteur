namespace Moteur.Entites;

internal class BubbleText:TriggerEntity
{
    private int time = 0;
    private String Text = "";
    private Rectangle origin;
    public BubbleText(String Text, Rectangle rect , int range ) : base(range)
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
        return;
    }

   
    protected override bool Moove()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateAnimation()
    {
        time = (time + 1) % 60;
        if (Sprite == null)
            return;
        Hitbox.X = origin.X - Hitbox.Width;
        Hitbox.Y = origin.Y - Hitbox.Height;
        if(!is_triggered())
        {
            //Destroy itself
            Camera.OnTenTick -= UpdateAnimation;
            isDead = true;
            System.GC.Collect();
        }
    }
}