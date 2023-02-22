namespace Moteur;

public class VoidArea 
{
    private bool[,] DangerousMatrice;
    private List<Bitmap> DangerousBloc;
    private int time = 0 ;
    private List<Bitmap> BaseImages;
    private SpriteManager ELectricAnim;
    private int Bloch => Level.blocH;
    private Level _level;
    public VoidArea(int w, int h, Level level)
    {
        _level = level;
        DangerousMatrice = new bool[w, h];
        DangerousBloc = new List<Bitmap>();
        BaseImages = new List<Bitmap>();
        ELectricAnim = new SpriteManager(Form1.RootDirectory + "Assets/Textures/ElecAnim.png" , 50 , 50 , false);
    }
    
    public bool this[int i , int j ]
    {
        get
        {
            return DangerousMatrice[i, j];
        }
        set
        {
            if (!value)
                return;




            var dangerousBloc = _level.getLevelMatrice()[i, j];
            if (dangerousBloc != null && !DangerousBloc.Contains(dangerousBloc))
            {
                BaseImages.Add((Bitmap)dangerousBloc.Clone());
                DangerousBloc.Add(dangerousBloc);
            }

            DangerousMatrice[i, j] = value;

        }
    }

    public  void UpdateAnimation()
    {
        try
        {

            time= (time +1) % 60;
           
        for (int i = 0; i < DangerousBloc.Count; i++)
        {
            var g = Graphics.FromImage(DangerousBloc[i]);
            g.Clear(Color.Transparent);
            g.DrawImage(BaseImages[i] , 0 ,0);
            if( time > 40) 
                g.DrawImage(ELectricAnim.nextCursor(), 0 , 0);
            g.Dispose();
        }
        
        }
        catch (Exception e)
        {
           
        }
    }

    public bool isCollidedWithEntity( Entity entity)
    {
        var r = entity.Hitbox;
        for (int i = entity[0]; i < entity[0] + r.Width; i++)
        {
            for (int j = entity[1]; j < entity[1] + r.Height; j++)
            {
                try
                {
                    if (DangerousMatrice[i / Bloch, j / Bloch])
                        return true;
                }
                catch (Exception e)
                {
                   
                }

                j += Bloch - (j % Bloch);
            }
            i += Bloch - (i% Bloch);
        }

        return false; 
    }


    public override string ToString()
    {
        String s = "";
        for (int i = 0; i < DangerousMatrice.GetLength(1); i++)
        {
            for (int j = 0; j < DangerousMatrice.GetLength(0); j++)
            {
                s += DangerousMatrice[j, i] ? "1" : "0";
            }

            s += "\n";
        }

        return s;
    }
}