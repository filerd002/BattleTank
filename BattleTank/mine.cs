using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleTank
{
    public class Mine : Bullet
    {
      
        public Mine() { }
        public Mine(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _rectangleTexture)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = _rectangleTexture;
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }
        public override void Die()
        {
            base.Die();
            if (player == 1)
            {
                 game.scoreManager.addScore(0, 200);
            }
            if (player == 2)
            {
                 game.scoreManager.addScore(1, 200);
            }
        }
        public override void Update()
        {
            base.Update();
          
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
          

        }
        public override void CheckCollision()
        {
            foreach (AI_Tank et in game.enemyTanks)
            {
                if ((Rectangle.Intersect(bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive)
                {
                    et.Explode();
                    this.Die();
                }
            }
            if (player == 2 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank1.location.X - (game.tank1.tankTexture.Width / 2), (int)game.tank1.location.Y - (game.tank1.tankTexture.Height / 2), game.tank1.tankTexture.Width, game.tank1.tankTexture.Height)).Width != 0) && game.tank1.alive)
            {
                game.tank1.Explode();
                this.Die();
            }
            if (player == 1 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank2.location.X - (game.tank2.tankTexture.Width / 2), (int)game.tank2.location.Y - (game.tank2.tankTexture.Height / 2), game.tank2.tankTexture.Width, game.tank2.tankTexture.Height)).Width != 0) && game.tank2.alive)
            {
                game.tank2.Explode();
                this.Die();
            }
        }
    }
}
