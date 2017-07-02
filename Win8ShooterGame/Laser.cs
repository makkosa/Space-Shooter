using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Laser
    {
        float moveSpeed;

        public Animation Animation { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PosDelta { get; set; }
        public bool Active { get; set; }

        public int Width
        {
            get { return Animation.FrameWidth; }
        }
        public int Height
        {
            get { return Animation.FrameHeight; }
        }

        public int Damage = 10;

        public void Initialize(Animation animation, Vector2 position, float speed, Vector2 posDelta)
        {
            moveSpeed = speed;
            Animation = animation;
            Position = position;
            PosDelta = posDelta;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            Position += PosDelta;
            Animation.Position = Position;
            Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
    }
}
