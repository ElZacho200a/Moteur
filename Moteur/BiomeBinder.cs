namespace Moteur;

public class BiomeBinder
{
    public static Dictionary<int , string> binder;
    public BiomeBinder()
    {
        var filename = Form1.RootDirectory + "Assets/ROOMS/BiomeBinder.bb";
        string rawData = File.ReadAllText(filename);
        var splittedData = rawData.Split("/");
        if (splittedData.Length % 2 != 0)
            throw new FileFormatException("BiomeBinder file seems to be not conform");
        for(int i = 0 ; i < splittedData.Length -1 ; i += 2)
        {
            foreach (string id  in splittedData[i+1].Split(";"))
            {
                try
                {
                    binder.Add(System.Int32.Parse(id),splittedData[i]);
                }
                catch (Exception e)
                {
                    throw new FileFormatException("BiomeBinder file seems to be not conform");
                }
                
            }
        }

    }
}