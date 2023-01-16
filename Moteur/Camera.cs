using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Moteur
{
    internal class Camera : Panel
    {
        int blocH;
        int FOV = 30;
        public static Player player;
        private static (int X , int Y , int Widht , int Height )  Scope ;
       
        public Camera(int Widht , int Height):base()
        {
           
            DoubleBuffered= true; // Extrêmement important permet d'avoir une image fluide 
             Scope = (0, 0, Widht , Height);
            blocH = Widht / FOV;
            
            Level.blocH = blocH;
            player = new Player();
            new Level(9);
            
            Timer timer= new Timer();
            timer.Interval= 10;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

           private  void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
           Invalidate();
            player.Update();
            Level.currentLevel.Update();

            // ajustement de la cam 
            UpdateScope();
            
        }

        public void UpdateScope()
        {
            var speedDouble = player.GetSpeed();
            (int vx , int vy ) speedInt = ((int)speedDouble.vx, (int)speedDouble.vy);
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            if ((Scope.X + Scope.Widht) / 3 > ( player.Coordonates.x ) || Scope.X +(Scope.Widht / 3 *2) < player.Coordonates.x )
            { // Changement de la caméra en X
                if ((Scope.X + Scope.Widht ) + (speedInt.vx) >= levelWidht)
                    Scope.X = levelWidht - Scope.Widht;
                else if (Scope.X + speedInt.vx <= 0)
                    Scope.X = 0;
                else
                    Scope.X += speedInt.vx ;
            }
        }

        public static void ResetScope()
        {
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            if (player.Coordonates.x > levelWidht / 2)
                Scope.X = levelWidht - player.Coordonates.x;
            else
                Scope.X = player.Coordonates.x;
        }

        public void mvPl(Keys k)
        {
            switch (k)
            {
                case Keys.Left:
                    {
                        player.KeyPressed(-1);
                        break;
                    }
                case Keys.Right:
                    {
                        player.KeyPressed(1);
                        break;

                    }
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            var levelMatrice = Level.currentLevel.getLevelMatrice();
            var pCoord = player.Coordonates;
            // Cette décal permet de mettre le bas du niveau en bas de l'écran cette utilité est voué à disparaitre
            var decalY =  Screen.FromControl(this).Bounds.Height;
            decalY -= levelMatrice.GetLength(1) * Level.blocH;
            // translation des tout les élement
            g.TranslateTransform(0.0F, (float)decalY, MatrixOrder.Append);
            //Translation des sprites en fonction des coord du joueur
            

            

            g.TranslateTransform( -Scope.X, -Scope.Y, MatrixOrder.Append);
            
            // Dessin du niveau
            for (int i = 0; i < levelMatrice.GetLength(0); i++)
            {
                for(int j =0; j < levelMatrice.GetLength(1); j++)
                {
                    if (levelMatrice[i,j] != null)
                    g.DrawImage(levelMatrice[i,j] ,new Point(i*Level.blocH,j* Level.blocH));
                }
            }
            //Dessin du joueur
            g.FillRectangle(new SolidBrush(Color.Green), new Rectangle(pCoord.x, pCoord.y, player.Hitbox.Width, player.Hitbox.Height));
            //Dessin des Entités
            foreach (var entity in Level.currentLevel.GetEntities())
            {
                g.FillRectangle(new SolidBrush(Color.BlanchedAlmond), entity.Hitbox);
            }
        }

    }
}
