using System.Security.Permissions;

namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        private Player? player; // Le ? évite un warning inutile mais est dans les fait facultatif
        private static int size = Level.blocH /10;
        private static Bitmap img = Image();
        public Bullet(int x, int y)
        {
            player = Camera.player; // Le player est déjà une ressource statique
            Coordonates = (x, y + player.Hitbox.Height / 4); //Setup des coordonnée
            Sprite = img; // Voir la Fonction Image  , elle dis tout
            Hitbox = new Rectangle(x, y, Sprite.Width, Sprite.Height); // Une fois l'image défini on setup la Hitbox
             // J'ai enlevé la gravité statique    
            Speed.vx = 30 * player.sensX;  // la vitesse est défini par le sens du player
            Acceleration.ax = 30 * player.sensX; // manque le sens j'y arrive pas jsuis con wallah 
           
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

        private  static Bitmap Image() // On peut se permettre de déssiner nous même l'image de la balle et ainsi évité de pollué L'éditeur
        {// De plus le dessin sur place évite la lecture d'un fichier accélérant ainsi le processus
            var img = new Bitmap(size, size, Camera.player.Sprite.PixelFormat); // Créer une Image de Vide de Dimension Level.blocH / 6XLevel.blocH / 6
            
            using Graphics g = Graphics.FromImage(img); // Créer provisoirement un Graphics permettant de Dessiner dans l'image
            {
                //Rempli l'image en Noir
                g.Clear(Color.Black);
            }
            return img;
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

