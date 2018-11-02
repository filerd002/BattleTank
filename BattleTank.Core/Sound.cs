using Microsoft.Xna.Framework.Audio;
using System;

namespace BattleTank.Core
{
    public class Sound
    {

   

        private SoundEffectInstance menuSound;

        private SoundEffectInstance hit;
        private SoundEffectInstance shot;
        private SoundEffectInstance klik;
        private SoundEffectInstance respawn;
        private SoundEffectInstance explosion;
        private SoundEffectInstance rustling;
        private SoundEffectInstance engine;
     



        public enum Sounds { MENU_SOUND,HIT,SHOT,KLIK,RESPAWN,EXPLOSION,RUSTLING, ENGINE }
        public Sound(Game1 game)
        {
            menuSound = game.Content.Load<SoundEffect>("Sounds\\menu_sound").CreateInstance();

            hit  = game.Content.Load<SoundEffect>("Sounds\\hit").CreateInstance();

            shot = game.Content.Load<SoundEffect>("Sounds\\shot").CreateInstance();

            klik = game.Content.Load<SoundEffect>("Sounds\\klik").CreateInstance();

            respawn = game.Content.Load<SoundEffect>("Sounds\\respawn").CreateInstance();

            explosion = game.Content.Load<SoundEffect>("Sounds\\explosion").CreateInstance();

            rustling = game.Content.Load<SoundEffect>("Sounds\\rustling").CreateInstance();

            engine = game.Content.Load<SoundEffect>("Sounds\\engine").CreateInstance();
            

        }

    


        public void PlaySound(Sounds sound)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.Play();           
                    break;
                case Sounds.HIT:
                    hit.Play();
                    break;
                case Sounds.SHOT:
                    shot.Play();
                    break;
                case Sounds.KLIK:
                    klik.Play();
                    break;
                case Sounds.RESPAWN:
                    respawn.Play();
                    break;
                case Sounds.EXPLOSION:
                    explosion.Play();
                    break;
                case Sounds.RUSTLING:
                    rustling.Play();
                    break;
                case Sounds.ENGINE:
                    engine.Play();
                  
                    break;
            }
 
        }

        public void ResumeSound(Sounds sound)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.Resume();
                    break;
                case Sounds.HIT:
                    hit.Resume();
                    break;
                case Sounds.SHOT:
                    shot.Resume();
                    break;
                case Sounds.KLIK:
                    klik.Resume();
                    break;
                case Sounds.RESPAWN:
                    respawn.Resume();
                    break;
                case Sounds.EXPLOSION:
                    explosion.Resume();
                    break;
                case Sounds.RUSTLING:
                    rustling.Resume();
                    break;
                case Sounds.ENGINE:
                    engine.Resume();

                    break;
            }

        }

        public void PauseSound(Sounds sound)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.Pause();
                    break;
                case Sounds.HIT:
                    hit.Pause();
                    break;
                case Sounds.SHOT:
                    shot.Pause();
                    break;
                case Sounds.KLIK:
                    klik.Pause();
                    break;
                case Sounds.RESPAWN:
                    respawn.Pause();
                    break;
                case Sounds.EXPLOSION:
                    explosion.Pause();
                    break;
                case Sounds.RUSTLING:
                    rustling.Pause();
                    break;
                case Sounds.ENGINE:
                    engine.Pause();

                    break;
            }

        }

        public void StopSound(Sounds sound)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
                     switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.Stop();
                    break;
                case Sounds.HIT:
                    hit.Stop();
                    break;
                case Sounds.SHOT:
                    shot.Stop();
                    break;
                case Sounds.KLIK:
                    klik.Stop();
                    break;
                case Sounds.RESPAWN:
                    respawn.Stop();
                    break;
                case Sounds.EXPLOSION:
                    explosion.Stop();
                    break;
                case Sounds.RUSTLING:
                    rustling.Stop();
                    break;
                case Sounds.ENGINE:
                    engine.Stop();
                    break;
            }
          
        }

        public void ConfigSound(Sounds sound, float volume,  float pitch, float pan)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.Volume = volume;
                    menuSound.Pitch = pitch;
                    menuSound.Pan = pan;      
                    break;
                case Sounds.HIT:
                    hit.Volume = volume;
                    hit.Pitch = pitch;
                    hit.Pan = pan;
                    break;
                case Sounds.SHOT:
                    shot.Volume = volume;
                    shot.Pitch = pitch;
                    shot.Pan = pan;
                    break;
                case Sounds.KLIK:
                    klik.Volume = volume;
                    klik.Pitch = pitch;
                    klik.Pan = pan;
                    break;
                case Sounds.RESPAWN:
                    respawn.Volume = volume;
                    respawn.Pitch = pitch;
                    respawn.Pan = pan;
                    break;
                case Sounds.EXPLOSION:
                    explosion.Volume = volume;
                    explosion.Pitch = pitch;
                    explosion.Pan = pan;
                    break;
                case Sounds.RUSTLING:
                    rustling.Volume = volume;
                    rustling.Pitch = pitch;
                    rustling.Pan = pan;
                    break;
                case Sounds.ENGINE:
                    engine.Volume = volume;
                    engine.Pitch = pitch;
                    engine.Pan = pan;
                    break;
            }

        }

        public void LoopingSound(Sounds sound, bool looped)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION ,RUSTLING
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    menuSound.IsLooped = looped;
                    break;
                case Sounds.HIT:
                    hit.IsLooped = looped;
                    break;
                case Sounds.SHOT:
                    shot.IsLooped = looped;
                    break;
                case Sounds.KLIK:
                    klik.IsLooped = looped;
                    break;
                case Sounds.RESPAWN:
                    respawn.IsLooped = looped;
                    break;
                case Sounds.EXPLOSION:
                    explosion.IsLooped = looped;
                    break;
                case Sounds.RUSTLING:
                    rustling.IsLooped = looped;
                    break;
                case Sounds.ENGINE:
                    engine.IsLooped = looped;
                    break;
            }

        }
        
    }
}


    

