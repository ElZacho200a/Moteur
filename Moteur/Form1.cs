namespace Moteur
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            camera = new Camera(3840);
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
                case Keys.T:
                    if(Camera.player.IsCollided(Camera.player.Coordonates));
                    MessageBox.Show("The Player is in a block !!!");
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