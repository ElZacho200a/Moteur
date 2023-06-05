using System.Diagnostics;
using System.Net.Mime;
using Raylib_cs;
using Image = Raylib_cs.Image;

namespace Moteur;

public class MiniGames
{
    private Image baseImage;
    private Player player;
    private int itemSize;
    private Point Origin;
    private Texture2D resizedImage;
    private Camera camera;

    public MiniGames(int Width, int Height, Player player)
    {
        this.player = player;
        double ratio =  Width / (double)(baseImage.width);
       
        itemSize = (int)(itemSize * ratio); // La taille des Slot
        
        //On défini l'origine de manière à ce que le menu soit centré
        Origin = new Point((Camera.Width - Width) / 2, (Camera.Height - Height) / 2);
        //On la redimensionne
        Raylib.ImageResize(ref baseImage , Width,Height);
        resizedImage = Raylib.LoadTextureFromImage(baseImage);
    }

    public void GAMING()
    {
        // Chemin vers le programme à exécuter
        var root = Directory.GetCurrentDirectory().Split("Moteur");
        string cheminProgramme = root[0] + @"miniGame\miniGame\miniGame\bin\Debug\net7.0\miniGame.exe";

        // Créer un processus pour exécuter le programme
        Process processus = new Process();

        // Définir les informations du processus
        processus.StartInfo.FileName = cheminProgramme;
        processus.StartInfo.UseShellExecute = false;

        // Démarrer le processus
        processus.Start();

        // Attendre que le processus se termine
        processus.WaitForExit();

        // Récupérer le code de sortie du processus
        int codeSortie = processus.ExitCode;

        // Afficher le code de sortie
        Console.WriteLine("Code de sortie : " + codeSortie);
    }
}