using Moteur.Entites;

namespace Moteur
{
    public partial class Form1 : Form
    {
        public static String RootDirectory = @"C:\Users\zache\source\repos\Moteur\Moteur\";
        public Rectangle size;
        public Form1()
        {
            size = Screen.FromControl(this).Bounds;
            camera = new Camera(size.Width, size.Height);
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
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
                case Keys.Left:
                    Camera.player.KeyPressed(-1);
                    break;
                case Keys.Right:
                    Camera.player.KeyPressed(1);
                    break;
                case Keys.Space:
                    Camera.player.jump();
                    break;
               
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right )
                Camera.player.KeyPressed(0);
        }
    }
}