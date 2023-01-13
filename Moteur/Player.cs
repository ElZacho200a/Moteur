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
        Coordonates = (0,0);
        this.MaxSpeed= 20;
            Hitbox = new Rectangle(0, 0, 50, 100);
        }
        public override void Update()
        {
            
            Moove();
        }
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }
    }
}
