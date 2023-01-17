using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    internal class Player : LivingEntity
    {


        public Player(): base()
        {
            spriteManager = new SpriteManager(@"C:\Users\zache\source\repos\Moteur\Moteur\Assets\Sprite\PlayerSprite.png", 100 , 50); 
            Coordonates = (200,0);
        this.MaxSpeed= 20;
        Hitbox = new Rectangle(0, 0, Level.blocH, Level.blocH * 2);
        }
        public override void Update()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            Moove();
            AnimationUpdate();
        }
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }

        private  void AnimationUpdate()
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
            else
            {
                if(sensY > 0 )
                {
                    Sprite = spriteManager.GetImage(4 , sensX);
                }
                else
                {
                    Sprite = spriteManager.GetImage(3, sensX);
                }
            }
        }

        public void jump()
        {
            
            // une vitesse négative est dirigée vers le haut tout du moins en Y
            if(IsCollided((Coordonates.x , Coordonates.y +1)))
            Speed.vy = (-MaxSpeed *2 - Speed.vx / 3) ;
        }

    }
}
