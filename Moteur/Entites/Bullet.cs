using System.Security.Permissions;
using Raylib_cs;
using Color = System.Drawing.Color;
using Image = Raylib_cs.Image;
using Rectangle = System.Drawing.Rectangle;

namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        private Player? player; // Le ? �vite un warning inutile mais est dans les fait facultatif
        private static int size = Level.blocH /10;
        
        public Bullet(int x, int y , Player player)
        {
            this.player = player; // Le player est d�j� une ressource statique
            Coordonates = (x, y + player.Hitbox.Height / 4); //Setup des coordonn�e
            Sprite = GetImage(); // Voir la Fonction Image  , elle dis tout
            Hitbox = new Rectangle(x, y, Sprite.width, Sprite.height); // Une fois l'image d�fini on setup la Hitbox
             // J'ai enlev� la gravit� statique    
            Speed.vx = 30 * player.sensX;  // la vitesse est d�fini par le sens du player
            Acceleration.ax = 30 * player.sensX; // manque le sens j'y arrive pas jsuis con wallah 
            type = Enum.EntityType.Bullet;

        }
        public override double Gravity => 0;
        public override void Update()
        {
            
            // On bouge la balle et check une potentielle collision avec un mob
            if (Moove() || IsCollidedWithMob()) 
            {
                //il est pr�f�rable de faire �a pour limit� la sur utilisation du Bag entities
                // Lorsqu'une entit� est morte elle est retir� automatiquement � la prochaine Frame
                isDead = true;  //si contact avec map => suppression 
            }
            else
            {
                Hitbox.X = this[0];
                Hitbox.Y = this[1];
            }

         
        }

        protected override void UpdateAnimation()
        {   // La balle n'ayant pas d'animation , cette m�thode est inutile
            throw new InvalidOperationException("Cette m�thode n'est pas cens� etre appel�e");
        }

        private  static Texture2D GetImage() // On peut se permettre de d�ssiner nous m�me l'image de la balle et ainsi �vit� de pollu� L'�diteur
        {// De plus le dessin sur place �vite la lecture d'un fichier acc�l�rant ainsi le processus
            var img = new Image(); // Cr�er une Image de Vide de Dimension Level.blocH / 6XLevel.blocH / 6
            var size = Level.blocH / 6;
            img.width = size;
            img.height = size;
            Raylib.ImageDrawRectangle(ref img ,0 ,0 ,size,size,Raylib_cs.Color.BLACK);
            var texture = Raylib.LoadTextureFromImage(img);
            Raylib.UnloadImage(img);
            return texture;
        }
        
        public bool IsCollidedWithMob()
        {
            var Mobs =// Si l'enti� est vivant et n'est pas un joueur
                from entity in Level.currentLevel.GetEntities()
                where entity != this && entity is not Player && entity is LivingEntity
                select entity;
            foreach (Entity entity in Mobs) // On parcours toute les entit�s
            {
             
                    if (this.Hitbox.IntersectsWith(entity.Hitbox)) // et si on est en contact avec elle
                    {
                        (entity as LivingEntity).Life -= 1; // Alors on lui inflige des dommages
                        return true; 
                    }
                
            }
            return false;
        }
    }
}

