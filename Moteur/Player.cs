using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    internal class Player : LivingEntity
    {
        public Player(): base()
        {
            spriteManager = new SpriteManager(Form1.RootDirectory +@"Assets\Sprite\PlayerSprite.png", 100 , 50); 
            Coordonates = (200,0);
            this.MaxSpeed = Level.blocH/4;
        Hitbox = new Rectangle(0, 0, Level.blocH, Level.blocH * 2);
            Camera.AddSubscriberTenTick(UpdateAnimation);
        }
        public override void Update()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            AdaptAnimation();
            Moove();
            
          
        }
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }
        
        
        
        
        public delegate void MyEventHandler();
        public event MyEventHandler MyEvent;
       

        public void KeyUp()
        {
            
            if(MyEvent != null)
                MyEvent();
        }
        public void AddSubscriber(MyEventHandler sub)
        {
            MyEvent += sub;
        }
 
        public void DelSubscriber(MyEventHandler sub)
        {
            MyEvent -= sub;
        }

        private void AdaptAnimation()
        {
            if (!IsCollided((Coordonates.x, Coordonates.y + 1))) // Equivalent de le joueur est sur le sol
            {
                if (sensY > 0)
                {
                    Sprite = spriteManager.GetImage(4, sensX);
                }
                else
                {
                    Sprite = spriteManager.GetImage(3, sensX);
                }
            }
        }
        protected  override void UpdateAnimation()
        {
            if (IsCollided((Coordonates.x, Coordonates.y + 1)))// Equivalent de le joueur est sur le sol
            {
                if (((int)(Speed.vx)) * sensX < 3)
                    Sprite = spriteManager.GetImage(0 , sensX);
                else if(spriteManager.cursor != 1)
                    Sprite = spriteManager.GetImage(1, sensX);
                else
                    Sprite = spriteManager.GetImage(2, sensX);
            }
           
            
        }

        public void jump()
        {
            
            // une vitesse négative est dirigée vers le haut tout du moins en Y
            if(IsCollided((Coordonates.x , Coordonates.y +1)))
            Speed.vy = (-MaxSpeed   - Speed.vx / 6) ;
        }

        public bool shoot()
        {
            return true;
        }

    }
}
