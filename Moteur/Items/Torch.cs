using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur.Items
{
    internal class Torch : Item
    {
        public override void OnCatch()
        {
            base.OnCatch();
            Camera.player.Light += 7;
        }

        public override void OnUse()
        {
          
        }
    }
}
