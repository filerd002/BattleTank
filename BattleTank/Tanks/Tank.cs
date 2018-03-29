using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Tanks
{
    public class Tank
    {
        //data members
        public Vector2 location;
        public Vector2 startingLocation;
        public Vector2 speed;
        public float rotation { get; set; }
        public Texture2D tankTexture { get; set; }
        public Vector2 origin { get; set; }
        public Game1 game { get; set; }
        public int player { get; set; }
        public int lives { get; set; }

        public int strong { get; set; }

        public int mines { get; set; }
        public bool barrier { get; set; }
        public int armor { get; set; }
        public float scale { get; set; }
        private ITankActionProvider _tankActionProvider;
        public bool alive;
        public Rectangle tankRect;
        public Particlecloud deathParticles;
        public Particlecloud respawnParticles;
        public Particlecloud hitParticles;
        public const float UP = -MathHelper.PiOver2;
        public const float UP_RIGHT = -MathHelper.PiOver4;
        public const float RIGHT = 0;
        public const float DOWN_RIGHT = MathHelper.PiOver4;
        public const float DOWN = MathHelper.PiOver2;
        public const float DOWN_LEFT = MathHelper.Pi - MathHelper.PiOver4;
        public const float LEFT = MathHelper.Pi;
        public const float UP_LEFT = -(MathHelper.Pi - MathHelper.PiOver4);
        public bool colliding = false;
        public Texture2D whiteRectangle;

        public bool enemy = false;
        public Texture2D barrierTexture;
        public Rectangle barrierRect;
        public Vector2 barrierLocation;
        public bool frozen = false;
        private float timerBush = 0f;

        //generic constructor
        public Tank()
        {

        }

        //overloaded constructor(s)
        public Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle, int _strong, int _mines, bool _barrier, ITankActionProvider tankActionProvider)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            location = _location;

            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = _scale;
            whiteRectangle = _whiteRectangle;
            strong = _strong;
            mines = _mines;
            barrier = _barrier;
            _tankActionProvider = tankActionProvider;
            alive = true;
            lives = 3;
            armor = 3;
            respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            hitParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (barrier)
                spriteBatch.Draw(barrierTexture, barrierLocation, null, null);
            else
            {
            }
            if (alive)
            {
                spriteBatch.Draw(tankTexture, location, null, null, origin, rotation, null, null);
            }
            else
            {

            }

            respawnParticles.Draw(spriteBatch);
            deathParticles.Draw(spriteBatch);
            if (hitParticles != null)
            {
                hitParticles.Draw(spriteBatch);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (!frozen)
                {
                    Move();
                }

                tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);

                colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.type == Tile.WALL || tile.type == Tile.WATER || tile.type == Tile.BUSH)
                        {
                            if ((tile.isColliding(tankRect).depth > 0))
                            {
                                float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                                timerBush -= timer;

                                colliding = true;
                                Collision collision = tile.isColliding(tankRect);
                                switch (collision.side)
                                {
                                    case Collision.Side.TOP:
                                        if (tile.type == Tile.WALL || tile.type == Tile.WATER)
                                            location.Y += collision.depth;
                                        if (tile.type == Tile.BUSH && timerBush <= 0)
                                        {
                                            game.sound.PlaySound(Sound.Sounds.RUSTLING);
                                            timerBush = 0.5f;
                                        }
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (tile.type == Tile.WALL || tile.type == Tile.WATER)
                                            location.Y -= collision.depth;
                                        if (tile.type == Tile.BUSH && timerBush <= 0)
                                        {
                                            game.sound.PlaySound(Sound.Sounds.RUSTLING);
                                            timerBush = 0.5f;
                                        }
                                        break;
                                    case Collision.Side.LEFT:
                                        if (tile.type == Tile.WALL || tile.type == Tile.WATER)
                                            location.X += collision.depth;
                                        if (tile.type == Tile.BUSH && timerBush <= 0)
                                        {
                                            game.sound.PlaySound(Sound.Sounds.RUSTLING);
                                            timerBush = 0.5f;
                                        }
                                        break;
                                    case Collision.Side.RIGHT:
                                        if (tile.type == Tile.WALL || tile.type == Tile.WATER)
                                            location.X -= collision.depth;
                                        if (tile.type == Tile.BUSH && timerBush <= 0)
                                        {
                                            game.sound.PlaySound(Sound.Sounds.RUSTLING);
                                            timerBush = 0.5f;
                                        }
                                        break;

                                }
                            }
                        }

                        else { continue; }

                    }
                }


            }
            else
            {

            }
            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null)
            {
                hitParticles.Update(gameTime);
            }


        }

        public virtual void Move()
        {
            TankControllerState controller = _tankActionProvider.GetTankControllerState();
            int xMovement = controller.MoveX;
            int yMovement = controller.MoveY;

            if (xMovement == 0 && yMovement == 0) return;

            float angle = -MathHelper.PiOver2; // Czołg w wyjściowej pozycji jest obrócony do góry.

            if (xMovement == 0) angle = -MathHelper.PiOver2; // Pozostaw wartość niezmienioną
            else if (yMovement == 0 && xMovement < 0) angle -= MathHelper.PiOver2; // Obróć w lewo o 90 stopni
            else if (yMovement == 0 && xMovement > 0) angle += MathHelper.PiOver2; // Obróć w prawo o 90 stopni
            else angle += (float) Math.Atan(yMovement / xMovement);

            if (yMovement < 0)
                angle += MathHelper
                    .Pi; // Kiedy czołg jedzie w doł to odwróc wyniki, bo tg ma zakress od -pi/2 do pi/2

            Rotate(angle);

            Move(angle, controller.SpeedBoost);
        }


        public void Move(float angle, bool isSpeeBoostUp)
        {
            this.location.X += (float)Math.Cos(angle) * this.speed.X * (isSpeeBoostUp ? 2 : 1);
            this.location.Y += (float)Math.Sin(angle) * this.speed.Y * (isSpeeBoostUp ? 2 : 1);
        }

        public void MoveLeft(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.X -= (2) + this.speed.X;
            }

            else
            {
                this.location.X -= this.speed.X;
            }
        }
        public void MoveRight(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.X += (2) + this.speed.X;
            }

            else
            {
                this.location.X += this.speed.X;
            }
        }
        public void MoveUp(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y -= (2) + this.speed.Y;
            }

            else
            {
                this.location.Y -= this.speed.Y;
            }
        }
        public void MoveDown(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y += (2) + this.speed.Y;
            }

            else
            {
                this.location.Y += this.speed.Y;
            }
        }
        public void Rotate(float angle)
        {
            this.rotation = angle;
        }
        public Bullet Fire()
        {
            game.sound.PlaySound(Sound.Sounds.SHOT);
            if (alive)
            {
                Color color = Color.Blue;



                if (player == 1)
                    color = Color.Green;
                if (player == 2)
                    color = Color.Red;




                if (rotation == UP)
                {

                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y, 5, 5), new Vector2(0, -20), color, player, UP, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y, 5, 20));
                }
                else if (rotation == UP_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 2, 5, 5), new Vector2(10, -10), color, player, UP_RIGHT, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 2, 20, 20));
                }
                else if (rotation == RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 5, (int)location.Y - 2, 5, 5), new Vector2(20, 0), color, player, RIGHT, whiteRectangle, new Rectangle((int)location.X - 5, (int)location.Y - 2, 20, 5));
                }
                else if (rotation == DOWN_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y, 5, 5), new Vector2(10, 10), color, player, DOWN_RIGHT, whiteRectangle, new Rectangle((int)location.X, (int)location.Y, 20, 20));
                }
                else if (rotation == DOWN)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 5, 5, 5), new Vector2(0, 20), color, player, DOWN, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 5, 5, 20));
                }
                else if (rotation == DOWN_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 2, 5, 5), new Vector2(-10, 10), color, player, DOWN_LEFT, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 2, 20, 20));
                }
                else if (rotation == LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y - 2, 5, 5), new Vector2(-20, 0), color, player, LEFT, whiteRectangle, new Rectangle((int)location.X, (int)location.Y - 2, 20, 5));
                }
                else if (rotation == UP_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 3, (int)location.Y - 3, 5, 5), new Vector2(-10, -10), color, player, UP, whiteRectangle, new Rectangle((int)location.X - 3, (int)location.Y - 3, 20, 20));
                }
                else
                {
                    return null;
                }

            }
            return null;

        }


        public virtual void Barrier()
        {
            barrierTexture = game.Content.Load<Texture2D>("Graphics//barrier");
            barrierRect = new Rectangle((int)location.X - (barrierTexture.Width / 2), (int)location.Y - (barrierTexture.Height / 2), barrierTexture.Width, barrierTexture.Height);
            barrierLocation = new Vector2((int)location.X - (barrierTexture.Width / 2), (int)location.Y - (barrierTexture.Height / 2));
            this.barrier = true;
        }


        public virtual void Hit()
        {
            game.sound.PlaySound(Sound.Sounds.HIT);
            armor -= 1;
            if (armor < 1)
            {
                Die();
            }
            else
            {
                hitParticles = new Particlecloud(location, game, player, whiteRectangle, Color.OrangeRed, 2, 6);
            }
        }
        public virtual void Die()
        {
            game.sound.PlaySound(Sound.Sounds.EXPLOSION);
            if (alive)
            {
                deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.OrangeRed, 2);
                alive = false;
                lives--;
                location = new Vector2(-100, -100);
            }
        }
        public virtual void Respawn(Vector2 _location)
        {

            if (!alive && lives > 0)
            {
                game.sound.PlaySound(Sound.Sounds.RESPAWN);
                location = _location;
                armor = 3;
                respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Green, 2);
                alive = true;
            }
        }

        public void Explode()
        {
            if (alive)
            {

                Die();
            }
        }



    }
}
