using Raylib_cs;
using Color = Raylib_cs.Color;
using Image = Raylib_cs.Image;

namespace Moteur;

public class VoidArea 
{
    private bool[,] DangerousMatrice;
    private List<Texture2D> DangerousBloc;
    private int time = 0 ;
    private List<Texture2D> BaseImages;
    public SpriteManager ELectricAnim;
    private int Bloch => Level.blocH;
    private Level _level;
    public VoidArea(int w, int h, Level level)
    {
        _level = level;
        DangerousMatrice = new bool[w, h];
        DangerousBloc = new List<Texture2D>();
        BaseImages = new List<Texture2D>();
        ELectricAnim = new SpriteManager(Program.RootDirectory + "Assets/Textures/ElecAnim.png" , 50 , 50 , false);
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
            if (!DangerousBloc.Contains((Texture2D)dangerousBloc))
            {
                BaseImages.Add((Texture2D)dangerousBloc);
                DangerousBloc.Add((Texture2D)dangerousBloc);
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

            ELectricAnim.nextCursor();
        }
        
        }
        catch (Exception e)
        {
           
        }
    }

    public void Destroy()
    {
        foreach (var variaTexture2D in BaseImages)
        {
            Raylib.UnloadTexture(variaTexture2D);
        }
        ELectricAnim.Destroy();
        foreach (var texture2D in DangerousBloc)
        {
            Raylib.UnloadTexture(texture2D);
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