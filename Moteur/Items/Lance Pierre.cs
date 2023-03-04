namespace Moteur.Items
{
    internal class LancePierre : Item
    {
        public override void OnCatch()
        {
            base.OnCatch();
            Camera.player.CanShoot = true;
        }

        public override void OnUse()
        {
        }
    }
}
