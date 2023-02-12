using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Moteur.Entites
{
    internal class BubbleInfo :ActiveEntity
    {

        private Rectangle rect;
        private Func<object> func;

        public BubbleInfo(Entity entity  , Func<object> getFunc)
        {
            this.rect = entity.Hitbox;
            func= getFunc;
            Sprite = new Bitmap(2 * Level.blocH, Level.blocH, PixelFormat.Format32bppArgb);
           
        }

       
        protected override bool Moove()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateAnimation()
        {
            Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Handel Gothic", Hitbox.Width / 20, FontStyle.Bold);
            //On s'assure que le text ne dépasse pas 4 lignes ,sinon on le tronc

            // Dessinez le texte sur l'image

            using var g = Graphics.FromImage(Sprite);
            {
                g.Clear(Color.FromArgb(0,0,0,0));
                g.DrawEllipse(new Pen(brush),0,0,Level.blocH , Level.blocH);
                g.FillEllipse(new SolidBrush(Color.White),0,0,Level.blocH , Level.blocH);
                g.DrawString( func().ToString(), font, brush, new PointF(Level.blocH / 10, Level.blocH / 2));
            }
           
        }


       

        public override void Update()
        {
            if(rect == null)
                return;
            var milieuX = rect.Width / 2 + rect.X;
            Hitbox.X = milieuX - Level.blocH;
            Hitbox.Y = rect.Y - Level.blocH;
            UpdateAnimation();
        }
    }
}
