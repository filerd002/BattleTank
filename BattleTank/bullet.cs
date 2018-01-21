using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BattleTank
{



    public class Bullet
    {
        public Game1 game;
        public Rectangle bulletRect;
        public Rectangle hitbulletRect;
        public Vector2 speed;
        public Color color { get; set; }
        public int player { get; set; }
        public float rotation { get; set; }
        public Texture2D rectangleTexture;
        public bool alive { get; set; }
        public int pointsOnHit { get; set; }
        public int pointsOnKill { get; set; }
        public Bullet() { }
        public Bullet(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _rectangleTexture, Rectangle _hitbulletRect)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = _rectangleTexture;
            hitbulletRect = _hitbulletRect;
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(rectangleTexture, bulletRect, color);
            }
        }
        public virtual void Update()
        {
            if (alive)
            {
                bulletRect.X += (int)speed.X;
                bulletRect.Y += (int)speed.Y;
                hitbulletRect.X += (int)speed.X;
                hitbulletRect.Y += (int)speed.Y;
                CheckCollision();
            }
        }
        public virtual void CheckCollision()
        {
            foreach (AI_Tank et in game.enemyTanks)
            {
                if ((Rectangle.Intersect(bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive && player <3)
                {
                    et.Hit();
                    game.scoreManager.addScore(player - 1, pointsOnHit);
                    if (!et.alive)
                    {
                        game.scoreManager.addScore(player - 1, pointsOnKill);
                    }
                    this.Die();
                }
            }

            if (player == 2 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank1.location.X - (game.tank1.tankTexture.Width / 2), (int)game.tank1.location.Y - (game.tank1.tankTexture.Height / 2), game.tank1.tankTexture.Width, game.tank1.tankTexture.Height)).Width != 0) && game.tank1.alive)
            {
                if (game.tank1.barrier == false)
                {
                    game.tank1.Hit();
                    game.scoreManager.addScore(1, pointsOnHit);
                    if (!game.tank1.alive)
                    {
                        game.scoreManager.addScore(1, pointsOnKill);
                    }
                }
                this.Die();
            }
            if (player == 1 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank2.location.X - (game.tank2.tankTexture.Width / 2), (int)game.tank2.location.Y - (game.tank2.tankTexture.Height / 2), game.tank2.tankTexture.Width, game.tank2.tankTexture.Height)).Width != 0) && game.tank2.alive)
            {
                if (game.tank2.barrier == false)
                {
                    game.tank2.Hit();
                    game.scoreManager.addScore(0, pointsOnHit);
                    if (!game.tank2.alive)
                    {
                        game.scoreManager.addScore(0, pointsOnKill);
                    }
                }
                this.Die();
            }
            //If CPU hits player 2
            if (player >2 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank2.location.X - (game.tank2.tankTexture.Width / 2), (int)game.tank2.location.Y - (game.tank2.tankTexture.Height / 2), game.tank2.tankTexture.Width, game.tank2.tankTexture.Height)).Width != 0) && game.tank2.alive)
            {
                if (game.tank2.barrier == false)
                {
                    game.tank2.Hit();
                    game.scoreManager.addScore(0, pointsOnHit);
                    if (!game.tank2.alive)
                    {
                        game.scoreManager.addScore(0, pointsOnKill);
                    }
                }
                this.Die();
            }
            //If CPU hits player 1
            if (player>2 && (Rectangle.Intersect(bulletRect, new Rectangle((int)game.tank1.location.X - (game.tank1.tankTexture.Width / 2), (int)game.tank1.location.Y - (game.tank1.tankTexture.Height / 2), game.tank1.tankTexture.Width, game.tank1.tankTexture.Height)).Width != 0) && game.tank1.alive)
            {
                if (game.tank1.barrier == false)
                {
                    game.tank1.Hit();
                    game.scoreManager.addScore(1, pointsOnHit);
                    if (!game.tank1.alive)
                    {
                        game.scoreManager.addScore(1, pointsOnKill);
                    }
                }
                this.Die();
            }
            foreach (Tile[] tiles in game.map.map)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile != null)
                    {


                        if ((tile.isColliding(hitbulletRect).depth > 0)) //If collision is not an empty collision
                        {
                            Collision collision = tile.isColliding(hitbulletRect);
                            this.Die();
                        }
                    }
                    else
                    { continue; }
                }
            }
        }
        public virtual void Die()
        {
            alive = false;
            speed = Vector2.Zero;
            color = Color.Transparent;
        }
    }
}
