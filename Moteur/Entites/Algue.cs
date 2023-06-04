namespace Moteur.Entites;

public class Algue : DecorativeEntity
{
    public Algue(int x, int y, string ImageFile_nbAnimation_Temp) : base(x, y, ImageFile_nbAnimation_Temp)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\Pigeon.png", 50, 50);
        
    }
}