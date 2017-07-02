using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class Enemy
    {
        public Animation Animation { get; set; }
        public Sound ExplosionSound { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public bool Active { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Value { get; set; }

        public int Width
        {
            get { return (int)(Animation.FrameWidth * Animation.Scale); }
        }
        public int Height
        {
            get { return (int)(Animation.FrameHeight * Animation.Scale); }
        }

        public const float MAX_MOVE_SPEED = 7f;

        public static TimeSpan SpawnTime = TimeSpan.FromSeconds(0.7);
        public static TimeSpan PrevSpawnTime = TimeSpan.Zero;
        public static TimeSpan AccelerationTime = TimeSpan.FromSeconds(5);
        public static TimeSpan PrevAccelerationTime = TimeSpan.Zero;

        public void Initialize(Animation animation, Vector2 position, int value, Sound sound)
        {
            Animation = animation;
            Position = position;
            ExplosionSound = sound;
            Active = true;
            Health = 10;
            Damage = 10;
            Value = value;
        }

        public void LoadContent(string texturePath, ContentManager content)
        {
            Texture = content.Load<Texture2D>(texturePath);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, float moveSpeed)
        {
            Position = new Vector2(Position.X, Position.Y + moveSpeed);
            Animation.Position = Position;
            Animation.Update(gameTime);

            if (Position.Y > graphics.GraphicsDevice.Viewport.TitleSafeArea.Height || Health <= 0)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
    }
}
