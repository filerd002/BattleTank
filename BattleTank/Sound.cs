using Microsoft.Xna.Framework.Audio;


namespace BattleTank
{
    public class Sound
    {

        float volume = 0.5f;
        float pitch = 0.0f;
        float pan = 0.0f;

        private SoundEffect menuSound;
        private SoundEffect hit;
        private SoundEffect shot;
        private SoundEffect klik;
        private SoundEffect respawn;
        private SoundEffect explosion;
     

        public enum Sounds { MENU_SOUND,HIT,SHOT,KLIK,RESPAWN,EXPLOSION }
        public Sound(Game1 game)
        {
            menuSound = game.Content.Load<SoundEffect>("Sounds\\menu_sound");
            hit = game.Content.Load<SoundEffect>("Sounds\\hit"); ;
            shot = game.Content.Load<SoundEffect>("Sounds\\shot"); ;
            klik = game.Content.Load<SoundEffect>("Sounds\\klik"); ;
            respawn = game.Content.Load<SoundEffect>("Sounds\\respawn"); ;
            explosion = game.Content.Load<SoundEffect>("Sounds\\explosion"); ;
    }
        public SoundEffect deploySound(Sounds sound)
        {
            SoundEffect soundEffect = null;
     
            switch (sound)
            {
                case Sounds.MENU_SOUND:
                    soundEffect = menuSound;


                    break;


            }
            return soundEffect;
        }



        public void PlaySound(Sounds sound)
        {
            //HIT,SHOT,KLIK,RESPAWN,EXPLOSION 
            switch (sound)
            {
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

            }
        }

    }
}


    

