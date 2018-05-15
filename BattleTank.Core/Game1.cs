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

        Texture2D background;
        Texture2D menuTexture;
        Texture2D menuWinAndLossTexture;
        Label LabelBattleTank;
        Label LabelwyborTrybGryTexture;
        Label LabelprzerwaTexture;
        Label LabelwinTexture;
        Label LabellossTexture;
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
        Label LabelSettingsTrybSterowania;
        Label LabelTrybSterowania1Gracza;
        Label LabelTrybSterowania2Gracza;
        Button ButtonSettingsTrybSterowaniaKlawMysz;
        Button ButtonSettingsTrybSterowaniaPad;
        Button ButtonSettingsTrybSterowaniaKlawMysz2;
        Button ButtonSettingsTrybSterowaniaPad2;
        Label LabelwyborPoziomTrud;
        Button ButtonPoziom1Trud;
        Button ButtonPoziom2Trud;
        Button ButtonPoziom3Trud;
        Button ButtonPoziom4Trud;
        Label LabelwyborCpuKlasyk;
        Button ButtonwyborCpuKlasykIlosc0;
        Button ButtonwyborCpuKlasykIlosc1;
        Button ButtonwyborCpuKlasykIlosc2;
        Button ButtonwyborCpuKlasykIlosc3;
        Label LabelwyborCpuKlamikaze;
        Button ButtonwyborCpuKlamikazeIlosc0;
        Button ButtonwyborCpuKlamikazeIlosc1;
        Button ButtonwyborCpuKlamikazeIlosc2;
        Button ButtonwyborCpuKlamikazeIlosc3;
        Button ButtonCzas1Gry;
        Button ButtonCzas2Gry;
        Button ButtonCzas3Gry;
        Label LabelwyborCzasGry;
        Label LabelSukcesPorazka1Gracza;
        Label LabelSukcesPorazka2Gracza;
        Button ButtondoBoju;
        Vector2 positionMouse;

        public Sound menuSound;
        public Sound sound;

        public PowerUp RandomPowerUp;
       public Random randy = new Random();

        bool keysStatus = false;
        bool LeftButtonStatus = false;

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

        public ITankActionProvider PlayerOneController { get; set; } // = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
        public ITankActionProvider PlayerTwoController { get; set; } = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
        public List<ITankActionProvider> AvailableGamepads { get; set; } = new List<ITankActionProvider>();
        internal VirtualGamepad VirtualGamepad { get; private set; }

        /// <summary>
        /// Odpowiada za transformacje widoku dla gracza przy rysowaniu zawartości.
        /// </summary>
        private Camera2D _camera;


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
         

            _camera = new Camera2D(GraphicsDevice.PresentationParameters);
            

            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0);
            whiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Graphics/Background");
            menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
            menuWinAndLossTexture = Content.Load<Texture2D>("Graphics/MenuWinAndLoss");
                    
            cursorTexture = Content.Load<Texture2D>("Graphics/cursor");

            scoreManager = new Score(this, 10);
            sound = new Sound(this);

            RandomPowerUp = new PowerUp(this);

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


            LabelBattleTank = new Label("BaTtLeTaNk", new Vector2((map.screenWidth / 2) - 195, (map.screenHeight / 2) - 135), null, 80);
            LabelBattleTank.CenterHorizontal();
            LabelwyborPoziomTrud = new Label("PoZiOm TrUdNoScI", new Vector2((map.screenWidth / 2) - 160,  (map.screenHeight / 2) - 220), null, 60);
            LabelwyborPoziomTrud.CenterHorizontal();
            LabelwyborCpuKlasyk = new Label("CpU kLaSyCzNyCh", new Vector2((map.screenWidth / 2) - 160,  (map.screenHeight / 2) - 100), null, 60);
            LabelwyborCpuKlasyk.CenterHorizontal();
            LabelwyborCpuKlamikaze = new Label("CpU kAmIkAzE", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 20), null, 60);
            LabelwyborCpuKlamikaze.CenterHorizontal();
            LabelwyborCzasGry = new Label("CzAs RoZgRyWkI", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 150), null, 60);
            LabelwyborCzasGry.CenterHorizontal();
            LabelSukcesPorazka1Gracza = new Label("GrAcZa 1", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60), null, 70);
            LabelSukcesPorazka1Gracza.CenterHorizontal();
            LabelSukcesPorazka2Gracza = new Label("GrAcZa 2", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60), null, 70);
            LabelSukcesPorazka2Gracza.CenterHorizontal();
            LabelSettingsTrybSterowania = new Label("TrYb StErOwAnIa", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 195), null, 70);
            LabelSettingsTrybSterowania.CenterHorizontal();
            LabelTrybSterowania1Gracza = new Label("GrAcZ 1", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 130), null, 60);
            LabelTrybSterowania1Gracza.CenterHorizontal();
            LabelTrybSterowania2Gracza = new Label("GrAcZ 2", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 5 ), null, 60);
            LabelTrybSterowania2Gracza.CenterHorizontal();
            LabelwyborTrybGryTexture = new Label("WyBoR TrYbU\n        gRy", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 160), null, 100);
            LabelwyborTrybGryTexture.CenterHorizontal();
            LabelwinTexture = new Label("SuKcEs", new Vector2((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140), null, 75);
            LabelwinTexture.CenterHorizontal();
            LabellossTexture = new Label("PrZeGrAnA", new Vector2((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140), null, 75);
            LabellossTexture.CenterHorizontal();
            LabelprzerwaTexture = new Label("PrZeRwA", new Vector2((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 145), null, 75);
            LabelprzerwaTexture.CenterHorizontal();


            ButtonKoniec = new Button("KoNiEc", new Vector2(), null, 60);
            ButtonKoniec.CenterHorizontal();
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
            ButtonSettingsTrybSterowaniaKlawMysz = new Button("KlAwIaTuRa/MySz", new Vector2((map.screenWidth / 2) - 270, (map.screenHeight / 2) - 60), null, 50);
            ButtonSettingsTrybSterowaniaPad = new Button("GaMePaD", new Vector2((map.screenWidth / 2) + 100, (map.screenHeight / 2) - 60), null, 50);
            ButtonSettingsTrybSterowaniaKlawMysz2 = new Button("KlAwIaTuRa/MySz",  new Vector2((map.screenWidth / 2) - 270, (map.screenHeight / 2) + 67), null, 50);
            ButtonSettingsTrybSterowaniaPad2 = new Button("GaMePaD", new Vector2((map.screenWidth / 2) + 100, (map.screenHeight / 2) + 67), null, 50);
            ButtonPowrot = new Button("PoWrOt", new Vector2((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 40), null, 60);
            ButtonNowaGra = new Button("NoWa GrA", new Vector2((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 20), null, 60);
            ButtonNowaGra.CenterHorizontal();
            ButtonPoziom1Trud = new Button("BaNaLnY", new Vector2((map.screenWidth / 2) - 385, (map.screenHeight / 2) - 155), null, 50);
            ButtonPoziom2Trud = new Button("PrZyZwOiTy", new Vector2((map.screenWidth / 2) - 210, (map.screenHeight / 2) - 155), null, 50);
            ButtonPoziom3Trud = new Button("ZaBoJcZy", new Vector2((map.screenWidth / 2) + 35, (map.screenHeight / 2) - 155), null, 50);
            ButtonPoziom4Trud = new Button("SzAlOnY", new Vector2((map.screenWidth / 2) + 225, (map.screenHeight / 2) - 155), null, 50);
            ButtonwyborCpuKlasykIlosc0 = new Button("0", new Vector2((map.screenWidth / 2) - 225, (map.screenHeight / 2) - 30), null, 50);
            ButtonwyborCpuKlasykIlosc1 = new Button("1", new Vector2((map.screenWidth / 2) - 90, (map.screenHeight / 2) - 30), null, 50);
            ButtonwyborCpuKlasykIlosc2 = new Button("2", new Vector2((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 30), null, 50);
            ButtonwyborCpuKlasykIlosc3 = new Button("3", new Vector2((map.screenWidth / 2) + 175, (map.screenHeight / 2) - 30), null, 50);
            ButtonwyborCpuKlamikazeIlosc0 = new Button("0", new Vector2((map.screenWidth / 2) - 225, (map.screenHeight / 2) + 90), null, 50);
            ButtonwyborCpuKlamikazeIlosc1 = new Button("1", new Vector2((map.screenWidth / 2) - 90, (map.screenHeight / 2) + 90), null, 50);
            ButtonwyborCpuKlamikazeIlosc2 = new Button("2", new Vector2((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 90), null, 50);
            ButtonwyborCpuKlamikazeIlosc3 = new Button("3", new Vector2((map.screenWidth / 2) + 175, (map.screenHeight / 2) + 90), null, 50);
            ButtonCzas1Gry = new Button("2 MiNuTy", new Vector2((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 90), null, 50);
            ButtonCzas1Gry.CenterHorizontal();
            ButtonCzas2Gry = new Button("5 MiNuT", new Vector2((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 35), null, 50);
            ButtonCzas2Gry.CenterHorizontal();
            ButtonCzas3Gry = new Button("10 MiNuT", new Vector2((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 20), null, 50);
            ButtonCzas3Gry.CenterHorizontal();
            ButtondoBoju = new Button("Do BoJu!!!",  new Vector2(), null, 60);
            ButtondoBoju.CenterHorizontal();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        

            UIElement.ActiveFont = Content.Load<SpriteFont>("Fonts/ActiveFont");
            UIElement.InActiveFont = Content.Load<SpriteFont>("Fonts/Inactivefont");
            UIElement.ActiveFont.Spacing = -75;
            UIElement.InActiveFont.Spacing = -75;

            UIElement.GraphicsDevice = GraphicsDevice;
            
            var fireButtonTexture = Content.Load<Texture2D>("Graphics/VirtualJoy/FireButton");
            var mineButtonTexture = Content.Load<Texture2D>("Graphics/VirtualJoy/MineButton");

            PlayerOneController = VirtualGamepad = new VirtualGamepad(
                Content.Load<Texture2D>("Graphics/VirtualJoy/JoystickBase"),
                Content.Load<Texture2D>("Graphics/VirtualJoy/JoystickTop"),
                new Button(fireButtonTexture, fireButtonTexture,
                    new Vector2((float) (GraphicsDevice.PresentationParameters.BackBufferWidth * 0.87),
                                (float) (GraphicsDevice.PresentationParameters.BackBufferHeight * 0.63)),null,(int)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.15)),
                new Button(mineButtonTexture, mineButtonTexture,
                    new Vector2((float)(GraphicsDevice.PresentationParameters.BackBufferWidth * 0.80),
                                (float)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.78)), null, (int)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.15)));
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
            
            if (Keyboard.GetState().IsKeyUp(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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

            if (RandomPowerUp.alive)
            {
                RandomPowerUp.Update(gameTime);
            }
            else {
                RandomPowerUp = new PowerUp(this);
                RandomPowerUp.Random();
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

         
                    ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 40, (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                    ButtonPowrot.CenterHorizontal();
                    if (ButtonPowrot.IsClicked(ref state))
                    {
                        soundOnOff = 1;
                        gameState = gameReturn;
                    }
                }

                else if (gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
                {
                    soundOnOff = 0;

                
                }

                if (ButtonNowaGra.IsClicked(ref state))
                {
                    WallInside = false;
                    map.Reset();
                    LeftButtonStatus = true;
                    RandomPowerUp.alive = false;
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
                ButtonKoniec.CenterHorizontal();
                if (ButtonKoniec.IsClicked(ref state))
                {
                    Exit();
                }

            }

            else if (gameState == GameState.START_GAME)
            {


                if (((Keyboard.GetState().IsKeyDown(Keys.Escape)) && keysStatus == false) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    Exit();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

              


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
                    ButtonKoniec.CenterHorizontal();
                    if (ButtonKoniec.IsClicked(ref state))
                    {
                        Exit();
                    }
                }
            }

            else if (gameState == GameState.SETTINGS)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    gameState = GameState.START_GAME;
                    keysStatus = true;


                }






                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

             
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



                ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 130, (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                ButtonPowrot.CenterHorizontal();
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

                if ((Keyboard.GetState().IsKeyDown(Keys.Escape) && keysStatus == false) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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



                    if (ButtonCzas1Gry.IsClicked(ref state) || czasWyscigu == 120)
                    {
                        ButtonCzas1Gry.IsMouseOver = true;
                        czasWyscigu = 120;
                    }

                    if (ButtonCzas2Gry.IsClicked(ref state) || czasWyscigu == 300)
                    {
                        ButtonCzas2Gry.IsMouseOver = true;
                        czasWyscigu = 300;
                    }

                    if (ButtonCzas3Gry.IsClicked(ref state) || czasWyscigu == 600)
                    {
                        ButtonCzas3Gry.IsMouseOver = true;
                        czasWyscigu = 600;
                    }

                    ButtondoBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 80,(int)ButtondoBoju.Width,(int)ButtondoBoju.Height);
                    ButtondoBoju.CenterHorizontal();
                    if (ButtondoBoju.IsClicked(ref state))
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





                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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


                   


                    if (ButtonPoziom1Trud.IsClicked(ref state) || poziomTrudnosci == 1)
                    {
                        poziomTrudnosci = 1;
                        ButtonPoziom1Trud.IsMouseOver = true;
                    }

                    if (ButtonPoziom2Trud.IsClicked(ref state) || poziomTrudnosci == 2)
                    {
                        poziomTrudnosci = 2;
                        ButtonPoziom2Trud.IsMouseOver = true;
                    }

                    if (ButtonPoziom3Trud.IsClicked(ref state) || poziomTrudnosci == 3)
                    {
                        poziomTrudnosci = 3;
                        ButtonPoziom3Trud.IsMouseOver = true;
                    }

                    if (ButtonPoziom4Trud.IsClicked(ref state) || poziomTrudnosci == 4)
                    {
                        poziomTrudnosci = 4;
                        ButtonPoziom4Trud.IsMouseOver = true;
                    }

                    

                    if (ButtonwyborCpuKlasykIlosc0.IsClicked(ref state) || iloscCPUKlasyk == 0)
                    {
                        ButtonwyborCpuKlasykIlosc0.IsMouseOver = true;
                        iloscCPUKlasyk = 0;
                    }

                    if (ButtonwyborCpuKlasykIlosc1.IsClicked(ref state) || iloscCPUKlasyk == 1)
                    {
                        ButtonwyborCpuKlasykIlosc1.IsMouseOver = true;
                        iloscCPUKlasyk = 1;
                    }

                    if (ButtonwyborCpuKlasykIlosc2.IsClicked(ref state) || iloscCPUKlasyk == 2)
                    {
                        ButtonwyborCpuKlasykIlosc2.IsMouseOver = true;
                        iloscCPUKlasyk = 2;
                    }

                    if (ButtonwyborCpuKlasykIlosc3.IsClicked(ref state) || iloscCPUKlasyk == 3)
                    {
                        ButtonwyborCpuKlasykIlosc3.IsMouseOver = true;
                        iloscCPUKlasyk = 3;
                    }

                    if (ButtonwyborCpuKlamikazeIlosc0.IsClicked(ref state) || iloscCPUKamikaze == 0)
                    {
                        ButtonwyborCpuKlamikazeIlosc0.IsMouseOver = true;
                        iloscCPUKamikaze = 0;
                    }

                    if (ButtonwyborCpuKlamikazeIlosc1.IsClicked(ref state) || iloscCPUKamikaze == 1)
                    {
                        ButtonwyborCpuKlamikazeIlosc1.IsMouseOver = true;
                        iloscCPUKamikaze = 1;
                    }

                    if (ButtonwyborCpuKlamikazeIlosc2.IsClicked(ref state) || iloscCPUKamikaze == 2)
                    {
                        ButtonwyborCpuKlamikazeIlosc2.IsMouseOver = true;
                        iloscCPUKamikaze = 2;
                    }

                    if (ButtonwyborCpuKlamikazeIlosc3.IsClicked(ref state) || iloscCPUKamikaze == 3)
                    {
                        ButtonwyborCpuKlamikazeIlosc3.IsMouseOver = true;
                        iloscCPUKamikaze = 3;
                    }

                    ButtondoBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 150, (int)ButtondoBoju.Width, (int)ButtondoBoju.Height);
                    ButtondoBoju.CenterHorizontal();
                    if (ButtondoBoju.IsClicked(ref state))
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

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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
            if (gameState != GameState.GAME_RUNNING_PLAYER_1)
            { 
                _camera.Scale = 1;
                _camera.Position = Vector2.Zero;
                _camera.Center = false;
            }

            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix());
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
                    LabelBattleTank.Draw(ref spriteBatch);

                    ButtonZagraj.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);
                    ButtonKoniec.Draw(ref spriteBatch);
                }


                if (gameState == GameState.CHOICE_OF_GAME_TYPE)
                {
                    LabelwyborTrybGryTexture.Draw(ref spriteBatch);
                    ButtonPlayer1.Draw(ref spriteBatch);
                    ButtonPlayer2.Draw(ref spriteBatch);
                    ButtonPlayer3.Draw(ref spriteBatch);
                    ButtonPlayer4.Draw(ref spriteBatch);
                }



                if (gameState == GameState.PAUSE)
                {

                    LabelprzerwaTexture.Draw(ref spriteBatch);
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
                    LabelwyborPoziomTrud.Draw(ref spriteBatch);
                    ButtonPoziom1Trud.Draw(ref spriteBatch);
                    ButtonPoziom2Trud.Draw(ref spriteBatch);
                    ButtonPoziom3Trud.Draw(ref spriteBatch);
                    ButtonPoziom4Trud.Draw(ref spriteBatch);

                    LabelwyborCpuKlasyk.Draw(ref spriteBatch);
                    ButtonwyborCpuKlasykIlosc0.Draw(ref spriteBatch);
                    ButtonwyborCpuKlasykIlosc1.Draw(ref spriteBatch);
                    ButtonwyborCpuKlasykIlosc2.Draw(ref spriteBatch);
                    ButtonwyborCpuKlasykIlosc3.Draw(ref spriteBatch);

                    LabelwyborCpuKlamikaze.Draw(ref spriteBatch);
                    ButtonwyborCpuKlamikazeIlosc0.Draw(ref spriteBatch);
                    ButtonwyborCpuKlamikazeIlosc1.Draw(ref spriteBatch);
                    ButtonwyborCpuKlamikazeIlosc2.Draw(ref spriteBatch);
                    ButtonwyborCpuKlamikazeIlosc3.Draw(ref spriteBatch);
                    ButtondoBoju.Draw(ref spriteBatch);
                }

                if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
                {
                    LabelwyborCzasGry.Draw(ref spriteBatch);
                    ButtonCzas1Gry.Draw(ref spriteBatch);
                    ButtonCzas2Gry.Draw(ref spriteBatch);
                    ButtonCzas3Gry.Draw(ref spriteBatch);

                    ButtondoBoju.Draw(ref spriteBatch);
                }

                if (gameState == GameState.SETTINGS)
                {
                    LabelSettingsTrybSterowania.Draw(ref spriteBatch);
                    LabelTrybSterowania1Gracza.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaKlawMysz.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaPad.Draw(ref spriteBatch);
                    LabelTrybSterowania2Gracza.Draw(ref spriteBatch);
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
                    LabelwinTexture.Draw(ref spriteBatch);
                    if (tank2.lives == 0)
                        LabelSukcesPorazka1Gracza.Draw(ref spriteBatch);
                    if (tank1.lives == 0)
                        LabelSukcesPorazka2Gracza.Draw(ref spriteBatch);
                }

                if (gameState == GameState.GAME_LOSS)
                {
                    LabellossTexture.Draw(ref spriteBatch);
                }
                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }




            if (gameState == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {

                if (RandomPowerUp.alive)
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
                if (RandomPowerUp.alive)
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
                _camera.Scale = Environment.OSVersion.Platform == PlatformID.Win32NT ? 1 : 2;
                _camera.Position = tank1.location;
                _camera.Center = true;
                _camera.MaxLeftTopCorner = new Point(0);
                _camera.MaxRightBottomCorner = new Point(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

                if (RandomPowerUp.alive)
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
                    bullet.Draw(spriteBatch);
                }
            }


            spriteBatch.End();
            if (gameState == GameState.GAME_RUNNING_PLAYER_1)
            {
                spriteBatch.Begin();
                VirtualGamepad.Draw(ref spriteBatch);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

