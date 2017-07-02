using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Shooter
{
    class GameMusic
    {
        public Song Song { get; set; }

        public GameMusic(ContentManager content, string songPath, float volume, bool isRepeating)
        {
            Song = content.Load<Song>(songPath);
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Volume = volume;
        }

    }
}
