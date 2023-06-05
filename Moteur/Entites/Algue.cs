namespace Moteur.Entites;

public class Algue : LivingEntity
{
    protected new int MaxSpeed => 0;

    private int time = 0;
    public Algue(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\Algue.png", 50, 50);
        Sprite = spriteManager.GetImage(1, sensX);
        Acceleration.ax = MaxSpeed;
        Life = 10;
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