namespace Moteur.Items
{
    internal class LancePierre : Item
    {
        public override void OnCatch(int index)
        {
            base.OnCatch(index);
            Level.Players[index].CanShoot = true;
        }

        public override void OnUse()
        {
        }
    }
}
