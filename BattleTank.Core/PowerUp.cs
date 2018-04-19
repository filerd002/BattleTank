﻿using System;
using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class PowerUp
    {


        public string PowerUpSpriteName;
        public Vector2 location;
        public int type;
        public Rectangle PowerUpkRect;
        public const int HEART = 0;
        public const int ARMOR = 1;
        public const int BARRIER = 2;
        public const int AMMO = 3;
        public const int MINE = 4;
        public const int MATRIX = 5;
        public bool alive;
        public Particlecloud respawnParticles;
        public Particlecloud deathParticles;
        public Texture2D whiteRectangle;
        public bool colliding = false;
        public int controlSound = 1;
        public Game1 game { get; set; }
        public Vector2 origin { get; set; }
        public Texture2D PowerUpTexture { get; set; }
        public PowerUp(Game1 _game, int _type, Vector2 _location, String _PowerUpSpriteName, Texture2D _whiteRectangle)
        {
            game = _game;
            location = _location;
            PowerUpTexture = _game.Content.Load<Texture2D>(_PowerUpSpriteName);
            origin = new Vector2(location.X + PowerUpTexture.Width / 2f, location.Y + PowerUpTexture.Height / 2f);
            type = _type;
            whiteRectangle = _whiteRectangle;
            alive = true;
            respawnParticles = new Particlecloud(origin, game, 1, whiteRectangle, Color.Gold, 2, 50);
            //  deathParticles = new Particlecloud(origin, game, 1, whiteRectangle, Color.Gold, 2, 50);
            PowerUpkRect = new Rectangle((int)location.X - (PowerUpTexture.Width / 2), (int)location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);
        }

        public virtual void Update(GameTime gameTime)
        {


            if (alive)
            {


                PowerUpkRect = new Rectangle((int)location.X - (PowerUpTexture.Width / 2), (int)location.Y - (PowerUpTexture.Height / 2), PowerUpTexture.Width, PowerUpTexture.Height);

                colliding = false;

                if (game.tank1 != null && (game.RandomPowerUp.isColliding(game.tank1.tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.tank1.lives += 0.25f;
                            break;
                        case ARMOR:
                            game.tank1.armor += 0.25f;
                            break;
                        case BARRIER:
                            game.tank1.StartBarrier();
                            break;
                        case AMMO:
                            game.tank1.strong++;
                            break;
                        case MINE:
                            game.tank1.mines++;
                            break;
                        case MATRIX:                         
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
                        case HEART:
                            game.tank2.lives += 0.25f;
                            break;
                        case ARMOR:
                            game.tank2.armor += 0.25f;
                            break;
                        case BARRIER:
                            game.tank2.StartBarrier();
                            break;
                        case AMMO:
                            game.tank2.strong++;
                            break;
                        case MINE:
                            game.tank2.mines++;
                            break;
                        case MATRIX:
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
                else if ((game.gameState == game.gameRunningPlayer1 || game.gameState == game.gameRunningPlayers2andCPU) && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 1 && (game.RandomPowerUp.isColliding(game.enemyTanks[0].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[0].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[0].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[0].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[0].strong++;
                            break;
                        case MATRIX:
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
                else if ((game.gameState == game.gameRunningPlayer1 || game.gameState == game.gameRunningPlayers2andCPU) && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 2 && (game.RandomPowerUp.isColliding(game.enemyTanks[1].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[1].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[1].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[1].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[1].strong++;
                            break;
                        case MATRIX:
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
                else if (game.gameState == game.gameRunningPlayer1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 3 && (game.RandomPowerUp.isColliding(game.enemyTanks[2].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[2].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[2].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[2].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[2].strong++;
                            break;
                        case MATRIX:
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

                else if (game.gameState == game.gameRunningPlayer1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 4 && (game.RandomPowerUp.isColliding(game.enemyTanks[3].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[3].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[3].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[3].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[3].strong++;
                            break;
                        case MATRIX:
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
                else if (game.gameState == game.gameRunningPlayer1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 5 && (game.RandomPowerUp.isColliding(game.enemyTanks[4].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[4].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[4].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[4].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[4].strong++;
                            break;
                        case MATRIX:
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
                else if (game.gameState == game.gameRunningPlayer1 && game.iloscCPUKamikaze + game.iloscCPUKlasyk >= 6 && (game.RandomPowerUp.isColliding(game.enemyTanks[5].tankRect).depth > 0))
                {
                    colliding = true;

                    switch (game.RandomPowerUp.type)
                    {
                        case HEART:
                            game.enemyTanks[5].lives += 0.25f;
                            break;
                        case ARMOR:
                            game.enemyTanks[5].armor += 0.25f;
                            break;
                        case BARRIER:
                            game.enemyTanks[5].StartBarrier();
                            break;
                        case AMMO:
                            game.enemyTanks[5].strong++;
                            break;
                        case MATRIX:
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




                //Colision dla powerup

                //

            }
            else
            {

            }
            respawnParticles.Update(gameTime);




        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
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
                else
                    game.timerPowerUp = -1;
            }
            else { }


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