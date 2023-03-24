using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using Image = Raylib_cs.Image;

namespace Moteur
{
    public abstract class Item
    {
        protected int Unit;
        protected Image Image;
        protected string Name;
        protected bool Catched = false;
        protected int Count = 1;
        protected string description = "Ceci est un item et quelqu'un à oublié de faire sa description shaaaammme";
        

        protected Item() 
        {
            Image = Raylib.LoadImage(Program.RootDirectory + $"Assets/Items_sprite/{this.GetType().Name}.png");
        }
        public Texture2D GetImage()
        {
            return Raylib.LoadTextureFromImage(Image);
        }

        public int GetCount
        {
            get => Count;
            set => Count = value;
        }
        
        public Texture2D GetResizedImage(int size = -1)
        {
            if (size == -1)
                size = Level.blocH;
            var img = Raylib.ImageCopy(Image);
            Raylib.ImageResize(ref img , size , size);
           
            return Raylib.LoadTextureFromImage(img);
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
