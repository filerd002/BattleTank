using System;
using System.Collections.Generic;
using System.Linq;
using BattleTank.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Tanks
{
    public class AI_Tank : Tank
    {
        private readonly int _aiLevel;  //Można wykorzystać do ustawienia poziomu trudności
        private float _targetDirection;
        private readonly List<Bullet> _enemyBullets = new List<Bullet>();
        private readonly bool _kamikazeMode = false;

        public AI_Tank(Game1 game, string tankSpriteName, Vector2 location, Vector2 maxSpeed,
            float rotation, int player, float scale, Texture2D whiteRectangle, int strong,
            bool barrier,bool frozen, float targetDirection, int aiLevel, bool kamikazeMode = false)
            : base(game, tankSpriteName, location, maxSpeed, rotation, player, scale,
                  whiteRectangle, strong, 0, barrier,frozen, null)
        {
            enemy = true;
            _aiLevel = aiLevel;

            _targetDirection = targetDirection;

            _kamikazeMode = kamikazeMode;

            if (_kamikazeMode)
            {
                speed = new Vector2(3, 3);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Bullet b in _enemyBullets)
            {
                if (b != null && b.alive)
                {
                    b.Draw(spriteBatch);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
     

            if (_timeLeftToNextShot <= TimeSpan.Zero)
            {
                if (!frozen)
                {
                    _enemyBullets.AddRange(Fire());
                }
            }

            base.Update(gameTime);
            foreach (Bullet b in _enemyBullets)
            {
                if (b != null && b.alive)
                {
                    b.Update();
                }
            
            }
        }

        public override void Move()
        {
            if (colliding)
            {
                switch ((int)_targetDirection)
                {
                    case (int)UP:
                        _targetDirection = RIGHT;
                        break;
                    case (int)RIGHT:
                        _targetDirection = DOWN;
                        break;
                    case (int)LEFT:
                        _targetDirection = UP;
                        break;
                    case (int)DOWN:
                        _targetDirection = LEFT;
                        break;
                    case (int)UP_LEFT:
                        _targetDirection = LEFT;
                        break;
                    case (int)DOWN_LEFT:
                        _targetDirection = LEFT;
                        break;
                    default:
                        break;
                }
            }
            switch ((int)_targetDirection)
            {
                case (int)UP:
                    MoveUp(false);
                    Rotate(UP);
                    break;
                case (int)RIGHT:
                    MoveRight(false);
                    Rotate(RIGHT);
                    break;
                case (int)LEFT:
                    MoveLeft(false);
                    Rotate(LEFT);
                    break;
                case (int)DOWN:
                    MoveDown(false);
                    Rotate(DOWN);
                    break;
                case (int)UP_LEFT:
                    MoveUp(false);
                    MoveLeft(false);
                    Rotate(UP_LEFT);
                    break;
                case (int)DOWN_LEFT:
                    MoveDown(false);
                    MoveLeft(false);
                    Rotate(DOWN_LEFT);
                    break;
                default:
                    break;
            }

            foreach (var userTank in new[] { game.tank1, game.tank2 }.Where(d => d.alive))
            {
                if (_kamikazeMode)
                {
                    if ((location - userTank.location).Length() <= _aiLevel * 2) // TODO: należy zamienić ten mnożnik na jakąś stałą
                    {
                        Explode();
                        if (userTank.barrier == false)
                        {
                            userTank.Explode();
                        }
                    }
                }
                
                var toUserTankXDistance = Math.Abs(location.X - userTank.location.X);
                var toUserTankYDistance = Math.Abs(location.Y - userTank.location.Y);

                if (toUserTankXDistance <= _aiLevel && location.Y >= userTank.location.Y)
                {
                    _targetDirection = UP;
                }

                if (toUserTankXDistance <= _aiLevel && location.Y <= userTank.location.Y)
                {
                    _targetDirection = DOWN;
                }

                if (toUserTankYDistance <= _aiLevel && location.X >= userTank.location.X)
                {
                    _targetDirection = LEFT;
                }

                if (toUserTankYDistance <= _aiLevel && location.X <= userTank.location.X)
                {
                    _targetDirection = RIGHT;
                }
            }
        }
    }
}
