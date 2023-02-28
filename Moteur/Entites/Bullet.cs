using System.Security.Permissions;

namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        private Player? player; // Le ? �vite un warning inutile mais est dans les fait facultatif
        private static int size = Level.blocH /10;
        private static Bitmap img = Image();
        public Bullet(int x, int y)
        {
            player = Camera.player; // Le player est d�j� une ressource statique
            Coordonates = (x, y + player.Hitbox.Height / 4); //Setup des coordonn�e
            Sprite = img; // Voir la Fonction Image  , elle dis tout
            Hitbox = new Rectangle(x, y, Sprite.Width, Sprite.Height); // Une fois l'image d�fini on setup la Hitbox
             // J'ai enlev� la gravit� statique    
            Speed.vx = 30 * player.sensX;  // la vitesse est d�fini par le sens du player
            Acceleration.ax = 30 * player.sensX; // manque le sens j'y arrive pas jsuis con wallah 
           
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

        private  static Bitmap Image() // On peut se permettre de d�ssiner nous m�me l'image de la balle et ainsi �vit� de pollu� L'�diteur
        {// De plus le dessin sur place �vite la lecture d'un fichier acc�l�rant ainsi le processus
            var img = new Bitmap(size, size, Camera.player.Sprite.PixelFormat); // Cr�er une Image de Vide de Dimension Level.blocH / 6XLevel.blocH / 6
            
            using Graphics g = Graphics.FromImage(img); // Cr�er provisoirement un Graphics permettant de Dessiner dans l'image
            {
                //Rempli l'image en Noir
                g.Clear(Color.Black);
            }
            return img;
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

