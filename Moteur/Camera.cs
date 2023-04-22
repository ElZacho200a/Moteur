using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Timers;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
using Timer = System.Timers.Timer;

namespace Moteur
{
    public class Camera 
    {
        public static int blocH => (int)(Width  * GameLoop.Cameras.Length / FOV);
        public static int FOV = 30;
        private byte frameCounter = 0;
        public  Player player;
        private  (int X, int Y, int Width, int Height ) Scope;
        public static int Height, Width;
        public Bitmap rawFront;
        public  PauseMenu PauseMenu;
        private DialogArea dialogArea;
        public int gameState = 0;
       

        public  (int X, int Y, int Width, int Height) GetScope()
        {
            return Scope;
        }

        public Camera(int Widht, int Heigt , int index) : base()
        {
            rawFront = new Bitmap(Widht,
                Heigt); // Une image Noir de la \n taille de l'écran permettant d'opti \nles rendu en mode Dark
           

          
           
            Height = Heigt;
            Width = Widht;
            Scope = (0, 0, Width, Height);


            player = new Player(this , index);
            if (Level.Players is null)
                Level.Players = new List<Player>();
            Level.Players.Add(player);
            if(index == 0)
                 new Level(12);
            PauseMenu = new PauseMenu(Width * 4 / 5, Height * 4 / 5 , player);
            dialogArea = new DialogArea(Width, Height);
            ResetScope();
            
            
            Console.WriteLine("JE peut print !!!");
            dialogArea.ToSay = "";

           
        }


        public  void Update(int index)
        {
           
            if (gameState != 0)
            {
               
              
            }
            else if (gameState == 0)
            {
                
                frameCounter = (byte)((frameCounter + 1) % 10);
                if (frameCounter % 10 == 0)
                {
                    if (OnTenTick != null)
                        OnTenTick();
                }


                
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


        public  bool isInScope(Rectangle rect)
        {
            if (Scope.X <= rect.X && rect.X <= Scope.Width + Scope.X)
                if (Scope.Y <= rect.Bottom && rect.Y <= Scope.Height + Scope.Y)
                    return true;
            return false;
        }


        public void UpdateScope()
        {
            var speedDouble = player.Speed1;
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

            if ((Scope.Y + (Scope.Height / 2) > (player.Coordonates.y) && player.sensY == -1) ||
                (Scope.Y + (Scope.Height /2) < player.Coordonates.y && player.sensY == 1))
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

       

        public  void ResetScope()
        {
            var levelWidht = Level.currentLevel.getCollisonMatrice().GetLength(0) * Level.blocH;
            if (Scope.Width > levelWidht)
                Scope.Width = levelWidht;
            else
                Scope.Width = Width;
            Scope.X = player.Coordonates.x - Scope.Width / 2;
            Scope.Y = player[1];
        }

        public Rectangle getRectFromScope()
        {
            return new Rectangle(Scope.X, Scope.Y, Scope.Width, Scope.Height);
        }

     

        public void rayControl(int index)
        {
            if (IsGamepadAvailable(index))
            {
                if (gameState == 0)
                {
                    if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
                    {
                        player.jump();
                    }

                    if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                    {
                        player.KeyUp();
                    }

                    if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
                    {
                        gameState = 1;
                    }
                    if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_RIGHT))
                    {
                        player.shoot();
                    }
                    
                    
                    var X = GetGamepadAxisMovement(index, GamepadAxis.GAMEPAD_AXIS_LEFT_X);
                    player.Acceleration1 = ((double)(player.getMaxSpeed * X), player.Acceleration1.ay);
                    if (player.isInWater())
                    {
                        var Y = GetGamepadAxisMovement(index, GamepadAxis.GAMEPAD_AXIS_LEFT_Y);
                        player.Speed1 = (player.Speed1.vx, (double)(player.getMaxSpeed * Y));
                    }
                    
                    
                }
                else if (gameState == 1)
                {
                    if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
                    {
                        gameState = 0;
                    } 
                }
                else if (gameState == 2)
                {
                    if (dialogArea.Finish)
                        if (IsGamepadButtonPressed(index, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                        {
                            dialogArea.Reset();
                            gameState = 0;
                        }
                    
                }
            }
            else if(index == 1)
            {
                if (Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_D) || Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_A))
                {
                    if (Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_A))
                        player.Acceleration1 = ((double)(player.getMaxSpeed * -1), player.Acceleration1.ay);
                    else
                        player.Acceleration1 = ((double)(player.getMaxSpeed ), player.Acceleration1.ay);
                }
                else
                {
                    player.Acceleration1 = ((double)(0), player.Acceleration1.ay);
                }
               
                if(Raylib.IsKeyPressed(Raylib_cs.KeyboardKey.KEY_SPACE))
                {
                    player.jump();
                }
                if(Raylib.IsKeyPressed(Raylib_cs.KeyboardKey.KEY_E))
                {
                    if(gameState == 0)
                        player.KeyUp();
                    if (gameState == 2)
                    {
                        if (dialogArea.Finish)
                        {
                            dialogArea.Reset();
                            gameState = 0;
                        }
                    }
                }
                if(Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_ESCAPE))
                {
                    if(gameState == 0 )
                        gameState = 1;
                    else if (gameState == 1)
                        gameState = 0;
                }
            }
        }
      
