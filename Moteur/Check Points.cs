namespace Moteur;

internal class Check_Points : Entity
{
    public override void OnCatch()
    {
        Camera.player.GetSave = Camera.player.Coordonates;
        base.OnCatch();
    }

    public override void OnUse()
    {
        throw new NotImplementedException();
    }
}