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
        this.Text = formate(Text);
        Camera.AddSubscriberTenTick(UpdateAnimation);
        
        
    }

    private String formate(String text)
    {
        var ret = "";
        for (int i = 0; i < text.Length; i++)
        {
            ret += text[i];
            if(i % 17 == 0 && i != 0 )
                ret+= "\n";
        }
        return ret;
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
        
        if (Sprite == null)
            return;
        Hitbox.X = origin.X - Hitbox.Width;
        Hitbox.Y = origin.Y - Hitbox.Height;
        
        if(time < Text.Length)
        {
            Wright(Text.Substring(0,time));
            time++;
        }



        if (!is_triggered())
        {
            //Destroy itself
            Camera.OnTenTick -= UpdateAnimation;
            isDead = true;
            System.GC.Collect();
        }
    }
    protected void Wright(String text)
    {
        using (Graphics g = Graphics.FromImage(Sprite))
        {
            // Configurez la police et la couleur de la police
            Font font = new Font("Handel Gothic", Hitbox.Width /17, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);

            // Dessinez le texte sur l'image
            g.DrawString(text, font, brush, new PointF(10, 20));
        }
        spriteManager.setSprite(Sprite);
    }
}