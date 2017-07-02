using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class Button
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)(Position.X - Width / 2), (int)(Position.Y - Height / 2), Width, Height); }
        }

        public float Scale { get; set; }
        public float MouseOverScale { get; set; }
        public bool IsMouseOver { get; set; }
        public bool IsSoundActive { get; set; }

        public int Width
        {
            get
            {
                if (IsMouseOver) return (int)(Texture.Width * MouseOverScale);
                else return (int)(Texture.Width * Scale);
            }
        }
        public int Height
        {
            get
            {
                if (IsMouseOver) return (int)(Texture.Height * MouseOverScale);
                else return (int)(Texture.Height * Scale);
            }
        }

        public Button(string texturePath, Vector2 position, ContentManager content, float scale, float overScale)
        {
            Texture = content.Load<Texture2D>(texturePath);
            Position = position;
            IsMouseOver = false;
            IsSoundActive = true;

            Scale = scale;
            MouseOverScale = overScale;
        }

        public void Update(int mouseStateX, int mouseStateY)
        {
            IsMouseOver = Rectangle.Contains(mouseStateX, mouseStateY);

            if (IsMouseOver) IsSoundActive = false;
            else IsSoundActive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
