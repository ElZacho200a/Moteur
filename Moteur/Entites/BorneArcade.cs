namespace Moteur.Entites;

internal class BorneArcade : PNJ
{
    private MiniGames miniGames;
    private Player player;
    protected string Text;
    
    public BorneArcade(int x, int y, string text) : base(text)
    {
        Coordonates = (x, y);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets/Sprite/BorneArcade.png", 1);
        Sprite = spriteManager.GetImage(0);
        Text = text;
        Hitbox = new Rectangle(x, y, Sprite.width, Sprite.height);
        foreach (var player in Level.Players)
        {
            player.AddSubscriber(play);
            miniGames = new MiniGames(Camera.Width, Camera.Height, player);
        }
    }

    ~BorneArcade()
    {
        foreach (var player in Level.Players)
            player.DelSubscriber(play);
    }

    protected override void UpdateAnimation()
    {
        return;
    }

    public override string getArgument => Text;
    protected void play(int index)
    {
        if(Level.Players[index].Hitbox.IntersectsWith(Hitbox))
            miniGames.GAMING();
    }

    protected override void say(int index)
    {
        return;
    }


    public override void Update()
    {
        return;
    }
}