namespace Moteur.Entites;

public class Fontaine : LivingEntity
{
    public Fontaine(int x, int y)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\anemone.png", 50, 50);
        Sprite = spriteManager.GetImage(0, sensX);
        Hitbox = new Rectangle(x,y, spriteManager.Width, spriteManager.Height);
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
            Sprite = spriteManager.GetImage(3, sensX);
        }

        if (spriteManager.cursor == 3)
            Sprite = spriteManager.GetImage(0, sensX);
    }
}