using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public abstract class Item
    {
        protected int Unit;
        protected Bitmap Image;
        protected string Name;
        protected bool Catched = false;
        protected string description = "Ceci est un item et quelqu'un à oublié de faire sa description shaaaammme";
        

        protected Item() 
        {
            Image = new Bitmap(Form1.RootDirectory + $"Assets/Items_sprite/{this.GetType().Name}.png");
        }
        public Bitmap GetImage()
        {
            return Image;
        }
        
        public Bitmap GetResizedImage(int size = -1)
        {
            if (size == -1)
                size = Level.blocH;
            return new Bitmap(Image ,size ,size);
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
