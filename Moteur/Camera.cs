using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Moteur.Entites.BubbleText;
using Timer = System.Timers.Timer;

namespace Moteur
{
    internal class Camera : Panel
    {
        public static int blocH;
        int FOV = 30;
        private byte frameCounter = 0;
        public static Player player;
        private static (int X , int Y , int Width , int Height )  Scope ;
        private  static int Height, Width;
        public static (int X, int Y, int Width, int Height) GetScope() { return Scope; }
        public Camera(int Widht , int Heigt):base()
        {
           Height = Heigt;
            Width = Widht;
            this.Size = new System.Drawing.Size(Widht,Height);
            DoubleBuffered = true; // Extrêmement important permet d'avoir une image fluide 
            Scope = (0, 0, Widht , Height );
            blocH = Widht / FOV;
            
            player = new Player();
            new Level(0);
            ResetScope();
            Timer timer= new Timer();
            timer.Interval= 1000/60;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

         
           private  async void OnTimedEvent(Object source, ElapsedEventArgs e)
           {
               frameCounter = (byte)((frameCounter + 1) % 10);
               if (frameCounter % 10 == 0)
            {
                
                if (OnTenTick != null)
                    OnTenTick();
            }


            if (Level.currentLevel.Update())
            {
                player.Update();
                Invalidate();
                // ajustement de la cam 
                UpdateScope();
            }
        }

           public delegate void MyEventHandler();
           public static event MyEventHandler OnTenTick; 
           
           public static void AddSubscriberTenTick(MyEventHandler sub)
           {
               OnTenTick += sub;
           }
 
           public static void DelSubscriberTenTick(MyEventHandler sub)
           {
               OnTenTick -= sub;
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
            if ((Scope.Y + (Scope.Height /  3) > (player.Coordonates.y) && player.sensY == -1) || (Scope.Y + (Scope.Height / 3 * 2 ) < player.Coordonates.y && player.sensY == 1 ))
            { // Changement de la caméra en X


                Scope.Y  +=speedInt.vy;
            }


            // Collision Caméra
            if ((Scope.X + Scope.Width) + (speedInt.vx) >= levelWidht)
            {
               Scope.X = levelWidht - Scope.Width;
                
            }
            else if (Scope.X + speedInt.vx <= 0)
            {
                Scope.X = 0;
            }

            if ((Scope.Y + Scope.Height ) + (speedInt.vy) >= levelHeight)
                Scope.Y = levelHeight - Scope.Height;
            else if (Scope.Y + speedInt.vy <= 0)
                Scope.Y = 0;

        }

        public static void ResetScope()
        {
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            if(Scope.Width> levelWidht)
                Scope.Width = levelWidht;
            else
                Scope.Width = Width ;
            Scope.X = player.Coordonates.x - Scope.Width/2;
            Scope.Y = player.Coordonates.y ;
        }

        public void mvPl(Keys k)
        {
            switch (k)
            {
                case Keys.Up:
                {
                    player.KeyUp();
                    break;
                }
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
            if (Level.currentLevel.getBackground() != null)
                g.DrawImage(Level.currentLevel.getBackground() , new Point(0,0));
            var debX = Scope.X / blocH;
            var debY = Scope.Y / blocH;
            if (debY >= levelMatrice.GetLength(1))
                debY = 0;
            if (debX >= levelMatrice.GetLength(0))
                debX = 0;

            // Dessin du niveau
            if(levelMatrice != null)
            for (int i = debX; i < levelMatrice.GetLength(0); i++)
            {
                for(int j = debY; j < levelMatrice.GetLength(1); j++)
                {
                    try
                        {
                        if (levelMatrice[i, j] != null && i * blocH < Scope.Width + Scope.X)
                            g.DrawImage(levelMatrice[i, j], new Point(i * Level.blocH, j * Level.blocH));
                        }
                        catch (Exception)
                        {

                          
                        }
                        

                    
                }
            }
            //Dessin du joueur


            //Dessin des Entités
           


                foreach (var entity in  Level.currentLevel.GetEntities())
            {

                try
                {
                    ;
                    if (isInScope(entity.Hitbox))
                        if(entity is ActiveEntity)
                        {
                            var active = entity as ActiveEntity;
                           if (active != null)
                                g.DrawImage(active.Sprite,active.Hitbox.Location);
                        }
                         else if (entity is Porte)
                        {
                            var porte = entity as Porte;
                            if(porte != null)
                                g.DrawImage(porte.texture , porte.Hitbox.Location);
                        }
                   
                    
               
                }
                catch (Exception)
                {


                }
            }
            
            try
            {
                g.DrawImage(player.Sprite, new Point(player.Coordonates.x, player.Coordonates.y));
            }
            catch (Exception)
            {

              
            }
        }

    }
}
