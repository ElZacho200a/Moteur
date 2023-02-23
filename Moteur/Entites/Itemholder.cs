namespace Moteur.Entites
{
    internal class Itemholder : ActiveEntity
    {
        private Item Item;
        public Itemholder(int x , int y, Item item) : base()
        { 
            Coordonates = (x,y);
            Item = item;
            setup();
        }

        public Itemholder(int x, int y, string name)
        {
            Coordonates = (x, y);
            Type type = Type.GetType( "Moteur.Items." +name);
            Item = (Item)Activator.CreateInstance(type);
            setup();
        }

        private  void setup()
        {
            Camera.player.AddSubscriber(GiveAndDestroy);
            Sprite = Item.GetImage();
            Hitbox = new Rectangle(this[0], this[1], Level.blocH, Level.blocH);
        }
        public override void Update()
        {
            return;
        }

        private void GiveAndDestroy()
        {
            if (Camera.player.Hitbox.IntersectsWith(Hitbox))
            {
                Camera.player.receiveItem(Item);
                Item.OnCatch();
                Camera.player.Equals(GiveAndDestroy);
                Level.currentLevel.RemoveEntity(this);
            }

        }
        protected override bool Moove()
        {
            throw new InvalidOperationException();
        }

        protected override void UpdateAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
