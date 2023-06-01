namespace Moteur.OnlineClass.Serveur;

public class OnlinePlayer : Player
{
    public OnlinePlayer(Camera camera, int index) : base(camera, index)
    {
        
    }

    public override void Update()
    {
        var status = OnlinePass.GetPlayerStatus();
        Moove();
    }
}