using Raylib_cs;

namespace Moteur;

internal class Porte :Sortie
{
    public Texture2D texture;
    private SoundManager _soundManager = new SoundManager();
    public Porte(int nextLevel, int x, int y , Texture2D Texture) : base(nextLevel, x, y)
    {
        texture = Texture;
        foreach (var player in Level.Players)
         player.AddSubscriber(HandleEvent);
            
        
      
        
        
    }

    public override void Destroy()
    {
        Raylib.UnloadTexture(texture);
    }

    private void HandleEvent(int index)
    {
        if(!Level.Players[index].Hitbox.Contains(trigger))
            return;
        
        Level.Players[index].DelSubscriber(HandleEvent);
        _soundManager.doorSong();
        LoadNextLevel();
    }
    public override void Update()
    {
     return; // Remplacer l'update systèmatique par un event   
    }
}