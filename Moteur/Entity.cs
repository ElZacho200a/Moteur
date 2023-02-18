namespace Moteur
{
    public abstract class Entity
    {
        public (int x, int y) Coordonates;
        public Rectangle Hitbox;
        protected bool isDead = false;
        public bool IsDead => isDead;
        public string name;
        public abstract void Update();

        public Point getCenter()
        {
            return new Point(Hitbox.X  + Hitbox.Width / 2, Hitbox.Y + Hitbox.Height / 2);
        }
        public Rectangle getRayonRectangle(float rayon)
        {
            int Rayon = (int)(rayon * Level.blocH);
            var p  = getCenter();
            var x = p.X - Rayon;
            var y = p.Y - Rayon;
            Rayon *= 2;
           return new Rectangle(x,y,Rayon,Rayon);

            
        }
        public override string ToString()
        {
            return $"{this.GetType().Name}";
        }

        public int this[int i]{
            get {
                if (i == 0)
                    return Coordonates.x;
                else if(i == 1)
                    return Coordonates.y;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public static  int getDistance(Entity entity1 , Entity entity2) {

            return (int)(Math.Sqrt( Math.Pow(entity1.Coordonates.x - entity2.Coordonates.x , 2)  +  Math.Pow(entity1.Coordonates.y - entity2.Coordonates.y, 2)));
        }

    }


    
}
