namespace Moteur.Entites;

internal abstract class PNJ:TriggerEntity
{
    protected string Text;
    public PNJ(string text) : base(5)
    {
        Text = text;
        foreach (var player in Level.Players)
         player.AddSubscriber(say);
        type = Enum.EntityType.PNJ;
    }

     ~PNJ()
    {
        foreach (var player in Level.Players)
            player.DelSubscriber(say);
    }
    public override string getArgument => Text;
    protected virtual void say(int index)
    {
        if(Level.Players[index].Hitbox.IntersectsWith(Hitbox))
            Level.Players[index].Camera.ShowDialog(Text);
    }
}