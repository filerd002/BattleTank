using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Mine : Bullet
    {
        private readonly Texture2D _mineTextureGreen;
        private readonly Texture2D _mineTextureRed;

        public Mine(Game1 game, Rectangle bulletRect, Vector2 speed, Color color, int player, float rotation, Texture2D rectangleTexture, TypeOfWeapon type)
            : base(game, bulletRect, speed, color, player, rotation, rectangleTexture, type)

        {
            if (_mineTextureGreen is null)
            {
                _mineTextureGreen = game.Content.Load<Texture2D>("Graphics/mineGreen");
            }

            if (_mineTextureRed is null)
            {
                _mineTextureRed = game.Content.Load<Texture2D>("Graphics/mineRed");
            }

            _rectangleTexture = base._player == 1 ? _mineTextureGreen : _mineTextureRed;
        }

        public override void Die()
        {
            base.Die();
            if (_player == 1)
            {
                 _game.ScoreManager.AddScore(0, POINTS_ON_KILL);
            }
            if (_player == 2)
            {
                 _game.ScoreManager.AddScore(1, POINTS_ON_KILL);
            }
        }
        
        public override void CheckCollision()
        {
            foreach (AI_Tank et in _game.EnemyTanks)
            {
                if ((Rectangle.Intersect(_bulletRect, new Rectangle((int)et.location.X - (et.TankTexture.Width / 2), (int)et.location.Y - (et.TankTexture.Height / 2), et.TankTexture.Width, et.TankTexture.Height)).Width != 0) && et.Alive)
                {
                    et.Explode();
                    this.Die();
                }
            }
            if ((_player == 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank1.location.X - (_game.Tank1.TankTexture.Width / 2), (int)_game.Tank1.location.Y - (_game.Tank1.TankTexture.Height / 2), _game.Tank1.TankTexture.Width, _game.Tank1.TankTexture.Height)).Width != 0) && _game.Tank1.Alive) && (_game.GameStateCurrent != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                _game.Tank1.Explode();
                this.Die();
            }
            if (_game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && (_player == 1 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.Tank2.location.X - (_game.Tank2.TankTexture.Width / 2), (int)_game.Tank2.location.Y - (_game.Tank2.TankTexture.Height / 2), _game.Tank2.TankTexture.Width, _game.Tank2.TankTexture.Height)).Width != 0) && _game.Tank2.Alive) && (_game.GameStateCurrent != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                _game.Tank2.Explode();
                this.Die();
            }
        }
    }
}
