using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class ParallaxingBackground
    {
        Texture2D texture;
        Vector2[][] positions;
        int speed;
        bool isTheFirstPassedTileRow;
        int count = 0;

        public int bgHeight
        {
            get { return texture.Height; }
        }
        int bgWidth
        {
            get { return texture.Width; }
        }

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
        {
            texture = content.Load<Texture2D>(texturePath);
            this.speed = speed;
            isTheFirstPassedTileRow = true;
            positions = new Vector2[screenHeight / texture.Height + 2][];

            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2[screenWidth / texture.Width + 2];

                for (int e = 0; e < screenWidth / texture.Width + 2; e++)
                {
                    positions[i][e] = new Vector2(e * texture.Width, i * texture.Height);
                }
            }
        }

        public void Update(GameTime gametime)
        {
            for (int i = 0; i < positions.Length; i++) // Update the positions of the background
            {
                for (int e = 0; e < positions[i].Length; e++)
                {
                    positions[i][e].Y += speed; // Update the position of the screen by adding the speed

                    if (speed <= 0)
                    {
                        if (positions[i][e].Y <= -texture.Height)
                        {
                            positions[i][e].Y = texture.Height * (positions.Length - 1);
                        }
                    }
                    else
                    {
                        if (positions[i][e].Y >= texture.Height * (positions.Length - 1))
                        {
                            if (!isTheFirstPassedTileRow) positions[i][e].Y = -texture.Height;
                            else
                            {
                                // Первую прошедшую текстуру необходимо сместить не только на высоту текстуры, но и на speed,
                                // чтобы не образовывалось промежутка между первой и последней текстурой
                                positions[i][e].Y = -texture.Height + speed;
                                count++;

                                if (count == positions[i].Length)
                                    isTheFirstPassedTileRow = false;
                            }
                        }
                    }
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
