﻿using System;
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
        private static (int X , int Y , int Width , int Height )  Scope ;
        public static (int X, int Y, int Width, int Height) GetScope() { return Scope; }
        public Camera(int Widht , int Height):base()
        {
            DoubleBuffered= true; // Extrêmement important permet d'avoir une image fluide 
            Scope = (0, 0, Widht , Height);
            blocH = Widht / FOV;
            Level.blocH = blocH;
            player = new Player();
            new Level(0);
            ResetScope();
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

        public static bool isInScope(Rectangle rect)
        {
            if(Scope.X <= rect.X &&  rect.Right <= Scope.Width + Scope.X )
                if(Scope.Y <= rect.Bottom && rect.Y <= Scope.Height + Scope.Y )
                    return true; 
            return false;
        }


        public void UpdateScope()
        {
            var speedDouble = player.GetSpeed();
            (int vx , int vy ) speedInt = ((int)speedDouble.vx, (int)speedDouble.vy);
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            var levelHeight = Level.currentLevel.getCollisonMatrice().GetLength(1) * Level.blocH;
            if (((Scope.X + Scope.Width) / 2 > ( player.Coordonates.x ) && player.sensX == -1 )|| (Scope.X +(Scope.Width /2) < player.Coordonates.x && player.sensX == 1) )
            { // Changement de la caméra en X
                
             
                    Scope.X += speedInt.vx ;
            }
            if (((Scope.Y + Scope.Height) / 2 > (player.Coordonates.y) && player.sensY == -1) || (Scope.Y + (Scope.Height / 2 ) < player.Coordonates.y && player.sensY == 1 ))
            { // Changement de la caméra en X


                Scope.Y += speedInt.vy;
            }



            if ((Scope.X + Scope.Width) + (speedInt.vx) >= levelWidht)
            {
               Scope.X = levelWidht - Scope.Width;
                
            }
            else if (Scope.X + speedInt.vx <= 0)
            {
                Scope.X = 0;
            }

            if ((Scope.Y + Scope.Height) + (speedInt.vy) >= levelHeight)
                Scope.Y = levelHeight - Scope.Height;
            else if (Scope.Y + speedInt.vy <= 0)
                Scope.Y = 0;

        }

        public static void ResetScope()
        {
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            Scope.X = player.Coordonates.x;
            Scope.Y = player.Coordonates.y;
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
           
            var decalY =  Screen.FromControl(this).Bounds.Height;
            decalY -= levelMatrice.GetLength(1) * Level.blocH;
            // translation des tout les élement 
            // Cette décal permet de mettre le bas du niveau en bas de l'écran cette utilité est voué à disparaitre
           //g.TranslateTransform(0.0F, (float)decalY, MatrixOrder.Append);
            //Translation des sprites en fonction des coord du joueur
            

            

            g.TranslateTransform( -Scope.X, -Scope.Y, MatrixOrder.Append);
            
            // Dessin du niveau
            for (int i = Scope.X / blocH; i < levelMatrice.GetLength(0); i++)
            {
                for(int j = Scope.Y / blocH; j < levelMatrice.GetLength(1); j++)
                {
                    if (levelMatrice[i, j] != null  && i  * blocH < Scope.Width + Scope.X )
                    {
                        g.DrawImage(levelMatrice[i, j], new Point(i * Level.blocH, j * Level.blocH));

                    }
                }
            }
            //Dessin du joueur
            try
            {
                g.DrawImage(player.Sprite, new Point(player.Coordonates.x, player.Coordonates.y));
            }
            catch (Exception)
            {

              
            }
            
            //Dessin des Entités
            foreach (var entity in Level.currentLevel.GetEntities())
            { if(isInScope(entity.Hitbox))
                    if(entity is ActiveEntity)
                    {
                        var active = entity as ActiveEntity;
                       if (active != null)
                            g.DrawImage(active.Sprite,active.Hitbox.Location);
                    }
               
            }
        }

    }
}