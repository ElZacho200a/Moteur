﻿
using Raylib_cs;

namespace Moteur
{
    public abstract class ActiveEntity : Entity
    {
        
        public ActiveEntity() { 
        Speed = (0,0);
        Acceleration= (0,0);

        
        }
        
        protected float light = 2f;

        public override void Destroy()
        {
            Raylib.UnloadTexture(Sprite);
            
        }
        public float Light
        {
            get => light;
            set
            {
                if (value > 0)
                    light = value;
            }
        }

        public virtual string GetSpriteManagerAtt()
        {
            if(spriteManager != null)
                return spriteManager.GetFileName().Split("Assets")[1]+"|"+spriteManager.Lenght;
            return "";
        }
        
        protected (double vx, double vy) Speed;
        protected (double ax, double ay) Acceleration;
        public int sensX => Speed.vx > 0 ? 1 : -1;
        public int sensY => Speed.vy > 0 ? 1 : -1;
        
        public SpriteManager GetSpriteManager => spriteManager;
        public Texture2D Sprite;
        protected SpriteManager spriteManager;
        protected abstract bool Moove();
        protected abstract void UpdateAnimation();
        public virtual  string getArgument => "";
        public (double vx, double vy) Speed1
        {
            get => Speed;
            set => Speed = value;
        }

        public (double ax, double ay) Acceleration1
        {
            get => Acceleration;
            set => Acceleration = value;
        }

        public byte getCurrentSprite()
        {
            
            return spriteManager is null ? (byte)(0) :spriteManager.RealCursor;
            
        }

        public string getForOnlineData()
        {
            var s = getCurrentSprite();
            return s + "x" + this[0] + "x" + this[1];
        }
        public bool isInLightRadius(int i, int j)
        {
            i *= Level.blocH;
            j *= Level.blocH;
            var p = getCenter();
            return  (Math.Pow(p.X - i , 2 ) + Math.Pow(p.Y - j , 2 ))  - (Level.blocH * Level.blocH) *4  > Math.Pow(Light * Level.blocH , 2);

        }

        public bool isInWater()
        {
            
            return Level.currentLevel.WaterArea is not null  && Level.currentLevel.WaterArea.isCollidedWithEntity(this);
        }
        public RawEntity CreateRawEntity()
        {
            return new RawEntity(this);
        }

        public static ActiveEntity  createActiveFromRaw(RawEntity rawEntity)
        {
            
            var type = Type.GetType(rawEntity.Essence);
            var argument = rawEntity.Argument;
            var arg = argument == ""  ? new object[]{rawEntity.X, rawEntity.Y} : new object[]{rawEntity.X, rawEntity.Y,argument};
            var entity = Activator.CreateInstance(type, arg) as ActiveEntity;
            if (rawEntity.CursorAnimation != 0) // Curseur
                entity.Sprite = entity.GetSpriteManager.GetImage(rawEntity.CursorAnimation);
            if (entity is LivingEntity) // Vie
                (entity as LivingEntity).Life = rawEntity.Life1;
            var acc = (rawEntity.Ax, rawEntity.Ay);
            var speed = (rawEntity.Vx, rawEntity.Vy);
            entity.Speed1 = speed; // Vitesse
            entity.Acceleration1 = acc;//Acceleration
            entity.name = rawEntity.Name;
            return entity;
        }
       
    }
}
