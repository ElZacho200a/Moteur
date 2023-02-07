using System.Net.Mime;

namespace Moteur.Entites;

internal abstract class PNJ:TriggerEntity
{
    protected string Text;
    public PNJ(string text) : base(8)
    {
        Text = text;
    }

    

   
}