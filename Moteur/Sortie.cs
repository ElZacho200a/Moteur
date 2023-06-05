namespace Moteur
{
    internal class Sortie : Entity
    {

        public int nextLevel = Level.currentLevel.ID;
        protected Point trigger;
        private Type type;
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
            bool Triggered = false;
            foreach (var player in Level.Players)
            if (player.Hitbox.IntersectsWith(Hitbox) )
            {
                
                if (!Used)
                    LoadNextLevel();
                Used = true;
                Triggered = true;
            }
           if(!Triggered)
            {
                Used= false;
            }
        }


        protected void LoadNextLevel()
        {
            LevelLoad= true;
            foreach (var player in Level.Players)
                player.ResetSucribers();
            Level.currentLevel.Deactivate();
            foreach (Sortie sortie in Level.currentLevel.GetEntities().Select(entity => entity ).Where(E => E is Sortie))
            {
                sortie.Used = true;
            }
            var ID = Level.currentLevel.ID;
            
            var nLevel = new Level(nextLevel );
            Type t = this.GetType();
            Level.Players[0].hasSaved = false;
            foreach (var entity in nLevel.GetEntities())
            {
                if(entity.GetType() == t )
                {
                       
                    var sortie = (Sortie)entity;
                        
                    if (sortie.nextLevel == ID)
                    {
                        
                       


                        Level.currentLevel.destroy();
                       
                        Level.currentLevel = nLevel;
                        foreach (var player in Level.Players)
                        {
                            player.Coordonates = (sortie.Coordonates.x,
                                sortie.Coordonates.y + Level.blocH - player.Hitbox.Height);
                            if (player[1] < 0 )
                                player.Coordonates.y += Level.blocH;
                            player.Hitbox.Location = new Point(player[0], player[1]);
                            player.Camera.ResetScope();
                        }

                        GC.Collect();
                        Level.currentLevel.Activate();
                        
                        return;
                    }
                       
                }

            }
        }
    }
}
