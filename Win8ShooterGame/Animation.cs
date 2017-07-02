using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Animation
    {
        Texture2D spriteStrip;

        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;

        Color color;
        Rectangle sourceRect = new Rectangle(); 
        Rectangle destinationRect = new Rectangle(); 

        public int FrameWidth; 
        public int FrameHeight;
        public float Scale; 
        public Vector2 Position;

        public bool Active; 
        public bool Looping;

        public float Rotation;
        Vector2 origin;
        SpriteEffects effects;
        float layerDepth;

        public void Initialize (Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime,
                    Color color, float scale, bool looping, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.Scale = scale;

            this.frameCount = frameCount;
            this.frameTime = frametime;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;

            this.Rotation = rotation;
            this.origin = origin;
            this.effects = effects;
            this.layerDepth = layerDepth;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (!Looping) Active = false;
                }

                elapsedTime = 0;
            }

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * Scale) / 2, (int)Position.Y - (int)(FrameHeight * Scale) / 2,
                                            (int)(FrameWidth * Scale), (int)(FrameHeight * Scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active) 
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, Rotation, origin, effects, layerDepth);
        }

        public void DrawWithRotation(SpriteBatch spriteBatch, float rotation)
        {
            if (Active)
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, rotation, origin, effects, layerDepth);
        }
    }
}
