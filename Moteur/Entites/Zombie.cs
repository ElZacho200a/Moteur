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
            this.MaxSpeed = 15;
            Hitbox = new Rectangle(x, y, spriteManager.Width  , spriteManager.Height); // J'ai modif , le rect doit prendre x,y en premier arg
            Life = 50;
            Sprite = spriteManager.GetImage(0, sensX);
            Acceleration.ax = (random.Next(3) == 1 ? 1 : -1) * MaxSpeed;
        }

        public override void Update()
        {
            
            if(!trigered)
            {
                trigered = is_triggered();
                
            }
            else
            {
                Acceleration.ax = MaxSpeed * sensPlayer;
            }
            if (Moove())
            {
                Acceleration.ax *= -1;
            }
            UpdateAnimation();
        }

       
        protected override void UpdateAnimation()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            
                if (spriteManager.cursor !=0 )
                    Sprite = spriteManager.GetImage(0, -sensX);
                else if (spriteManager.cursor != 1)
                    Sprite = spriteManager.GetImage(1, -sensX);
                else
                    Sprite = spriteManager.GetImage(2, -sensX);// pour les frames
            
        }
    }
}
