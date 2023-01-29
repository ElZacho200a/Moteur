using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur.Entites
{
    internal class ElRatz : LivingEntity
    {

        public ElRatz(int x, int y)
        {
            Coordonates = (x, y);
            Hitbox = new Rectangle(x, y, Level.blocH, Level.blocH);
            spriteManager = new SpriteManager(Form1.RootDirectory + "Assets\\Sprite\\Ratz.png", 50, 50);
            Sprite = spriteManager.GetImage(0, sensX);
            MaxSpeed = 10;
            Speed.vx = 0;
            Speed.vy = 0;
            Acceleration.ax = -MaxSpeed;
        }

        public override void Update()
        {
            UpdateAnimation();
            if (Moove())
            {
               
                Speed.vx *= -1 ;
               
            }
            Acceleration.ax =sensX * MaxSpeed;
            Hitbox.X = Coordonates.x;
            Hitbox.Y = Coordonates.y;
            if(Camera.player.Hitbox.IntersectsWith(Hitbox))
            {
                Level.currentLevel.removeEntity(this);
            }
        }

        protected override void UpdateAnimation()
        {
            Sprite = spriteManager.GetImage(0, -sensX);
        }
    }
}
