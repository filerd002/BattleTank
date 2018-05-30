using System;
using System.Collections.Generic;
using System.Linq;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.Tanks
{
    public class AI_Tank : Tank
    {
        private readonly int _aiLevel;  //Można wykorzystać do ustawienia poziomu trudności
        private TankControllerState _targetDirection;
        private float _oldTargetDirection;
        private readonly List<Bullet> _enemyBullets = new List<Bullet>();
        private readonly bool _kamikazeMode = false;

        private readonly TimeSpan MAX_AGGRESSIVE_TIME = new TimeSpan(0, 0, 0, 5);
        private TimeSpan _aggressiveTimeLeft = TimeSpan.Zero;
        private bool _isAggressive => _aggressiveTimeLeft.TotalMilliseconds > 0;

        Tank[] tanks;

        public AI_Tank(Game1 game, TankColors tankSpriteName, Vector2 location, Vector2 maxSpeed,
            float rotation, int player, float scale, Texture2D whiteRectangle, int strong,
            bool barrier, bool frozen, float targetDirection, int aiLevel, bool kamikazeMode = false)
            : base(game, tankSpriteName, location, maxSpeed, rotation, player, scale,
                  whiteRectangle, strong, 0, barrier, frozen, null)
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

        public override void Update(GameTime gameTime)
        {
            _aggressiveTimeLeft -= gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        public override bool TryFire(out Bullet[] bullets)
        {
            bullets = Fire();

            if (bullets?.Length > 0 )
                return true;

            return false;
        }

        /// <inheritdoc />
        public override void Hit()
        {
            _aggressiveTimeLeft = MAX_AGGRESSIVE_TIME;
            base.Hit();
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
                switch ((int)(_oldTargetDirection * 10))
                {
                    case (int)(UP * 10):
                        _oldTargetDirection = UP_RIGHT;
                        break;
                    case (int)(UP_RIGHT * 10):
                        _oldTargetDirection = RIGHT;
                        break;
                    case (int)(RIGHT * 10):
                        _oldTargetDirection = DOWN_RIGHT;
                        break;
                    case (int)(DOWN_RIGHT * 10):
                        _oldTargetDirection = DOWN;
                        break;
                    case (int)(DOWN * 10):
                        _oldTargetDirection = DOWN_LEFT;
                        break;
                    case (int)(DOWN_LEFT * 10):
                        _oldTargetDirection = LEFT;
                        break;
                    case (int)(LEFT * 10):
                        _oldTargetDirection = UP_LEFT;
                        break;
                    case (int)(UP_LEFT * 10):
                        _oldTargetDirection = UP;
                        break;
                }
            }
            switch ((int)(_oldTargetDirection * 10))
            {
                case (int)(UP * 10):
                    MoveUp(false);
                    Rotate(UP);         
                    break;
                case (int)(RIGHT * 10):
                    MoveRight(false);
                    Rotate(RIGHT);
                    break;
                case (int)(LEFT * 10):
                    MoveLeft(false);
                    Rotate(LEFT);
                    break;
                case (int)(DOWN * 10):
                    MoveDown(false);
                    Rotate(DOWN);
                    break;
                case (int)(UP_LEFT * 10):
                    MoveUp(false);
                    MoveLeft(false);
                    Rotate(UP_LEFT);
                    break;
                case (int)(DOWN_LEFT * 10):
                    MoveDown(false);
                    MoveLeft(false);
                    Rotate(DOWN_LEFT);
                    break;
                case (int)(UP_RIGHT * 10):
                    MoveUp(false);
                    MoveRight(false);
                    Rotate(UP_RIGHT);
                    break;
                case (int)(DOWN_RIGHT * 10):
                    MoveDown(false);
                    MoveRight(false);
                    Rotate(DOWN_RIGHT);
                    break;
                default:
                    break;
            }

            if (game.gameReturn == Game1.GameState.GAME_RUNNING_PLAYER_1)
                tanks = new[] { game.tank1 };
            else
                tanks = new[] { game.tank1, game.tank2 };

            foreach (var userTank in tanks.Where(d => d.alive))
            {
                if (_kamikazeMode)
                {
                    if ((location - userTank.location).Length() <= _aiLevel * 2) // TODO: należy zamienić ten mnożnik na jakąś stałą
                    {
                        Explode();
                        if (userTank.Barrier == false)
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
                _targetDirection = _targetDirection.Rotate(0.1);
                base.MoveTank(_targetDirection);

                return;
            }

            Tank nearestUserTank = game.tank1;
            Vector2 differenceToUserTank = (location - game.tank1.location);
            float distanceToNearestUserTank = differenceToUserTank.Length();

            if (game.gameReturn == Game1.GameState.GAME_RUNNING_PLAYER_1)
                tanks = new[] { game.tank1 };
            else
                tanks = new[] { game.tank1, game.tank2 };

            // Sprawdź jaki czołg gracza jest najbliżej
            foreach (Tank tank in tanks)
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
                    if (nearestUserTank.Barrier == false)
                    {
                        nearestUserTank.Explode();
                    }
                }
            }

            float sightDistance = _isAggressive ? float.PositiveInfinity : 150;
            if (distanceToNearestUserTank < (_aiLevel * sightDistance))
            {

                double xDifference = -(differenceToUserTank.X / (_aiLevel * 150 * 1.5));
                double yDifference = (differenceToUserTank.Y / (_aiLevel * 150 * 1.5));

                if (Math.Abs(xDifference) < 0.003) xDifference = 0;
                if (Math.Abs(yDifference) < 0.003) yDifference = 0;

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Before addition: {xDifference} x {yDifference}");
#endif

                if (_kamikazeMode)
                {
                    // To normalnie mogą być dwie linijki, ale rozbito na cztery aby ułatwić debugowanie.
                    double xAddition = (1 - Math.Abs(xDifference)) * 0.8 * Math.Sign(xDifference);
                    double yAddition = (1 - Math.Abs(yDifference)) * 0.8 * Math.Sign(yDifference);
                    xDifference += xAddition;
                    yDifference += yAddition;
                }

                _targetDirection = new TankControllerState(
                    moveX: (float)(xDifference),
                    moveY: (float)(yDifference),
                    safely: true);
            }

            base.MoveTank(_targetDirection);

        }
    }
}
