using Moteur.Entites;

namespace Moteur
{
    public partial class Form1 : Form
    {
        public static String RootDirectory;
        public Rectangle size;
        private string currentDirectory = Directory.GetCurrentDirectory();
        public Form1()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            RootDirectory = currentDirectory.Split("bin")[0];
            size = Screen.FromControl(this).Bounds;
            camera = new Camera(size.Width, size.Height);
            InitializeComponent();
        }

        private void camera_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
            camera.mvPl(e.KeyCode);
            camera.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           switch(e.KeyCode)
            {
                case Keys.Up :
                    Camera.player.KeyUp();
                    break;
                case Keys.Left:
                    Camera.player.KeyPressed(-1);
                    break;
                case Keys.Right:
                    Camera.player.KeyPressed(1);
                    break;
                case Keys.Space:
                    Camera.player.jump();
                    break;
                case Keys.Z :
                    Level.currentLevel.Dark = !Level.currentLevel.Dark;
                    break;
                case Keys.S :
                    Camera.player.light--;
                    break;
                    break;
                case Keys.P :
                    Camera.player.light++;
                    break;
                case Keys.Enter: // le tir 
                    try // pour le debug
                    {
                        Camera.player.shoot();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("t'as merde frero");
                        throw;
                    }
                    break;

            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right )
                Camera.player.KeyPressed(0);
        }

        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {
          
        }
    }
}