

namespace Moteur.Entites;

internal class OldMan : PNJ
{
    private int time = 0;
    private BubbleText? bubbleText = null;
    bool somethingTosay = false;
    public OldMan(int x , int y ) : base("")
    {
        Speed.vx = new Random().Next(2) == 0 ? 1 : -1;
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets/Sprite/OldMan.png",118,75);
        Sprite = spriteManager.GetImage(0, sensX);
        Hitbox = new Rectangle(x, y, Sprite.width, Sprite.height);
        Camera.AddSubscriberTenTick(UpdateAnimation);
        type = Enum.EntityType.OldMan;
    }
    
    
    public OldMan(int x , int y , string text ) : base(text)
    {
        Speed.vx = new Random().Next(2) == 0 ? 1 : -1;
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets/Sprite/OldMan.png",118,75);
        Sprite = spriteManager.GetImage(0, sensX);
        Coordonates = (x, y);
        Hitbox = new Rectangle(x, y, Sprite.width, Sprite.height);
        somethingTosay = text != "";  
        type = Enum.EntityType.OldMan;
    }

    public override void Update()
    {
        UpdateAnimation();
    
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