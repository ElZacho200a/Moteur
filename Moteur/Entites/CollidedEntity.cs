namespace Moteur.Entites;

public class CollidedEntity : DecorativeEntity
{
    public CollidedEntity(int x, int y, string ImageFile_nbAnimation_Temp) : base(x, y, ImageFile_nbAnimation_Temp)
    {
    }

    public bool checkCollided(Rectangle rect)
    {
        try
        {
            var Intersect = getInteresectRect(rect, Hitbox);
            return true;
        }
        catch{}
        return false;
    }

    public static Rectangle getInteresectRect(Rectangle rect1, Rectangle rect2)
    {
        if (!rect1.IntersectsWith(rect2))
            throw new  ArithmeticException("Rect does not intersect");
        else
        {
            var x = Math.Max(rect1.X, rect2.Y);
            var y = Math.Max(rect1.Y, rect2.Y);
            var w = Math.Min(rect1.Right - x, rect2.Right - x);
            var h = Math.Min(rect1.Bottom - y, rect2.Bottom - y);
            return new Rectangle(x, y, w, h);
        }
    }
}