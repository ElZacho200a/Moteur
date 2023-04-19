namespace Moteur.Entites
{
    internal class Skeletton : TriggerEntity
    {
        protected bool trigered = false;
        Random random = new Random();

        protected  new int MaxSpeed => 8; 
        public Skeletton(int x, int y) : base(15)
        {
            spriteManager = new SpriteManager(Program.RootDirectory + @"Assets\Sprite\Zombie.png", 50, 72);
            Coordonates = (x, y);
            
            Hitbox = new Rectangle(x, y, spriteManager.Width  , spriteManager.Height);
            Life = 50;
            Sprite = spriteManager.GetImage(0, sensX);
            Acceleration.ax = (random.Next(3) == 1 ? 1 : -1) * MaxSpeed;
            type = Enum.EntityType.Skeletton;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateAnimation()
        {
            throw new NotImplementedException();
        }
    }
}