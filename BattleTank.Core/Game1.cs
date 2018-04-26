using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleTank.Core.GUI;
using BattleTank.Core.Input;
using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static BattleTank.Core.Tanks.Tank;

namespace BattleTank.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Map map;
        public bool WallInside = false;
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D whiteRectangle;
        public Tank tank1;
        public Tank tank2;
        public List<AI_Tank> enemyTanks = new List<AI_Tank>();
        List<Bullet> bullets = new List<Bullet>();
        public List<Mine> mines = new List<Mine>();
        public Score scoreManager;
        Rectangle debugRect;
        Rectangle tank2DebugRect;

    
        int timer1control = 0;
  
        Texture2D background;
        Texture2D menuTexture;
        Texture2D menuWinAndLossTexture;
        Texture2D BattleTankTexture;
        Texture2D wyborTrybGryTexture;
        Texture2D przerwaTexture;
        Texture2D winTexture;
        Texture2D lossTexture;
        Texture2D cursorTexture;
        Button ButtonPlayer1;
        Button ButtonPlayer2;
        Button ButtonPlayer3;
        Button ButtonPlayer4;
        Button ButtonNowaGra;
        Button ButtonPowrot;
        Button ButtonKoniec;
        Button ButtonZagraj;
        Button ButtonSettings;
        Texture2D SettingsTrybSterowania;
        Button ButtonSettingsTrybSterowaniaKlawMysz;
        Button ButtonSettingsTrybSterowaniaPad;
        Button ButtonSettingsTrybSterowaniaKlawMysz2;
        Button ButtonSettingsTrybSterowaniaPad2;
        Texture2D wyborPoziomTrud;
        Button Poziom1Trud;
        Button Poziom2Trud;
        Button Poziom3Trud;
        Button Poziom4Trud;
        Texture2D wyborCpuKlasyk;
        Button wyborCpuKlasykIlosc0;
        Button wyborCpuKlasykIlosc1;
        Button wyborCpuKlasykIlosc2;
        Button wyborCpuKlasykIlosc3;
        Texture2D wyborCpuKlamikaze;
        Button wyborCpuKlamikazeIlosc0;
        Button wyborCpuKlamikazeIlosc1;
        Button wyborCpuKlamikazeIlosc2;
        Button wyborCpuKlamikazeIlosc3;
        Button Czas1Gry;
        Button Czas2Gry;
        Button Czas3Gry;
        Texture2D wyborCzasGry;
        Texture2D SukcesPorazka1Gracza;
        Texture2D SukcesPorazka2Gracza;
        Button doBoju;
        Vector2 positionMouse;

        public Sound menuSound;
        public Sound sound;

        public PowerUp RandomPowerUp;
        string PowerUpSpriteName;
        public float timerPowerUp = 10f;
      
        Random randy = new Random();

        bool keysStatus = false;
        bool LeftButtonStatus = false;


        PowerUp.PowerUpType typePowerUp;
       
 

        int soundOnOff = 0;

        public int poziomTrudnosci = 2;
        public int iloscCPUKlasyk = 1;
        public int iloscCPUKamikaze = 1;
        public float czasWyscigu = 300f;   
        public enum GameState
        {
            START_GAME,
            SETTINGS,
            CHOICE_OF_GAME_TYPE,
            CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU,
            CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG,
            PAUSE,
            GAME_RUNNING_PLAYER_1,
            GAME_RUNNING_PLAYERS_2,
            GAME_RUNNING_PLAYERS_2_AND_CPU,
            GAME_RUNNING_RACE,
            GAME_WIN,
            GAME_LOSS

        }
      
        public GameState gameState;
        public GameState gameReturn;

        SoundEffectInstance soundEffectInstance = null;

        public ITankActionProvider PlayerOneController { get; set; } = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;

        public ITankActionProvider PlayerTwoController { get; set; } = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;

        public List<ITankActionProvider> AvailableGamepads { get; set; } = new List<ITankActionProvider>();
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = false;
            gameState = GameState.START_GAME;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            // UNCOMMENT NEXT THREE COMMENTS FOR FULLSCREEN
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % 48; //Makes the window size a divisor of 48 so the tiles fit more cleanly.
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % 48;
            //  graphics.PreferredBackBufferWidth = 48 * 20;
            // graphics.PreferredBackBufferHeight = 48 * 16;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0);
            whiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Graphics/Background");
            menuTexture = Content.Load<Texture2D>("Graphics/Ramka");

            menuWinAndLossTexture = Content.Load<Texture2D>("Graphics/MenuWinAndLoss");
            BattleTankTexture = Content.Load<Texture2D>("Graphics/battleTank");
            wyborTrybGryTexture = Content.Load<Texture2D>("Graphics/wyborTrybGry");
            winTexture = Content.Load<Texture2D>("Graphics/sukces");
            lossTexture = Content.Load<Texture2D>("Graphics/przegrana");
            przerwaTexture = Content.Load<Texture2D>("Graphics/przerwa");

            cursorTexture = Content.Load<Texture2D>("Graphics/cursor");

            scoreManager = new Score(this, 10);
            debugRect = new Rectangle();
            tank2DebugRect = new Rectangle();
            sound = new Sound(this);

            // Zainicjalizuj odłgos kliknięcia
            Button.ClickSound = Content.Load<SoundEffect>("Sounds\\klik");
            Button.Effect = Content.Load<Effect>(@"Shaders\GreyscaleEffect");

            menuSound = new Sound(this);
            soundEffectInstance = menuSound.deploySound(Sound.Sounds.MENU_SOUND).CreateInstance();
            soundOnOff = 0;
            AvailableGamepads = GamePads.GetAllAvailableGamepads();
            base.Initialize();
            positionMouse = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                                    graphics.GraphicsDevice.Viewport.Height / 2);

            ButtonKoniec = new Button("KoNiEc", new Vector2(), null, 60);
            
            ButtonZagraj = new Button("ZaGrAj", new Vector2(0, (map.screenHeight / 2) - 40), null, 60);
            ButtonZagraj.CenterHorizontal();

            ButtonSettings = new Button("UsTaWiEnIa", new Vector2(0, (map.screenHeight / 2) + 20), null, 60);
            ButtonSettings.CenterHorizontal();

            ButtonPlayer1 = new Button("GrAcZ vS cPu", new Vector2((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 60), null, 50);
            ButtonPlayer1.CenterHorizontal();
            ButtonPlayer2 = new Button("GrAcZ vS gRaCz", new Vector2((map.screenWidth / 2) - 135, (map.screenHeight / 2) - 10), null, 50);
            ButtonPlayer2.CenterHorizontal();
            ButtonPlayer3 = new Button("2 GrAcZy Vs CpU", new Vector2((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 40), null, 50);
            ButtonPlayer3.CenterHorizontal();
            ButtonPlayer4 = new Button("WyScIg Z cZaSeM", new Vector2((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 90), null, 50);
            ButtonPlayer4.CenterHorizontal();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics/wyborPoziomTrud");
            wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics/wyborCpuKlasyk");
            wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics/wyborCpuKlamikaze");
            wyborCzasGry = this.Content.Load<Texture2D>("Graphics/wyborCzasGry");
            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka1Gracza");
            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka2Gracza");
            SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics/trybSterowania");

            UIElement.ActiveFont = Content.Load<SpriteFont>("Fonts/ActiveFont");
            UIElement.InActiveFont = Content.Load<SpriteFont>("Fonts/Inactivefont");
            UIElement.ActiveFont.Spacing = -40;
            UIElement.InActiveFont.Spacing = -40;

            UIElement.GraphicsDevice = GraphicsDevice;

             
            ButtonPowrot = new Button(
                Content.Load<Texture2D>("Graphics/powrot"),
                Content.Load<Texture2D>("Graphics/powrot1"),
                new Rectangle());

            ButtonNowaGra = new Button(
                Content.Load<Texture2D>("Graphics/nowagra"),
                Content.Load<Texture2D>("Graphics/nowagra1"),
                new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50));

            ButtonSettingsTrybSterowaniaKlawMysz = new Button(
                Content.Load<Texture2D>("Graphics/trybSterowaniaKlawMysz"),
                Content.Load<Texture2D>("Graphics/trybSterowaniaKlawMysz1"),
                new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) - 60, 250, 50));
            ButtonSettingsTrybSterowaniaPad = new Button(
                Content.Load<Texture2D>("Graphics/trybSterowaniaPad"),
                Content.Load<Texture2D>("Graphics/trybSterowaniaPad1"),
                new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 60, 250, 50));

            ButtonSettingsTrybSterowaniaKlawMysz2 = new Button(
                Content.Load<Texture2D>("Graphics/trybSterowaniaKlawMysz"),
                Content.Load<Texture2D>("Graphics/trybSterowaniaKlawMysz1"),
                new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) + 67, 250, 50));
            ButtonSettingsTrybSterowaniaPad2 = new Button(
                Content.Load<Texture2D>("Graphics/trybSterowaniaPad"),
                Content.Load<Texture2D>("Graphics/trybSterowaniaPad1"),
                new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 67, 250, 50));

            Poziom1Trud = new Button(
                Content.Load<Texture2D>("Graphics/Poziom1Trud"),
                Content.Load<Texture2D>("Graphics/Poziom1Trud1"),
                new Rectangle((map.screenWidth / 2) - 400, (map.screenHeight / 2) - 155, 250, 50));
            Poziom2Trud = new Button(
                Content.Load<Texture2D>("Graphics/Poziom2Trud"),
                Content.Load<Texture2D>("Graphics/Poziom2Trud1"),
                new Rectangle((map.screenWidth / 2) - 210, (map.screenHeight / 2) - 155, 250, 50));
            Poziom3Trud = new Button(
                Content.Load<Texture2D>("Graphics/Poziom3Trud"),
                Content.Load<Texture2D>("Graphics/Poziom3Trud1"),
                new Rectangle((map.screenWidth / 2) - 15, (map.screenHeight / 2) - 155, 250, 50));
            Poziom4Trud = new Button(
                Content.Load<Texture2D>("Graphics/Poziom4Trud"),
                Content.Load<Texture2D>("Graphics/Poziom4Trud1"),
                new Rectangle((map.screenWidth / 2) + 155, (map.screenHeight / 2) - 155, 250, 50));

            wyborCpuKlasykIlosc0 =  new Button(
                Content.Load<Texture2D>("Graphics/0"), 
                Content.Load<Texture2D>("Graphics/01"), 
                new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) - 30, 60, 50));
            wyborCpuKlasykIlosc1 =  new Button(
                Content.Load<Texture2D>("Graphics/1"), 
                Content.Load<Texture2D>("Graphics/11"), 
                new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) - 30, 60, 50));
            wyborCpuKlasykIlosc2 =  new Button(
                Content.Load<Texture2D>("Graphics/2"), 
                Content.Load<Texture2D>("Graphics/21"), 
                new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 30, 60, 50));
            wyborCpuKlasykIlosc3 =  new Button(
                Content.Load<Texture2D>("Graphics/3"), 
                Content.Load<Texture2D>("Graphics/31"), 
                new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) - 30, 60, 50));

            wyborCpuKlamikazeIlosc0 = new Button(
                Content.Load<Texture2D>("Graphics/0"), 
                Content.Load<Texture2D>("Graphics/01"), 
                new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) + 90, 60, 50));
            wyborCpuKlamikazeIlosc1 = new Button(
                Content.Load<Texture2D>("Graphics/1"), 
                Content.Load<Texture2D>("Graphics/11"), 
                new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) + 90, 60, 50));
            wyborCpuKlamikazeIlosc2 = new Button(
                Content.Load<Texture2D>("Graphics/2"), 
                Content.Load<Texture2D>("Graphics/21"), 
                new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 90, 60, 50));
            wyborCpuKlamikazeIlosc3 = new Button(
                Content.Load<Texture2D>("Graphics/3"), 
                Content.Load<Texture2D>("Graphics/31"), 
                new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) + 90, 60, 50));

            Czas1Gry = new Button(
                Content.Load<Texture2D>("Graphics/Czas1Gry"), 
                Content.Load<Texture2D>("Graphics/Czas1Gry1"),
                new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 90, 250, 50));
            Czas2Gry = new Button(
                Content.Load<Texture2D>("Graphics/Czas2Gry"),
                Content.Load<Texture2D>("Graphics/Czas2Gry1"),
                new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 35, 250, 50));
            Czas3Gry = new Button(
                Content.Load<Texture2D>("Graphics/Czas3Gry"),
                Content.Load<Texture2D>("Graphics/Czas3Gry1"),
                new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 20, 250, 50));
            
            doBoju = new Button(Content.Load<Texture2D>("Graphics/doBoju"),
                Content.Load<Texture2D>("Graphics/doBoju1"),
                new Rectangle());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            PointerState state =  PointerState.GetState();
            
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {

                keysStatus = false;
            }

            if (state.MainAction == ButtonState.Released)
            {
                LeftButtonStatus = false;
            }



            if (gameState == GameState.START_GAME || gameState == GameState.CHOICE_OF_GAME_TYPE || gameState == GameState.SETTINGS)
                WallInside = false;
            else
            {
                WallInside = true;



            }
            


          


            if (RandomPowerUp != null)
            {
                RandomPowerUp.Update(gameTime);
            }
            float timer1 = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            timerPowerUp -= timer1;
            if (timerPowerUp < 0)
            {

                if (timer1control == 0)
                {


                    Vector2 positionPowerUp = new Vector2(randy.Next(50, graphics.PreferredBackBufferWidth - 50), randy.Next(50, graphics.PreferredBackBufferHeight - 50));

                    typePowerUp = (PowerUp.PowerUpType)randy.Next(Enum.GetNames(typeof(PowerUp.PowerUpType)).Length);
                    switch (typePowerUp)
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

                    RandomPowerUp = new PowerUp(this, typePowerUp, positionPowerUp, PowerUpSpriteName, whiteRectangle);


                    timer1control = 1;
                }
                else if (timer1control == 1)
                {
                    RandomPowerUp.controlSound = 1;
                    RandomPowerUp = null;

                    timer1control = 0;
                }

                timerPowerUp = 10;
            }





            if (soundOnOff == 1)
            {
                soundEffectInstance.Stop();
            }

            else if (soundOnOff == 0)
            {
                soundEffectInstance.Play();

            }
            foreach (Tank tankInGame in enemyTanks.Concat(new List<Tank> { tank1, tank2 }).Where(d => !(d is null)))
            {
                if ((tankInGame.location.X) < 0)
                {
                    tankInGame.location.X = map.screenWidth;
                }
                if ((tankInGame.location.X) > map.screenWidth)
                {
                    tankInGame.location.X = 0;
                }
                if ((tankInGame.location.Y) < 0)
                {
                    tankInGame.location.Y = map.screenHeight;
                }
                if ((tankInGame.location.Y) > map.screenHeight)
                {
                    tankInGame.location.Y = 0;
                }

            }

            if (gameState == GameState.GAME_RUNNING_PLAYER_1 || gameState == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {
                if (gameState == GameState.GAME_RUNNING_PLAYER_1 && tank1.lives <= 0)
                {
                    gameState = GameState.GAME_LOSS;
                }
                else if (gameState == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU && tank1.lives <= 0 && tank2.lives <= 0)
                {
                    gameState = GameState.GAME_LOSS;
                }
                else if (enemyTanks.All((tank) => !tank.alive && tank.lives <= 0))
                {
                    gameState = GameState.GAME_WIN;
                }
            }

            if (gameState == GameState.GAME_RUNNING_PLAYERS_2)
            {
                if (tank1.lives == 0)
                {
                    gameState = GameState.GAME_WIN;
                }
                if (tank2.lives == 0)
                {
                    gameState = GameState.GAME_WIN;
                }
            }


            if (gameState == GameState.GAME_RUNNING_RACE)
            {
                float timerWyscig = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                czasWyscigu -= timerWyscig;
                if (czasWyscigu < 0)
                {
                    if (scoreManager.getScore(0) > scoreManager.getScore(1))
                    {
                        tank2.lives = 0;

                    }
                    if (scoreManager.getScore(0) < scoreManager.getScore(1))
                    {
                        tank1.lives = 0;
                    }
                    gameState = GameState.GAME_WIN;
                }
            }






            if (gameState == GameState.PAUSE || gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
            {


                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (gameState == GameState.PAUSE)
                {

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 145, 320, 80)))
                    {

                        przerwaTexture = this.Content.Load<Texture2D>("Graphics/przerwa1");

                    }
                    else
                    {
                        przerwaTexture = this.Content.Load<Texture2D>("Graphics/przerwa");
                    }


                    ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50);
                    if (ButtonPowrot.IsClicked(ref state))
                    {
                        soundOnOff = 1;
                        gameState = gameReturn;
                    }
                }

                else if (gameState == GameState.GAME_WIN)
                {

                    soundOnOff = 0;

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 250, 50)))
                    {

                        winTexture = this.Content.Load<Texture2D>("Graphics/sukces1");

                    }
                    else
                    {
                        winTexture = this.Content.Load<Texture2D>("Graphics/sukces");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70)))
                    {

                        if (tank2.lives == 0)
                            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka1Gracza1");
                        if (tank1.lives == 0)
                            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka2Gracza1");

                    }
                    else
                    {

                        if (tank2.lives == 0)
                            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka1Gracza");
                        if (tank1.lives == 0)
                            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka2Gracza");



                    }


                }
                else if (gameState == GameState.GAME_LOSS)
                {
                    soundOnOff = 0;

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 250, 50)))
                    {

                        lossTexture = this.Content.Load<Texture2D>("Graphics/przegrana1");

                    }
                    else
                    {
                        lossTexture = this.Content.Load<Texture2D>("Graphics/przegrana");
                    }
                }

                if (ButtonNowaGra.IsClicked(ref state))
                {
                    WallInside = false;
                    map.Reset();
                    LeftButtonStatus = true;
                    timerPowerUp = 10f;
                    timer1control = 0;
                    czasWyscigu = 300f;
                    if (LeftButtonStatus)
                    {
                        soundEffectInstance.Stop();
                        enemyTanks.Clear();
                        mines.Clear();
                        tank1.lives = 0;
                        tank2.lives = 0;
                        Initialize();

                    }
                }

                ButtonKoniec.Position = new Vector2((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80);
                if (ButtonKoniec.IsClicked(ref state))
                {
                    Exit();
                }

            }

            else if (gameState == GameState.START_GAME)
            {


                if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) && keysStatus == false)
                {
                    Exit();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 195, (map.screenHeight / 2) - 145, 380, 100)))
                {

                    BattleTankTexture = this.Content.Load<Texture2D>("Graphics/battleTank1");

                }
                else
                {
                    BattleTankTexture = this.Content.Load<Texture2D>("Graphics/battleTank");
                }
                if (!LeftButtonStatus)
                { 
                    if (ButtonZagraj.IsClicked(ref state))
                    {
                        gameState = GameState.CHOICE_OF_GAME_TYPE;
                        LeftButtonStatus = true;
                    }

                    if (ButtonSettings.IsClicked(ref state))
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        gameState = GameState.SETTINGS;
                    }

                    ButtonKoniec.Position = new Vector2((map.screenWidth / 2) - (float)(ButtonKoniec.Width /2), (map.screenHeight / 2) + 80);
                    if (ButtonKoniec.IsClicked(ref state))
                    {
                        Exit();
                    }
                }
            }

            else if (gameState == GameState.SETTINGS)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    gameState = GameState.START_GAME;
                    keysStatus = true;


                }






                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                #region Headers
                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 195, 300, 70)))
                {
                    SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics/trybSterowania1");
                }
                else
                {
                    SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics/trybSterowania");
                }

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 130, 300, 60)))
                {
                    SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka1Gracza1");
                }
                else
                {
                    SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka1Gracza");
                }

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 3, 300, 60)))
                {
                    SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka2Gracza1");
                }
                else
                {
                    SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics/SukcesPorazka2Gracza");
                }
                #endregion

                #region Set Keyboard control for players
                if (ButtonSettingsTrybSterowaniaKlawMysz.IsClicked(ref state))
                {
                    PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                }
                else if (PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz.IsMouseOver = true;
                }

                if (ButtonSettingsTrybSterowaniaKlawMysz2.IsClicked(ref state))
                {
                        PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                }
                else if (PlayerTwoController.Equals(KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz2.IsMouseOver = true;
                }
                #endregion

                if (AvailableGamepads.Count > 0)
                {
                    if (ButtonSettingsTrybSterowaniaPad.IsClicked(ref state))
                    {
                            PlayerOneController = AvailableGamepads[0];
                            if (AvailableGamepads.Count == 1)
                                PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                    }
                    else if (!PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad.IsMouseOver = true;
                    }
                
                    if (ButtonSettingsTrybSterowaniaPad2.IsClicked(ref state))
                    {
                            if (AvailableGamepads.Count > 1)
                                PlayerTwoController = AvailableGamepads[1];
                            else if (AvailableGamepads.Count == 1)
                            {
                                PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                                PlayerTwoController = AvailableGamepads[0];
                            }
                    }
                    else if (!PlayerTwoController.Equals(KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad2.IsMouseOver = true;
                    }
                }
                else
                {
                    ButtonSettingsTrybSterowaniaPad.IsEnabled = false;
                    ButtonSettingsTrybSterowaniaPad2.IsEnabled = false;
                }



                ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 130, 250, 50);
                if (ButtonPowrot.IsClicked(ref state))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    LeftButtonStatus = true;
                    gameState = GameState.START_GAME;
                }
            }


            else if (gameState == GameState.CHOICE_OF_GAME_TYPE)
            {
                // To też nie jest najlepsze miejsce na tworzenie czołgów. Ale jest to
                // tuż po wybraniu ustawień przez użytkownika. Ze względu na to aby robić
                // jak najmniej komplikacji tutaj będą one tworzone. Jednak w przyszłości należy
                // przenieśc je jeszcze bliżej samej rozgrywki
                tank1 = new Tank(this, TankColors.GREEN, new Vector2(50, 50), new Vector2(3, 3), 1, 1, 1f, whiteRectangle, 1, 3, false,false, PlayerOneController);
                tank2 = new Tank(this, TankColors.RED, new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, 1, 3, false,false, PlayerTwoController);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && keysStatus == false)
                {

                    gameState = GameState.START_GAME;
                    keysStatus = true;


                }





                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;




                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (LeftButtonStatus == false)
                {

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 145, 300, 70)))
                    {

                        wyborTrybGryTexture = this.Content.Load<Texture2D>("Graphics/wyborTrybGry1");

                    }
                    else
                    {
                        wyborTrybGryTexture = this.Content.Load<Texture2D>("Graphics/wyborTrybGry");
                    }






                    if (ButtonPlayer1.CheckIsMouseOver(ref state))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka1");
                        if (ButtonPlayer1.IsClicked(ref state))
                        {
                            tank2.lives = 0;
                            tank2.armor = 0;
                            tank2.alive = false;
                            LeftButtonStatus = true;
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = GameState.GAME_RUNNING_PLAYER_1;
                        }
                    }
                    if (ButtonPlayer2.CheckIsMouseOver(ref state))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka2");
                        if (ButtonPlayer2.IsClicked(ref state))
                        {
                            LeftButtonStatus = true;
                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
                            iloscCPUKlasyk = 0;
                            iloscCPUKamikaze = 0;
                            soundOnOff = 1;
                            gameState = GameState.GAME_RUNNING_PLAYERS_2;
                            gameReturn = GameState.GAME_RUNNING_PLAYERS_2;
                        }
                    }

                    if (ButtonPlayer3.CheckIsMouseOver(ref state))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka3");
                        if (ButtonPlayer3.IsClicked(ref state))
                        {
                            LeftButtonStatus = true;
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = GameState.GAME_RUNNING_PLAYERS_2_AND_CPU;
                        }
                    }

                    if (ButtonPlayer4.CheckIsMouseOver(ref state))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka4");
                        if (ButtonPlayer4.IsClicked(ref state))
                        {
                            LeftButtonStatus = true;
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG;
                            gameReturn = GameState.GAME_RUNNING_RACE;
                        }
                    }
                }

            }

            else if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                    gameState = GameState.CHOICE_OF_GAME_TYPE;
                    keysStatus = true;



                }



                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;





                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (LeftButtonStatus == false)
                {


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 150, 300, 60)))
                    {

                        wyborCzasGry = this.Content.Load<Texture2D>("Graphics/wyborCzasGry1");

                    }
                    else
                    {
                        wyborCzasGry = this.Content.Load<Texture2D>("Graphics/wyborCzasGry");
                    }

                    if (Czas1Gry.IsClicked(ref state) || czasWyscigu == 120)
                    {
                        Czas1Gry.IsMouseOver = true;
                        czasWyscigu = 120;
                    }

                    if (Czas2Gry.IsClicked(ref state) || czasWyscigu == 300)
                    {
                        Czas2Gry.IsMouseOver = true;
                        czasWyscigu = 300;
                    }

                    if (Czas3Gry.IsClicked(ref state) || czasWyscigu == 600)
                    {
                        Czas3Gry.IsMouseOver = true;
                        czasWyscigu = 600;
                    }

                    doBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 80, 300, 60);
                    if (doBoju.IsClicked(ref state))
                    {
                        LeftButtonStatus = true;
                        tank1.lives = 1;
                        tank2.lives = 1;
                        tank1.mines = 10;
                        tank2.mines = 10;
                        map.WallBorder = randy.Next(5);
                        WallInside = true;
                        map.Reset();
                        iloscCPUKlasyk = 0;
                        iloscCPUKamikaze = 0;
                        sound.PlaySound(Sound.Sounds.KLIK);
                        soundOnOff = 1;
                        gameState = GameState.GAME_RUNNING_RACE;
                    }




                }

            }


            //

            else if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
            {





                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                    gameState = GameState.CHOICE_OF_GAME_TYPE;
                    keysStatus = true;
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");


                }



                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;




                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (LeftButtonStatus == false)
                {


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160,
                        (map.screenHeight / 2) - 220, 300, 60)))
                    {

                        wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics/wyborPoziomTrud1");

                    }
                    else
                    {
                        wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics/wyborPoziomTrud");
                    }


                    if (Poziom1Trud.IsClicked(ref state) || poziomTrudnosci == 1)
                    {
                        poziomTrudnosci = 1;
                        Poziom1Trud.IsMouseOver = true;
                    }

                    if (Poziom2Trud.IsClicked(ref state) || poziomTrudnosci == 2)
                    {
                        poziomTrudnosci = 2;
                        Poziom2Trud.IsMouseOver = true;
                    }

                    if (Poziom3Trud.IsClicked(ref state) || poziomTrudnosci == 3)
                    {
                        poziomTrudnosci = 3;
                        Poziom3Trud.IsMouseOver = true;
                    }

                    if (Poziom4Trud.IsClicked(ref state) || poziomTrudnosci == 4)
                    {
                        poziomTrudnosci = 4;
                        Poziom4Trud.IsMouseOver = true;
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160,
                        (map.screenHeight / 2) - 100, 300, 60)))
                    {

                        wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics/wyborCpuKlasyk1");

                    }
                    else
                    {
                        wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics/wyborCpuKlasyk");
                    }

                    if (wyborCpuKlasykIlosc0.IsClicked(ref state) || iloscCPUKlasyk == 0)
                    {
                        wyborCpuKlasykIlosc0.IsMouseOver = true;
                        iloscCPUKlasyk = 0;
                    }

                    if (wyborCpuKlasykIlosc1.IsClicked(ref state) || iloscCPUKlasyk == 1)
                    {
                        wyborCpuKlasykIlosc1.IsMouseOver = true;
                        iloscCPUKlasyk = 1;
                    }

                    if (wyborCpuKlasykIlosc2.IsClicked(ref state) || iloscCPUKlasyk == 2)
                    {
                        wyborCpuKlasykIlosc2.IsMouseOver = true;
                        iloscCPUKlasyk = 2;
                    }

                    if (wyborCpuKlasykIlosc3.IsClicked(ref state) || iloscCPUKlasyk == 3)
                    {
                        wyborCpuKlasykIlosc3.IsMouseOver = true;
                        iloscCPUKlasyk = 3;
                    }

                    //
                    //
                    //

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160,
                        (map.screenHeight / 2) + 20, 300, 60)))
                    {
                        wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics/wyborCpuKlamikaze1");
                    }
                    else
                    {
                        wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics/wyborCpuKlamikaze");
                    }

                    if (wyborCpuKlamikazeIlosc0.IsClicked(ref state) || iloscCPUKamikaze == 0)
                    {
                        wyborCpuKlamikazeIlosc0.IsMouseOver = true;
                        iloscCPUKamikaze = 0;
                    }

                    if (wyborCpuKlamikazeIlosc1.IsClicked(ref state) || iloscCPUKamikaze == 1)
                    {
                        wyborCpuKlamikazeIlosc1.IsMouseOver = true;
                        iloscCPUKamikaze = 1;
                    }

                    if (wyborCpuKlamikazeIlosc2.IsClicked(ref state) || iloscCPUKamikaze == 2)
                    {
                        wyborCpuKlamikazeIlosc2.IsMouseOver = true;
                        iloscCPUKamikaze = 2;
                    }

                    if (wyborCpuKlamikazeIlosc3.IsClicked(ref state) || iloscCPUKamikaze == 3)
                    {
                        wyborCpuKlamikazeIlosc3.IsMouseOver = true;
                        iloscCPUKamikaze = 3;
                    }

                    doBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 150, 300, 60);
                    if (doBoju.IsClicked(ref state))
                    {
                        LeftButtonStatus = true;
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        Vector2 speedCPU;

                        if (poziomTrudnosci == 3)
                            speedCPU = new Vector2(4, 4);
                        else
                            speedCPU = new Vector2(3, 3);

                        TankColors[] availableTankColors = new[] {TankColors.BLUE, TankColors.PINK, TankColors.YELLOW}; 

                        for (int i = 0; i < (iloscCPUKamikaze + iloscCPUKlasyk); i++)
                        {
                            enemyTanks.Add(new AI_Tank(this, 
                                availableTankColors[i % availableTankColors.Length],
                                new Vector2(map.screenWidth / 2f, (int)map.screenHeight / 2f), 
                                speedCPU, 0, 3 + i, 1f, whiteRectangle, 1, false, false, 
                                MathHelper.WrapAngle(MathHelper.PiOver4 * 3 * i), poziomTrudnosci, i >= iloscCPUKlasyk));
                        }


                        if (poziomTrudnosci == 3)
                        {
                            tank1.mines = 1;
                            tank1.lives = 1;

                            if (gameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                tank2.mines = 1;
                                tank2.lives = 1;
                            }
                        }
                        else if (poziomTrudnosci == 2)
                        {
                            tank1.mines = 3;
                            tank1.lives = 2;
                            tank1.armor = 2;
                            if (gameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                tank2.mines = 3;
                                tank2.lives = 2;
                                tank2.armor = 2;
                            }
                        }
                        else
                        {
                            tank1.mines = 5;
                            tank1.lives = 3;
                            tank1.armor = 3;
                            if (gameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                tank2.mines = 5;
                                tank2.lives = 3;
                                tank2.armor = 3;
                            }
                        }





                        map.WallBorder = randy.Next(5);
                        WallInside = true;
                        map.Reset();
                        soundOnOff = 1;

                        if (gameReturn == GameState.GAME_RUNNING_PLAYER_1)
                        {
                            gameState = GameState.GAME_RUNNING_PLAYER_1;

                        }


                        if (gameReturn == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
                        {
                            gameState = GameState.GAME_RUNNING_PLAYERS_2_AND_CPU;

                        }

                    }
                }
            }

            else
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    soundOnOff = 0;
                    gameState = GameState.PAUSE;
                }

                if (gameState != GameState.PAUSE)
                {
                    map.Update(gameTime);

                    mines.ForEach(c => c.Update());
                    mines.RemoveAll(d => !d.IsAlive);

                    bullets.ForEach(c => c.Update());
                    bullets.RemoveAll(d => !d.IsAlive);

                    foreach (Tank tank in enemyTanks.Concat(new [] {tank1, tank2}))
                    {
                        tank.Update(gameTime);

                        if (!tank.alive) continue;

                        if (tank.TryFire(out Bullet[] newBullets))
                        {
                            bullets.AddRange(newBullets);
                        }

                        if (tank.TryPlantMine(out Mine mine))
                        {
                            mines.Add(mine);
                        }
                    }

                    // Zastanów się Filipie czy tego potrzebujesz, skoro jest to nie używane akutalnie.
                    debugRect = new Rectangle((int)tank1.location.X - (tank1.tankTexture.Width / 2), (int)tank1.location.Y - (tank1.tankTexture.Height / 2), tank1.tankTexture.Width, tank1.tankTexture.Height);
                    tank2DebugRect = new Rectangle((int)tank2.location.X - (tank2.tankTexture.Width / 2), (int)tank2.location.Y - (tank2.tankTexture.Height / 2), tank2.tankTexture.Width, tank2.tankTexture.Height);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.WhiteSmoke);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);

            map.Draw(spriteBatch);

            foreach (Mine mine in mines)
            {
                mine.Draw(spriteBatch);
            }

            if (gameState == GameState.CHOICE_OF_GAME_TYPE || gameState == GameState.PAUSE || gameState == GameState.START_GAME)
            {

                spriteBatch.Draw(menuTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);

                if (gameState == GameState.START_GAME)
                {
                    spriteBatch.Draw(BattleTankTexture, new Rectangle((map.screenWidth / 2) - 195, (map.screenHeight / 2) - 145, 380, 100), Color.White);

                    ButtonZagraj.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);
                    ButtonKoniec.Draw(ref spriteBatch);
                }


                if (gameState == GameState.CHOICE_OF_GAME_TYPE)
                {
                    spriteBatch.Draw(wyborTrybGryTexture, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 145, 300, 70), Color.White);
                    ButtonPlayer1.Draw(ref spriteBatch);
                    ButtonPlayer2.Draw(ref spriteBatch);
                    ButtonPlayer3.Draw(ref spriteBatch);
                    ButtonPlayer4.Draw(ref spriteBatch);
                }



                if (gameState == GameState.PAUSE)
                {

                    spriteBatch.Draw(przerwaTexture, new Rectangle((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 145, 320, 80), Color.White);
                    ButtonPowrot.Draw(ref spriteBatch);
                    ButtonNowaGra.Draw(ref spriteBatch);
                    ButtonKoniec.Draw(ref spriteBatch);
                }
                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }

            if (gameState == GameState.SETTINGS || gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU || gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
            {
                spriteBatch.Draw(menuTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);


                if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
                {
                    spriteBatch.Draw(wyborPoziomTrud, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 220, 300, 60), Color.White);
                    Poziom1Trud.Draw(ref spriteBatch);
                    Poziom2Trud.Draw(ref spriteBatch);
                    Poziom3Trud.Draw(ref spriteBatch);
                    Poziom4Trud.Draw(ref spriteBatch);

                    spriteBatch.Draw(wyborCpuKlasyk, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 100, 300, 60), Color.White);
                    wyborCpuKlasykIlosc0.Draw(ref spriteBatch);
                    wyborCpuKlasykIlosc1.Draw(ref spriteBatch);
                    wyborCpuKlasykIlosc2.Draw(ref spriteBatch);
                    wyborCpuKlasykIlosc3.Draw(ref spriteBatch);

                    spriteBatch.Draw(wyborCpuKlamikaze, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 20, 300, 60), Color.White);
                    wyborCpuKlamikazeIlosc0.Draw(ref spriteBatch);
                    wyborCpuKlamikazeIlosc1.Draw(ref spriteBatch);
                    wyborCpuKlamikazeIlosc2.Draw(ref spriteBatch);
                    wyborCpuKlamikazeIlosc3.Draw(ref spriteBatch);
                    doBoju.Draw(ref spriteBatch);
                }

                if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
                {
                    spriteBatch.Draw(wyborCzasGry, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 150, 300, 60), Color.White);
                    Czas1Gry.Draw(ref spriteBatch);
                    Czas2Gry.Draw(ref spriteBatch);
                    Czas3Gry.Draw(ref spriteBatch);

                    doBoju.Draw(ref spriteBatch);
                }

                if (gameState == GameState.SETTINGS)
                {
                    spriteBatch.Draw(SettingsTrybSterowania, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 195, 300, 70), Color.White);
                    spriteBatch.Draw(SukcesPorazka1Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 130, 300, 60), Color.White);

                    ButtonSettingsTrybSterowaniaKlawMysz.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaPad.Draw(ref spriteBatch);

                    spriteBatch.Draw(SukcesPorazka2Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 3, 300, 60), Color.White);

                    ButtonSettingsTrybSterowaniaKlawMysz2.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaPad2.Draw(ref spriteBatch);

                    ButtonPowrot.Draw(ref spriteBatch);
                }


                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);

            }

            if (gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
            {
                spriteBatch.Draw(menuWinAndLossTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);
                ButtonNowaGra.Draw(ref spriteBatch);
                ButtonKoniec.Draw(ref spriteBatch);

                if (gameState == GameState.GAME_WIN)
                {
                    spriteBatch.Draw(winTexture, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 300, 70), Color.White);
                    if (tank2.lives == 0)
                        spriteBatch.Draw(SukcesPorazka1Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70), Color.White);
                    if (tank1.lives == 0)
                        spriteBatch.Draw(SukcesPorazka2Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70), Color.White);

                }

                if (gameState == GameState.GAME_LOSS)
                {
                    spriteBatch.Draw(lossTexture, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 300, 70), Color.White);

                }
                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }




            if (gameState == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {

                if (timer1control == 1)
                    RandomPowerUp.Draw(spriteBatch);

                tank1.Draw(spriteBatch);
                tank2.Draw(spriteBatch);
                foreach (AI_Tank et in enemyTanks)
                {
                    et.Draw(spriteBatch);
                }

                map.Draw(spriteBatch);

                scoreManager.Draw(spriteBatch);

                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }
            }
            if (gameState == GameState.GAME_RUNNING_PLAYERS_2 || gameState == GameState.GAME_RUNNING_RACE)
            {
                //
                if (timer1control == 1)
                    RandomPowerUp.Draw(spriteBatch);




                tank1.Draw(spriteBatch);
                tank2.Draw(spriteBatch);



                map.Draw(spriteBatch);

                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }
                scoreManager.Draw(spriteBatch);


            }
            if (gameState == GameState.GAME_RUNNING_PLAYER_1)
            {
                if (timer1control == 1)
                    RandomPowerUp.Draw(spriteBatch);

                tank1.Draw(spriteBatch);

                foreach (AI_Tank et in enemyTanks)
                {
                    et.Draw(spriteBatch);
                }
                scoreManager.Draw(spriteBatch);

                map.Draw(spriteBatch);
                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

