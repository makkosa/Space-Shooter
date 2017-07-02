using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Shooter
{
    class Sound
    {
        public SoundEffect SoundEffect { get; set; }
        public SoundEffectInstance SoundEffectInstance { get; set; }
        public bool Played { get; set; }

        public Sound(ContentManager content, string soundPath, float volume, bool isLooped)
        {
            SoundEffect = content.Load<SoundEffect>(soundPath);
            SoundEffectInstance = SoundEffect.CreateInstance();
            SoundEffectInstance.Volume = volume;
            SoundEffectInstance.IsLooped = isLooped;
            Played = false;
        }
    }
}
