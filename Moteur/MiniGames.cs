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

    public MiniGames(int Width, int Height, Player _player)
    {
        this.player = _player;
        double ratio =  Width / (double)(baseImage.width);
       
        itemSize = (int)(itemSize * ratio); // La taille des Slot
        
        //On défini l'origine de manière à ce que le menu soit centré
        Origin = new Point((Camera.Width - Width) / 2, (Camera.Height - Height) / 2);
        //On la redimensionne
        Raylib.ImageResize(ref baseImage , Width,Height);
        resizedImage = Raylib.LoadTextureFromImage(baseImage);

    }
}