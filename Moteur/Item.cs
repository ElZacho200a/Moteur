﻿using System;
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

        protected Item() 
        {
            Image = new Bitmap(Form1.RootDirectory + $"Assets/Items_sprite/{this.GetType().Name}.png");
        }
        public Bitmap GetImage()
        {
            return Image;
        }
        public abstract void OnCatch();
        public abstract void OnUse();
    }
}
