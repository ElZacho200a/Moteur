using System.Drawing.Drawing2D;
using System.Media;
using Moteur.Entites;
using Moteur.Items;
using Raylib_cs;
using Color = System.Drawing.Color;
using Keys = System.Windows.Forms.Keys;
using Rectangle = System.Drawing.Rectangle;
using NAudio.Wave;

namespace Moteur
{
    public class Player : LivingEntity
    {
        protected  new int MaxSpeed
        {
            get { return Level.blocH / (isInWater() ? 8 : 4); }
        }

        public override double Gravity
        {
            get
            {
                if (isInWater())
                    return 0;
                else
                    return Level.blocH / 130.0;
            }
        }

        public bool hasSaved = true;
        private Texture2D? darkFront;
        private Point LastPos;
        private List<Item> inventory;
        private bool canshoot = false;
        public Camera Camera;
        public int index;
        private bool isInWaterPos => Hitbox.Height < Hitbox.Width;
      
        private SoundManager _soundManager = new SoundManager();
        public List<Item> Inventory
        {
            get => inventory;
        }

        public IEnumerable<Item> GetKey
        {
            get
            {
                var key = from keys in inventory
                    where keys.GetType() == typeof(Keys)
                    select keys;
                foreach (var cle in key)
                {
                    yield return cle;
                }
            }
        }

        public bool CanShoot
        {
            get => canshoot;
            set => canshoot = value;
        }


        public Texture2D? DarkFront
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
                
