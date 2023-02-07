

namespace Moteur.Entites;

internal class OldMan : PNJ
{
    private int time = 0;
    private BubbleText? bubbleText = null;
    public OldMan(int x , int y ) : base("text")
    {
        Speed.vx = new Random().Next(2) == 0 ? 1 : -1;
        spriteManager = new SpriteManager(Form1.RootDirectory + "Assets/Sprite/OldMan.png",118,75);
        Sprite = spriteManager.GetImage(0, sensX);
        Hitbox = new Rectangle(x, y, Sprite.Width, Sprite.Height);
        Camera.AddSubscriberTenTick(UpdateAnimation);
    }
    
    
    public OldMan(int x , int y , string text ) : base(text)
    {
        Speed.vx = new Random().Next(2) == 0 ? 1 : -1;
        spriteManager = new SpriteManager(Form1.RootDirectory + "Assets/Sprite/OldMan.png",118,75);
        Sprite = spriteManager.GetImage(0, sensX);
        Coordonates = (x, y);
        Hitbox = new Rectangle(x, y, Sprite.Width, Sprite.Height);
        
    }

    public override void Update()
    {
        //UpdateAnimation();
        if (is_triggered())
        {
           
            if (!trigger)
            {
                bubbleText = new BubbleText("Je suis si seul...", Hitbox, TriggerRange);
                Level.currentLevel.addEntity(bubbleText);
               
                trigger = true;
            }

        }
        else 
        {
            trigger= false;
            bubbleText = null;
        }
    }

    protected override void UpdateAnimation()
    {
        if (trigger)
        {
            Sprite = spriteManager.GetImage(2, sensX);
            return;
        }
        time = (time +1) % 30;
        if (time >= 25)
        {
            Sprite = spriteManager.GetImage(1, sensX);
        }
        else
        {
            Sprite = spriteManager.GetImage(0, sensX);
        }
        

    }
}