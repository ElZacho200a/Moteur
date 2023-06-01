
namespace Moteur
{
    public abstract class LivingEntity : ActiveEntity
    {
        protected int life;
        public virtual double Gravity
        {
            get
            {
               
                return Level.blocH / (isInWater() ? 500.0 : 130.0);
               
                // Level.blocH / x -> x  ; Plus x  est grand plus la gravité est faible
            }
        }

        /* public virtual double Gravity { get; set; } = 80/Level.blocH;*/ ////pour overrider pour modifier la gravite sans toucher celle des autres entites
        protected virtual int MaxSpeed() { return 10 ; }


        protected override bool Moove()
        {
            bool toReturn = false;
            
            (int nextX, int nextY) nextCoord = (Coordonates.x + (int)Speed.vx, Coordonates.y);
            // On ne met pas le Y en next car On check les collison en X puis en Y
            if (!IsCollided(Coordonates))
            {
                if (IsCollided(nextCoord))
                {
                    toReturn = true;
                    nextCoord.nextX -= (int)Speed.vx;
                    Acceleration.ax = 0;
                }

                nextCoord.nextY +=
                    (int)Speed.vy; // On ajoute alors le Y pour checker une potentielle collision vertical

                //Test pour Y
                if (IsCollided(nextCoord))
                {
                    if (sensY == 1)
                        nextCoord.nextY = PutOnground(nextCoord);
                    else
                        nextCoord.nextY -= (int)Speed.vy;
                    Speed.vy = 0;
                    Acceleration.ay = 0;

                }
                else
                {
                    Acceleration.ay = Gravity; // Dans le cas ou l'entité  ne touche pas le sol, elle subit la gravité 
                }
            }

            Coordonates = nextCoord; // Une fois le traitement des nouvelles coordonnées on les substitut au coord actuelles
                
                //Fin de la mise à jour de coordonné de l'entité
                //Mise à jour de la vitesse avec l'accélération
                if (Acceleration.ay == 0)
                    Speed.vx = (Acceleration.ax + Speed.vx) / 2; // Sur le sol
                else
                    Speed.vx = (Acceleration.ax + Speed.vx) / 3; // Dans les airs
                                                                 // Faire une moyenne permet de simuler un frottement ou un frein pour empécher que l'entité de prenne trop de vitesse
                
                // on Ajoute la force gravitationnel à la vitesse en Y 
                
                Speed.vy += Acceleration.ay;
                return toReturn;
            
        }



    protected int  PutOnground((int x, int y) Coord)
        {
            while (!IsCollided(Coord)){
                Coord.y += Level.blocH;
            }
            return Coord.y  - Coord.y % Level.blocH;
        }
    public bool IsCollided((int x, int y)Coord)
        {
            int blocH = Level.blocH; // Récupération de la taille en pixel des blocs
            var CollisionMatrice = Level.currentLevel.getCollisonMatrice();
            //Check des CollidedEntity
            Rectangle toCheck  = new Rectangle(Coord.x, Coord.y , Hitbox.Width , Hitbox.Height );
            foreach (var collidedEntity in Level.currentLevel.getCollidedEntity())
            {
                if (collidedEntity.Hitbox.IntersectsWith(toCheck))
                    return true;
            }

            if (false && this is Player)
            {
                var me = this as Player;
                foreach (var player in Level.Players)
                {
                if(player.index != me.index)
                    if (toCheck.IntersectsWith(player.Hitbox))
                        return true;
                }
            }
            
            // Check de sortie de Bounds
            if(Coord.y+Hitbox.Height >blocH * CollisionMatrice.GetLength(1) )
                return true;
            if (Coord.x < 0 )
                return true;
            if(Coord.x + Hitbox.Width > blocH * CollisionMatrice.GetLength(0))
                return true;
           
            // Mise à l'échelle de la Hitbox par rapport à la grille de collisions
            
            for (int i = toCheck.X; i < toCheck.Width +toCheck.X   ; i += 1)
            {
                for(int j = toCheck.Y;j < toCheck.Height+ toCheck.Y   ; j += 1)
                {
                    try
                    { 
                        if (CollisionMatrice[i /blocH, j /blocH])
                            return true;
                    }
                    catch (Exception)
                    {
                      return false;
                    }


                    j += blocH - (j % blocH) - 1;
                }
                i += blocH -  (i % blocH);
            }
            return false;

        }

        public int Life // pour chopper la vie des mobs 
        {
            get => life;
            set => life = value;
        }

        public double GetGravity // pour modifier la gravite de certaines entites
        {
            get => Gravity;
            
        }
    }
}
