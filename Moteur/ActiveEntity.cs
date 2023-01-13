using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    internal abstract class ActiveEntity : Entity
    {
        
        public ActiveEntity() { 
        Speed = (0,0);
        Acceleration= (0,0);

        
        }
        
        
        protected (double vx, double vy) Speed;
        protected (double ax, double ay) Acceleration;
        protected Image Sprite { get; }
        protected abstract bool Moove();
        





    }
}
