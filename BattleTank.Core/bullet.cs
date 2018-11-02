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
        public Particlecloud shotParticles;
        public Particlecloud deathParticles;
        public TypeOfWeapon _type;

        public enum TypeOfWeapon
        {
            BULLET, MINE
        }

        public bool IsAlive { get; set; }
        public bool IsCollide { get; set; }

        public Bullet(Game1 game, Rectangle bulletRect, Vector2 speed, Color color, int player, float rotation, Texture2D rectangleTexture, TypeOfWeapon type)
        {
            _game = game;
            _type = type;
            _bulletRect = bulletRect;
            _speed = speed;
            _color = color;
            _player = player;
            _rotation = rotation;
            _rectangleTexture = rectangleTexture;
            IsAlive = true;
            IsCollide = false;
            shotParticles = new Particlecloud(new Vector2(bulletRect.X + speed.X, bulletRect.Y + speed.Y), game, player, rectangleTexture, _type.Equals(TypeOfWeapon.BULLET) ? Color.MonoGameOrange : Color.Gray, 1, 2);

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            if (!IsAlive) return;

            if (IsCollide)
            {
                deathParticles.Draw(spriteBatch);

            }

            if (shotParticles != null)
            {
                shotParticles.Draw(spriteBatch);

            }

            spriteBatch.Draw(_rectangleTexture, _bulletRect, _color);

        }

        public virtual void Update(GameTime gameTime)
        {

            if (!IsAlive) return;

            if (deathParticles != null)
            {
                deathParticles.Update(gameTime);
                if (!deathParticles.Particles[deathParticles.Particles.Length - 1].alive)
                    IsAlive = false;
            }

            if (shotParticles != null)
            {
                shotParticles.Update(gameTime);
            }

            _bulletRect.X += (int)_speed.X;
            _bulletRect.Y += (int)_speed.Y;

            if (!IsCollide)
                CheckCollision();
        }

        public virtual void CheckCollision()
        {
            foreach (AI_Tank et in _game.enemyTanks)
            {
                if ((Rectangle.Intersect(_bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive && _player < 3)
                {
                    if (et.Barrier == false)
                    {
                        et.Hit();

                        if (!et.alive)
                        {
                            _game.scoreManager.addScore(_player - 1, POINTS_ON_KILL);
                        }
                        this.Die();

                    }
                }
            }

            if ((_player == 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank1.location.X - (_game.tank1.tankTexture.Width / 2), (int)_game.tank1.location.Y - (_game.tank1.tankTexture.Height / 2), _game.tank1.tankTexture.Width, _game.tank1.tankTexture.Height)).Width != 0) && _game.tank1.alive) && (_game.gameState != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                if (_game.tank1.Barrier == false)
                {
                    _game.tank1.Hit();

                    if (!_game.tank1.alive)
                    {
                        _game.scoreManager.addScore(1, POINTS_ON_KILL);
                    }
                }
                this.Die();

            }
            if (_game.gameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && (_player == 1 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank2.location.X - (_game.tank2.tankTexture.Width / 2), (int)_game.tank2.location.Y - (_game.tank2.tankTexture.Height / 2), _game.tank2.tankTexture.Width, _game.tank2.tankTexture.Height)).Width != 0) && _game.tank2.alive) && (_game.gameState != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                if (_game.tank2.Barrier == false)
                {
                    _game.tank2.Hit();

                    if (!_game.tank2.alive)
                    {
                        _game.scoreManager.addScore(0, POINTS_ON_KILL);
                    }
                }
                this.Die();

            }
            //If CPU hits player 2

            if (_game.gameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && _player > 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank2.location.X - (_game.tank2.tankTexture.Width / 2), (int)_game.tank2.location.Y - (_game.tank2.tankTexture.Height / 2), _game.tank2.tankTexture.Width, _game.tank2.tankTexture.Height)).Width != 0) && _game.tank2.alive)
            {
                if (_game.tank2.Barrier == false)
                {
                    _game.tank2.Hit();

                    if (!_game.tank2.alive)
                    {

                    }
                }
                this.Die();

            }

            //If CPU hits player 1
            if (_player > 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank1.location.X - (_game.tank1.tankTexture.Width / 2), (int)_game.tank1.location.Y - (_game.tank1.tankTexture.Height / 2), _game.tank1.tankTexture.Width, _game.tank1.tankTexture.Height)).Width != 0) && _game.tank1.alive)
            {
                if (_game.tank1.Barrier == false)
                {
                    _game.tank1.Hit();

                    if (!_game.tank1.alive)
                    {

                    }
                }
                this.Die();

            }
            foreach (Tile[] tiles in _game.map.map)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile != null)
                    {


                        if ((tile.isColliding(_bulletRect).depth > 0)) //If collision is not an empty collision
                        {
                            Collision collision = tile.isColliding(_bulletRect);
                            if (tile.type == Tile.TileType.WALL)
                                this.Die();

                        }
                    }
                    else
                    { continue; }
                }
            }

            if (_bulletRect.X < 0 || _bulletRect.Y < 0 || _bulletRect.X > _game.map.screenWidth || _bulletRect.Y > _game.map.screenHeight)
            {
                this.Die();

            }
        }
        public virtual void Die()
        {

            deathParticles = new Particlecloud(new Vector2(_bulletRect.X - _speed.X, _bulletRect.Y - _speed.Y), _game, _player, _rectangleTexture, Color.OrangeRed, 6, 6);
            IsCollide = true;
            _speed = Vector2.Zero;
            _color = Color.Transparent;
        }
    }
}
