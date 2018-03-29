using System;
using BattleTank.Input;
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

        /// <summary>
        /// Opóźnienie pomiędzy kolejnymi strzałami w milisekundach
        /// </summary>
        public readonly TimeSpan FIRE_DELAY = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Określa ile czasu zostało do następnego strzału
        /// </summary>
        private TimeSpan _timeLeftToNextShot = TimeSpan.Zero;
        /// <summary>
        /// Opóźnienie pomiędzy kolejnymi strzałami w miliseksundach
        /// </summary>
        public readonly TimeSpan PLANT_MINE_DELAY = TimeSpan.FromMilliseconds(2000);
        /// <summary>
        /// Okresla ile zcasu zostało do następnego strzału
        /// </summary>
        private TimeSpan _timeLeftToPlantMine = TimeSpan.Zero;

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
                _timeLeftToNextShot -= gameTime.ElapsedGameTime;
                _timeLeftToPlantMine -= gameTime.ElapsedGameTime;

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
            else angle += (float)Math.Atan(yMovement / xMovement);

            if (yMovement < 0)
                angle += MathHelper.Pi; // Kiedy czołg jedzie w doł to odwróc wyniki, bo tg ma zakress od -pi/2 do pi/2

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

        public bool TryFire(out Bullet[] bullets)
        {
            TankControllerState controller = _tankActionProvider.GetTankControllerState();
            if (controller.Fire)
            {
                bullets = Fire();
                return true;
            }

            bullets = new Bullet[0];
            return false;
        }
        public Bullet[] Fire()
        {
            if (!alive) return new Bullet[0];

            if (_timeLeftToNextShot > TimeSpan.Zero) return new Bullet[0];

            _timeLeftToNextShot = FIRE_DELAY;

            game.sound.PlaySound(Sound.Sounds.SHOT);

            Color color = Color.Blue;

            if (player == 1)
                color = Color.Green;
            if (player == 2)
                color = Color.Red;

            float xFraction = (float) Math.Cos(rotation); // składowa pozioma aktualnego obrotu
            float yFraction = (float) Math.Sin(rotation); // składowa pionowa akutlanego obrutu

            float bulletMaxSpeed = 20;
            Vector2 bulletSpeed = new Vector2(xFraction * bulletMaxSpeed, yFraction * bulletMaxSpeed);

            float bulletShowDistance = 5; // Odległość od czołgu w jakiej ma się pojawić pocisk, zbyt mała może powodować kolizje z strzelającym
            Rectangle bulletStartPosition = new Rectangle( (int)(location.X + xFraction * bulletShowDistance),
                                                           (int)(location.Y + yFraction * bulletShowDistance), 5, 5);

            var bullets = new Bullet[strong];

            for (int i = 0; i < this.strong; i++) // stwórz na raz tyle pociskow, ile mocy ma czołg.
                bullets[i] = new Bullet(game, bulletStartPosition, bulletSpeed, color, player, 0, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y, 5, 20));

            return bullets;
        }

        public bool TryPlantMine(out Mine mine)
        {
            mine = null;

            if (mines <= 0 || _timeLeftToPlantMine > TimeSpan.Zero) return false;

            TankControllerState controller = _tankActionProvider.GetTankControllerState();
            if (!controller.PlantMine)
                return false;
            
            if (!alive) return false;

            _timeLeftToPlantMine = PLANT_MINE_DELAY;

            Color color = Color.Orange;

            Rectangle minePositiono = new Rectangle(this.location.ToPoint(), new Point(20, 20));
            mine = new Mine(game, minePositiono, Vector2.Zero, Color.Orange, player, 0);
            mines--;
            _timeLeftToPlantMine = PLANT_MINE_DELAY;
            return true;
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
