namespace Moteur
{
    public class SpriteManager
    {
        Bitmap[] Sprite;
        public int Height , Width ;
        public byte cursor = 0;
        private String name;
        private string filename;
        private int h, w;
        public SpriteManager(String filename , int h , int w)
        { 
            Bitmap img = new Bitmap(filename);
            this.filename = filename;
            this.h = h;
            this.w= w;
            int nmbSprite = img.Width / w;
                img = new Bitmap(img, new Size(img.Width * Level.blocH / 50, img.Height * Level.blocH / 50 ));
                Height = img.Height;
                Width = img.Width / nmbSprite ;
                Sprite = new Bitmap[img.Width  * 2/ Width];
                name = Path.GetFileName(filename) + $"X{Sprite.Length / 2}";
          

            fillSprite(img);
        }
        public Bitmap GetImage(byte toGet , int sens)
        {
            cursor = toGet;
            if (sens < 0)
                toGet += (byte)(Sprite.Length / 2 );
            
            return Sprite[toGet];
        }

        public override string ToString()
        {
            return name;
        }
        public void setSprite(Bitmap b)
        {
            Sprite[cursor] = b;
        }
        public SpriteManager getOriginal()
        {
           return new SpriteManager(filename, h, w);
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