       public void ShowDialog(String Text)
        {
            dialogArea.ToSay = Text;
            gameState = 2;
        }

       public void ShowLifeBar(int pv)
       {
           
       }


        private Bitmap back;
        public  void rayDraw(int index)
        {
          
          
           
           
            Raylib.ClearBackground(Raylib_cs.Color.BLACK);
         
            Raylib.BeginMode2D(new Camera2D(new Vector2(-Scope.X + Width*index  , -Scope.Y) , Vector2.Zero, 0,1));
            //Dessin des Blocs
                //Teinte des Blocs
                var Tint = Raylib_cs.Color.WHITE;
                //Récuperation des Données Utiles
            var blocs = Level.currentLevel.getLevelMatrice();
            var Opacitymap = Level.currentLevel.BackgroundNeedded;
            var backGroundBloc = Level.currentLevel.BackGroundMatrice;
            var Backbounds = (backGroundBloc.GetLength(0), backGroundBloc.GetLength(1));
                //Setup des aires de Dessin
             Rectangle OptiDrawRect;
             if (Level.currentLevel.Dark)
                 OptiDrawRect = player.getRayonRectangle(player.Light);
             else
             {
                 OptiDrawRect = getRectFromScope();
                 OptiDrawRect.Height += Level.blocH;
             }   
                //Setup des Informations de Dessins
                var debX = OptiDrawRect.X / blocH;
                var debY = OptiDrawRect.Y / blocH;
                if (debX < 0)
                    debX = 0;
                if (debY < 0)
                    debY = 0;
                var endX = debX + OptiDrawRect.Width / blocH  +2;
                if (endX >= blocs.GetLength(0))
                    endX = blocs.GetLength(0);
                
                var endY = debY + OptiDrawRect.Height / blocH  +1 ;
                if (endY >= blocs.GetLength(1))
                    endY = blocs.GetLength(1) ;
                var DangerousAnim = Level.currentLevel.VoidArea.ELectricAnim;
                //Application des textures sur la fenêtre
                BeginScissorMode(index * Width + OptiDrawRect.X  -Scope.X
                    ,OptiDrawRect.Y -Scope.Y
                    ,OptiDrawRect.Width
                    ,OptiDrawRect.Height);
            for (int i = debX; i < endX; i++)
                for (int j = debY; j < endY; j++)
                {
                    try
                    {

                   
                    var x = i * blocH;
                    var y = j * blocH;
                    if (Level.currentLevel.haveBackground() )
                    {
                        var BackTexture =(Texture2D)(backGroundBloc[i % Backbounds.Item1, j % Backbounds.Item2]);
                        DrawTexture(BackTexture , x , y ,Tint);
                       
                    }

                    if (Level.currentLevel.VoidArea[i, j])
                    {
                        Raylib.DrawTexture(DangerousAnim.GetImage(DangerousAnim.cursor) , x ,y , Raylib_cs.Color.WHITE);
                    }
                    if (blocs[i, j] != null)
                    {
                        var BlocTexture = (Texture2D)(blocs[i, j]);
                        DrawTexture(BlocTexture, x, y, Tint);
                    }
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
                // Dessin des Entités
                foreach (var entity in Level.currentLevel.GetEntities())
                {
                    try
                    {
                        if (entity.Hitbox.IntersectsWith(OptiDrawRect) )
                            if (entity is ActiveEntity)
                            {
                                var active = entity as ActiveEntity;
                                if (active != null)
                                    Raylib.DrawTexture(active.Sprite , active[0] , active[1],Raylib_cs.Color.WHITE);
                            }
                            else if (entity is Porte)
                            {
                                var porte = entity as Porte;
                                Raylib.DrawTexture(porte.texture , porte[0] , porte[1],Raylib_cs.Color.WHITE);
                            }
                    }
                    catch (Exception)
                    {
                    }
                }
                    
                //Dessin du Joueur
                foreach (var player in Level.Players)
                    if(isInScope(player.Hitbox))
                     DrawTexture(player.Sprite , player[0] , player[1], Raylib_cs.Color.WHITE);
                
               
                //Dessin de l'eau Devant le joueur

                if (Level.currentLevel.WaterArea is not null)
                {
                  Level.currentLevel.WaterArea.draw(this.getRectFromScope());
                    
                }
                
                
                 EndScissorMode();
                //
                //Ajout sur l'écran en fonction des particularité de la Room ou de l'état du jeu
                if (Level.currentLevel.Dark)
                {
                    var front = (Texture2D)player.DarkFront;
                    var p = player.getCenter();
                    p.Offset(-front.width / 2, -front.height / 2);
                    BeginScissorMode(index * Width , 0, Width , Height);
                   DrawTexture(front,p.X,p.Y,Raylib_cs.Color.WHITE);
                   EndScissorMode();
                }
                if(index == 1)
               
               EndMode2D();
               Raylib.BeginMode2D(new Camera2D(new Vector2(Width*index  , 0) , Vector2.Zero, 0,1));

                if (gameState == 1)
                {
                    PauseMenu.Draw();
                }else if (gameState == 2)
                {
                    if (dialogArea.ShowAndDraw())
                    {
                    
                    }
                   
                }
                EndMode2D();
                
        }

       
        
    }
    
}