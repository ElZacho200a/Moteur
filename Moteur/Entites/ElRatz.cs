using System.Formats.Asn1;
using System.Windows.Forms.VisualStyles;

namespace Moteur.Entites;

internal class ElRatz : LivingEntity
{
    public ElRatz(int x, int y)
    {
        Coordonates = (x, y);
        Hitbox = new Rectangle(x,y,Level.blocH,Level.blocH);
        spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Ratz.png", 50, 50);
        Sprite = spriteManager.GetImage(0, sensX);
        MaxSpeed = 10;
        Acceleration.ax = MaxSpeed;
    }

    public override void Update()
    {
        if (Moove())
        {
            Speed.vx = Speed.vx * -1;
        }
        UpdateAnimation();
        Acceleration.ax = MaxSpeed * sensX;

        if (Camera.player.Hitbox.IntersectsWith(Hitbox))
        {
            var level = Level.currentLevel;
            level.RemoveEntity(this);
        }
    }

    protected override void UpdateAnimation()
    {
        Sprite = spriteManager.GetImage(0, -sensX);
        Hitbox.X = Coordonates.x;
        Hitbox.Y = Coordonates.y;
    }
}