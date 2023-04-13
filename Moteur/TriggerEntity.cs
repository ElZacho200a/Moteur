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
            foreach (var player in Level.Players)
                if (Math.Abs(this[0] -player[0]) / Level.blocH <= TriggerRange)
                    return true;
            return false;
        }

        protected int sensPlayer => Level.Players[0][0] > this[0] ? 1 : -1;
    }
}
