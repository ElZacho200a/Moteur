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
            camera.mvPl(e.KeyCode);
            
        }

        
    }
}