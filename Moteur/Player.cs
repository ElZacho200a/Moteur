using System.Drawing.Drawing2D;
using Moteur.Entites;

namespace Moteur
{
    internal class Player : LivingEntity
    {
        protected  new int MaxSpeed => Level.blocH/4;
        private Bitmap? darkFront;

        public Bitmap DarkFront
        {
            get
            {
                if (darkFront == null) 
                    darkFront = GenerateDarkFront();
                return darkFront;
            }
            
        }

        public new float Light
        {
            get => light;
            set
            {
                if(value <= 0)
                    return;
                light = value;
                darkFront.Dispose();
                darkFront = GenerateDarkFront();
            }
        }
        
        public Player()
        {
            spriteManager = new SpriteManager(Form1.RootDirectory +@"Assets\Sprite\PlayerSprite.png", 100 , 50); 
            Coordonates = (Level.blocH*2,Level.blocH*4);
            
            Hitbox = new Rectangle(0, 0, Level.blocH, Level.blocH * 2);
            Camera.AddSubscriberTenTick(UpdateAnimation);
        }
        public override void Update()
        {
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            AdaptAnimation();
            Moove();
            
          
        }
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }
        
        
        
        
        public delegate void MyEventHandler();
        public event MyEventHandler MyEvent;
       

        public void KeyUp()
        {
            
            if(MyEvent != null)
                MyEvent();
        }
        public void AddSubscriber(MyEventHandler sub)
        {
            MyEvent += sub;
        }
 
        public void DelSubscriber(MyEventHandler sub)
        {
            MyEvent -= sub;
        }

        private void AdaptAnimation()
        {
            if (!IsCollided((Coordonates.x, Coordonates.y + 1))) // Equivalent de le joueur est sur le sol
            {
                if (sensY > 0)
                {
                    Sprite = spriteManager.GetImage(4, sensX);
                }
                else
                {
                    Sprite = spriteManager.GetImage(3, sensX);
                }
            }
        }
        protected  override void UpdateAnimation()
        {
            if (IsCollided((Coordonates.x, Coordonates.y + 1)))// Equivalent de le joueur est sur le sol
            {
                if (((int)(Speed.vx)) * sensX < 3)
                    Sprite = spriteManager.GetImage(0 , sensX);
                else if(spriteManager.cursor != 1)
                    Sprite = spriteManager.GetImage(1, sensX);
                else
                    Sprite = spriteManager.GetImage(2, sensX);
            }
           
            
        }
        public void ResetSprite()
        {
            byte saveCursor = spriteManager.cursor;
            spriteManager = spriteManager.getOriginal();
            Sprite = spriteManager.GetImage(saveCursor, sensX);
            Hitbox .Width = Sprite.Width;
            Hitbox .Height = Sprite.Height;
        }
        public void jump()
        {
            
            // une vitesse négative est dirigée vers le haut tout du moins en Y
            if(IsCollided((Coordonates.x , Coordonates.y +1)))
            Speed.vy = (-MaxSpeed   -Math.Abs(Speed.vx) / 6) ;
        }

        public bool shoot()
        {
            var bullet = new Bullet(Coordonates.x, Coordonates.y);
            Level.currentLevel.addEntity(bullet); // j'ajoute l'entite au bag
            return true;
        }
        
        private PathGradientBrush GetGrandient( int Needed)
        {
           
            GraphicsPath path = new GraphicsPath();
            var rect = getRayonRectangle(Light);
            rect.X = Needed;
            rect.Y = Needed;
            path.AddEllipse(rect);

            // Use the path to construct a brush.
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);

            // Set the color at the center of the path to blue.
            pthGrBrush.CenterColor = Color.FromArgb(120, 0, 0, 0);

            // Set the color along the entire boundary 
            // of the path to aqua.
            Color[] colors = { Color.FromArgb(255, 0, 0, 0) };
            pthGrBrush.SurroundColors = colors;
            // 
            return pthGrBrush;
        }
        
        private Bitmap GenerateDarkFront()
        {
            var neededDecal = (4 * Level.blocH);
            var pthGrBrush = GetGrandient( neededDecal/2);
            
            var front = new Bitmap((int)pthGrBrush.Rectangle.Width +neededDecal ,(int)pthGrBrush.Rectangle.Height +neededDecal);
            using (var g = Graphics.FromImage(front))
            {
                g.Clear(Color.Black);
                
                g.CompositingMode = CompositingMode.SourceCopy;
                
                g.FillEllipse( pthGrBrush, pthGrBrush.Rectangle );
               
                g.CompositingMode = CompositingMode.SourceOver;
                pthGrBrush.Dispose();
               
            }

            
            return front  ;
        }
    }
}
