namespace Moteur
{
    internal abstract class ActiveEntity : Entity
    {
        
        public ActiveEntity() { 
        Speed = (0,0);
        Acceleration= (0,0);

        
        }
        
        protected float light = 2f;

        public float Light
        {
            get => light;
            set
            {
                if (value > 0)
                    light = value;
            }
        }

        protected (double vx, double vy) Speed;
        protected (double ax, double ay) Acceleration;
        public int sensX => Speed.vx > 0 ? 1 : -1;
        public int sensY => Speed.vy > 0 ? 1 : -1;
        public Bitmap Sprite;
        protected SpriteManager spriteManager;
        protected abstract bool Moove();
        protected abstract void UpdateAnimation();
        public (double vx, double vy) GetSpeed()
        {
            return Speed;
        }

        public bool isInLightRadius(int i, int j)
        {
            i *= Level.blocH;
            j *= Level.blocH;
            var p = getCenter();
            return  (Math.Pow(p.X - i , 2 ) + Math.Pow(p.Y - j , 2 ))  - (Level.blocH * Level.blocH) *4  > Math.Pow(Light * Level.blocH , 2);

        }



    }
}
