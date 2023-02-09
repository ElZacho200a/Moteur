using System.Security.Permissions;

namespace Moteur.Entites
{
    internal class Bullet : LivingEntity
    {
        /*public override double Gravity { get; set; } = 20 / Level.blocH;*/ //override pour modifier la gravite sans toucher celle des autres entites
        //public static Player _player;
        //public static LivingEntity mob;
        public Bullet (int x, int y)
        {
            Coordonates = (x,y);
            spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Bullet.png", 8, 8);
            Hitbox = new Rectangle(x, y, spriteManager.Width, spriteManager.Height);
            Sprite = spriteManager.GetImage(0, sensX);
            Speed.vx = 10;
            Acceleration.ax = 10 * sensX; // manque le sens j'y arrive pas jsuis con wallah 
            //_player = Camera.player;
            //mob = new Zombie(x, y); 
        }
        public override void Update()
        {
            UpdateAnimation();
            if (Moove())
            {
                var level = Level.currentLevel;
                level.RemoveEntity(this); //si contact avec map => suppression 
            }

            /*if (Camera.player.Hitbox.IntersectsWith(this.Hitbox)) // a patch parce que pour l'instant direct en contact avec le joueur
            {
                var level = Level.currentLevel;
                level.RemoveEntity(this);
                _player.GetLife -= 1;
            }*/
            /*if (Hitbox.IntersectsWith(mob.Hitbox))
            {
                mob.GetLife -= 1;
                var level = Level.currentLevel;
                level.RemoveEntity(this);
            }*/
        }
        
        protected override void UpdateAnimation()
        {
            Sprite = spriteManager.GetImage(0, -sensX);
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
        }
        
    }
}

