namespace Moteur.Entites;

public class Corail : LivingEntity
{
    public Corail(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\corail.png", 50, 50);
        Sprite = spriteManager.GetImage(0, sensX);
    }
    public override void Update()
    {
        UpdateAnimation();
    }

    protected override void UpdateAnimation()
    {
        if (spriteManager.cursor == 0)
        {
            Sprite = spriteManager.GetImage(1, sensX);
        }
        if (spriteManager.cursor == 1)
        {
            Sprite = spriteManager.GetImage(2, sensX);
        }
        if (spriteManager.cursor == 2)
        {
            Sprite = spriteManager.GetImage(0, sensX);
        }
    }
}