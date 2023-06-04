namespace Moteur.OnlineClass.Serveur;

public class OnlinePlayer : Player
{
    public OnlinePlayer(Camera camera, int index) : base(camera, index)
    {
        
    }

    public void setState(string data)
    {
        var state = data.Split("|");
        Sprite = spriteManager.GetImage(Convert.ToByte(state[0]));
        Coordonates = (Convert.ToInt32(state[1]), Convert.ToInt32(state[2]));
        if (state.Length > 3)
        {
            switch (state[3])
            {
                case "KeyUp":
                    KeyUp();
                    break;
                case "shoot":
                    shoot();
                    break;
            }
        }

    }
}