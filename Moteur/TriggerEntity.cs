using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
   
    internal abstract class TriggerEntity : LivingEntity
    {
        protected bool trigger = false;
        protected int TriggerRange;

        protected TriggerEntity(int triggerRange) {
            TriggerRange = triggerRange;
        }

        protected  bool is_triggered()
        {
            var p = Camera.player;
            return (Math.Abs(this[0] -p[0])/Level.blocH <= TriggerRange);
        }

        protected int sensPlayer => Camera.player[0] > this[0] ? 1 : -1;
    }
}
