namespace Moteur.Entites
{
    internal class Pigeon : ActiveEntity
    {

        Random random= new Random();
       
        private bool trigered =false;
        private byte triggerRange =9;
        public Pigeon(int x , int y) {
            spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Pigeon.png", 50, 50);
            
            Speed.vy = 10;
            Speed.vx = 10 * ((random.Next(2) == 1 ) ? -1 : 1 );
            Sprite = spriteManager.GetImage(0,sensX);
            this.Hitbox = new Rectangle(x, y, 50, 50);
        }
        public override void Update()
        {
            if (!trigered && Math.Abs(Camera.player[0] - this.Hitbox.X)  < Level.blocH * triggerRange)
            {
                trigered = true;
                
            }
            if (trigered)
            {
                UpdateAnimation();
                Moove();
            }

        }

        protected override void UpdateAnimation() {

            if (spriteManager.cursor != 1)
                Sprite = spriteManager.GetImage(1 , sensX);
            else
                Sprite = spriteManager.GetImage(2, sensX);
           
        }
        protected override bool Moove()
        {
            //Coordonates.x += 
           // Coordonates.y += 
            Hitbox.X -= (int)Speed.vx; ;
            Hitbox.Y -=  (random.Next((int)(Speed.vy * 2))  + (int)Speed.vy ); 

            return true;
        }
    }
}
