namespace Moteur
{
    public static class Program
    {
        
         public static string RootDirectory ;

        public static Camera Camera;
        
        public static void Main(string[] args)
        {


            uint nbPlayers = 1;
            if (args.Length > 0)
            {
                nbPlayers = Convert.ToUInt32(args[0]);
                if (args.Length > 1)
                {
                    OnlinePass.start();
                }
            }
           
            var currentDirectory = Directory.GetCurrentDirectory();
            RootDirectory = currentDirectory.Split("bin")[0];
            var size = Screen.AllScreens[0].Bounds;
            // défini le nombre de joueurs
            GameLoop.Role = "Host";
            OnlinePass.start();
            GameLoop.start(nbPlayers);
           
        }
 
    }
}