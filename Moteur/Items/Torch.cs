using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur.Items
{
    internal class Torch : Item
    {
        public override void OnCatch(int index)
        {
            base.OnCatch(index);
           Level.Players[index].Light += 7;
        }

        public override void OnUse()
        {
          
        }
    }
}
