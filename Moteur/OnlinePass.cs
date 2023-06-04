using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using Moteur.OnlineClass.Serveur;

namespace Moteur;

public  static class OnlinePass
{
     static bool Host = true;
    public static string servIP = "127.0.0.1";
    public static int Port = 8888;
     static  byte[] buffer = new byte[4096];
     public static string RoomCode = "";
     public static List<OnlinePlayer> OnlinePlayers = new List<OnlinePlayer>();
     public static TcpClient client;



     public static void start() 
    {
        
        client = new TcpClient();

        try
        {
            client.Connect(servIP, Port);
            
        }
        catch (Exception e)
        {
          return;
        }
        // Connexion au serveur
        

        // Lecture du flux de données du serveur
        if (RoomCode != "")
            Ask(RoomCode);
        else
        {
            Ask("YO");
             setupRoom();
        }
           
    }

     private static void setupRoom()
     {
         Ask("" , false);
         NetworkStream stream = client.GetStream();
         int bytesRead = stream.Read(buffer, 0, buffer.Length);
         string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
         Console.WriteLine(response);
         RoomCode = response;  
     }
     
     
     public  static string GetPlayerStatus()
     {
         var player = GameLoop.Cameras[0].player;
         return $"{player.sensX},{player.getCurrentSprite()},{player[0]},{player[1]}\n";
     }

     private static string GetEntitiesStatus()
     {
         var result = "";
         var entities = Level.currentLevel.GetEntities().ToList();
         var serializer = new XmlSerializer(typeof(RawEntity));
         foreach (var ent in entities.Where(entity => entity is ActiveEntity))
         {
             var act = ent as ActiveEntity;
             act.CreateRawEntity();
             result += $"{act.sensX},{act.getCurrentSprite()},{act[0]},{act[1]}\n";
             
         }
         return result;
     }


     public static string Ask(string request , bool response = true)
     {
         if (client == null)
             return "No Serv Connected";
         byte[] requestData = Encoding.ASCII.GetBytes(request);

         try
         {

         
         // Obtenir le flux réseau du client
         NetworkStream stream = client.GetStream();

         // Envoyer la requête au serveur
         stream.Write(requestData, 0, requestData.Length);
         if(response)
         return GetResponse();
         return "";
         }
         catch (Exception e)
         {
             return "";
         }
     }

     private static string GetResponse()
     {
         NetworkStream stream = client.GetStream();
         int bytesRead = stream.Read(buffer, 0, buffer.Length);
         return Encoding.ASCII.GetString(buffer, 0, bytesRead);
     }
}