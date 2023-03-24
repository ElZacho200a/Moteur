namespace Moteur.Entites;

internal abstract class PNJ:TriggerEntity
{
    protected string Text;
    public PNJ(string text) : base(5)
    {
        Text = text;
        Camera.player.AddSubscriber(say);
    }

     ~PNJ()
    {
        Camera.player.DelSubscriber(say);
    }
    public override string getArgument => Text;
    protected void say()
    {
        if(Camera.player.Hitbox.IntersectsWith(Hitbox))
            Level.Camera.ShowDialog(Text);
    }
}