                if(darkFront !=null)
                    Raylib.UnloadTexture((Texture2D)darkFront);
                darkFront = GenerateDarkFront();
            }
        }

        public int getMaxSpeed => MaxSpeed;
        public Player(Camera camera , int index)
        {
             Camera = camera;
            this.index = index ;
            inventory = new List<Item> { };
            var path = index  > 0 ? @"Assets\Sprite\PlayerSprite1.png" : @"Assets\Sprite\PlayerSprite.png" ;
            spriteManager = new SpriteManager(Program.RootDirectory +path , 100 , 50); 
            Coordonates = (Level.blocH*2,Level.blocH*4);
            LastPos = new Point(0, 0);
            light = 5;
            Hitbox = new Rectangle(0, 0, Level.blocH, Level.blocH * 2);
           
            Camera.AddSubscriberTenTick(UpdateAnimation);
        }

      


        public override void Update()
        {
           
            if (!isInWaterPos && isInWater())
            {
                spriteManager.Destroy();
                spriteManager = new SpriteManager(Program.RootDirectory +@"Assets\Sprite\SwimSprite.png" , 50 , 100);
                Sprite = spriteManager.GetImage(0);
                Coordonates= (this[0] , this[1] +Hitbox.Height / 2);
                
                Hitbox.Height = Sprite.height;
                Hitbox.Width = Sprite.width;
                
            }else if(isInWaterPos && !isInWater())
            {
                spriteManager.Destroy();
                var path = index  > 0 ? @"Assets\Sprite\PlayerSprite1.png" : @"Assets\Sprite\PlayerSprite.png" ;
                spriteManager = new SpriteManager(Program.RootDirectory +path , 100 , 50);
                Sprite = spriteManager.GetImage(0);   
                Coordonates= (this[0] , this[1] -Hitbox.Height );
                Hitbox.Height = Sprite.height;
                Hitbox.Width = Sprite.width;
            }
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            try
            {
            AdaptAnimation();
            }
            catch (Exception e)
            {
                
            }
           
            Moove();
            
            
            if (Speed.vy > 0 && Level.currentLevel.VoidArea.isCollidedWithEntity(this))
            {
               die();
            }
           
        }


        public void die()
        {
            (int a , (int x , int y ) Ppos ) = GameLoop.LoadSave();
            Speed = (0, 0);
            Acceleration = (0, 0);
            Coordonates = Ppos;
            Camera.ResetScope();
        }
        
        public void KeyPressed(int sens)
        {
            Acceleration.ax = sens * MaxSpeed;
        }
        
        
        
        
        public delegate void MyEventHandler( int index);
        public event MyEventHandler MyEvent;
       

        public void KeyUp()
        {
            
            if(MyEvent != null)
                MyEvent(index);
        }
        public void AddSubscriber(MyEventHandler sub)
        {
            MyEvent += sub;
        }
 
        public void DelSubscriber(MyEventHandler sub)
        {
            MyEvent -= sub;
        }

        public void ResetSucribers()
        {
            MyEvent = null;
        }

        private void AdaptAnimation()
        {
            if(isInWaterPos)
                return;
                
            if (!IsCollided((Coordonates.x, Coordonates.y + 1))) // Equivalent de le joueur est sur le sol
            {
                if (sensY > 0)
                {
                  
                    Sprite = spriteManager.GetImage(6, sensX);
                }
                else
                {
                    if(spriteManager .cursor == 4 ||spriteManager .cursor == 5 )
                        Sprite = spriteManager.GetImage(5, sensX);
                    Sprite = spriteManager.GetImage(4, sensX);
                }
                
            }
           
        }
        protected  override void UpdateAnimation()
        {

            if (isInWaterPos)
            {
                if (((int)(Speed.vx)) * sensX < 3)
                    Sprite = spriteManager.GetImage(0 , sensX);
                else if (spriteManager.cursor == 0)
                    Sprite = spriteManager.GetImage(1, sensX);
                else if (spriteManager.cursor == 1 )
                    Sprite = spriteManager.GetImage(2, sensX);
                else
                    Sprite = spriteManager.GetImage(0, sensX);
                return;
            }

            if (IsCollided((Coordonates.x, Coordonates.y + 1)) )// Equivalent de le joueur est sur le sol
            {
                if (((int)(Speed.vx)) * sensX < 3)
                    Sprite = spriteManager.GetImage(0 , sensX);
                else if (spriteManager.cursor == 1 ||spriteManager.cursor == 2)
                    Sprite = spriteManager.GetImage((byte)(spriteManager.cursor +1), sensX);
                else
                    Sprite = spriteManager.GetImage(1, sensX);
                if (!hasSaved)
                {
                    GameLoop.SaveGame();
                    hasSaved = true;
                }
                    
              
            }

            
                
        }
        public void ResetSprite()
        {
            byte saveCursor = spriteManager.cursor;
            var saveManager = spriteManager.getOriginal();
            spriteManager.Destroy();
            spriteManager = saveManager;
            Sprite = spriteManager.GetImage(saveCursor, sensX);
            Hitbox .Width = Sprite.width;
            Hitbox .Height = Sprite.height;
        }
        public void receiveItem(Item item)
        {
            if (item == null) { return; }
            Inventory.Add(item);
        }


        private bool DoubleJump = false;
        public void jump()
        {
            var max = Level.blocH / 4;
            // une vitesse négative est dirigée vers le haut tout du moins en Y
            if (IsCollided((Coordonates.x, Coordonates.y + 1)))
            {
                Speed.vy = (-max - Math.Abs(Speed.vx) / 6);
                DoubleJump = true;
             
            }
           
            else if (IsCollided((this[0] + (int)Speed.vx, Coordonates.y + (int)Speed.vy)) &&
                     (Raylib.GetGamepadAxisMovement(index, GamepadAxis.GAMEPAD_AXIS_LEFT_X) > 0 ? 1 : -1) != sensX &&
                      sensX != 0)
                   
            {
                
                    Speed.vy = (-max - Math.Abs(Speed.vx) / 6);
               
               

            }
            else if (DoubleJump)
            {
                DoubleJump = false;
                Speed.vy = (-max - Math.Abs(Speed.vx) / 6);
            }
            else
            {
                return;
            }
            _soundManager.jumpSong();
        }

        public bool shoot()
        {
            if (canshoot)
            {
                var bullet = new Bullet(Coordonates.x, Coordonates.y , this);
                Level.currentLevel.addEntity(bullet); // j'ajoute l'entite au bag
                return true;
                
                /*
                var balles = from balle in Inventory
                    where balle.GetType() == typeof(Bullets)
                    select balle;
                if (balles.Any())
                {
                    
                }*/
            }

            return false;
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
        
        private Texture2D GenerateDarkFront()
        {
            var neededDecal = (0 * Level.blocH);

            var transparent = new Raylib_cs.Color(50, 40, 0, 100);
            var rect = getRayonRectangle((int)(Light ));
            rect.Width =  (int)(rect.Width );
            rect.Height =  (int)(rect.Height );
            RenderTexture2D texture = Raylib.LoadRenderTexture(rect.Width, rect.Height);
            Raylib.BeginTextureMode(texture);
            Raylib.ClearBackground(Raylib_cs.Color.BLACK);
            Raylib.DrawCircleGradient(
                rect.Width/2,
                rect.Height/2, 
                Math.Max(rect.Width/2, rect.Height/2), 
                transparent, 
                Raylib_cs.Color.BLACK);
            Raylib.EndTextureMode();
            var texture2D = texture.texture;
            return texture2D;



        }
    }
}
