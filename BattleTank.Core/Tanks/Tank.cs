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
        public Vector2 location ;
        public Vector2 StartingLocation { get; set; }
        public bool Alive { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 SlowSpeed { get; set; }
        public Vector2 NormalSpeed { get; set; }
        public float Rotation { get; set; }
        public Texture2D TankTexture { get; set; }
        public Vector2 Origin { get; set; }
        public Game1 Game { get; set; }
        public int Player { get; set; }
        public float Lives { get; set; }

        public int Strong { get; set; }

        public int Mines { get; set; }
        public float Armor { get; set; }
        public float Scale { get; set; }
        public Rectangle TankRect { get; set; }
        public Particlecloud DeathParticles { get; set; }
        public Particlecloud RespawnParticles { get; set; }
        public Particlecloud HitParticles { get; set; }
        public ITankActionProvider TankActionProvider { get; set; }

        public const float UP = -MathHelper.PiOver2;
        public const float UP_RIGHT = -MathHelper.PiOver4;
        public const float RIGHT = 0;
        public const float DOWN_RIGHT = MathHelper.PiOver4;
        public const float DOWN = MathHelper.PiOver2;
        public const float DOWN_LEFT = MathHelper.Pi - MathHelper.PiOver4;
        public const float LEFT = MathHelper.Pi;
        public const float UP_LEFT = -(MathHelper.Pi - MathHelper.PiOver4);
        public bool Colliding { get; set; }  = false;
        public bool FallIntoMud { get; set; }  = false;
        public Texture2D WhiteRectangle { get; set; }

        public bool Enemy { get; set; } = false;
        public Texture2D BarrierTexture { get; set; }
        public Vector2 BarrierLocation { get; set; }

        private float timerBush = 0f;
        private readonly float[] pitchSoundEngine = new float[9];

        readonly int tankRectWidth;
        readonly int tankRectHeight;

        protected TimeSpan _timeEngineEffectTime = TimeSpan.Zero;

        #region Respawn
        public bool CanRespawn => !Alive && Lives > 0 && _timeLeftToRespawn <= TimeSpan.Zero;
        private readonly TimeSpan BACK_ALIVE_DELAY = TimeSpan.FromSeconds(2);
        private TimeSpan _timeLeftToRespawn = TimeSpan.Zero;
        #endregion

        #region Frozen
        public bool Frozen => _timeLeftForFrozen > TimeSpan.Zero;
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
                    TankTexture = _game.Content.Load<Texture2D>("Graphics/BlueTank");
                    break;
                case TankColors.GREEN:
                    TankTexture = _game.Content.Load<Texture2D>("Graphics/GreenTank");
                    break;
                case TankColors.PINK:
                    TankTexture = _game.Content.Load<Texture2D>("Graphics/PinkTank");
                    break;
                case TankColors.RED:
                    TankTexture = _game.Content.Load<Texture2D>("Graphics/RedTank");
                    break;
                case TankColors.YELLOW:
                    TankTexture = _game.Content.Load<Texture2D>("Graphics/YellowTank");
                    break;
            }

            BarrierTexture = _game.Content.Load<Texture2D>("Graphics/barrier");

            location = _location;

            StartingLocation = _location;
            Speed = _speed;
            NormalSpeed = Speed;
            SlowSpeed = Speed - new Vector2(1.5f, 1.5f);
            Rotation = _rotation;         
            Game = _game;
            tankRectWidth =  (((Game.Graphics.PreferredBackBufferWidth / Game.Settings.ElementsOnTheWidth) * 2) / 3 ) ;
            tankRectHeight =  (((Game.Graphics.PreferredBackBufferHeight / Game.Settings.ElementsOnTheHeight) * 2) / 3) ;

            Origin = new Vector2(this.TankTexture.Width / 2f, this.TankTexture.Height / 2f);

            Player = _player;
            Scale = _scale;
            WhiteRectangle = _whiteRectangle;
            Strong = _strong;
            Mines = _mines;
            TankActionProvider = tankActionProvider;
            Alive = true;
            Lives = 3;
            Armor = 1;
            RespawnParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.Gray, 0);
            DeathParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.Gray, 0);
            HitParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.Gray, 0);
            TankRect = new Rectangle((int)location.X - (tankRectWidth /2), (int)location.Y - (tankRectHeight /2), tankRectWidth, tankRectHeight);
            Game.SoundsTanks[Player] = new Sound(Game);
            pitchSoundEngine[Player] = (float)(Game.Randy.Next(-50, 50) )/ 100;
            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE,0.8f, pitchSoundEngine[Player], 0f);

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // || !alive w poniższym warunku jest konieczne do tego, aby Particles były widoczne po śmierci.
            if (Game.Camera.VisibleArea.Contains(location) || !Alive)
                DrawTank(spriteBatch);
            else
                DrawTankPointer(spriteBatch);
         }

        private void DrawTankPointer(SpriteBatch spriteBatch)
        {
            int squareWidth = 17;
            Texture2D rect = new Texture2D(Game.GraphicsDevice, squareWidth, squareWidth);

            DrawCricleInTexture(rect);

            Rectangle visArea = Game.Camera.VisibleArea;
            const int displayEdgeDistance = 25; 

            float x = location.X < visArea.X ? visArea.X + displayEdgeDistance : location.X > visArea.Width + visArea.X ? visArea.Width + visArea.X - displayEdgeDistance : location.X;
            float y = location.Y < visArea.Y ? visArea.Y + displayEdgeDistance : location.Y > visArea.Height + visArea.Y ? visArea.Height + visArea.Y - displayEdgeDistance : location.Y;

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
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(TankColor));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        private void DrawTank(SpriteBatch spriteBatch)
        {

            if (Barrier)
            {
                Color barrierColor = GetColor();

                BarrierLocation = new Vector2((int)location.X - (BarrierTexture.Width / 2), (int)location.Y - (BarrierTexture.Height / 2));
                if (_timeLeftForBarrier.Seconds <= 3)
                {
                    if (_timeLeftForBarrier.Milliseconds.IsWithin(500, 750) || _timeLeftForBarrier.Milliseconds.IsWithin(0, 250))
                        spriteBatch.Draw(BarrierTexture, BarrierLocation, barrierColor);
                }
                else
                {
                    spriteBatch.Draw(BarrierTexture, BarrierLocation, barrierColor);
                }
            }



            if (Alive)
            {

#pragma warning disable CS0618 // Typ lub składowa jest przestarzała
                spriteBatch.Draw(TankTexture, null, new Rectangle((int)location.X, (int)location.Y, tankRectWidth, tankRectHeight), null, Origin, Rotation, new Vector2(1, 1), Color.White, SpriteEffects.None, 0);
#pragma warning restore CS0618 // Typ lub składowa jest przestarzała

            }
            if (Frozen)
            {

                spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/FrozenEfekt"), location, null, Color.Lerp(Color.Transparent, Color.White, ((_timeLeftForFrozen.Seconds + 1) / 5f)), Rotation, Origin, 1, SpriteEffects.None, 1);
             
            }

            RespawnParticles.Draw(spriteBatch);
            DeathParticles.Draw(spriteBatch);
            if (HitParticles != null)
            {
                HitParticles.Draw(spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Alive)
            {
                _timeLeftToNextShot -= gameTime.ElapsedGameTime;
                _timeLeftToPlantMine -= gameTime.ElapsedGameTime;
                _timeLeftForBarrier -= gameTime.ElapsedGameTime;
                _timeLeftForFrozen -= gameTime.ElapsedGameTime;
                _timeEngineEffectTime -= gameTime.ElapsedGameTime;
                _timeLeftForVibration -= gameTime.ElapsedGameTime;

           
                      
                if ((_timeLeftForVibration <= TimeSpan.Zero) && (TankActionProvider is XInputGamepadTankActionProvider c))
                {
                    c.Vibrate(0);                   
                }

                if (!Frozen)
                {
                    MoveTank();
                }

                TankRect = new Rectangle((int)location.X - (tankRectWidth / 2), (int)location.Y - (tankRectHeight / 2), tankRectWidth, tankRectHeight);

                Colliding = false;

                if (!FallIntoMud)
                {
                    Speed = NormalSpeed;
                    Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 0.8f, pitchSoundEngine[Player], 0f);
                }
                foreach (Tile[] tiles in Game.Map.MapCurrent)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.Type == Tile.TileType.WALL || tile.Type == Tile.TileType.WATER || tile.Type == Tile.TileType.BUSH || tile.Type == Tile.TileType.MUD)
                        {
                            if ((tile.IsColliding(TankRect).Depth > 0))
                            {
                                float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                                timerBush -= timer;
                                if (tile.Type != Tile.TileType.BUSH && tile.Type != Tile.TileType.MUD)
                                    Colliding = true;

                                if (tile.Type == Tile.TileType.MUD)
                                    FallIntoMud = true;

                                Collision collision = tile.IsColliding(TankRect);
                                switch (collision.SideCollision)
                                {
                                    case Collision.Side.TOP:
                                        if (tile.Type == Tile.TileType.WALL || tile.Type == Tile.TileType.WATER)
                                            location.Y += collision.Depth;
                                        if (tile.Type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.Type == Tile.TileType.MUD)
                                        {
                                            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[Player] - 0.2f, 0f);
                                            Speed = SlowSpeed;
                                        }
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (tile.Type == Tile.TileType.WALL || tile.Type == Tile.TileType.WATER)
                                            location.Y -= collision.Depth;
                                        if (tile.Type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.Type == Tile.TileType.MUD)
                                        {
                                            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[Player] - 0.2f, 0f);
                                            Speed = SlowSpeed;
                                        }
                                        break;
                                    case Collision.Side.LEFT:
                                        if (tile.Type == Tile.TileType.WALL || tile.Type == Tile.TileType.WATER)
                                            location.X += collision.Depth;
                                        if (tile.Type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.Type == Tile.TileType.MUD)
                                        {
                                            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[Player] - 0.2f, 0f);
                                            Speed = SlowSpeed;
                                        }
                                        break;
                                    case Collision.Side.RIGHT:
                                        if (tile.Type == Tile.TileType.WALL || tile.Type == Tile.TileType.WATER)
                                            location.X -= collision.Depth;
                                        if (tile.Type == Tile.TileType.BUSH && timerBush <= 0)
                                        {
                                            RustlingEffectSound();
                                        }
                                        if (tile.Type == Tile.TileType.MUD)
                                        {
                                            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[Player] - 0.2f, 0f);
                                            Speed = SlowSpeed;
                                        }
                                        break;

                                }
                            }
                            FallIntoMud = false;
                        }

                     

                    }
                }
                        
                foreach (Tank tank in Game.EnemyTanks.Where(et => !(et._kamikazeMode)).Concat(new List<Tank> { Game.Tank1, Game.Tank2 }).Where(d => !(d is null)))
                {               
                    if (((!tank.Player.Equals(Player) && !Enemy) || tank.Enemy.Equals(Enemy)) && (tank.IsColliding(TankRect).Depth > 0))
                    {
                         
                            Collision collision = tank.IsColliding(TankRect);
                            switch (collision.SideCollision)
                            {
                                case Collision.Side.TOP:                             
                                        location.Y += collision.Depth;                                 
                                    break;
                                case Collision.Side.BOTTOM:
                                        location.Y -= collision.Depth;                                  
                                    break;
                                case Collision.Side.LEFT:
                                        location.X += collision.Depth;
                                    break;
                                case Collision.Side.RIGHT:
                                        location.X -= collision.Depth;
                                    break;

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
            RespawnParticles.Update(gameTime);
            DeathParticles.Update(gameTime);
            if (HitParticles != null)
            {
                HitParticles.Update(gameTime);
            }
        }

        public Collision IsColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, TankRect);
         
                if (intersect.Width > 0 || intersect.Height > 0)
                {

                    if (possibleCollisionRect.Top < TankRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > TankRect.Y)
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.TOP, depth);
                    }
                    if (possibleCollisionRect.Bottom > TankRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.BOTTOM, depth);
                    }
                    if (possibleCollisionRect.Left < TankRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > TankRect.Right)
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.LEFT, depth);
                    }
                    if (possibleCollisionRect.Right > TankRect.Right - TankRect.Width && possibleCollisionRect.Right > TankRect.Left && ( Game.Settings.Widescreen && Environment.OSVersion.Platform == PlatformID.Win32NT ) ? ( Math.Abs(intersect.Width) < Math.Abs(intersect.Height)) : false )
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.RIGHT, depth);
                    }
                }
            

            return new Collision();
        }



        private void RustlingEffectSound()
        {
            Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.RUSTLING, 0.8f, (float)(Game.Randy.Next(-60, 60)) / 100, 0f);
            Game.SoundsTanks[Player].PlaySound(Sound.Sounds.RUSTLING);
            timerBush = 0.5f;
        }

        public virtual void MoveTank(TankControllerState? state = null)
        {

            if (Player > 2 || TankActionProvider.IsConnectedTankController())
            {

                TankControllerState controller = state ?? TankActionProvider.GetTankControllerState();
                float xMovement = controller.MoveX;
                float yMovement = controller.MoveY;

                if (Math.Abs(xMovement) < float.Epsilon && Math.Abs(yMovement) < float.Epsilon)
                {
                    Game.SoundsTanks[Player].PauseSound(Sound.Sounds.ENGINE);
                    return;
                }
                else
                {
                    Game.SoundsTanks[Player].PlaySound(Sound.Sounds.ENGINE);
                }

                if (controller.SpeedBoost)
                    Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 1f, pitchSoundEngine[Player] + 0.2f, 0f);
                else
                    Game.SoundsTanks[Player].ConfigSound(Sound.Sounds.ENGINE, 0.8f, pitchSoundEngine[Player], 0f);

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
                Game.GameStateCurrent = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }


        }

        public void Move(Vector2 currentSpeed, bool isSpeeBoostUp)
        {

            this.location.X += ((float)Math.Cos(Rotation) * this.Speed.X * (isSpeeBoostUp ? 2 : 1)) * currentSpeed.Length();
            this.location.Y += ((float)Math.Sin(Rotation) * this.Speed.Y * (isSpeeBoostUp ? 2 : 1)) * currentSpeed.Length();
        }

        public void MoveLeft(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.X -= (2) + this.Speed.X;
            }

            else
            {
                this.location.X -= this.Speed.X;
            }
        }

        public void MoveRight(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.X += (2) + this.Speed.X;
            }

            else
            {
                this.location.X += this.Speed.X;
            }
        }

        public void MoveUp(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y -= (2) + this.Speed.Y;
            }

            else
            {
                this.location.Y -= this.Speed.Y;
            }
        }

        public void MoveDown(bool isBoostPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y += (2) + this.Speed.Y;
            }

            else
            {
                this.location.Y += this.Speed.Y;
            }
        }

        public void Rotate(float angle)
        {
            this.Rotation = angle;
        }

        public virtual bool TryFire(out Bullet[] bullets)
        {
            if (Player > 2 || TankActionProvider.IsConnectedTankController())
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
                Game.GameStateCurrent = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }

            bullets = new Bullet[0];
            return false;
        }

        protected Bullet[] Fire()
        {
            if (!Alive) return new Bullet[0];

            if (Frozen) return new Bullet[0];

            if (_timeLeftToNextShot > TimeSpan.Zero) return new Bullet[0];

            _timeLeftToNextShot = FIRE_DELAY;

            Game.SoundsTanks[Player].PlaySound(Sound.Sounds.SHOT);

            Color color = Color.Blue;

            if(TankColor.Equals(TankColors.PINK))
                color = Color.Pink;
            if (TankColor.Equals(TankColors.BLUE))
                color = Color.Blue;
            if (TankColor.Equals(TankColors.YELLOW))
                color = Color.Yellow;

            if (Player == 1)
                color = Color.Green;
            if (Player == 2)
                color = Color.Red;

            float xFraction = (float)Math.Cos(Rotation); // składowa pozioma aktualnego obrotu
            float yFraction = (float)Math.Sin(Rotation); // składowa pionowa akutlanego obrutu

            float bulletMaxSpeed = 20;
            Vector2 bulletSpeed = new Vector2(xFraction * bulletMaxSpeed, yFraction * bulletMaxSpeed);

            float bulletShowDistance = 5; // Odległość od czołgu w jakiej ma się pojawić pocisk, zbyt mała może powodować kolizje z strzelającym
            Rectangle bulletStartPosition = new Rectangle((int)(location.X + xFraction * bulletShowDistance),
                                                           (int)(location.Y + yFraction * bulletShowDistance), 5, 7);

            var bullets = new Bullet[Strong];

            for (int i = 0; i < this.Strong; i++) // stwórz na raz tyle pociskow, ile mocy ma czołg.
                bullets[i] = new Bullet(Game, bulletStartPosition, bulletSpeed, color, Player, 0, WhiteRectangle, Bullet.TypeOfWeapon.BULLET);

            return bullets;
        }

        public bool TryPlantMine(out Mine mine)
        {

            mine = null;

            if (Player > 2 || TankActionProvider.IsConnectedTankController())
            {

                if (Mines <= 0 || _timeLeftToPlantMine > TimeSpan.Zero) return false;

                TankControllerState controller = TankActionProvider.GetTankControllerState();
                if (!controller.PlantMine)
                    return false;

                if (!Alive) return false;

                _timeLeftToPlantMine = PLANT_MINE_DELAY;


                Rectangle minePositiono = new Rectangle(this.location.ToPoint(), new Point(20, 20));
                mine = new Mine(Game, minePositiono, Vector2.Zero, Color.Orange, Player, 0, WhiteRectangle, Bullet.TypeOfWeapon.MINE);
                Mines--;
                _timeLeftToPlantMine = PLANT_MINE_DELAY;
                return true;
            }
            else
            {
                Game.GameStateCurrent = Game1.GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED;
            }
            return false;
        }

        public virtual void StartBarrier()
        {
            _timeLeftForBarrier = BARRIER_TIME;
        }

        public virtual void StartFrozen()
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
            Game.SoundsTanks[Player].PlaySound(Sound.Sounds.HIT);
            if (Armor > 0)
                Armor -= 0.25f;
            else if (Armor == 0)
            {
                Lives -= 0.25f;
                if (Lives % 1 == 0)
                {
                    Die();
                }
            }
            HitParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.OrangeRed, 2, 6);


        }

        public virtual void Die()
        {
            Game.SoundsTanks[Player].StopSound(Sound.Sounds.ENGINE);
            Game.SoundsTanks[Player].PlaySound(Sound.Sounds.EXPLOSION);
            if (Alive)
            {
                if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_RACE)
                { Lives++; }

                DeathParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.OrangeRed, 2);
                Alive = false;
                location = new Vector2(-100, -100);
                _timeLeftToRespawn = BACK_ALIVE_DELAY;
            }
            if (Lives <= 0)
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
        
           location = Game.Map.FindNonColidingPosition(TankRect.Width, TankRect.Height);
            Game.SoundsTanks[Player].PlaySound(Sound.Sounds.RESPAWN);
            _timeEngineEffectTime = TimeSpan.Zero;
            Armor = 1;
            Strong = 1;
            RespawnParticles = new Particlecloud(location, Game, Player, WhiteRectangle, Color.Green, 2);
            Alive = true;
        }

        public void Explode()
        {
            if (!Alive) return;

            if (Barrier) return;

            Armor = 0;

            // czyli kiedy pozostała wartość życia jest wartością całkowitą
            if (Math.Abs(Lives - (int)Lives) <= float.Epsilon)
                Lives--;
            // Jeżeli pozostała ilosć życia jest większa niż część całkowita, odejmij część ułamkową
            else
                Lives -= Lives - (int)Lives;

            Die();

        }
    }
}
