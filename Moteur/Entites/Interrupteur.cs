namespace Moteur.Items
{
    internal class Interrupteur : ActiveEntity
    {
        private bool power = false;
        //private Type type;
        private Porte _porte;
        protected Point trigger;
        private SoundManager _soundManager = new SoundManager();

        public Interrupteur(int x, int y, Porte porte)
        {
            Coordonates = (x, y);
            _porte = porte;
            //spriteManager =
            Sprite = spriteManager.GetImage(0, sensX);
            type = Enum.EntityType.Interrupteur;
            int blocH = Level.blocH;
            trigger = new Point( x  + (blocH / 2) ,y + blocH/2 );
        }

        public bool GetPower
        {
            get => power;
            set => power = value;
        }

        public override void Update()
        {
            if (power)
            {
                _porte.Update();
                UpdateAnimation();
            }
        }

        protected override bool Moove()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateAnimation()
        {
            Sprite = spriteManager.GetImage(1, sensX);
        }
        
        private void HandleEvent(int index)
        {
            if(!Level.Players[index].Hitbox.Contains(trigger))
                return;
        
            Level.Players[index].DelSubscriber(HandleEvent);
            //_soundManager.//jvais ajouter un bruit d'interrupteur
            power = true;
        }
    }
}