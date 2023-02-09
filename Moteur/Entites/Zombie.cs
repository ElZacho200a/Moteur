using System.Security.Cryptography;

namespace Moteur.Entites
{
    internal class Zombie : TriggerEntity
    {
        protected bool trigered = false;
        Random random = new Random();
        public Zombie(int x, int y) : base (15) // 15 est la Trigger Range 
        {
            spriteManager = new SpriteManager(Form1.RootDirectory + @"Assets\Sprite\Zombie.png", 50, 72);
            Coordonates = (x, y);
            this.MaxSpeed = 8;
            Hitbox = new Rectangle(x, y, spriteManager.Width  , spriteManager.Height); // J'ai modif , le rect doit prendre x,y en premier arg
            Life = 50;
            Sprite = spriteManager.GetImage(0, sensX);
            Acceleration.ax = (random.Next(3) == 1 ? 1 : -1) * MaxSpeed;
        }

        public override void Update()
        {
            if (this.Life >= 0)
            {
                Level.currentLevel.RemoveEntity(this);
            }
            if(!trigered)
            {
                trigered = is_triggered();
                Random rand = Random.Shared;
                int randint = rand.Next(100);
                if (Moove() && randint < 50)
                {
                    Speed.vy = (-MaxSpeed * 1  - Speed.vx / 6) ;
                }

                if (Moove() && randint > 50)
                {
                    Speed.vx = Speed.vx * -1;
                }
            }
            MaxSpeed = 15;
            Acceleration.ax = MaxSpeed * sensPlayer;
            UpdateAnimation();
            if (Moove())
            {
                Speed.vy = (-MaxSpeed * 1  - Speed.vx / 6) ;
            }

            if (Camera.player.Hitbox.IntersectsWith(this.Hitbox))
            {
                Speed.vx = 0;
            }
        }

       
        protected override void UpdateAnimation()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            if (!Camera.player.Hitbox.IntersectsWith(this.Hitbox))
            {
                if (spriteManager.cursor !=0 )
                    Sprite = spriteManager.GetImage(0, -sensX);
                else if (spriteManager.cursor != 1)
                    Sprite = spriteManager.GetImage(1, -sensX);
                else
                    Sprite = spriteManager.GetImage(2, -sensX);// pour les frames
            }
            else
            {
                Sprite = spriteManager.GetImage(2, -sensPlayer); // pour pas qu'il lag a cote du perso
            }
        }
    }
}
