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
        public static int blocH => (int)(Width / FOV);
        public static int FOV = 30;
        private byte frameCounter = 0;
        public static Player player;
        private static (int X, int Y, int Width, int Height ) Scope;
        public static int Height, Width;
        public Bitmap rawFront;
        public  PauseMenu PauseMenu;
        private DialogArea dialogArea;
        public int gameState = 0;
       

        public static (int X, int Y, int Width, int Height) GetScope()
        {
            return Scope;
        }

        public Camera(int Widht, int Heigt) : base()
        {
            rawFront = new Bitmap(Widht,
                Heigt); // Une image Noir de la \n taille de l'écran permettant d'opti \nles rendu en mode Dark
            using (var g = Graphics.FromImage(rawFront))
            {
                g.Clear(Color.Black);
            }

          
            InitWindow(0,0 , "Test de la manette");
            //ToggleFullscreen();
            Widht = Raylib.GetMonitorWidth(0);
            Heigt = Raylib.GetMonitorHeight(0);
            //back = new Bitmap(Widht, Height);
            SetTargetFPS(60);
            Height = Heigt;
            Width = Widht;
            Scope = (0, 0, Width, Height);


            player = new Player();
            Level.Camera = this;
            new Level(0);
            PauseMenu = new PauseMenu(Width * 4 / 5, Height * 4 / 5 , player);
            dialogArea = new DialogArea(Width, Height);
            ResetScope();
            
            
            Console.WriteLine("JE peut print !!!");
            dialogArea.ToSay = "";

            while (!Raylib.WindowShouldClose())
            {
                GameLoop();
            }
        }


        private  void GameLoop()
        {
            rayControl();
            rayDraw();
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


                if (Level.currentLevel.Update())
                {
                    
                    player.Update();
                    // ajustement de la cam 
                    UpdateScope();
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


        public static bool isInScope(Rectangle rect)
        {
            if (Scope.X <= rect.X && rect.Right <= Scope.Width + Scope.X)
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

        public void OnInput(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                if (gameState == 0)
                    gameState = 1;
                else if (gameState == 1)
                    gameState = 0;
            if (gameState == 2)
            {
                if (e.KeyCode == Keys.E)
                {
                        gameState = 0;
                        dialogArea.Reset();
                    }
            }
            else if(gameState == 0)
            switch(e.KeyCode)
            {
                case Keys.E :
                    player.KeyUp();
                    break;
                case Keys.Left:
                    player.KeyPressed(-1);
                    break;
                case Keys.Right:
                    player.KeyPressed(1);
                    break;
                case Keys.Space:
                    player.jump();
                    break;
                case Keys.Z :
                    //gameState = 2;
                    break;
                case Keys.Enter: // le tir 
                    try // pour le debug
                    {
                        player.shoot();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("t'as merde frero");
                        throw;
                    }
                    break;

            }
        }

        protected void rayControl()
        {
            if (IsGamepadAvailable(0))
            {
                if (gameState == 0)
                {
                    if (IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
                    {
                        player.jump();
                    }

                    if (IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                    {
                        player.KeyUp();
                    }

                    if (IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
                    {
                        gameState = 1;
                    }
                    var X = GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_X);
                    player.Acceleration1 = ((double)(player.getMaxSpeed * X), player.Acceleration1.ay);
                }
                else if (gameState == 1)
                {
                    if (IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
                    {
                        gameState = 0;
                    } 
                }
                else if (gameState == 2)
                {
                    if (dialogArea.Finish)
                        if (IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                        {
                            dialogArea.Reset();
                            gameState = 0;
                        }
                    
                }
            }
            else
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
               
                if(Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_SPACE))
                {
                    player.jump();
                }
                if(Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_E))
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
       /* protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var levelMatrice = Level.currentLevel.getLevelMatrice();
            var pCoord = player.Coordonates;

           // var decalY = Screen.FromControl(this).Bounds.Height;
            //decalY -= levelMatrice.GetLength(1) * Level.blocH;
            // translation des tout les élement 
            // Cette décal permet de mettre le bas du niveau en bas de l'écran cette utilité est voué à disparaitre
            //g.TranslateTransform(0.0F, (float)decalY, MatrixOrder.Append);
            //Translation des sprites en fonction des coord du joueur


            g.TranslateTransform(-Scope.X, -Scope.Y);

            drawBlocs(g);
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
                    if (entity.Hitbox.IntersectsWith(OptiDrawRect) )
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
            //Ajout sur l'écran en fonction des particularité de la Room ou de l'état du jeu
            if (Level.currentLevel.Dark)
            {
                var front = player.DarkFront;
                var p = player.getCenter();
                p.Offset(-front.Width / 2, -front.Height / 2);
                g.DrawImage(front, p);
            }

            g.TranslateTransform(Scope.X ,Scope.Y);
            if (gameState == 1)
            {
                PauseMenu.Draw(g);
            }else if (gameState == 2)
            {
                if (dialogArea.ShowAndDraw(g))
                {
                    
                }
                   
            }
            
            
        }/*


        protected override void OnPaintBackground(PaintEventArgs paintEventArgs)
        {
         //   var g = paintEventArgs.Graphics;
           // g.TranslateTransform(-Scope.X, -Scope.Y);
           
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
                            if (Level.currentLevel.haveBackground() && !Level.currentLevel.BackgroundNeedded[i, j])
                            {
                                g.DrawImage(BackGroundMatrice[i % BackW, j % BackH], i * Level.blocH, j * Level.blocH);
                               //DrawBloc(i * Level.blocH , j  * Level.blocH ,BackGroundMatrice[i % BackW, j % BackH] , OptiDrawRect , g );
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
                
        }*/
       public void ShowDialog(String Text)
        {
            dialogArea.ToSay = Text;
            gameState = 2;
        }


        private Bitmap back;
        public unsafe void rayDraw()
        {
            BeginDrawing();
            Raylib.ClearBackground(Raylib_cs.Color.BLACK);
            Raylib.BeginMode2D(new Camera2D(new Vector2(-Scope.X , -Scope.Y) , Vector2.Zero, 0,1));
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
                var endX = debX + OptiDrawRect.Width / blocH + 1 ;
                var endY = debY + OptiDrawRect.Height / blocH + 1;
                var DangerousAnim = Level.currentLevel.VoidArea.ELectricAnim;
                //Application des textures sur la fenêtre
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
                
                DrawTexture(player.Sprite , player[0] , player[1], Raylib_cs.Color.WHITE);
                
                //
                //Ajout sur l'écran en fonction des particularité de la Room ou de l'état du jeu
                if (Level.currentLevel.Dark)
                {
                    var front = (Texture2D)player.DarkFront;
                    var p = player.getCenter();
                    p.Offset(-front.width / 2, -front.height / 2);
                   DrawTexture(front,p.X,p.Y,Raylib_cs.Color.WHITE);
                }

               EndMode2D();
                if (gameState == 1)
                {
                    PauseMenu.Draw();
                }else if (gameState == 2)
                {
                    if (dialogArea.ShowAndDraw())
                    {
                    
                    }
                   
                }
                    EndDrawing();
        }

       
        
    }
    
}