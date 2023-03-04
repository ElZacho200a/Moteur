namespace Moteur.Items
{
    internal class Bullets : Item
    {
        public override void OnCatch()
        {
            base.OnCatch();
            this.Count = 15;
        }

        public override void OnUse()
        {
            this.Count -= 1;
        }
    }
}
