namespace Moteur.Entites
{
    internal class Zombie : LivingEntity
    {
        protected bool trigered = false;
        protected ushort triggerRange = 15;
        public Zombie(int x, int y)
        {
            spriteManager = new SpriteManager(Form1.RootDirectory + @"Assets\Sprite\Zombie.png", 25, 39);
            Coordonates = (x, y);
            this.MaxSpeed = 15;
            Hitbox = new Rectangle(0, 0, Level.blocH / 2, (Level.blocH * 39) / 50); // a modifier ptet
            Life = 50;
            this.Speed = (5, 5);
        }

        public override void Update()
        {
            if (!trigered && Math.Abs(Camera.player[0] - this.Hitbox.X)  < Level.blocH * triggerRange)
            {
                trigered = true;
            }
            if (trigered)
            {
                Moove();
            }
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            Moove();
            UpdateAnimation();
        }

        protected override bool Moove()
        {
            Speed = (MaxSpeed, MaxSpeed);
            return true;
        }

        protected override void UpdateAnimation()
        {
            if (IsCollided((Coordonates.x, Coordonates.y + 1)))
            {
                if (((int)(Speed.vx)) * sensX < 3)
                    Sprite = spriteManager.GetImage(0, sensX);
                else if (spriteManager.cursor != 1)
                    Sprite = spriteManager.GetImage(1, sensX);
                else
                    Sprite = spriteManager.GetImage(2, sensX);// pour les frames
            }
        }
    }
}
