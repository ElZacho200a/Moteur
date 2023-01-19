using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class SpriteManager
    {
        Bitmap[] Sprite;
        int Height , Width ;
        public byte cursor = 0;
        public SpriteManager(String filename , int h , int w)
        { 
            Bitmap img = new Bitmap(filename);
            
            img = new Bitmap(img, new Size(img.Width / 50 * Level.blocH , img.Height / 50 * Level.blocH));
            Height = h / 50 * Level.blocH;
            Width = w / 50 * Level.blocH;
            Sprite = new Bitmap[img.Width / Width * 2];
            fillSprite(img);
        }
        public Bitmap GetImage(byte toGet , int sens)
        {
            cursor = toGet;
            if (sens < 0)
                toGet += (byte)(Sprite.Length / 2 );
            
            return Sprite[toGet];
        }

        public void fillSprite(Bitmap img)
        {
           
            Rectangle rect =  new Rectangle(0,0,Width,Height);
            
            for (int i = 0; i < Sprite.Length / 2; i++)
            {
                Sprite[i] = img.Clone(rect , img.PixelFormat);
                rect.X += Width; 
            }
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            
            for (int i = Sprite.Length / 2; i < Sprite.Length ; i++)
            {
                rect.X -= Width;
                Sprite[ i] = img.Clone(rect, img.PixelFormat);
                
            }
        }


    }
}
