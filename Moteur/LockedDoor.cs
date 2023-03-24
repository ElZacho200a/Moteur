using Raylib_cs;
using Keys = Moteur.Items.Keys;

namespace Moteur;

internal class LockedDoor : Porte
{
    private Item _key;
    public LockedDoor(int nextLevel, int x, int y, Texture2D Texture, Keys keys) : base(nextLevel, x, y, Texture)
    {
        texture = Texture;
        Camera.player.AddSubscriber(HandleEvent);
        _key = keys;
    }
    private void HandleEvent()
    {
        if(!Camera.player.Hitbox.Contains(trigger))
            return;
        if (Camera.player.Inventory.Contains(_key))
        {
            Camera.player.DelSubscriber(HandleEvent);
            LoadNextLevel();
        }
    }
    public override void Update()
    {
        return; // Remplacer l'update systèmatique par un event   
    }
}