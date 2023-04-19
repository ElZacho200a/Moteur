using Raylib_cs;
using Interrupteur = Moteur.Items.Interrupteur;

namespace Moteur;

internal class PassageSecret : Porte
{
    protected Interrupteur _interrupteur;
    public PassageSecret(int nextLevel, int x, int y, Texture2D Texture, Interrupteur interrupteur) : base(nextLevel, x, y, Texture)
    {
        texture = Texture;
        _interrupteur = interrupteur;
    }

    public override void Update()
    {
        Destroy();
    }
}