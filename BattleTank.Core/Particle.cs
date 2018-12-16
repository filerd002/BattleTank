using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    class Particle
    {
        public Rectangle particleRect;
        public Vector2 speed;
        public Color Color { get; set; }
        public int Player { get; set; }
        public float Rotation { get; set; }
        public Texture2D particleTexture;
        public bool Alive { get; set; }
        public bool Decay { get; set; }
        public bool Fade { get; set; }
        public float DecayTime { get; set; }
        public float InitDecayTime { get; set; }

        public Game1 Game { get; }

        public Particle() { }
        public Particle(Game1 _game, Rectangle _particleRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _particleTexture)
        {
            Game = _game;
            particleRect = _particleRect;
            speed = _speed;
            Color = _color;
            Player = _player;
            Rotation = _rotation;
            particleTexture = _particleTexture;
            Alive = true;
            Decay = false;
            DecayTime = float.MinValue;
            InitDecayTime = DecayTime;
            Fade = false;
        }
        public Particle(Game1 _game, Rectangle _particleRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _particleTexture, float _decayTime, bool _fade)
        {
            Game = _game;
            particleRect = _particleRect;
            speed = _speed;
            Color = _color;
            Player = _player;
            Rotation = _rotation;
            particleTexture = _particleTexture;
            Alive = true;
            Decay = true;
            DecayTime = 0;
            InitDecayTime = _decayTime;
            Fade = _fade;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(particleTexture, particleRect, Color);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (Alive)
            {
                particleRect.X += (int)speed.X;
                particleRect.Y += (int)speed.Y;
                if(Decay)
                {
                    DecayTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    if(Fade)
                    {
                        Color = Color.Lerp(Color, Color.Transparent, DecayTime/InitDecayTime);
                    }
                    if (DecayTime >= InitDecayTime)
                    {
                        Die();
                    }
                }
            }
        }
        public void Die()
        {
            Alive = false;
            speed = Vector2.Zero;
            Color = Color.Transparent;
        }
    }
}
