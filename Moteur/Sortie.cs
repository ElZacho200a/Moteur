namespace Moteur
{
    internal class Sortie : Entity
    {

        public int nextLevel = Level.currentLevel.ID;
         protected Point trigger;
        bool Used = true;
        public bool LevelLoad = false;
       
        public Sortie(int nextLevel , int x , int y) { 
        
            this.nextLevel = nextLevel;
            this.Coordonates = (x, y);
            var blocH = Level.blocH;
            Hitbox = new Rectangle(x, y, 50, 50);
           trigger = new Point( x  + (blocH / 2) ,y + blocH/2 );
        }

        public override void Update()
        {
           
            if (Camera.player.Hitbox.IntersectsWith(Hitbox))
            {
                
                if (!Used)
                    LoadNextLevel();
                Used = true;

            }
            else
            {
                Used= false;
            }
        }


        protected void LoadNextLevel()
        {
            LevelLoad= true;
            Camera.player.ResetSucribers();
            Level.currentLevel.Deactivate();
            foreach (Sortie sortie in Level.currentLevel.GetEntities().Select(entity => entity ).Where(E => E is Sortie))
            {
                sortie.Used = true;
            }
            var ID = Level.currentLevel.ID;
            
            var nLevel = new Level(nextLevel );
            Type t = this.GetType();
            foreach (var entity in nLevel.GetEntities())
            {
                if(entity.GetType() == t )
                {
                       
                    var sortie = (Sortie)entity;
                        
                    if (sortie.nextLevel == ID)
                    {
                        
                       


                        Level.currentLevel.destroy();
                       
                        Level.currentLevel = nLevel;
                        
                        Camera.player.Coordonates = (sortie.Coordonates.x, sortie.Coordonates.y + Level.blocH - Camera.player.Hitbox.Height);
                        Camera.player.Hitbox.Location = new Point(Camera.player[0], Camera.player[1]);
                        Camera.ResetScope();
                        GC.Collect();
                        Level.currentLevel.Activate();
                        
                        return;
                    }
                       
                }

            }
        }
    }
}
