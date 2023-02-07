

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
        Camera.OnTenTick += UpdateAnimation;
    }
    
    
    public OldMan(int x , int y , string text ) : base(text)
    {
        Speed.vx = new Random().Next(2) == 0 ? 1 : -1;
        spriteManager = new SpriteManager(Form1.RootDirectory + "Assets/Sprite/OldMan.png",118,75);
        Sprite = spriteManager.GetImage(0, sensX);
        Coordonates = (x, y);
        Hitbox = new Rectangle(x, y, Sprite.Width, Sprite.Height);
        Camera.OnTenTick += UpdateAnimation;
    }

    public override void Update()
    {
        trigger = is_triggered();
        if (trigger)
        {

            if (bubbleText == null)
            {
                bubbleText = new BubbleText("Je suis si seul ...", Hitbox);
                Level.currentLevel.addEntity(bubbleText);
                UpdateAnimation();
            }
        }
        else if (!(bubbleText == null))
        {
            bubbleText.destroy();
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
        time = (time +1 % 40);
        if (time == 0)
        {
            Sprite = spriteManager.GetImage(1, sensX);
        }
        else
        {
            Sprite = spriteManager.GetImage(0, sensX);
        }
        

    }
}