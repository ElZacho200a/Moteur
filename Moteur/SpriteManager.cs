using Raylib_cs;
using static Raylib_cs.Raylib;
using Image = Raylib_cs.Image;
using Rectangle = System.Drawing.Rectangle;

namespace Moteur
{
    public class SpriteManager
    {
        Texture2D[] Sprite;
        public int Height , Width ;
        public byte cursor = 0;
        private String name;
        private string filename;
        private int h, w;
        private bool Symetric;
        public  SpriteManager(String filename , int h , int w , bool symetric  = true)
        {
            unsafe
            {
                Image img = Raylib.LoadImage(filename);
                this.filename = filename;
                this.h = h;
                this.w= w;
                Symetric = symetric;
                int nmbSprite = img.width / w;
                ImageResize(ref img ,img.width * Level.blocH / 50, img.height * Level.blocH / 50 );
                Height = img.height;
                Width = img.width / nmbSprite ;
                if(symetric)
                    Sprite = new Texture2D[img.width  * 2/ Width];
                else 
                    Sprite = new Texture2D[img.width / Width];
                name = Path.GetFileName(filename) + $"X{Sprite.Length / 2}";
          

                fillSprite(img);
            }
        }
        public SpriteManager(String filename ,int nb_Animation, bool symetric  = true)
        { 
            Image img = Raylib.LoadImage(filename);
            this.filename = filename;
            this.h =  img.height;
            this.w= img.width / nb_Animation;
            Symetric = symetric;
            int nmbSprite = img.width / w;
            ImageResize(ref img ,img.width * Level.blocH / 50, img.height * Level.blocH / 50 );
            Height = img.height;
            Width = img.width / nmbSprite ;
            if(symetric)
                Sprite = new Texture2D[img.width  * 2/ Width];
            else 
                Sprite = new Texture2D[img.width / Width];
            name = Path.GetFileName(filename) + $"X{Sprite.Length / 2}";
          

            fillSprite(img);
        }
        public Texture2D GetImage(byte toGet , int sens)
        {
            cursor = toGet;
            if (sens < 0)
                toGet += (byte)(Sprite.Length / 2 );
            
            return Sprite[toGet];
        }

        public Texture2D nextCursor()
        {
            var img =  Sprite[cursor];
            cursor =(byte) ((cursor + 1) % Sprite.Length);
            return img;
        }
        public Texture2D GetImage(byte toGet ) // Dans le cas où le Sprite n'est pas Symétrique
        {
            cursor = toGet;
            return Sprite[toGet];
        }
        public override string ToString()
        {
            return name;
        }
        public void setSprite(Texture2D b)
        {
            Sprite[cursor] = b;
        }
        public SpriteManager getOriginal()
        {
           return new SpriteManager(filename, h, w);
        }
        public  void fillSprite(Raylib_cs.Image img)
        {
           
            Raylib_cs.Rectangle rect =  new Raylib_cs.Rectangle(0,0,Width,Height);

            var len = Sprite.Length ;
            if (Symetric)
                len /= 2;
            for (int i = 0; i < len; i++)
            {
                var support = Raylib.ImageCopy(img);
                Raylib.ImageCrop(ref support ,rect);
                Sprite[i] =  LoadTextureFromImage(support);
                Raylib.UnloadImage(support);
                rect.x += Width; 
            }
            if(!Symetric)
                return;
            Raylib.ImageFlipHorizontal(ref img);
            
            
            for (int i = Sprite.Length / 2; i < Sprite.Length ; i++)
            {
                rect.x -= Width;
                var support = Raylib.ImageCopy(img);
                Raylib.ImageCrop(ref support ,rect);
                Sprite[i] =  LoadTextureFromImage(support);
                Raylib.UnloadImage(support);
                
            }
        }

        public void Destroy()
        {
            foreach (var texture in Sprite)
            {
                Raylib.UnloadTexture(texture);
            }
        }


    }
}
