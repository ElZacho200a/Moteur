using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Moteur.Entites
{
    internal class BubbleInfo :ActiveEntity
    {

        private Entity entity;
        private Func<object> func;

        public BubbleInfo(Entity entity  , Func<object> getFunc)
        {
            this.entity = entity;
            func= getFunc;
            Sprite = new Bitmap(2 * Level.blocH, Level.blocH, Camera.player.Sprite.PixelFormat);
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
                g.DrawString( func().ToString(), font, brush, new PointF(10, 30));
            }
           
        }


       

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
