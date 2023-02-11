namespace Moteur.Entites;

internal abstract class PNJ:TriggerEntity
{
    protected string Text;
    public PNJ(string text) : base(5)
    {
        Text = text;
    }

    

   
}