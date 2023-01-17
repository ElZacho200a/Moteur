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
        Coordonates = (200,0);
        this.MaxSpeed= 20;
            Hitbox = new Rectangle(0, 0, Level.blocH, Level.blocH * 2);
        }
        public override void Update()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            Moove();
        }
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }

        public void jump()
        {
            
            // une vitesse négative est dirigée vers le haut tout du moins en Y
            if(IsCollided((Coordonates.x , Coordonates.y +1)))
            Speed.vy = (-MaxSpeed *2 - Speed.vx / 3) ;
        }

    }
}
