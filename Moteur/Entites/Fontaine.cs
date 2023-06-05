namespace Moteur.Entites;

public class Fontaine : LivingEntity
{
    private int time = 0;
    public Fontaine(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\Fontaine.png", 100, 50);
        Sprite = spriteManager.GetImage(0, sensX);
        Hitbox = new Rectangle(x,y, spriteManager.Width, spriteManager.Height);
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