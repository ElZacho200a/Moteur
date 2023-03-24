namespace Moteur.Entites;

internal class ElRatz : LivingEntity
{
    protected new int MaxSpeed => 10;
    public ElRatz(int x, int y)//Obligatoire selon la convention de Genève 
    {
        Coordonates = (x, y);
        Hitbox = new Rectangle(x,y,Level.blocH,Level.blocH);
        spriteManager = new SpriteManager(Program.RootDirectory + "Assets\\Sprite\\ElRatz.png", 50, 50);
        Sprite = spriteManager.GetImage(0, sensX);
      
        Acceleration.ax = MaxSpeed;
        Life = 10;
       }

    public override void Update()
    {
        if (Moove())
        {
            Speed.vx = Speed.vx * -1;
        }
        UpdateAnimation();
        Acceleration.ax = MaxSpeed * sensX;

        if (Camera.player.Hitbox.IntersectsWith(Hitbox))
        {
            this.Life -= 1;
        }

        if (this.Life == 0)
        {
            isDead= true;
        }
    }

    protected override void UpdateAnimation()
    {
        Sprite = spriteManager.GetImage(0, -sensX);
        Hitbox.X = Coordonates.x;
        Hitbox.Y = Coordonates.y;
    }
}