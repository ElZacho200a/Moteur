namespace Moteur;

public class VoidArea : Entity
{
    private bool[,] DangerousMatrice;
    private List<Bitmap> DangerousBloc;
    private int time = 0 ;

    public VoidArea(int w, int h)
    {
        DangerousMatrice = new bool[w, h];
    }
    
    public bool this[int i , int j ]
    {
        get
        {
            return DangerousMatrice[i, j];
        }
        set
        {
             var dangerousBloc = Level.currentLevel.getLevelMatrice()[i, j];
             if (dangerousBloc != null && !DangerousBloc.Contains(dangerousBloc))
             {
                 DangerousBloc.Add(dangerousBloc);
             }
            DangerousMatrice[i, j] = value;
        }
    }

    public override void Update()
    {
        time = (time + 1 ) % 60;
    }
}