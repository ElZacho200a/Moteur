using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;

namespace Moteur.Entites
{
    internal class Zombie : TriggerEntity
    {
        Random random = new Random();
        protected new int MaxSpeed => 8;
       
        public Zombie(int x, int y) : base (15) // 15 est la Trigger Range 
        {
            spriteManager = new SpriteManager(Program.RootDirectory + @"Assets\Sprite\Zombie.png", 72, 50);
            Coordonates = (x, y);
            Hitbox = new Rectangle(x, y, spriteManager.Width  , spriteManager.Height); // J'ai modif , le rect doit prendre x,y en premier arg
            Life = 50;
            Sprite = spriteManager.GetImage(0, sensX);
            Acceleration.ax = (random.Next(2) == 1 ? 1 : -1) * MaxSpeed;
        }

        public override void Update()
        {
            if (this.Life <= 0)
            {
                isDead = true;
            }
            if(!is_triggered())
            {
                Random rand = Random.Shared;
                int randint = rand.Next(2);
                if (Moove() && randint == 0)
                {
                    Speed.vy = (-MaxSpeed * 1  - Speed.vx / 6) ;
                }

                else if (Moove())
                {
                    Speed.vx = Speed.vx * -1;
                }

                Acceleration.ax = MaxSpeed * this.sensX;
                UpdateAnimation();
            }
            else
            {
                if (Moove())
                {
                    Speed.vy = (-MaxSpeed * 1  - Speed.vx / 6) ;
                }
                Acceleration.ax = MaxSpeed * sensPlayer;
                UpdateAnimation();
            }

            foreach (var player in Level.Players)
            if (player.Hitbox.IntersectsWith(this.Hitbox))
            {
                Speed.vx = 0;
            }
        }

       
        protected override void UpdateAnimation()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            foreach (var player in Level.Players)
            if (!player.Hitbox.IntersectsWith(this.Hitbox))
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
