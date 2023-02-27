using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    internal abstract class Item
    {
        protected int Unit;
        protected Bitmap Image;
        protected string Name;
        protected bool Catched = false;
        

        protected Item() 
        {
            Image = new Bitmap(Form1.RootDirectory + $"Assets/Items_sprite/{this.GetType().Name}.png");
        }
        public Bitmap GetImage()
        {
            return Image;
        }
        
        public Bitmap GetResizedImage()
        {
            return new Bitmap(Image ,Level.blocH,Level.blocH);
        }
        public virtual void OnCatch()
        {
            if (Catched)
                return;
            Catched = true;
        }
        public abstract void OnUse();
    }

   
}
