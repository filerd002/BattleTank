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
        public readonly TimeSpan RANDOM_DELAY = TimeSpan.FromSeconds(15);
        /// <summary>
        /// Określa ile czasu zostało do następnego losowania
        /// </summary>
        protected TimeSpan _timeLeftToRandom = TimeSpan.Zero;
        #endregion

        public string PowerUpSpriteName;
        public Vector2 location;
        public PowerUpType type;
        public Rectangle PowerUpkRect;
        public bool alive;
        public Particlecloud respawnParticles;
        public Particlecloud deathParticles;
        public Texture2D whiteRectangle;
        public bool colliding = false;
        public int controlSound = 1;
        public Game1 game { get; set; }
        public Vector2 origin { get; set; }
        public Texture2D PowerUpTexture { get; set; }
        public PowerUp(Game1 _game)
        {
            game = _game;
            location = new Vector2(_game.randy.Next(50, _game.graphics.PreferredBackBufferWidth - 50), _game.randy.Next(50, _game.graphics.PreferredBackBufferHeight - 50));
            type = (PowerUp.PowerUpType)game.randy.Next(Enum.GetNames(typeof(PowerUp.PowerUpType)).Length);
            switch (type)
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
            origin = new Vector2(location.X + PowerUpTexture.Width / 2f, location.Y + PowerUpTexture.Height / 2f);
            whiteRectangle = new Texture2D(_game.GraphicsDevice, 1, 1);
            alive = true;
            respawnParticles = new Particlecloud(origin, game, 1, whiteRectangle, Color.Gold, 2, 50);
            //  deathParticles = new Particlecloud(origin, game, 1, whiteRectangle, Color.Gold, 2, 50);
            PowerUpkRect = new Rectangle((int)location.X - (PowerUpTexture.Width / 2), (int)location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);
        }


        public virtual void Update(GameTime gameTime)
        {

            _timeLeftToRandom -= gameTime.ElapsedGameTime;

            if (alive && _timeLeftToRandom.TotalSeconds>0)
            {
               

                PowerUpkRect = new Rectangle((int)location.X - (PowerUpTexture.Width / 2), (int)location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);

                colliding = false;

                if (game.tank1 != null && (game.RandomPowerUp.isColliding(game.tank1.tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.tank1.lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.tank1.armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.tank1.StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.tank1.strong++;
                            break;
                        case PowerUpType.MINE:
                            game.tank1.mines++;
                            break;
                        case PowerUpType.MATRIX:                         
                                foreach (AI_Tank et in game.enemyTanks)
                                {
                                    et.Frozen();
                                }
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;


                    }



                    alive = false;
                }
                else if (game.tank2 != null && (game.RandomPowerUp.isColliding(game.tank2.tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.tank2.lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.tank2.armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.tank2.StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.tank2.strong++;
                            break;
                        case PowerUpType.MINE:
                            game.tank2.mines++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in game.enemyTanks)
                                {
                                    et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            break;

                    }



                    alive = false;
                }
                else if ((game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 || game.gameState == Game1.GameState.GAME_RUNNING_PLAYERS_2_AND_CPU) && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 1 && (game.RandomPowerUp.isColliding(game.enemyTanks[0].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[0].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[0].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[0].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[0].strong++;
                            break;
                        case PowerUpType.MATRIX:
                             foreach (AI_Tank et in game.enemyTanks)
                                { 
                                    if(!et.Equals(game.enemyTanks[0]))
                                    et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }
                else if ((game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 || game.gameState == Game1.GameState.GAME_RUNNING_PLAYERS_2_AND_CPU) && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 2 && (game.RandomPowerUp.isColliding(game.enemyTanks[1].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[1].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[1].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[1].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[1].strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in game.enemyTanks)
                                {
                                    if (!et.Equals(game.enemyTanks[1]))
                                        et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }
                else if (game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 3 && (game.RandomPowerUp.isColliding(game.enemyTanks[2].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[2].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[2].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[2].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[2].strong++;
                            break;
                        case PowerUpType.MATRIX:
                            foreach (AI_Tank et in game.enemyTanks)
                                {
                                    if (!et.Equals(game.enemyTanks[2]))
                                        et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }

                else if (game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 4 && (game.RandomPowerUp.isColliding(game.enemyTanks[3].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[3].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[3].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[3].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[3].strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in game.enemyTanks)
                                {
                                    if (!et.Equals(game.enemyTanks[3]))
                                        et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }
                else if (game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 5 && (game.RandomPowerUp.isColliding(game.enemyTanks[4].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[4].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[4].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[4].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[4].strong++;
                            break;
                        case PowerUpType.MATRIX:
                                foreach (AI_Tank et in game.enemyTanks)
                                {
                                    if (!et.Equals(game.enemyTanks[4]))
                                        et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }
                else if (game.gameState == Game1.GameState.GAME_RUNNING_PLAYER_1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 6 && (game.RandomPowerUp.isColliding(game.enemyTanks[5].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case PowerUpType.HEART:
                            game.enemyTanks[5].lives += 0.25f;
                            break;
                        case PowerUpType.ARMOR:
                            game.enemyTanks[5].armor += 0.25f;
                            break;
                        case PowerUpType.BARRIER:
                            game.enemyTanks[5].StartBarrier();
                            break;
                        case PowerUpType.AMMO:
                            game.enemyTanks[5].strong++;
                            break;
                        case PowerUpType.MATRIX:
                           foreach (AI_Tank et in game.enemyTanks)
                                {
                                    if (!et.Equals(game.enemyTanks[5]))
                                        et.Frozen();
                                }
                            if (game.tank1.alive)
                                game.tank1.Frozen();
                            if (game.tank2.alive)
                                game.tank2.Frozen();
                            break;
                    }



                    alive = false;
                }

            }
            else
            {
                _timeLeftToRandom = TimeSpan.Zero;
                alive = false;
            }
            respawnParticles.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive && _timeLeftToRandom.TotalSeconds > 0)
            {
                PowerUpkRect = new Rectangle((int)location.X - (PowerUpTexture.Width / 2), (int)location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);

                colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(PowerUpkRect).depth > 0))
                            {
                                colliding = true;
                            }
                        }
                        else { continue; }

                    }
                }

                if (!colliding)
                {
                    if (game.RandomPowerUp.controlSound == 1)
                    {
                        game.sound.PlaySound(Sound.Sounds.RESPAWN);
                        game.RandomPowerUp.controlSound = 0;

                    }

                    spriteBatch.Draw(PowerUpTexture, location, null, null);


                    respawnParticles.Draw(spriteBatch);


                }
                else { }
                
            }
            else {
                _timeLeftToRandom = TimeSpan.Zero;
                alive = false;
            }


        }

        public virtual void Random()
        {
            if (_timeLeftToRandom.TotalSeconds <= 0 )
            {
           
                _timeLeftToRandom = RANDOM_DELAY;
            }    

        }
        public Collision isColliding(Rectangle possibleCollisionRect)
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
