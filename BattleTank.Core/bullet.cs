using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Bullet
    {
        protected Game1 _game;
        protected Rectangle _bulletRect;
        protected Vector2 _speed;
        protected Color _color;
        protected int _player;
        protected float _rotation;
        protected Texture2D _rectangleTexture;
        public const int POINTS_ON_KILL = 1;

        public enum TypeOfWeapon
        {
            BULLET, MINE
        }

        public bool IsAlive { get; set; }
        public bool IsCollide { get; set; }
        public Particlecloud ShotParticles { get; set; }
        public Particlecloud DeathParticles { get; set; }
        public TypeOfWeapon Type { get; set; }

        public Bullet(Game1 game, Rectangle bulletRect, Vector2 speed, Color color, int player, float rotation, Texture2D rectangleTexture, TypeOfWeapon type)
        {
            _game = game;
            Type = type;
            _bulletRect = bulletRect;
            _speed = speed;
            _color = color;
            _player = player;
            _rotation = rotation;
            _rectangleTexture = rectangleTexture;
            IsAlive = true;
            IsCollide = false;
            ShotParticles = new Particlecloud(new Vector2(bulletRect.X + speed.X, bulletRect.Y + speed.Y), game, player, rectangleTexture, Type.Equals(TypeOfWeapon.BULLET) ? Color.MonoGameOrange : Color.Gray, 1, 2);

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            if (!IsAlive) return;

            if (IsCollide)
            {
                DeathParticles.Draw(spriteBatch);

            }

            if (ShotParticles != null)
            {
                ShotParticles.Draw(spriteBatch);

            }

            spriteBatch.Draw(_rectangleTexture, _bulletRect, _color);

        }

        public virtual void Update(GameTime gameTime)
        {

            if (!IsAlive) return;

            if (DeathParticles != null)
            {
                DeathParticles.Update(gameTime);
                if (!DeathParticles.Particles[DeathParticles.Particles.Length - 1].Alive)
                    IsAlive = false;
            }

            if (ShotParticles != null)
            {
                ShotParticles.Update(gameTime);
            }

            _bulletRect.X += (int)_speed.X;
            _bulletRect.Y += (int)_speed.Y;

            if (!IsCollide)
                CheckCollision();
        }

        public virtual void CheckCollision()
        {
            foreach (AI_Tank et in _game.EnemyTanks)
            {
                if (((Rectangle.Intersect(_bulletRect, new Rectangle((int)et.location.X - (et.TankTexture.Width / 2), (int)et.location.Y - (et.TankTexture.Height / 2), et.TankTexture.Width, et.TankTexture.Height)).Width != 0) && et.Alive && _player < 3) && !et.Barrier)
                {
                        et.Hit();

                        if (!et.Alive)
                        {
                            _game.ScoreManager.AddScore(_player - 1, POINTS_ON_KILL);
                        }
                        this.Die();
                }
                
            }

            if ((_player == 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank1.location.X - (_game.Tank1.TankTexture.Width / 2), (int)_game.Tank1.location.Y - (_game.Tank1.TankTexture.Height / 2), _game.Tank1.TankTexture.Width, _game.Tank1.TankTexture.Height)).Width != 0) && _game.Tank1.Alive) && (_game.GameStateCurrent != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                if (!_game.Tank1.Barrier)
                {
                    _game.Tank1.Hit();

                    if (!_game.Tank1.Alive)
                    {
                        _game.ScoreManager.AddScore(1, POINTS_ON_KILL);
                    }
                }
                this.Die();

            }
            if (_game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && (_player == 1 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank2.location.X - (_game.Tank2.TankTexture.Width / 2), (int)_game.Tank2.location.Y - (_game.Tank2.TankTexture.Height / 2), _game.Tank2.TankTexture.Width, _game.Tank2.TankTexture.Height)).Width != 0) && _game.Tank2.Alive) && (_game.GameStateCurrent != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                if (!_game.Tank2.Barrier)
                {
                    _game.Tank2.Hit();

                    if (!_game.Tank2.Alive)
                    {
                        _game.ScoreManager.AddScore(0, POINTS_ON_KILL);
                    }
                }
                this.Die();

            }
            //If CPU hits player 2

            if (_game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && _player > 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank2.location.X - (_game.Tank2.TankTexture.Width / 2), (int)_game.Tank2.location.Y - (_game.Tank2.TankTexture.Height / 2), _game.Tank2.TankTexture.Width, _game.Tank2.TankTexture.Height)).Width != 0) && _game.Tank2.Alive)
            {
                if (!_game.Tank2.Barrier)
                {
                    _game.Tank2.Hit();              
                }
                this.Die();

            }

            //If CPU hits player 1
            if (_player > 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank1.location.X - (_game.Tank1.TankTexture.Width / 2), (int)_game.Tank1.location.Y - (_game.Tank1.TankTexture.Height / 2), _game.Tank1.TankTexture.Width, _game.Tank1.TankTexture.Height)).Width != 0) && _game.Tank1.Alive)
            {
                if (!_game.Tank1.Barrier)
                {
                    _game.Tank1.Hit();
                }
                this.Die();

            }
            foreach (Tile[] tiles in _game.Map.MapCurrent)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile != null && tile.IsColliding(_bulletRect).Depth > 0 && tile.Type == Tile.TileType.WALL) //If collision is not an empty collision
                    {                                         
                        this.Die();
                    }                                   
                }
            }

            if (_bulletRect.X < 0 || _bulletRect.Y < 0 || _bulletRect.X > _game.Map.ScreenWidth || _bulletRect.Y > _game.Map.ScreenHeight)
            {
                this.Die();

            }
        }
        public virtual void Die()
        {

            DeathParticles = new Particlecloud(new Vector2(_bulletRect.X - _speed.X, _bulletRect.Y - _speed.Y), _game, _player, _rectangleTexture, Color.OrangeRed, 6, 6);
            IsCollide = true;
            _speed = Vector2.Zero;
            _color = Color.Transparent;
        }
    }
}
