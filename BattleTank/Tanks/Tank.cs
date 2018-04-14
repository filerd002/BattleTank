using System;
using BattleTank.Extensions;
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
        public float lives { get; set; }

        public int strong { get; set; }

        public int mines { get; set; }
        public float armor { get; set; }
        public float scale { get; set; }
        public ITankActionProvider TankActionProvider { get; set; }
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
        public Vector2 barrierLocation;

        private float timerBush = 0f;

        public bool barrier => _timeLeftForBarrier > TimeSpan.Zero;
        public bool frozen => _timeLeftForFrozen > TimeSpan.Zero;

        /// <summary>
        /// Czas po jakim czolg zostanie rozmrozony
        /// </summary>
        public readonly TimeSpan FROZEN_TIME = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Czas jaki został do rozmrozenia
        /// </summary>
        protected TimeSpan _timeLeftForFrozen = TimeSpan.Zero;
        /// <summary>
        /// Czas po jakim osłona zostanie zdjęcia
        /// </summary>
        public readonly TimeSpan BARRIER_TIME = TimeSpan.FromSeconds(10);
        /// <summary>
        /// Czas jaki został do zniknięcie osłony
        /// </summary>
        protected TimeSpan _timeLeftForBarrier = TimeSpan.Zero;
        /// <summary>
        /// Opóźnienie pomiędzy kolejnymi strzałami w milisekundach
        /// </summary>
        public readonly TimeSpan FIRE_DELAY = TimeSpan.FromMilliseconds(500);
        /// <summary>
        /// Określa ile czasu zostało do następnego strzału
        /// </summary>
        protected TimeSpan _timeLeftToNextShot = TimeSpan.Zero;
        /// <summary>
        /// Opóźnienie pomiędzy kolejnymi strzałami w miliseksundach
        /// </summary>
        public readonly TimeSpan PLANT_MINE_DELAY = TimeSpan.FromMilliseconds(2000);
        /// <summary>
        /// Okresla ile zcasu zostało do następnego strzału
        /// </summary>
        private TimeSpan _timeLeftToPlantMine = TimeSpan.Zero;

        private TimeSpan _timeLeftForVibration = TimeSpan.Zero;

        //overloaded constructor(s)
        public Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed,
                    float _rotation, int _player, float _scale, Texture2D _whiteRectangle,
                    int _strong, int _mines, bool _barrier, bool _frozen, ITankActionProvider tankActionProvider)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            barrierTexture = _game.Content.Load<Texture2D>("Graphics/barrier");

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
            TankActionProvider = tankActionProvider;
            alive = true;
            lives = 3;
            armor = 1;
            respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            hitParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);

            if (_barrier) Barrier();
            if (_frozen) Frozen();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (barrier)
            {
                barrierLocation = new Vector2((int)location.X - (barrierTexture.Width / 2), (int)location.Y - (barrierTexture.Height / 2));

                if (_timeLeftForBarrier.Seconds <= 3)
                {
                    if (_timeLeftForBarrier.Milliseconds.IsWithin(500, 750) || _timeLeftForBarrier.Milliseconds.IsWithin(0, 250))
                        spriteBatch.Draw(barrierTexture, barrierLocation, Color.White);
                }
                else
                {
                    spriteBatch.Draw(barrierTexture, barrierLocation, Color.White);
                }
            }



            if (alive)
            {
                spriteBatch.Draw(tankTexture, location, null, Color.White, rotation, origin, 1, SpriteEffects.None, 1);

            }
            if (frozen)
            {

                spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//FrozenEfekt"), location, null, Color.Lerp(Color.Transparent, Color.White, ((_timeLeftForFrozen.Seconds + 1) / 5f)), rotation, origin, 1, SpriteEffects.None, 1);


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
                _timeLeftForBarrier -= gameTime.ElapsedGameTime;
                _timeLeftForFrozen -= gameTime.ElapsedGameTime;

                _timeLeftForVibration -= gameTime.ElapsedGameTime;

                if (_timeLeftForVibration <= TimeSpan.Zero)
                {
                    if (TankActionProvider is XInputGamepadTankActionProvider c)
                    {
                        c.Vibrate(0);
                    }
                }

                if (!frozen)
                {
                    MoveTank();
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
                                if (tile.type != Tile.BUSH)
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
            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null)
            {
                hitParticles.Update(gameTime);
            }


        }

        public virtual void MoveTank(TankControllerState? state = null)
        {
            TankControllerState controller = state ?? TankActionProvider.GetTankControllerState();
            float xMovement = controller.MoveX;
            float yMovement = controller.MoveY;

            if (Math.Abs(xMovement) < float.Epsilon && Math.Abs(yMovement) < float.Epsilon) return;

            float tan = Math.Abs(xMovement) > double.Epsilon
                ? Math.Abs(yMovement) / xMovement
                : float.PositiveInfinity; // Obliczam tangensa konta w górnej połowie układu współrzędnego

            float angle = -MathHelper.PiOver2;

            if (Math.Abs(tan) < float.Epsilon && xMovement > 0) // czołg w rzeczywistości porusza się w prawo
                angle += MathHelper.PiOver2;
            else if (Math.Abs(tan) < float.Epsilon && xMovement < 0) // czołg porusza się w rzeczywistyości w lewo
                angle -= MathHelper.PiOver2;
            else
                angle += (float)(((tan > 0 ? 1 : -1) * MathHelper.PiOver2) - Math.Atan(tan));

            if (yMovement < 0) // Kiedy czołg chce jechać w dół
                angle = (-angle);

            Rotate(angle);

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"{xMovement} x {yMovement}, {tan} = tan({angle}°");
#endif

            Move(new Vector2(Math.Abs(xMovement), Math.Abs(yMovement)), controller.SpeedBoost);
        }

        public void Move(Vector2 currentSpeed, bool isSpeeBoostUp)
        {
            this.location.X += ((float)Math.Cos(rotation) * this.speed.X * (isSpeeBoostUp ? 2 : 1)) * currentSpeed.Length();
            this.location.Y += ((float)Math.Sin(rotation) * this.speed.Y * (isSpeeBoostUp ? 2 : 1)) * currentSpeed.Length();
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
            TankControllerState controller = TankActionProvider.GetTankControllerState();
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

            float xFraction = (float)Math.Cos(rotation); // składowa pozioma aktualnego obrotu
            float yFraction = (float)Math.Sin(rotation); // składowa pionowa akutlanego obrutu

            float bulletMaxSpeed = 20;
            Vector2 bulletSpeed = new Vector2(xFraction * bulletMaxSpeed, yFraction * bulletMaxSpeed);

            float bulletShowDistance = 5; // Odległość od czołgu w jakiej ma się pojawić pocisk, zbyt mała może powodować kolizje z strzelającym
            Rectangle bulletStartPosition = new Rectangle((int)(location.X + xFraction * bulletShowDistance),
                                                           (int)(location.Y + yFraction * bulletShowDistance), 5, 5);

            var bullets = new Bullet[strong];

            for (int i = 0; i < this.strong; i++) // stwórz na raz tyle pociskow, ile mocy ma czołg.
                bullets[i] = new Bullet(game, bulletStartPosition, bulletSpeed, color, player, 0, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y, 5, 7));

            return bullets;
        }

        public bool TryPlantMine(out Mine mine)
        {
            mine = null;

            if (mines <= 0 || _timeLeftToPlantMine > TimeSpan.Zero) return false;

            TankControllerState controller = TankActionProvider.GetTankControllerState();
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
            _timeLeftForBarrier = BARRIER_TIME;
        }

        public virtual void Frozen()
        {
            _timeLeftForFrozen = FROZEN_TIME;
        }

        public virtual void Hit()
        {
               if (TankActionProvider is XInputGamepadTankActionProvider c)
            {
                c.Vibrate(0.5f);
                _timeLeftForVibration = TimeSpan.FromMilliseconds(100);
            }
            game.sound.PlaySound(Sound.Sounds.HIT);
            if (armor > 0)
                armor -= 0.25f;
            else if (armor == 0)
            {
                lives -= 0.25f;
                if (lives % 1 == 0)
                {
                    Die();
                }
            }
            hitParticles = new Particlecloud(location, game, player, whiteRectangle, Color.OrangeRed, 2, 6);
         

        }

        public virtual void Die()
        {
            game.sound.PlaySound(Sound.Sounds.EXPLOSION);
            if (alive)
            {
                if (game.gameState == game.gameRunningWyscig)
                { lives++; }

                deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.OrangeRed, 2);
                alive = false;
                location = new Vector2(-100, -100);
            }
            if (lives <= 0  )
            {
                if (TankActionProvider is XInputGamepadTankActionProvider c)
                {
                    c.Vibrate(0);
                    _timeLeftForVibration = TimeSpan.Zero;
                }
            }
            else
            {
                if (TankActionProvider is XInputGamepadTankActionProvider c)
                {
                    c.Vibrate(1f);
                    _timeLeftForVibration = TimeSpan.FromMilliseconds(500);
                }
            }
        }

        public virtual void Respawn(Vector2 _location)
        {
            if (!alive && lives > 0)
            {
                game.sound.PlaySound(Sound.Sounds.RESPAWN);
                location = _location;
                armor = 1;
                strong = 1;
                respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Green, 2);
                alive = true;
            }
        }

        public void Explode()
        {
            if (!alive) return;

            if (barrier) return;

            armor = 0;

            // czyli kiedy pozostała wartość życia jest wartością całkowitą
            if (Math.Abs(lives - (int) lives) <= float.Epsilon) 
                lives--;
            // Jeżeli pozostała ilosć życia jest większa niż część całkowita, odejmij część ułamkową
            else
                lives -= lives - (int) lives;

            Die();

        }
    }
}
