using System.Drawing;

namespace Moteur.Entites;

internal class BubbleText:TriggerEntity
{
    private int time = 0;
    private String Text = "";
    private Rectangle origin;
    private int firstLine = 0;
    Graphics g;
    Bitmap emptySprite;
    public BubbleText(String Text, Rectangle rect , int range ) : base(range)
    {
        /*var filename = Form1.RootDirectory + "Assets\\Textures\\DialogBox.png";
        spriteManager = new SpriteManager( filename, 94, 100);
        Coordonates = (rect.X, rect.Y);
        Sprite = spriteManager.GetImage(0, sensX);
        emptySprite = new SpriteManager(filename, 94, 100).GetImage(0, 1);
        g = Graphics.FromImage(Sprite);
        origin = rect;
        Hitbox = new Rectangle(origin.X - Sprite.Width , origin.Y - Sprite.Height, Sprite.Width, Sprite.Height);
        emptySprite = Sprite.Clone(new Rectangle(0, 0,Hitbox.Width,Hitbox.Height) , Sprite.PixelFormat);
        this.Text = formate(Text);
       Camera.AddSubscriberTenTick(UpdateAnimation);
        */
        
    }

    private String formate(String text)
    {
        var ret = "";
        var splitted = text.Split(" ");
        int count = 0;
        int maxOnLine = 15;
        for (int i = 0; i < splitted.Length; i++)
        {
            if (ret.Length + splitted[i].Length - count > maxOnLine)
            {
                ret += "\n";
                count += maxOnLine;
            }
            ret += " " + splitted[i];
            
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
        
        //if (Sprite == null)
          //  return;
        Hitbox.X = origin.X - Hitbox.Width;
        Hitbox.Y = origin.Y - Hitbox.Height;
        
        if(time <= Text.Length)
        {
            Wright(Text.Substring(firstLine,time));
            time+=1;
        }



        if (!is_triggered())
        {
            //Destroy itself
             Camera.OnTenTick -= UpdateAnimation;
            isDead = true;
            g.Dispose();
            System.GC.Collect();
        }
    }
    private int CountLine(string text)
    {

        return text.Split("\n").Length;
    }
    private string removeFirstLine(string text)
    {
        bool first = false;
        var ret = "";
        foreach(var c in text)
         if(first)
            {
                ret += c;
            }
            else
            {
                first = c == '\n';
            }
    time -= Text.Length - ret.Length; 
    return ret;
    }
    protected void Wright(String text)
    {
        
        // Configure la police et la couleur de la police
        Brush brush = new SolidBrush(Color.Black);
        Font font = new Font("Handel Gothic", Hitbox.Width / 20, FontStyle.Bold);
        //On s'assure que le text ne dépasse pas 4 lignes ,sinon on le tronc

        // Dessinez le texte sur l'image


        g.DrawString(text, font, brush, new PointF(10, 30));
        if (CountLine(text) > 4)
        {
            for (int i = 0; i < 4; i++)
            {
                Text = removeFirstLine(Text);
            }
           
            g.DrawImage(emptySprite, new Point(0, 0));
        }
       

        spriteManager.setSprite(Sprite);

    }
}