namespace Moteur.Entites
{
    internal class Skeletton : TriggerEntity
    {
        protected bool trigered = false;
        Random random = new Random();
        public Skeletton(int x, int y) : base(15)
        {
            spriteManager = new SpriteManager(Form1.RootDirectory + @"Assets\Sprite\Zombie.png", 50, 72);
            Coordonates = (x, y);
            this.MaxSpeed = 8;
            Hitbox = new Rectangle(x, y, spriteManager.Width  , spriteManager.Height);
            Life = 50;
            Sprite = spriteManager.GetImage(0, sensX);
            Acceleration.ax = (random.Next(3) == 1 ? 1 : -1) * MaxSpeed;
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