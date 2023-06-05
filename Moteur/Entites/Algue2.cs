namespace Moteur.Entites;

public class Algue2 : ActiveEntity
{
    private int time = 0;
    public Algue2(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\Algue2.png", 50, 50);
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