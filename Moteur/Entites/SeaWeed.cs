namespace Moteur.Entites;

public class SeaWeed : ActiveEntity
{
    private int time = 0;
    public SeaWeed(int x, int y)
    {
        Coordonates = (x, y);
        Hitbox = new Rectangle(x, y, Level.blocH, Level.blocH);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\SeaWeed.png", 50, 50, false);
        Sprite = spriteManager.GetImage(0);
    }


    public override void Update()
    {
        time = (time + 1) % 20; 
        if(time == 0)
            Sprite = spriteManager.nextCursor();
    }

    protected override bool Moove()
    {
        return false;
    }

    protected override void UpdateAnimation()
    {
       return;
    }
}