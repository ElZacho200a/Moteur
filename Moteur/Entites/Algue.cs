﻿namespace Moteur.Entites;

public class Algue : LivingEntity
{
    protected new int MaxSpeed => 0;
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
        UpdateAnimation();
    }

    protected override void UpdateAnimation()
    {
        if (spriteManager.cursor == 0)
            Sprite = spriteManager.GetImage(1, sensX);
        if (spriteManager.cursor == 1)
            Sprite = spriteManager.GetImage(2, sensX);
        if (spriteManager.cursor == 2)
            Sprite = spriteManager.GetImage(3, sensX);
        if (spriteManager.cursor == 3)
            Sprite = spriteManager.GetImage(4, sensX);
        if (spriteManager.cursor == 4)
            Sprite = spriteManager.GetImage(5, sensX);
        if (spriteManager.cursor == 5)
            Sprite = spriteManager.GetImage(0, sensX);
    }
}