namespace Moteur
{
    public static class Program
    {
        public static string RootDirectory ;

        public static Camera Camera;
        public static void Main(string[] args)
        {
            uint x = 0;
            if (args.Length > 0)
                x = UInt32.Parse(args[0]);
            var currentDirectory = Directory.GetCurrentDirectory();
            RootDirectory = currentDirectory.Split("bin")[0];
            var size = Screen.AllScreens[0].Bounds;
            // défini le nombre de joueurs
            GameLoop.start(2);
        }
 
    }
}