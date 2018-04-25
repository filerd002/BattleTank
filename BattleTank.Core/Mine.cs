using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Mine : Bullet
    {
        private static Texture2D _mineTextureGreen;
        private static Texture2D _mineTextureRed;

        public Mine(Game1 game, Rectangle bulletRect, Vector2 speed, Color color, int player, float rotation)
            : base(game, bulletRect, speed, color, player, rotation, null)

        {
            if (_mineTextureGreen is null) _mineTextureGreen = game.Content.Load<Texture2D>("Graphics/mineGreen");
            if (_mineTextureRed is null) _mineTextureRed = game.Content.Load<Texture2D>("Graphics/mineRed");
            
            _rectangleTexture = base._player == 1 ? _mineTextureGreen : _mineTextureRed;
        }

        public override void Die()
        {
            base.Die();
            if (_player == 1)
            {
                 _game.scoreManager.addScore(0, POINTS_ON_KILL);
            }
            if (_player == 2)
            {
                 _game.scoreManager.addScore(1, POINTS_ON_KILL);
            }
        }
        
        public override void CheckCollision()
        {
            foreach (AI_Tank et in _game.enemyTanks)
            {
                if ((Rectangle.Intersect(_bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive)
                {
                    et.Explode();
                    this.Die();
                }
            }
            if ((_player == 2 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank1.location.X - (_game.tank1.tankTexture.Width / 2), (int)_game.tank1.location.Y - (_game.tank1.tankTexture.Height / 2), _game.tank1.tankTexture.Width, _game.tank1.tankTexture.Height)).Width != 0) && _game.tank1.alive) && (_game.gameState != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                _game.tank1.Explode();
                this.Die();
            }
            if ((_player == 1 && (Rectangle.Intersect(_bulletRect, new Rectangle((int)_game.tank2.location.X - (_game.tank2.tankTexture.Width / 2), (int)_game.tank2.location.Y - (_game.tank2.tankTexture.Height / 2), _game.tank2.tankTexture.Width, _game.tank2.tankTexture.Height)).Width != 0) && _game.tank2.alive) && (_game.gameState != Game1.GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU))
            {
                _game.tank2.Explode();
                this.Die();
            }
        }
    }
}
