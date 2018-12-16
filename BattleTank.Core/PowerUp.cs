using System;
using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class PowerUp
    {
        public enum PowerUpType
        {
            HEART,
            ARMOR,
            BARRIER,
            AMMO,
            MINE,
            MATRIX,
        }

        #region Random
        /// <summary>
        /// Opóźnienie pomiędzy kolejnymi losowaniami w sekundach
        /// </summary>
        public readonly TimeSpan RANDOM_DELAY = TimeSpan.FromSeconds(10);
        /// <summary>
        /// Określa ile czasu zostało do następnego losowania
        /// </summary>
        protected TimeSpan _timeLeftToRandom = TimeSpan.Zero;
        #endregion

        public string PowerUpSpriteName { get; set; }
        public Vector2 Location { get; set; }
        public PowerUpType Type { get; set; }
        public Rectangle PowerUpkRect { get; set; }
        public bool Alive { get; set; }
        public Particlecloud RespawnParticles { get; set; }
        public Particlecloud DeathParticles { get; set; }
        public Texture2D WhiteRectangle { get; set; }
        public bool Colliding { get; set; } = false;
        public int ControlSound { get; set; } = 1;
        public Game1 Game { get; set; }
        public Vector2 Origin { get; set; }
        public Texture2D PowerUpTexture { get; set; }
        public PowerUp(Game1 _game)
        {
            Game = _game;
            Location = new Vector2(_game.Randy.Next(50, _game.Graphics.PreferredBackBufferWidth - 50), _game.Randy.Next(50, _game.Graphics.PreferredBackBufferHeight - 50));
            Type = (PowerUp.PowerUpType)Game.Randy.Next(Enum.GetNames(typeof(PowerUp.PowerUpType)).Length);
            switch (Type)
            {
                case PowerUp.PowerUpType.HEART:
                    PowerUpSpriteName = "Graphics/PowerUpHeart";
                    break;
                case PowerUp.PowerUpType.ARMOR:
                    PowerUpSpriteName = "Graphics/PowerUpArmor";
                    break;
                case PowerUp.PowerUpType.BARRIER:
                    PowerUpSpriteName = "Graphics/PowerUpBarrier";
                    break;
                case PowerUp.PowerUpType.AMMO:
                    PowerUpSpriteName = "Graphics/PowerUpAmmo";
                    break;
                case PowerUp.PowerUpType.MINE:
                    PowerUpSpriteName = "Graphics/PowerUpMine";
                    break;
                case PowerUp.PowerUpType.MATRIX:
                    PowerUpSpriteName = "Graphics/PowerUpMatrix";
                    break;
            }
            PowerUpTexture = _game.Content.Load<Texture2D>(PowerUpSpriteName);
            Origin = new Vector2(Location.X + PowerUpTexture.Width / 2f, Location.Y + PowerUpTexture.Height / 2f);
            WhiteRectangle = new Texture2D(_game.GraphicsDevice, 1, 1);
            Alive = true;
            RespawnParticles = new Particlecloud(Origin, Game, 1, WhiteRectangle, Color.Gold, 2, 50);
            PowerUpkRect = new Rectangle((int)Location.X - (PowerUpTexture.Width / 2), (int)Location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);
        }


        public virtual void Update(GameTime gameTime)
        {

            _timeLeftToRandom -= gameTime.ElapsedGameTime;

            if (Alive && _timeLeftToRandom.TotalSeconds>0)
            {
               

                PowerUpkRect = new Rectangle((int)Location.X - (PowerUpTexture.Width / 2), (int)Location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);

                Colliding = false;

                if (Game.Tank1 != null && (Game.RandomPowerUp.IsColliding(Game.Tank1.TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.Tank1.Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.Tank1.Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.Tank1.StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.Tank1.Strong++;
                            break;
                        case PowerUpType.MINE:
                            Game.Tank1.Mines++;
                            break;
                        case PowerUpType.MATRIX:                         
                                foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    et.StartFrozen();
                                }
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;


                    }



                    Alive = false;
                }
                else if (Game.Tank2 != null && (Game.RandomPowerUp.IsColliding(Game.Tank2.TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.Tank2.Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.Tank2.Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.Tank2.StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.Tank2.Strong++;
                            break;
                        case PowerUpType.MINE:
                            Game.Tank2.Mines++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            break;

                    }



                    Alive = false;
                }
                else if ((Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 || Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYERS_2_AND_CPU) && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 1 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[0].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[0].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[0].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[0].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[0].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                             foreach (AI_Tank et in Game.EnemyTanks)
                                { 
                                    if(!et.Equals(Game.EnemyTanks[0]))
                                {
                                    et.StartFrozen();
                                }
                            }
                            if (Game.Tank1.Alive)
                            {
                                Game.Tank1.StartFrozen();
                            }

                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                            {
                                Game.Tank2.StartFrozen();
                            }

                            break;
                    }



                    Alive = false;
                }
                else if ((Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 || Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYERS_2_AND_CPU) && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 2 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[1].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[1].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[1].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[1].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[1].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    if (!et.Equals(Game.EnemyTanks[1]))
                                        et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;
                    }



                    Alive = false;
                }
                else if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 3 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[2].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[2].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[2].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[2].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[2].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                            foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    if (!et.Equals(Game.EnemyTanks[2]))
                                        et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;
                    }



                    Alive = false;
                }

                else if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 4 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[3].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[3].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[3].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[3].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[3].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    if (!et.Equals(Game.EnemyTanks[3]))
                                        et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;
                    }



                    Alive = false;
                }
                else if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 5 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[4].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[4].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[4].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[4].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[4].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    if (!et.Equals(Game.EnemyTanks[4]))
                                        et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;
                    }



                    Alive = false;
                }
                else if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Settings.OpponentsCPUKamikaze + Game.Settings.OpponentsCPUClassic >= 6 && (Game.RandomPowerUp.IsColliding(Game.EnemyTanks[5].TankRect).Depth > 0))
                {
                    Colliding = true;

                    switch (Game.RandomPowerUp.Type)
                    {
                        case PowerUpType.HEART:
                            Game.EnemyTanks[5].Lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            Game.EnemyTanks[5].Armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            Game.EnemyTanks[5].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            Game.EnemyTanks[5].Strong++;
                            break;
                        case PowerUpType.MATRIX:
                           foreach (AI_Tank et in Game.EnemyTanks)
                                {
                                    if (!et.Equals(Game.EnemyTanks[5]))
                                        et.StartFrozen();
                                }
                            if (Game.Tank1.Alive)
                                Game.Tank1.StartFrozen();
                            if (Game.GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Game.Tank2.Alive)
                                Game.Tank2.StartFrozen();
                            break;
                    }



                    Alive = false;
                }

            }
            else
            {
                _timeLeftToRandom = TimeSpan.Zero;
                Alive = false;
            }
            RespawnParticles.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive && _timeLeftToRandom.TotalSeconds > 0)
            {
                PowerUpkRect = new Rectangle((int)Location.X - (PowerUpTexture.Width / 2), (int)Location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);

                Colliding = false;
                foreach (Tile[] tiles in Game.Map.MapCurrent)
                {
                    foreach (Tile tile in tiles)
                    {                   
                        if (tile != null && (tile.IsColliding(PowerUpkRect).Depth > 0))
                        {
                             Colliding = true;
                        }                      
                    }
                }

                if (!Colliding)
                {
                    if (Game.RandomPowerUp.ControlSound == 1)
                    {
                        Game.SoundMenu.PlaySound(Sound.Sounds.RESPAWN);
                        Game.RandomPowerUp.ControlSound = 0;

                    }
   
                    spriteBatch.Draw(PowerUpTexture, new Rectangle((int)Location.X, (int)Location.Y, Game.Map.TileWidth * 2/3, Game.Map.TileHeight * 2/3), Color.White);

                    RespawnParticles.Draw(spriteBatch);


                }
            
                
            }
            else {
                _timeLeftToRandom = TimeSpan.Zero;
                Alive = false;
            }


        }

        public virtual void Random()
        {
            if (_timeLeftToRandom.TotalSeconds <= 0 )
            {
           
                _timeLeftToRandom = RANDOM_DELAY;
            }    

        }
        public Collision IsColliding(Rectangle possibleCollisionRect)
        {

            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, PowerUpkRect);

            if (intersect.Width > 0 || intersect.Height > 0)
            {

                if (possibleCollisionRect.Top < PowerUpkRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > PowerUpkRect.Y)
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.TOP, depth);
                }
                if (possibleCollisionRect.Bottom > PowerUpkRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.BOTTOM, depth);
                }
                if (possibleCollisionRect.Left < PowerUpkRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > PowerUpkRect.Right)
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.LEFT, depth);
                }
                if (possibleCollisionRect.Right > PowerUpkRect.Right - PowerUpkRect.Width && possibleCollisionRect.Right > PowerUpkRect.Left && Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.RIGHT, depth);
                }
            }

            return new Collision();
        }
    }
}
