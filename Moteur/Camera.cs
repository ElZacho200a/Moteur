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
        int FOV = 50;
        public static Player player;
        public Camera(int Widht):base()
        {
           
            DoubleBuffered= true; // Extrêmement important permet d'avoir une image fluide 
            player= new Player();
            blocH = Widht / FOV;
            Level.blocH = blocH;
            new Level(6);
            Timer timer= new Timer();
            timer.Interval= 10;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

           private  void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
           Invalidate();
            player.Update();
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
            // Cette décal permet de mettre le bas du niveau en bas de l'écran cette utilité est voué à disparaitre
            var decalY =  Screen.FromControl(this).Bounds.Height;
            decalY -= levelMatrice.GetLength(1) * Level.blocH;
            // translation des tout les élement
            g.TranslateTransform(0.0F, (float)decalY, MatrixOrder.Append);
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
            g.FillRectangle(new SolidBrush(Color.Green), new Rectangle(player.Coordonates.x, player.Coordonates.y, player.Hitbox.Width, player.Hitbox.Height));

        }

    }
}
