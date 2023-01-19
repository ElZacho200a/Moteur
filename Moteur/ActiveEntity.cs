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
        public int sensX => Speed.vx > 0 ? 1 : -1;
        public int sensY => Speed.vy > 0 ? 1 : -1;
        public Bitmap Sprite;
        protected SpriteManager spriteManager;
        protected abstract bool Moove();
        protected abstract void UpdateAnimation();
        public (double vx, double vy) GetSpeed()
        {
            return Speed;
        }




    }
}
