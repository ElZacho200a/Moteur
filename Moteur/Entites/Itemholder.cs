namespace Moteur.Entites
{
    internal class Itemholder : ActiveEntity
    {
        private Item Item;
        private Helper help;
        private String _itemName;

        public string ItemName
        {
            get => _itemName;
           
        }
        public override string getArgument => _itemName;
        public Itemholder(int x , int y, Item item) : base()
        { 
            Coordonates = (x,y);
            Item = item;
            setup();
        }

        public Itemholder(int x, int y, string name)
        {
            Coordonates = (x, y);
            _itemName = name;
            Type type = Type.GetType( "Moteur.Items." +name);
            Item = (Item)Activator.CreateInstance(type);
            setup();
        }

        private  void setup()
        {
            Camera.player.AddSubscriber(GiveAndDestroy);
            Sprite = Item.GetResizedImage();
            Hitbox = new Rectangle(this[0], this[1], Level.blocH, Level.blocH);
        }
        public override void Update()
        {
            if (Math.Abs(Camera.player[0] - this[0]) < 3 * Level.blocH)
            {
                if (help == null)
                    help = new Helper(this);
            }
            else if (help != null)
                {
                    help.kill();
                    help = null;
                }


        }

        private void GiveAndDestroy()
        {
            
            if (Camera.player.Hitbox.IntersectsWith(Hitbox))
            {
                if (help != null)
                {
                    help.kill();
                    
                }
                Camera.player.receiveItem(Item);
                Item.OnCatch();
                Camera.player.DelSubscriber(GiveAndDestroy);
                isDead = true; 
            }

        }
        protected override bool Moove()
        {
            throw new InvalidOperationException();
        }

        protected override void UpdateAnimation()
        {
            throw new InvalidOperationException();
        }
        
        
        internal class Helper : ActiveEntity
        {

            private int time = 0;
            private static bool exist = false;
            public Helper(Entity parent) : base()
            {
               if(exist)
                return;
               exist = true;
                
                spriteManager = new SpriteManager(Form1.RootDirectory + "Assets/Textures/ItemHelp.png", 50, 50, false);
                Hitbox = new Rectangle(
                    parent[0], 
                    (int)(parent[1] - (1.5 * spriteManager.Height)), 
                    spriteManager.Width,
                    spriteManager.Height
                    );
                Sprite = spriteManager.GetImage(0);

                Level.currentLevel.addEntity(this);
            }
            public override void Update()
            {
                if(isDead)
                    return;
                time = (time + 1) % 10;
                if(time != 0)
                    return;
                Sprite = spriteManager.nextCursor();
            }

            public void kill()
            {
                Level.currentLevel.RemoveEntity(this);
                exist = false;
                isDead = true;
               
            }
            protected override bool Moove()
            {
                throw new InvalidOperationException();
            }

            protected override void UpdateAnimation()
            {
                throw new InvalidOperationException();
            }
        }
    }
}
