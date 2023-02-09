namespace Moteur.Entites
{
    internal class Bullet : ActiveEntity
    {
        public static Player player;
        public static Camera camera;
        public static PaintEventArgs paint;
        public static LivingEntity mob;
        public Bullet  (int x, int y)
        {
            Coordonates = (x,y);
            Hitbox = new Rectangle(x, y, spriteManager.Width, spriteManager.Height);
            spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Bullet.png", 8, 8);
            Sprite = spriteManager.GetImage(0, sensX);
            Speed.vx = 10;
        }
        public override void Update()
        {
            if (player.shoot())
            {
                Moove();
                UpdateAnimation();
            }

            if (Moove())
            {
                Level.currentLevel.RemoveEntity(this);
                mob.GetLife -= 1;
            }
        }

        protected override bool Moove()
        {
            Level.currentLevel.addEntity(this);
            paint.Graphics.DrawImage(spriteManager.GetImage(2, sensX), new Point(player.Coordonates.x,player.Coordonates.y));
            UpdateAnimation();
            if (Hitbox.IntersectsWith(mob.Hitbox))
            {
                return true;
            }
            return false;
        }

        protected override void UpdateAnimation()
        {
            Sprite = spriteManager.GetImage(0, -sensX);
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
        }
    }
}

