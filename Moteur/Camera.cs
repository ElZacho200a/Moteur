using System.Drawing.Drawing2D;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Moteur
{
    internal class Camera : Panel
    {
        public static int blocH => (int)(Width / FOV);
        public static int FOV = 30;
        private byte frameCounter = 0;
        public static Player player;
        private static (int X, int Y, int Width, int Height ) Scope;
        public static int Height, Width;
        public Bitmap rawFront;

        public static (int X, int Y, int Width, int Height) GetScope()
        {
            return Scope;
        }

        public Camera(int Widht, int Heigt) : base()
        {
            rawFront = new Bitmap(Widht,
                Heigt); // Une image Noir de la taille de l'écran permettant d'opti les rendu en mode Dark
            using (var g = Graphics.FromImage(rawFront))
            {
                g.Clear(Color.Black);
            }


            Height = Heigt;
            Width = Widht;

            this.Size = new System.Drawing.Size(Widht, Height);
            DoubleBuffered = true; // Extrêmement important permet d'avoir une image fluide 
            Scope = (0, 0, Widht, Height);


            player = new Player();
            new Level(0);

            ResetScope();
            Timer timer = new Timer();
            timer.Interval = 1000 / 60;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }


        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
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
            if (Scope.X <= rect.X && rect.Right <= Scope.Width + Scope.X)
                if (Scope.Y <= rect.Bottom && rect.Y <= Scope.Height + Scope.Y)
                    return true;
            return false;
        }


        public void UpdateScope()
        {
            var speedDouble = player.GetSpeed();
            if (Width > Level.currentLevel.GetRealSize.w * blocH)
                Scope.Width = Level.currentLevel.GetRealSize.w * blocH;
            else
            {
                Scope.Width = Width;
            }

            (int vx, int vy ) speedInt = ((int)speedDouble.vx, (int)speedDouble.vy);
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            var levelHeight = Level.currentLevel.getCollisonMatrice().GetLength(1) * Level.blocH;
            if ((Scope.X + Scope.Width / 3 > (player.Coordonates.x) && player.sensX == -1) ||
                (Scope.X + (Scope.Width / 3) < player.Coordonates.x && player.sensX == 1))
            {
                // Changement de la caméra en X


                Scope.X += speedInt.vx;
            }

            if ((Scope.Y + (Scope.Height / 3) > (player.Coordonates.y) && player.sensY == -1) ||
                (Scope.Y + (Scope.Height / 3 * 2) < player.Coordonates.y && player.sensY == 1))
            {
                // Changement de la caméra en X


                Scope.Y += speedInt.vy;
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

            if ((Scope.Y + Scope.Height) + (speedInt.vy) >= levelHeight)
                Scope.Y = levelHeight - Scope.Height;
            else if (Scope.Y + speedInt.vy <= 0)
                Scope.Y = 0;
        }

        public static int BlocSizeSetter((int w, int h ) size)
        {
            if (Width > size.w * Width / FOV)
            {
                return Width / (size.w);
            }


            return Width / FOV;
        }

        public static void ResetScope()
        {
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            if (Scope.Width > levelWidht)
                Scope.Width = levelWidht;
            else
                Scope.Width = Width;
            Scope.X = player.Coordonates.x - Scope.Width / 2;
            Scope.Y = player.Coordonates.y;
        }

        public Rectangle getRectFromScope()
        {
            return new Rectangle(Scope.X, Scope.Y, Scope.Width, Scope.Height);
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
                /*case Keys.Enter:
                {
                    player.shoot();
                    break;
                }*/
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var levelMatrice = Level.currentLevel.getLevelMatrice();
            var pCoord = player.Coordonates;

            var decalY = Screen.FromControl(this).Bounds.Height;
            decalY -= levelMatrice.GetLength(1) * Level.blocH;
            // translation des tout les élement 
            // Cette décal permet de mettre le bas du niveau en bas de l'écran cette utilité est voué à disparaitre
            //g.TranslateTransform(0.0F, (float)decalY, MatrixOrder.Append);
            //Translation des sprites en fonction des coord du joueur


            g.TranslateTransform(-Scope.X, -Scope.Y);


            //Variable pour l'optimisation de l'affichage

            Rectangle OptiDrawRect;
            if (Level.currentLevel.Dark)
            {
                OptiDrawRect = player.getRayonRectangle(player.Light * 3 / 4);
            }
            else
            {
                OptiDrawRect = getRectFromScope();
                OptiDrawRect.Height += Level.blocH;
            }

            //Dessin du joueur


            //Dessin des Entités


            foreach (var entity in Level.currentLevel.GetEntities())
            {
                try
                {
                    if (entity.Hitbox.IntersectsWith(OptiDrawRect) && isInScope(entity.Hitbox))
                        if (entity is ActiveEntity)
                        {
                            var active = entity as ActiveEntity;
                            if (active != null)
                                g.DrawImage(active.Sprite, active.Hitbox.Location);
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

            if (Level.currentLevel.Dark)
            {
                var front = player.DarkFront;
                var p = player.getCenter();
                p.Offset(-front.Width / 2, -front.Height / 2);
                g.DrawImage(front, p);
            }
        }


        protected override void OnPaintBackground(PaintEventArgs paintEventArgs)
        {
            var g = paintEventArgs.Graphics;
            g.TranslateTransform(-Scope.X, -Scope.Y);
            drawBlocs(g);
        }


        protected void drawBlocs(Graphics g)
        {
            Rectangle OptiDrawRect;
            if (Level.currentLevel.Dark)
                OptiDrawRect = player.getRayonRectangle(player.Light);
            else
            {
                OptiDrawRect = getRectFromScope();
                OptiDrawRect.Height += Level.blocH;
            }

            var levelMatrice = Level.currentLevel.getLevelMatrice();
            var BackGroundMatrice = Level.currentLevel.BackGroundMatrice;
            var BackW = BackGroundMatrice.GetLength(0);
            var BackH = BackGroundMatrice.GetLength(1);
            var debX = OptiDrawRect.X / blocH;
            var debY = OptiDrawRect.Y / blocH;
            if (debY >= levelMatrice.GetLength(1))
                debY = 0;
            if (debX >= levelMatrice.GetLength(0))
                debX = 0;
            var endX = debX + OptiDrawRect.Width / blocH;
            var endY = debY + OptiDrawRect.Height / blocH;
            var p = player.getCenter();
            p.X /= Level.blocH;
            p.Y /= Level.blocH;
            // Dessin du niveau
            if (levelMatrice != null)
                for (int i = debX; i <= endX; i++)
                {
                    for (int j = debY; j <= endY; j++)
                    {
                        try
                        {
                            // if(Level.currentLevel.Dark && player.isInLightRadius(i,j))
                            //    continue;
                            if (Level.currentLevel.haveBackground() && !Level.currentLevel.getCollisonMatrice()[i, j])
                            {
                                g.DrawImage(BackGroundMatrice[i % BackW, j % BackH], i * Level.blocH, j * Level.blocH);
                            }

                            if (levelMatrice[i, j] != null)
                                g.DrawImage(levelMatrice[i, j], i * Level.blocH, j * Level.blocH);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            var portes = from entity in Level.currentLevel.GetEntities() where entity is Porte select entity;
            foreach (Porte porte in portes)
            {
                g.DrawImage(porte.texture, porte.Hitbox.Location);
            }
                
        }
    }
}