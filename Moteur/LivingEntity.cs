using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moteur
{
    internal abstract class LivingEntity : ActiveEntity
    {
        protected int Life;
        double Gravity = 1 ;
        protected int MaxSpeed;

        protected override bool Moove()
        {
            bool toReturn = false;
            (int nextX, int nextY) nextCoord = (Coordonates.x + (int)Speed.vx, Coordonates.y);
            // On ne met pas le Y en next car On check les collison en X puis en Y
            if (IsCollided(nextCoord))
            {
                toReturn = true;
                nextCoord.nextX -= (int)Speed.vx;
                Acceleration.ax = 0;

            }
            nextCoord.nextY += (int)Speed.vy;
            //Test pour Y
            if (IsCollided(nextCoord))
            {
               
                nextCoord.nextY -= (int)Speed.vy;
                Acceleration.ay = 0;
               
            }
            else { Acceleration.ay += Gravity; } // Dans le cas ou l'entité  ne touche pas le sol, elle subit la gravité 
            Coordonates = nextCoord;
            //Fin de la mise à jour de coordonné de l'entité
            //Mise à jour de la vitesse avec l'accélération
            if(Acceleration.ay == 0)
            Speed.vx = (Acceleration.ax + Speed.vx )  / 2; // Sur le sol
            else
            Speed.vx= (Acceleration.ax + Speed.vx) /  4; // Dans les airs
            // Faire une moyenne permet de simuler un frottement ou un frein pour empécher que l'entité de prenne trop de vitesse

            Speed.vy += Acceleration.ay;
            return toReturn;
        }


    protected bool IsCollided((int x, int y)Coord)
        {
            int blocH = Level.blocH; // Récupération de la taille en pixel des blocs
            var CollisionMatrice = Level.currentLevel.getCollisonMatrice();
            if(Coord.y+Hitbox.Height >blocH * CollisionMatrice.GetLength(1))
                return true;
            Rectangle toCheck  = new Rectangle(Coord.x/blocH, Coord.y / blocH, Hitbox.Width / blocH, Hitbox.Height / blocH);
            // Mise à l'échelle de la Hitbox par rapport à la grille de collisions
            
            for (int i = toCheck.X; i < toCheck.Width +toCheck.X ; i++)
            {
                for(int j = toCheck.Y;j < toCheck.Height+ toCheck.Y +1; j++)
                {
                    if (CollisionMatrice[i, j] == 1)
                        return true;
                }
            }
            return false;

        }

    }
}
