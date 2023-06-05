namespace Moteur.Entites;

public class Corail : ActiveEntity
{
    private int time = 0;
    public Corail(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\corail.png", 50, 50);
        Sprite = spriteManager.GetImage(0, sensX);
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