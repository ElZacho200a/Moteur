namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        public static Player player;
        public static Camera camera;
        public static PaintEventArgs paint;
        public static LivingEntity mob;
        public Bullet  (int x, int y)
        {
            Coordonates = (x,y);
            spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Bullet.png", 8, 8);
            Hitbox = new Rectangle(x, y, spriteManager.Width, spriteManager.Height);
            Sprite = spriteManager.GetImage(0, sensX);
            Speed.vx = 10;
        }
        public override void Update()
        {
            UpdateAnimation();
            if (Moove())
            {
                Level.currentLevel.RemoveEntity(this);
            }
            if (Hitbox.IntersectsWith(mob.Hitbox))
            {
                mob.GetLife -= 1;
                Level.currentLevel.RemoveEntity(this);
            }
        }
        
        protected override void UpdateAnimation()
        {
            Sprite = spriteManager.GetImage(0, -sensX);
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
        }
    }
}

