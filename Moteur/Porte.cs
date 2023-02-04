namespace Moteur;

internal class Porte :Sortie
{
    public Bitmap texture;
    public Porte(int nextLevel, int x, int y , Bitmap Texture) : base(nextLevel, x, y)
    {
        texture = Texture;
        Camera.player.AddSubscriber(HandleEvent);
        
        
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