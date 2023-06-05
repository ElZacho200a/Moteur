namespace Moteur.Entites;

public class anemone : ActiveEntity
{
    private int time = 0;
    public anemone(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\anemone.png", 50, 50);
        Sprite = spriteManager.GetImage(1, sensX);
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