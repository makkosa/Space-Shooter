using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class HealthBar
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public float Scale { get; set; }

        Rectangle sourceRect { get; set; }
        Rectangle destinationRect { get; set; }

        public HealthBar(string texturePath, ContentManager content, int frameWidth, int frameHeight, float scale, Vector2 position)
        {
            Texture = content.Load<Texture2D>(texturePath);
            Position = position;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Scale = scale;
        }

        public void Update(int health)
        {
            sourceRect = new Rectangle((health / 10) * FrameWidth, 0, FrameWidth, FrameHeight);
            destinationRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(FrameWidth * Scale), (int)(FrameHeight * Scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRect, sourceRect, Color.White);
        }
    }
}
