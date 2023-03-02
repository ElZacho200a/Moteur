using Moteur.Entites;

namespace Moteur;

 public class RawEntity
        {
            private String _essence ;
            private int _x, _y;
            private int Life;
            private double vx, vy, ax, ay; 
            private byte _cursorAnimation;
            private String name,argument;


            public string Argument
            {
                get => argument;
                set => argument = value;
            }

            public RawEntity(ActiveEntity entity)
            {

                var Speed = entity.Speed1;
                var Acceleration = entity.Acceleration1;
                var spriteManager = entity.GetSpriteManager;
                _x = entity[0];
                _y = entity[1];
                vx = Speed.vx;
                vy = Speed.vy;
                ax = Acceleration.ax;
                ay = Acceleration.ay;
                name = entity.name;
                argument = entity.getArgument;
                if (spriteManager != null)
                    _cursorAnimation = spriteManager.cursor;
                else
                    _cursorAnimation = 0;
                if (entity is LivingEntity)
                    Life = (entity as LivingEntity).Life;
                else
                    Life = 0;
                _essence  = "Moteur.Entites." +  entity.ToString();

              
               
                
            }

            public string Name
            {
                get => name;
                set => name = value ;
            }
          

            public RawEntity(){}

            public ActiveEntity Recover( )
            {

                 return ActiveEntity.createActiveFromRaw(this);
            }
            public string Essence
            {
                get => _essence;
                set => _essence = value;
            }

            public int X
            {
                get => _x;
                set => _x = value;
            }

            public int Y
            {
                get => _y;
                set => _y = value;
            }

            public int Life1
            {
                get => Life;
                set => Life = value;
            }

            public double Vx
            {
                get => vx;
                set => vx = value;
            }

            public double Vy
            {
                get => vy;
                set => vy = value;
            }

            public double Ax
            {
                get => ax;
                set => ax = value;
            }

            public double Ay
            {
                get => ay;
                set => ay = value;
            }

            public byte CursorAnimation
            {
                get => _cursorAnimation;
                set => _cursorAnimation = value;
            }
        }