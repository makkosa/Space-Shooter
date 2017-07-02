using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class StaticBackground
    {
        Texture2D texture;
        Vector2[][] positions;

        public int bgHeight
        {
            get { return texture.Height; }
        }
        int bgWidth
        {
            get { return texture.Width; }
        }

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight)
        {
            texture = content.Load<Texture2D>(texturePath);

            positions = new Vector2[screenHeight / texture.Height + 1][];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2[screenWidth / texture.Width + 1];

                for (int e = 0; e < screenWidth / texture.Width + 1; e++)
                {
                    positions[i][e] = new Vector2(e * texture.Width, i * texture.Height);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                for (int e = 0; e < positions[i].Length; e++)
                {
                    Rectangle rectBg = new Rectangle((int)positions[i][e].X, (int)positions[i][e].Y, bgWidth, bgHeight);
                    spriteBatch.Draw(texture, rectBg, Color.White);
                }
            }
        }
    }
}
