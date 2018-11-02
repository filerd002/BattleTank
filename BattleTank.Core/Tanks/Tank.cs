using System;
using System.Collections.Generic;
using System.Linq;
using BattleTank.Core.Extensions;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.Tanks
{
    public class Tank
    {
        public enum TankColors
        {
            BLUE,
            GREEN,
            PINK,
            RED,
            YELLOW
        }
        public TankColors TankColor { get; set; }
        //data members
        public Vector2 location;
        public Vector2 startingLocation;
        public Vector2 speed;
        public Vector2 slowSpeed;
        public Vector2 normalSpeed;
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
        public bool fallIntoMud = false;
        public Texture2D whiteRectangle;

        public bool enemy = false;
        public Texture2D barrierTexture;
        public Vector2 barrierLocation;

        private float timerBush = 0f;
        private float[] pitchSoundEngine = new float[9];

        int tankRectWidth;
        int tankRectHeight;

        protected TimeSpan _timeEngineEffectTime = TimeSpan.Zero;

        #region Respawn
        public bool CanRespawn => !alive && lives > 0 && _timeLeftToRespawn <= TimeSpan.Zero;
        private readonly TimeSpan BACK_ALIVE_DELAY = TimeSpan.FromSeconds(2);
        private TimeSpan _timeLeftToRespawn = TimeSpan.Zero;
        #endregion

        #region Frozen
        public bool frozen => _timeLeftForFrozen > TimeSpan.Zero;
        /// <summary>
        /// Czas po jakim czolg zostanie rozmrozony
        /// </summary>
        public readonly TimeSpan FROZEN_TIME = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Czas jaki został do rozmrozenia
        /// </summary>
        protected TimeSpan _timeLeftForFrozen = TimeSpan.Zero;
        #endregion

        #region Barrier
        public bool Barrier => _timeLeftForBarrier > TimeSpan.Zero;
        /// <summary>
        /// Czas po jakim osłona zostanie zdjęcia
        /// </summary>
        public readonly TimeSpan BARRIER_TIME = TimeSpan.FromSeconds(10);
        /// <summary>
        /// Czas jaki został do zniknięcie osłony
        /// </summary>
        protected TimeSpan _timeLeftForBarrier = TimeSpan.Zero;
        #endregion

        #region Fire
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
        #endregion

        #region PlantMine
        public readonly TimeSpan PLANT_MINE_DELAY = TimeSpan.FromMilliseconds(2000);
        /// <summary>
        /// Okresla ile zcasu zostało do następnego strzału
        /// </summary>
        private TimeSpan _timeLeftToPlantMine = TimeSpan.Zero;
        #endregion

        private TimeSpan _timeLeftForVibration = TimeSpan.Zero;

        //overloaded constructor(s)
        public Tank(Game1 _game, TankColors tankColor, Vector2 _location, Vector2 _speed,
                    float _rotation, int _player, float _scale, Texture2D _whiteRectangle,
                    int _strong, int _mines, bool _barrier, bool _frozen, ITankActionProvider tankActionProvider)
        {
            TankColor = tankColor;

            switch (TankColor)
            {
                case TankColors.BLUE:
                    tankTexture = _game.Content.Load<Texture2D>("Graphics/BlueTank");
                    break;
                case TankColors.GREEN:
                    tankTexture = _game.Content.Load<Texture2D>("Graphics/GreenTank");
                    break;
                case TankColors.PINK:
                    tankTexture = _game.Content.Load<Texture2D>("Graphics/PinkTank");
                    break;
                case TankColors.RED:
                    tankTexture = _game.Content.Load<Texture2D>("Graphics/RedTank");
                    break;
                case TankColors.YELLOW:
                    tankTexture = _game.Content.Load<Texture2D>("Graphics/YellowTank");
                    break;
            }

            barrierTexture = _game.Content.Load<Texture2D>("Graphics/barrier");

            location = _location;

            startingLocation = _location;
            speed = _speed;
            normalSpeed = speed;
            slowSpeed = speed - new Vector2(1.5f, 1.5f);
            rotation = _rotation;         
            game = _game;
            tankRectWidth =  (((game.graphics.PreferredBackBufferWidth / game.settings.elementsOnTheWidth) * 2) / 3 ) ;
            tankRectHeight =  (((game.graphics.PreferredBackBufferHeight / game.settings.elementsOnTheHeight) * 2) / 3) ;

            // origin = new Vector2(tankRectWidth / 2f, tankRectHeight / 2f);
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);

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
            tankRect = new Rectangle((int)location.X - (tankRectWidth /2), (int)location.Y - (tankRectHeight /2), tankRectWidth, tankRectHeight);
            //tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
            game.soundsTanks[player] = new Sound(game);
            pitchSoundEngine[player] = (float)(game.randy.Next(-50, 50) )/ 100;
            game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE,0.8f, pitchSoundEngine[player], 0f);

            if (_barrier) StartBarrier();
            if (_frozen) Frozen();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // || !alive w poniższym warunku jest konieczne do tego, aby Particles były widoczne po śmierci.
            if (game.Camera.VisibleArea.Contains(location) || !alive)
                DrawTank(spriteBatch);
            else
                DrawTankPointer(spriteBatch);
         }

        private void DrawTankPointer(SpriteBatch spriteBatch)
        {
            int squareWidth = 17;
            Texture2D rect = new Texture2D(game.GraphicsDevice, squareWidth, squareWidth);

            DrawCricleInTexture(rect);

            Rectangle visArea = game.Camera.VisibleArea;
            const int displayEdgeDistance = 25; 

            float x = location.X < visArea.X ? visArea.X + displayEdgeDistance
                    : location.X > visArea.Width + visArea.X ? visArea.Width + visArea.X - displayEdgeDistance : location.X;
            float y = location.Y < visArea.Y ? visArea.Y + displayEdgeDistance
                : location.Y > visArea.Height + visArea.Y ? visArea.Height + visArea.Y - displayEdgeDistance : location.Y;

            spriteBatch.Draw(rect, new Vector2(x, y), Color.White);
        }

        void DrawCricleInTexture(Texture2D rect)
        {
            int squareWidth = rect.Width;
            Color[] data = new Color[rect.Width * rect.Height];
            Color color = GetColor();

            for (int i = 0; i < squareWidth; ++i)
            {
                for (int j = 0; j < squareWidth; ++j)
                {
                    int halfOfSquareWidth = squareWidth / 2;
                    int squareRoot = (i - halfOfSquareWidth) * (i - halfOfSquareWidth)
                                     + (j - halfOfSquareWidth) * (j - halfOfSquareWidth);

                    double root = Math.Sqrt(squareRoot);
                    if (root > halfOfSquareWidth)
                    {
                        continue;
                    }
                    data[i * squareWidth + j] = color;
                }
            }
            rect.SetData(data);
        }

        private Color GetColor()
        {
            switch (TankColor)
            {
                case TankColors.BLUE:
                    return Color.Blue;
                case TankColors.GREEN:
                    return Color.Green;
                case TankColors.PINK:
                    return Color.DeepPink;
                case TankColors.RED:
                    return Color.Red;
                case TankColors.YELLOW:
                    return Color.Yellow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(TankColor));
            }
        }

        private void DrawTank(SpriteBatch spriteBatch)
        {

            if (Barrier)
            {
                Color barrierColor = GetColor();

                barrierLocation = new Vector2((int)location.X - (barrierTexture.Width / 2), (int)location.Y - (barrierTexture.Height / 2));
                if (_timeLeftForBarrier.Seconds <= 3)
                {
                    if (_timeLeftForBarrier.Milliseconds.IsWithin(500, 750) || _timeLeftForBarrier.Milliseconds.IsWithin(0, 250))
                        spriteBatch.Draw(barrierTexture, barrierLocation, barrierColor);
                }
                else
                {
                    spriteBatch.Draw(barrierTexture, barrierLocation, barrierColor);
                }
            }



            if (alive)
            {

                spriteBatch.Draw(tankTexture, null, new Rectangle((int)location.X, (int)location.Y, tankRectWidth, tankRectHeight), null, origin, rotation, new Vector2(1, 1), Color.White, SpriteEffects.None, 0);

            }
            if (frozen)
            {

                spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics/FrozenEfekt"), location, null, Color.Lerp(Color.Transparent, Color.White, ((_timeLeftForFrozen.Seconds + 1) / 5f)), rotation, origin, 1, SpriteEffects.None, 1);
             
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
                _timeEngineEffectTime -= gameTime.ElapsedGameTime;
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

                tankRect = new Rectangle((int)location.X - (tankRectWidth / 2), (int)location.Y - (tankRectHeight / 2), tankRectWidth, tankRectHeight);

                colliding = false;

                if (!fallIntoMud)
                {
                    speed = normalSpeed;
                    game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 0.8f, pitchSoundEngine[player], 0f);
                }
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.type == Tile.TileType.WALL || tile.type == Tile.TileType.WATER || tile.type == Tile.TileType.BUSH || tile.type == Tile.TileType.MUD)
                        {
                            if ((tile.isColliding(tankRect).depth > 0))
                            {
                                float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                                timerBush -= timer;
                                if (tile.type != Tile.TileType.BUSH && tile.type != Tile.TileType.MUD)
                                    colliding = true;

                                if (tile.type == Tile.TileType.MUD)
                                    fallIntoMud = true;

                                Collision collision = tile.isColliding(tankRect);
                                switch (collision.side)
                                {
                                    case Collision.Side.TOP:
                                        if (tile.type == Tile.TileType.WALL || tile.type == Tile.TileType.WATER)
                                            location.Y += collision.depth;
                                        if (tile.type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.type == Tile.TileType.MUD)
                                        {
                                            game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[player] - 0.2f, 0f);
                                            speed = slowSpeed;
                                        }
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (tile.type == Tile.TileType.WALL || tile.type == Tile.TileType.WATER)
                                            location.Y -= collision.depth;
                                        if (tile.type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.type == Tile.TileType.MUD)
                                        {
                                            game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[player] - 0.2f, 0f);
                                            speed = slowSpeed;
                                        }
                                        break;
                                    case Collision.Side.LEFT:
                                        if (tile.type == Tile.TileType.WALL || tile.type == Tile.TileType.WATER)
                                            location.X += collision.depth;
                                        if (tile.type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.type == Tile.TileType.MUD)
                                        {
                                            game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[player] - 0.2f, 0f);
                                            speed = slowSpeed;
                                        }
                                        break;
                                    case Collision.Side.RIGHT:
                                        if (tile.type == Tile.TileType.WALL || tile.type == Tile.TileType.WATER)
                                            location.X -= collision.depth;
                                        if (tile.type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.type == Tile.TileType.MUD)
                                        {
                                            game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[player] - 0.2f, 0f);
                                            speed = slowSpeed;
                                        }
                                        break;

                                }
                            }
                            fallIntoMud = false;
                        }

                        else
                        {
                            continue;
                        }

                    }
                }
                        
                foreach (Tank tank in game.enemyTanks.Where(c => !(c._kamikazeMode)).Concat(new List<Tank> { game.tank1, game.tank2 }).Where(d => !(d is null)))
                {               
                    if (!tank.player.Equals(player) && !enemy || tank.enemy.Equals(enemy))
                    {
                        if ((tank.isColliding(tankRect).depth > 0))
                        {
                         
                            Collision collision = tank.isColliding(tankRect);
                            switch (collision.side)
                            {
                                case Collision.Side.TOP:                             
                                        location.Y += collision.depth;                                 
                                    break;
                                case Collision.Side.BOTTOM:
                                        location.Y -= collision.depth;                                  
                                    break;
                                case Collision.Side.LEFT:
                                        location.X += collision.depth;
                                    break;
                                case Collision.Side.RIGHT:
                                        location.X -= collision.depth;
                                    break;

                            }
                        }
                    }
                }
            }
            else
            {
                _timeLeftToRespawn -= gameTime.ElapsedGameTime;
                if (CanRespawn)
                {
                    Respawn();
                }
            }
            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null)
            {
                hitParticles.Update(gameTime);
            }
        }

        public Collision isColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, tankRect);
         
                if (intersect.Width > 0 || intersect.Height > 0)
                {

                    if (possibleCollisionRect.Top < tankRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > tankRect.Y)
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.TOP, depth);
                    }
                    if (possibleCollisionRect.Bottom > tankRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.BOTTOM, depth);
                    }
                    if (possibleCollisionRect.Left < tankRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > tankRect.Right)
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.LEFT, depth);
                    }
                    if (possibleCollisionRect.Right > tankRect.Right - tankRect.Width && possibleCollisionRect.Right > tankRect.Left && Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.RIGHT, depth);
                    }
                }
            

            return new Collision();
        }



        private void RustlingEffectSound()
        {
            game.soundsTanks[player].ConfigSound(Sound.Sounds.RUSTLING, 0.8f, (float)(game.randy.Next(-60, 60)) / 100, 0f);
            game.soundsTanks[player].PlaySound(Sound.Sounds.RUSTLING);
            timerBush = 0.5f;
        }

        public virtual void MoveTank(TankControllerState? state = null)
        {

            if (player > 2 || TankActionProvider.IsConnectedTankController())
            {

                TankControllerState controller = state ?? TankActionProvider.GetTankControllerState();
                float xMovement = controller.MoveX;
                float yMovement = controller.MoveY;

                if (Math.Abs(xMovement) < float.Epsilon && Math.Abs(yMovement) < float.Epsilon)
                {
                    game.soundsTanks[player].PauseSound(Sound.Sounds.ENGINE);
                    return;
                }
                else
                {
                    game.soundsTanks[player].PlaySound(Sound.Sounds.ENGINE);
                }

                if (controller.SpeedBoost)
                    game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[player] + 0.2f, 0f);
                else
                    game.soundsTanks[player].ConfigSound(Sound.Sounds.ENGINE, 0.8f, pitchSoundEngine[player], 0f);

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
            else
            {
                game.gameState = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }


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

        public virtual bool TryFire(out Bullet[] bullets)
        {
            if (player > 2 || TankActionProvider.IsConnectedTankController())
            {

                TankControllerState controller = TankActionProvider.GetTankControllerState();
                if (controller.Fire)
                {
                    bullets = Fire();
                    return true;
                }
            }
            else
            {
                game.gameState = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }

            bullets = new Bullet[0];
            return false;
        }

        protected Bullet[] Fire()
        {
            if (!alive) return new Bullet[0];

            if (frozen) return new Bullet[0];

            if (_timeLeftToNextShot > TimeSpan.Zero) return new Bullet[0];

            _timeLeftToNextShot = FIRE_DELAY;

            game.soundsTanks[player].PlaySound(Sound.Sounds.SHOT);

            Color color = Color.Blue;

            if(TankColor.Equals(TankColors.PINK))
                color = Color.Pink;
            if (TankColor.Equals(TankColors.BLUE))
                color = Color.Blue;
            if (TankColor.Equals(TankColors.YELLOW))
                color = Color.Yellow;

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
                                                           (int)(location.Y + yFraction * bulletShowDistance), 5, 7);

            var bullets = new Bullet[strong];

            for (int i = 0; i < this.strong; i++) // stwórz na raz tyle pociskow, ile mocy ma czołg.
                bullets[i] = new Bullet(game, bulletStartPosition, bulletSpeed, color, player, 0, whiteRectangle, Bullet.TypeOfWeapon.BULLET);

            return bullets;
        }

        public bool TryPlantMine(out Mine mine)
        {

            mine = null;

            if (player > 2 || TankActionProvider.IsConnectedTankController())
            {

                if (mines <= 0 || _timeLeftToPlantMine > TimeSpan.Zero) return false;

                TankControllerState controller = TankActionProvider.GetTankControllerState();
                if (!controller.PlantMine)
                    return false;

                if (!alive) return false;

                _timeLeftToPlantMine = PLANT_MINE_DELAY;

                Color color = Color.Orange;

                Rectangle minePositiono = new Rectangle(this.location.ToPoint(), new Point(20, 20));
                mine = new Mine(game, minePositiono, Vector2.Zero, Color.Orange, player, 0, whiteRectangle, Bullet.TypeOfWeapon.MINE);
                mines--;
                _timeLeftToPlantMine = PLANT_MINE_DELAY;
                return true;
            }
            else
            {
                game.gameState = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }
            return false;
        }

        public virtual void StartBarrier()
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
            game.soundsTanks[player].PlaySound(Sound.Sounds.HIT);
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
            game.soundsTanks[player].StopSound(Sound.Sounds.ENGINE);
            game.soundsTanks[player].PlaySound(Sound.Sounds.EXPLOSION);
            if (alive)
            {
                if (game.gameState == Game1.GameState.GAME_RUNNING_RACE)
                { lives++; }

                deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.OrangeRed, 2);
                alive = false;
                location = new Vector2(-100, -100);
                _timeLeftToRespawn = BACK_ALIVE_DELAY;
            }
            if (lives <= 0)
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

        private void Respawn()
        {
        
           location = game.map.FindNonColidingPosition(tankRect.Width, tankRect.Height);
            game.soundsTanks[player].PlaySound(Sound.Sounds.RESPAWN);
            _timeEngineEffectTime = TimeSpan.Zero;
            armor = 1;
            strong = 1;
            respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Green, 2);
            alive = true;
        }

        public void Explode()
        {
            if (!alive) return;

            if (Barrier) return;

            armor = 0;

            // czyli kiedy pozostała wartość życia jest wartością całkowitą
            if (Math.Abs(lives - (int)lives) <= float.Epsilon)
                lives--;
            // Jeżeli pozostała ilosć życia jest większa niż część całkowita, odejmij część ułamkową
            else
                lives -= lives - (int)lives;

            Die();

        }
    }
}
