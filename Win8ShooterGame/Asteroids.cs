using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class Asteroids
    {
        public Animation Animation { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PosDelta { get; set; }
        public bool Active { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Value { get; set; }
        public float Angle { get; set; }

        public int Width
        {
            get { return (int)(Animation.FrameWidth * Animation.Scale); }
        }
        public int Height
        {
            get { return (int)(Animation.FrameHeight * Animation.Scale); }
        }

        public static TimeSpan SpawnTime = TimeSpan.FromSeconds(6);
        public static TimeSpan PrevSpawnTime = TimeSpan.Zero;
        public static TimeSpan FirstSpawnTime = TimeSpan.FromSeconds(8);
        public static float MoveSpeed = 4f;

        public const int VALUE = 10;

        public void Initialize(Animation animation, Vector2 position)
        {
            Animation = animation;
            Position = position;
            Active = true;
            Health = 10;
            Damage = 10;
            Value = 10;
            Angle = 0;
        }

        public void LoadContent(string texturePath, ContentManager content)
        {
            Texture = content.Load<Texture2D>(texturePath);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            Position += PosDelta;
            Animation.Position = Position;
            Animation.Update(gameTime);
            Angle += 0.04f;

            if (Position.Y > graphics.GraphicsDevice.Viewport.TitleSafeArea.Height + Height || Health <= 0)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.DrawWithRotation(spriteBatch, Angle);
        }
    }
}
