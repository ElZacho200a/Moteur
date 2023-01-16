using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public abstract class Entity
    {
        public (int x, int y) Coordonates { get; set; }
        public Rectangle Hitbox;

        public abstract void Update();

        public override string ToString()
        {
            return $"{this.GetType().Name}";
        }

    }
}
