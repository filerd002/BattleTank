using System;
using System.Collections.Generic;
using BattleTank.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Tanks
{
    public class AI_Tank : Tank
    {
        public int Level_AI;  //Można wykorzystać do ustawienia poziomu trudności
        public float targetDirection;
        public List<Bullet> enemyBullets = new List<Bullet>();
        private float delayOfFire = 1;
        private const float FIRE_DELAY = 1;
        public bool KamikazeMode = false;


        public AI_Tank(Game1 game, string tankSpriteName, Vector2 location, Vector2 maxSpeed, 
            float rotation, int player, float scale, Texture2D whiteRectangle, int strong, 
            bool barrier, float targetDirection, int aiLevel, bool kamikazeMode = false)
            : base(game, tankSpriteName, location, maxSpeed, rotation, player, scale, 
                  whiteRectangle, strong, 0, barrier, null)
        {
            enemy = true;
            Level_AI = aiLevel;

            this.targetDirection = targetDirection;

            KamikazeMode = kamikazeMode;

            if (KamikazeMode)
            {
                speed = new Vector2(3, 3);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Bullet b in enemyBullets)
            {
                if (b != null && b.alive)
                {
                    b.Draw(spriteBatch);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!alive) return;

            float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            delayOfFire -= timer;
            if (delayOfFire <= 0)
            {
                if (!frozen)
                {
                    enemyBullets.AddRange(Fire());
                }
                delayOfFire = FIRE_DELAY;
            }
            base.Update(gameTime);
            foreach (Bullet b in enemyBullets)
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
                switch ((int)targetDirection)
                {
                    case (int)UP:
                        targetDirection = RIGHT;
                        break;
                    case (int)RIGHT:
                        targetDirection = DOWN;
                        break;
                    case (int)LEFT:
                        targetDirection = UP;
                        break;
                    case (int)DOWN:
                        targetDirection = LEFT;
                        break;
                    case (int)UP_LEFT:
                        targetDirection = LEFT;
                        break;
                    case (int)DOWN_LEFT:
                        targetDirection = LEFT;
                        break;
                    default:
                        break;
                }
            }
            switch ((int)targetDirection)
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

            if (KamikazeMode)
            {

                if ((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && (location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI))
                {
                    Explode();
                    if (game.tank1.barrier == false)
                    {
                        game.tank1.Die();
                    }
                }
                if ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && (location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI))
                {
                    Explode();
                    if (game.tank2.barrier == false)
                    {
                        game.tank2.Die();
                    }

                }

            }

            if (game.gameState == game.gameRunningPlayers2andCPU)
            {

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y > game.tank1.location.Y) || ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && location.Y > game.tank2.location.Y))
                {


                    targetDirection = UP;

                }

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y < game.tank1.location.Y) || ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && location.Y < game.tank2.location.Y))
                {

                    targetDirection = DOWN;

                }

                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X > game.tank1.location.X) || ((location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI) && location.X > game.tank2.location.X))
                {


                    targetDirection = LEFT;

                }

                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X < game.tank1.location.X) || ((location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI) && location.X < game.tank2.location.X))
                {


                    targetDirection = RIGHT;


                }
            }
            else
            {

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y > game.tank1.location.Y))
                {


                    targetDirection = UP;

                }

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y < game.tank1.location.Y))
                {

                    targetDirection = DOWN;

                }

                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X > game.tank1.location.X))
                {


                    targetDirection = LEFT;

                }
                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X < game.tank1.location.X))
                {


                    targetDirection = RIGHT;


                }

            }


        }

    }
}
