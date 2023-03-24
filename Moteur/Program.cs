namespace Moteur
{
    public static class Program
    {
        
         public static string RootDirectory ;

        public static Camera Camera;
        public static void Main()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            RootDirectory = currentDirectory.Split("bin")[0];
            var size = Screen.AllScreens[0].Bounds;
            Camera = new Camera(1920,1080);
        }
 
    }
}