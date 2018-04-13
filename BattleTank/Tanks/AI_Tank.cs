﻿using System;
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
        private TankControllerState _targetDirection;
        private float _oldTargetDirection;
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

            _oldTargetDirection = targetDirection;

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

        public override void MoveTank(TankControllerState? state = null)
        {
            if (_aiLevel < 4)
                StandardAI();
            else
                ExperimentalAI();
        }

        private void StandardAI()
        {
            if (colliding)
            {
                switch ((int)_oldTargetDirection)
                {
                    case (int)UP:
                        _oldTargetDirection = RIGHT;
                        break;
                    case (int)RIGHT:
                        _oldTargetDirection = DOWN;
                        break;
                    case (int)LEFT:
                        _oldTargetDirection = UP;
                        break;
                    case (int)DOWN:
                        _oldTargetDirection = LEFT;
                        break;
                    case (int)UP_LEFT:
                        _oldTargetDirection = LEFT;
                        break;
                    case (int)DOWN_LEFT:
                        _oldTargetDirection = LEFT;
                        break;
                    default:
                        break;
                }
            }
            switch ((int)_oldTargetDirection)
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
                    _oldTargetDirection = UP;
                }

                if (toUserTankXDistance <= _aiLevel && location.Y <= userTank.location.Y)
                {
                    _oldTargetDirection = DOWN;
                }

                if (toUserTankYDistance <= _aiLevel && location.X >= userTank.location.X)
                {
                    _oldTargetDirection = LEFT;
                }

                if (toUserTankYDistance <= _aiLevel && location.X <= userTank.location.X)
                {
                    _oldTargetDirection = RIGHT;
                }
            }
        }

        private void ExperimentalAI()
        {
            Random random = new Random(DateTimeOffset.Now.Millisecond);
            if (colliding)
            {
                _targetDirection = _targetDirection.Rotate(MathHelper.PiOver4);
                base.MoveTank(_targetDirection);

                return;
            }

            Tank nearestUserTank = game.tank1;
            Vector2 differenceToUserTank = (location - game.tank1.location);
            float distanceToNearestUserTank = differenceToUserTank.Length();

            // Sprawdź jaki czołg gracza jest najbliżej
            foreach (Tank tank in new[] { game.tank1, game.tank2 })
            {
                float distanceToCurrentTank = (location - tank.location).Length();

                if (distanceToCurrentTank <= distanceToNearestUserTank)
                {
                    distanceToNearestUserTank = distanceToCurrentTank;
                    differenceToUserTank = (location - tank.location);
                    nearestUserTank = tank;
                }
            }

            // Jeżeli prędkość czołgu jest bliska zero nadaj maksymalną prędkośc w losowym kierunku.
            if (Math.Abs(_targetDirection.MoveX) < float.Epsilon && Math.Abs(_targetDirection.MoveY) < float.Epsilon)
            {
                _targetDirection = new TankControllerState(1, 0).Rotate(MathHelper.TwoPi * random.NextDouble());
            }
            else
            {
                _targetDirection = _targetDirection.SafelySpeedUp(1.1f).Rotate(MathHelper.PiOver4 / 10 * (random.NextDouble() - 0.5));
            }

            if (_kamikazeMode)
            {
                if (distanceToNearestUserTank <= (_aiLevel * 10))
                {
                    Explode();
                    if (nearestUserTank.barrier == false)
                    {
                        nearestUserTank.Die();
                    }
                }
                if (distanceToNearestUserTank < (_aiLevel * 150))
                {

                    var xDifference = -(differenceToUserTank.X / (_aiLevel * 150*1.5));
                    var yDifference = (differenceToUserTank.Y / (_aiLevel * 150*1.5));

                    if (Math.Abs(xDifference) < 0.003) xDifference = 0;
                    if (Math.Abs(yDifference) < 0.003) yDifference = 0;

                    var xAddition = (1 - Math.Abs(xDifference)) * 0.8 * Math.Sign(xDifference);
                    var yAddition = (1 - Math.Abs(yDifference)) * 0.8 * Math.Sign(yDifference);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Before addition: {xDifference} x {yDifference}");
#endif
                    _targetDirection = new TankControllerState(
                        moveX: (float)(xDifference + xAddition),
                        moveY: (float)(yDifference + yAddition));
                }
            }
            else
            {
                if (distanceToNearestUserTank < (_aiLevel * 150) && distanceToNearestUserTank > 50)
                {
                    _targetDirection = new TankControllerState(
                        moveX: (nearestUserTank.location.X - location.X) / (_aiLevel * 150),
                        moveY: (location.Y - nearestUserTank.location.Y) / (_aiLevel * 150),
                        fire: false, speedBoost: false, plantMine: false);
                }
            }

            base.MoveTank(_targetDirection);

        }
    }
}
