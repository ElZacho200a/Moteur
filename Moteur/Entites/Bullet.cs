using System.Security.Permissions;
using Raylib_cs;
using Color = System.Drawing.Color;
using Image = Raylib_cs.Image;
using Rectangle = System.Drawing.Rectangle;

namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        private Player? player; // Le ? évite un warning inutile mais est dans les fait facultatif
        private static int size = Level.blocH /10;
        
        public Bullet(int x, int y , Player player)
        {
            this.player = player; // Le player est déjà une ressource statique
            Coordonates = (x, y + player.Hitbox.Height / 4); //Setup des coordonnée
            Sprite = GetImage(); // Voir la Fonction Image  , elle dis tout
            Hitbox = new Rectangle(x, y, Sprite.width, Sprite.height); // Une fois l'image défini on setup la Hitbox
             // J'ai enlevé la gravité statique    
            Speed.vx = 30 * player.sensX;  // la vitesse est défini par le sens du player
            Acceleration.ax = 30 * player.sensX; // manque le sens j'y arrive pas jsuis con wallah 
            type = Enum.EntityType.Bullet;

        }
        public override double Gravity => 0;
        public override void Update()
        {
            
            // On bouge la balle et check une potentielle collision avec un mob
            if (Moove() || IsCollidedWithMob()) 
            {
                //il est préfèrable de faire ça pour limité la sur utilisation du Bag entities
                // Lorsqu'une entité est morte elle est retiré automatiquement à la prochaine Frame
                isDead = true;  //si contact avec map => suppression 
            }
            else
            {
                Hitbox.X = this[0];
                Hitbox.Y = this[1];
            }

         
        }

        protected override void UpdateAnimation()
        {   // La balle n'ayant pas d'animation , cette méthode est inutile
            throw new InvalidOperationException("Cette méthode n'est pas censé etre appelée");
        }

        private  static Texture2D GetImage() // On peut se permettre de déssiner nous même l'image de la balle et ainsi évité de pollué L'éditeur
        {// De plus le dessin sur place évite la lecture d'un fichier accélérant ainsi le processus
            var img = new Image(); // Créer une Image de Vide de Dimension Level.blocH / 6XLevel.blocH / 6
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
            var Mobs =// Si l'entié est vivant et n'est pas un joueur
                from entity in Level.currentLevel.GetEntities()
                where entity != this && entity is not Player && entity is LivingEntity
                select entity;
            foreach (Entity entity in Mobs) // On parcours toute les entités
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

