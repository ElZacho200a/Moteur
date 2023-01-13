using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    internal abstract class Entity
    {
        public (int x, int y) Coordonates { get; set; }
        public Rectangle Hitbox { get; set; }

        public abstract void Update();
    }
}
