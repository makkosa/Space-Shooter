using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class Player
    {
        public Animation Animation { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position;

        public bool Active { get; set; }
        public int Health { get; set; }
        public int Score { get; set; }
        public const int MAX_HEALTH = 100;

        public int Width
        {
            get { return (int)(Animation.FrameWidth * Animation.Scale); }
        }
        public int Height
        {
            get { return (int)(Animation.FrameHeight * Animation.Scale); }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            Animation = animation;
            Position = position;
            Active = true;
            Health = MAX_HEALTH;
            Score = 0;
        }

        public void LoadContent(string texturePath, ContentManager content)
        {
            Texture = content.Load<Texture2D>(texturePath);
        }


        public void Update(GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);

            if (Health == 0)
            {
                Active = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
    }
}
