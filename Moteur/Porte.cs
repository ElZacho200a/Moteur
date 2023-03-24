using Raylib_cs;

namespace Moteur;

internal class Porte :Sortie
{
    public Texture2D texture;
    public Porte(int nextLevel, int x, int y , Texture2D Texture) : base(nextLevel, x, y)
    {
        texture = Texture;
        Camera.player.AddSubscriber(HandleEvent);
        
        
    }

    public override void Destroy()
    {
        Raylib.UnloadTexture(texture);
    }

    private void HandleEvent()
    {
        if(!Camera.player.Hitbox.Contains(trigger))
            return;
        
        Camera.player.DelSubscriber(HandleEvent);
        LoadNextLevel();
    }
    public override void Update()
    {
     return; // Remplacer l'update systèmatique par un event   
    }
}