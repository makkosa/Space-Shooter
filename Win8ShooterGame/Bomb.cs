using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class Bomb
    {
        public Animation Animation { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public bool Active { get; set; }
        public float MoveSpeed { get; set; }

        public TimeSpan SpawnTime { get; set; }
        public TimeSpan PrevSpawnTime { get; set; }
        public TimeSpan FirstSpawnTime { get; set; }

        public int Width
        {
            get { return (int)(Animation.FrameWidth * Animation.Scale); }
        }
        public int Height
        {
            get { return (int)(Animation.FrameHeight * Animation.Scale); }
        }

        public Bomb(float firstSpawnTime, float spawnInterval)
        {
            SpawnTime = TimeSpan.FromSeconds(spawnInterval);
            PrevSpawnTime = TimeSpan.Zero;
            FirstSpawnTime = TimeSpan.FromSeconds(firstSpawnTime);
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            Animation = animation;
            Position = position;
            Active = true;
            MoveSpeed = 4.5f;
        }

        public void LoadContent(string texturePath, ContentManager content)
        {
            Texture = content.Load<Texture2D>(texturePath);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            Position = new Vector2(Position.X, Position.Y + MoveSpeed);
            Animation.Position = Position;
            Animation.Update(gameTime);

            if (Position.Y > graphics.GraphicsDevice.Viewport.TitleSafeArea.Height)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
    }
}
