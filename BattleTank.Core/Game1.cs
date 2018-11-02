
using System;
using System.Collections.Generic;
using System.Linq;
using BattleTank.Core.GUI;
using BattleTank.Core.Input;
using BattleTank.Core.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static BattleTank.Core.Settings;
using static BattleTank.Core.Tanks.Tank;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace BattleTank.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Map map;
        public Settings settings;
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
        Label LabelStatementAboutControllerDisconnected;
        Button ButtonSettingsTrybSterowaniaKlawMysz;
        Button ButtonSettingsTrybSterowaniaPad;
        Button ButtonSettingsTrybSterowaniaKlawMysz2;
        Button ButtonSettingsTrybSterowaniaPad2;
        Button ButtonSettingsTrybSterowaniaBasic;
        Button ButtonSettingsTrybSterowaniaAdvanced;
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

        public Sound sound;
        public Sound[] soundsTanks;
        public PowerUp RandomPowerUp;
        public Random randy = new Random();

        bool keysStatus = false;
        bool LeftButtonStatus = false;
        bool RightButtonStatus = false;

        public enum GameState
        {         
            START_GAME,
            SETTINGS_WINDOWS,
            SETTINGS_ANDROID,
            CHOICE_OF_GAME_TYPE,
            CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU,
            CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE,
            PAUSE,
            GAME_RUNNING_PLAYER_1,
            GAME_RUNNING_PLAYERS_2,
            GAME_RUNNING_PLAYERS_2_AND_CPU,
            GAME_RUNNING_RACE,
            GAME_WIN,
            GAME_LOSS,
            STATEMENT_ABOUT_CONTROLLER_DISCONNECTED
        }
      

        public enum FunctionsOfTheNavigationKeys
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            CONFIRM,
            BACK
        }

//Tuple<List<Keys> - lista klawiszy na klawiaturze  Tuple<List<Buttons>, int[]>> - lista przycisków na GamePad(XBOX) tablica wartości dla GamePad(Generic)
        public Dictionary<FunctionsOfTheNavigationKeys, Tuple< List<Keys>, Tuple<List<Buttons>, int[]>>> navigationKeys = new Dictionary<FunctionsOfTheNavigationKeys, Tuple<List<Keys>, Tuple<List<Buttons>, int[]>>>
        {
             { FunctionsOfTheNavigationKeys.UP, Tuple.Create( new List<Keys>(){Keys.Up,Keys.W },  Tuple.Create( new List<Buttons>(){ Buttons.DPadUp, Buttons.LeftThumbstickUp }, new int[] { -1, 0 } ) )  },
             { FunctionsOfTheNavigationKeys.DOWN, Tuple.Create( new List<Keys>(){Keys.Down,Keys.S },   Tuple.Create(new List<Buttons>(){ Buttons.DPadDown, Buttons.LeftThumbstickDown }, new int[] {-1, 18000 } )  )  },
             { FunctionsOfTheNavigationKeys.LEFT, Tuple.Create( new List<Keys>(){Keys.Left,Keys.A },   Tuple.Create(new List<Buttons>(){ Buttons.DPadLeft, Buttons.LeftThumbstickLeft }, new int[] {-1, 27000 } )  )  },
             { FunctionsOfTheNavigationKeys.RIGHT, Tuple.Create( new List<Keys>(){Keys.Right, Keys.D },  Tuple.Create( new List<Buttons>(){ Buttons.DPadRight, Buttons.LeftThumbstickRight }, new int[] {-1, 9000 } )  )  },
             { FunctionsOfTheNavigationKeys.CONFIRM, Tuple.Create( new List<Keys>(){Keys.Enter },   Tuple.Create(new List<Buttons>(){ Buttons.Start, Buttons.RightStick }, new int[] { 9, -2 } )  )  },
             { FunctionsOfTheNavigationKeys.BACK, Tuple.Create( new List<Keys>(){Keys.Escape, Keys.Back },   Tuple.Create(new List<Buttons>(){ Buttons.Back, Buttons.LeftStick }, new int[] { 8, -2 } )  )  },

        };

        public bool IsPressing(Tuple<List<Keys>, Tuple<List<Buttons>, int[]>> items)
        {

#if WINDOWS
            List<GenericGamepadTankActionProvider> genericControllersAvailable = new List<GenericGamepadTankActionProvider>();

            foreach (ITankActionProvider tankActionProvider in AvailableGamepads.Where(e => e.GetType().Name.Equals("GenericGamepadTankActionProvider") && ((GenericGamepadTankActionProvider)e)._joystick.Poll().IsSuccess).ToList())
            {
                genericControllersAvailable.Add((GenericGamepadTankActionProvider)tankActionProvider);
            }
#endif

#if ANDROID
            return (!items.Item1.TrueForAll(c => Keyboard.GetState().IsKeyUp(c)));

#elif WINDOWS
            return (!items.Item1.TrueForAll(c => Keyboard.GetState().IsKeyUp(c))) ||
             (!items.Item2.Item1.TrueForAll(c => GamePad.GetState(PlayerIndex.One).IsButtonUp(c)) ||
             (!items.Item2.Item1.TrueForAll(c => GamePad.GetState(PlayerIndex.Two).IsButtonUp(c)))) ||
         (items.Item2.Item2[1] != -2 ? (!genericControllersAvailable.TrueForAll(c => !c._joystick.GetCurrentState().GetPointOfViewControllers()[0].Equals(items.Item2.Item2[1]))) : false) ||
          (items.Item2.Item2[0] != -1 ? (!genericControllersAvailable.TrueForAll(c => !c._joystick.GetCurrentState().GetButtons()[items.Item2.Item2[0]])) : false);

#else
            return false;

#endif
        }






        public Dictionary<GameState, Button[,]> buttonsInMemu;

        int currentButtonX = 0;
        int currentButtonY = 0;

        public GameState gameState;
        public GameState gameReturn;

        public ITankActionProvider PlayerOneController { get; set; } = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
        public ITankActionProvider PlayerTwoController { get; set; } = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
        public List<ITankActionProvider> AvailableGamepads { get; set; } = new List<ITankActionProvider>();
        internal VirtualGamepad VirtualGamepad { get; private set; }

        void BlockKeysAndMouseAndDefaultCurrentButton()
        {
            keysStatus = true;
            LeftButtonStatus = true;
            RightButtonStatus = true;
            currentButtonX = 0;
            currentButtonY = 0;
        }


        /// <summary>
        /// Odpowiada za transformacje widoku dla gracza przy rysowaniu zawartości.
        /// </summary>
        public Camera2D Camera;


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
            settings = new Settings();

            IsMouseVisible = false;
            gameState = GameState.START_GAME;
            gameReturn = GameState.START_GAME;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % settings.elementsOnTheWidth;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % settings.elementsOnTheHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Camera = new Camera2D(GraphicsDevice.PresentationParameters);

            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0, 0);
            whiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Graphics/Background");
            menuTexture = Content.Load<Texture2D>("Graphics/Ramka");

            cursorTexture = Content.Load<Texture2D>("Graphics/cursor");

            scoreManager = new Score(this, 10);
            sound = new Sound(this);
            soundsTanks = new Sound[9];

            RandomPowerUp = new PowerUp(this);

            // Zainicjalizuj odłgos kliknięcia
            Button.ClickSound = Content.Load<SoundEffect>("Sounds\\klik");
            Button.Effect = Content.Load<Effect>(@"Shaders\GreyscaleEffect");



            sound.PlaySound(Sound.Sounds.MENU_SOUND);
            AvailableGamepads = GamePads.GetAllAvailableGamepads();
            base.Initialize();
            positionMouse = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                                    graphics.GraphicsDevice.Viewport.Height / 2);


            LabelBattleTank = new Label("BaTtLeTaNk", new Vector2((map.screenWidth / 2) - 195, (map.screenHeight / 2) - 135), null, 80);
            LabelBattleTank.CenterHorizontal();
            LabelwyborPoziomTrud = new Label("PoZiOm TrUdNoScI", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 220), null, 60);
            LabelwyborPoziomTrud.CenterHorizontal();
            LabelwyborCpuKlasyk = new Label("CpU kLaSyCzNyCh", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 100), null, 60);
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
            LabelTrybSterowania2Gracza = new Label("GrAcZ 2", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 5), null, 60);
            LabelTrybSterowania2Gracza.CenterHorizontal();
            LabelwyborTrybGryTexture = new Label("WyBoR TrYbU\n        gRy", new Vector2((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 160), null, 100);
            LabelwyborTrybGryTexture.CenterHorizontal();
            LabelwinTexture = new Label("SuKcEs", new Vector2((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140), null, 75);
            LabelwinTexture.CenterHorizontal();
            LabellossTexture = new Label("PrZeGrAnA", new Vector2((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140), null, 75);
            LabellossTexture.CenterHorizontal();
            LabelprzerwaTexture = new Label("PrZeRwA", new Vector2((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 185), null, 75);
            LabelprzerwaTexture.CenterHorizontal();
            LabelStatementAboutControllerDisconnected = new Label("KoNtRoLeR zOsTaL oDlAcZoNy\n       PrZeJdZ dO UsTaWiEn,\n      AbY zMiEnIc StErOwAnIe", new Vector2((map.screenWidth / 2) - 300, (map.screenHeight / 2) - 160), null, 130);
            LabelStatementAboutControllerDisconnected.CenterHorizontal();

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
            ButtonSettingsTrybSterowaniaKlawMysz2 = new Button("KlAwIaTuRa/MySz", new Vector2((map.screenWidth / 2) - 270, (map.screenHeight / 2) + 67), null, 50);
            ButtonSettingsTrybSterowaniaPad2 = new Button("GaMePaD", new Vector2((map.screenWidth / 2) + 100, (map.screenHeight / 2) + 67), null, 50);
            ButtonSettingsTrybSterowaniaBasic = new Button("PoDsTaWoWy", new Vector2((map.screenWidth / 2) - 270, (map.screenHeight / 2) - 70), null, 60);
            ButtonSettingsTrybSterowaniaBasic.CenterHorizontal();
            ButtonSettingsTrybSterowaniaAdvanced = new Button("ZaAwAnSoWaNy", new Vector2((map.screenWidth / 2) - 270, (map.screenHeight / 2) + 20), null, 60);
            ButtonSettingsTrybSterowaniaAdvanced.CenterHorizontal();
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
            ButtondoBoju = new Button("Do BoJu!!!", new Vector2(), null, 60);
            ButtondoBoju.CenterHorizontal();

            buttonsInMemu = new Dictionary<GameState, Button[,]>
            {
                { GameState.START_GAME, new Button[3, 1] {  { ButtonZagraj },
                                                            { ButtonSettings },
                                                            { ButtonKoniec } } },
                { GameState.CHOICE_OF_GAME_TYPE, new Button[4, 1] { { ButtonPlayer1 },
                                                                    { ButtonPlayer2 },
                                                                    { ButtonPlayer3 },
                                                                    { ButtonPlayer4 } } },
                { GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE, new Button[4, 1] { { ButtonCzas1Gry },
                                                                                         { ButtonCzas2Gry },
                                                                                         { ButtonCzas3Gry },
                                                                                         { ButtondoBoju } } },
                { GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU, new Button[4, 4] { { ButtonPoziom1Trud, ButtonPoziom2Trud, ButtonPoziom3Trud, ButtonPoziom4Trud },
                                                                                        { ButtonwyborCpuKlasykIlosc0, ButtonwyborCpuKlasykIlosc1, ButtonwyborCpuKlasykIlosc2, ButtonwyborCpuKlasykIlosc3 },
                                                                                        { ButtonwyborCpuKlamikazeIlosc0, ButtonwyborCpuKlamikazeIlosc1, ButtonwyborCpuKlamikazeIlosc2, ButtonwyborCpuKlamikazeIlosc3 },
                                                                                        { ButtondoBoju, ButtondoBoju, ButtondoBoju, ButtondoBoju }} },
                { GameState.PAUSE, new Button[2, 2] { { ButtonPowrot, ButtonNowaGra },
                                                      { ButtonSettings , ButtonKoniec } } },
                { GameState.GAME_WIN, new Button[2, 1] { { ButtonNowaGra },
                                                         { ButtonKoniec } } },
                { GameState.GAME_LOSS, new Button[2, 1] { { ButtonNowaGra },
                                                          { ButtonKoniec } } },
                { GameState.SETTINGS_WINDOWS, new Button[3, 2] { { ButtonSettingsTrybSterowaniaKlawMysz, ButtonSettingsTrybSterowaniaPad },
                                                         { ButtonSettingsTrybSterowaniaKlawMysz2, ButtonSettingsTrybSterowaniaPad2 },
                                                         { ButtonPowrot, ButtonPowrot } } },
                { GameState.SETTINGS_ANDROID, new Button[3, 1] { { ButtonSettingsTrybSterowaniaBasic },
                                                                    { ButtonSettingsTrybSterowaniaAdvanced },
                                                                    { ButtonPowrot } } },
                { GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED, new Button[1, 1] { { ButtonSettings } } }
            };
   
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

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                PlayerOneController = VirtualGamepad = new VirtualGamepad(
                Content.Load<Texture2D>("Graphics/VirtualJoy/JoystickBase"),
                Content.Load<Texture2D>("Graphics/VirtualJoy/JoystickTop"),
                new Button(fireButtonTexture, fireButtonTexture,
                    new Vector2((float)(GraphicsDevice.PresentationParameters.BackBufferWidth * 0.87),
                                (float)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.63)), null, (int)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.15)),
                new Button(mineButtonTexture, mineButtonTexture,
                    new Vector2((float)(GraphicsDevice.PresentationParameters.BackBufferWidth * 0.80),
                                (float)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.78)), null, (int)(GraphicsDevice.PresentationParameters.BackBufferHeight * 0.15)),
                                VirtualGamepad.DirectionsOfMovement.BASIC);
            }

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
            PointerState state = PointerState.GetState();



            if (          
                !(IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.UP]) ||
                IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.DOWN]) ||
                IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.LEFT]) ||
                IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.RIGHT]) ||
                IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) ||
                IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM])))

            {
                keysStatus = false;
            }

            if (state.LeftButtonAction == ButtonState.Released)
            {
                LeftButtonStatus = false;
            }

            if (state.RightButtonAction == ButtonState.Released)
            {
                RightButtonStatus = false;
            }


            if (gameState == GameState.START_GAME || gameState == GameState.CHOICE_OF_GAME_TYPE || gameState == GameState.SETTINGS_WINDOWS || gameState == GameState.SETTINGS_ANDROID)
                WallInside = false;
            else
            {
                WallInside = true;
            }

            if (RandomPowerUp.alive)
            {
                RandomPowerUp.Update(gameTime);
            }
            else
            {
                RandomPowerUp = new PowerUp(this);
                RandomPowerUp.Random();
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
                if (Environment.OSVersion.Platform == PlatformID.Unix && tank1.alive)
                    Camera.Scale = 2;
                else
                    Camera.Scale = 1;


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
                settings.raceTime -= timerWyscig;
                if (settings.raceTime < 0)
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

            if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU ||
              gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE ||
              gameState == GameState.CHOICE_OF_GAME_TYPE ||
              gameState == GameState.SETTINGS_WINDOWS ||
              gameState == GameState.SETTINGS_ANDROID ||
              gameState == GameState.START_GAME ||
              gameState == GameState.PAUSE || 
              gameState == GameState.GAME_WIN || 
              gameState == GameState.GAME_LOSS ||
              gameState == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)

            {

                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.DOWN]) || IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.UP]) || IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.RIGHT]) || IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.LEFT])) && keysStatus == false)
                {

                    if (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.DOWN]))
                    {
                        if (currentButtonX == buttonsInMemu[gameState].GetLength(0) - 1)
                            currentButtonX = 0;
                        else
                            currentButtonX++;
                    }
                    if (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.UP]))
                    {
                        if (currentButtonX == 0)
                            currentButtonX = buttonsInMemu[gameState].GetLength(0) - 1;
                        else
                            currentButtonX--;
                    }
                    if (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.RIGHT]))

                    {
                        if (currentButtonY == buttonsInMemu[gameState].GetLength(1) - 1)
                            currentButtonY = 0;
                        else
                            currentButtonY++;
                    }
                    if (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.LEFT]))
                    {
                        if (currentButtonY == 0)
                            currentButtonY = buttonsInMemu[gameState].GetLength(1) - 1;
                        else
                            currentButtonY--;
                    }
                    keysStatus = true;
                }

                if (!LeftButtonStatus && !keysStatus)
                {
                    for (int x = 0; x < buttonsInMemu[gameState].GetLength(0); x++)
                    {
                        for (int y = 0; y < buttonsInMemu[gameState].GetLength(1); y++)
                        {

                            if (buttonsInMemu[gameState][x, y].CheckIsMouseOver(ref state))
                            {
                                currentButtonX = x;
                                currentButtonY = y;
                            }
                            else
                                buttonsInMemu[gameState][x, y].IsMouseOver = false;
                        }
                    }
                }
            }




            if (gameState == GameState.PAUSE || gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS || gameState == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
            {

                soundsTanks.ToList<Sound>().ForEach((i) => { if (null != i) i.PauseSound(Sound.Sounds.ENGINE); });

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (gameState == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");

                    ButtonSettings.CenterHorizontal();

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettings) ? (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            gameState = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            gameState = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                }


                if (gameState == GameState.PAUSE)
                {

                    ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 235, (map.screenHeight / 2) - 60, (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                    ButtonNowaGra.UIElementRectangle = new Rectangle((map.screenWidth / 2) + 25, (map.screenHeight / 2) - 60 , (int)ButtonNowaGra.Width, (int)ButtonNowaGra.Height);
                    ButtonSettings.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 265, (map.screenHeight / 2) + 60, (int)ButtonSettings.Width, (int)ButtonSettings.Height);

                    ButtonKoniec.Position = new Vector2((map.screenWidth / 2) + 55, (map.screenHeight / 2) + 60);
              

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonNowaGra) ? (ButtonNowaGra.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        WallInside = false;
                        map.Reset();
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        RandomPowerUp.alive = false;
                        settings.raceTime = (float)RaceTime.Minutes_5;
                          
                            sound.PauseSound(Sound.Sounds.MENU_SOUND);
                            enemyTanks.Clear();
                            mines.Clear();
                            bullets.Clear();
                            tank1.lives = 0;
                            if (gameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1)
                                tank2.lives = 0;
                            Initialize();
                        
                    }

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettings) ? (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            gameState = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            gameState = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonKoniec) ? (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        Exit();
                    }

                    if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus) || (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPowrot) ? (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false))
                    {
                        sound.PauseSound(Sound.Sounds.MENU_SOUND);
                        gameState = gameReturn;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }


                }

                else if (gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
                {

                    if (gameReturn.Equals(GameState.GAME_RUNNING_PLAYER_1))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka1");
                    else if (gameReturn.Equals(GameState.GAME_RUNNING_PLAYERS_2))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka2");
                    else if (gameReturn.Equals(GameState.GAME_RUNNING_PLAYERS_2_AND_CPU))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka3");
                    else if (gameReturn.Equals(GameState.GAME_RUNNING_RACE))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka4");

                    sound.ResumeSound(Sound.Sounds.MENU_SOUND);



                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonNowaGra) ? (ButtonNowaGra.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        WallInside = false;
                        map.Reset();
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        RandomPowerUp.alive = false;
                        settings.raceTime = (float)RaceTime.Minutes_5;
                        if (LeftButtonStatus)
                        {
                            sound.PauseSound(Sound.Sounds.MENU_SOUND);
                            enemyTanks.Clear();
                            mines.Clear();
                            bullets.Clear();
                            tank1.lives = 0;
                            if (gameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1)
                                tank2.lives = 0;
                            Initialize();
                        }
                    }



                    ButtonKoniec.Position = new Vector2((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80);
                    ButtonKoniec.CenterHorizontal();

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonKoniec) ? (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        Exit();
                    }

                }
            }

            else if (gameState == GameState.START_GAME)
            {


                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    Exit();
                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (!LeftButtonStatus && !keysStatus)
                {

                    ButtonKoniec.Position = new Vector2((map.screenWidth / 2) - (float)(ButtonKoniec.Width / 2), (map.screenHeight / 2) + 80);
                    ButtonKoniec.CenterHorizontal();


                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonZagraj) ? (ButtonZagraj.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            gameState = GameState.CHOICE_OF_GAME_TYPE;
                        else
                        {
                            tank1 = new Tank(this, TankColors.GREEN, new Vector2(50, 50), new Vector2(3, 3), 1, 1, 1f, whiteRectangle, 1, 3, false, false, PlayerOneController);
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = GameState.GAME_RUNNING_PLAYER_1;
                        }
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                    else if(buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettings) ? (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            gameState = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            gameState = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }


                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonKoniec) ? (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        Exit();
                    }
                }
            }

            else if (gameState == GameState.SETTINGS_WINDOWS)
            {


                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus ))
                {

                    if (gameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        gameState = GameState.START_GAME;
                    }
                    else
                    {
                        tank1.TankActionProvider = PlayerOneController;
                        if(tank2 != null)
                        tank2.TankActionProvider = PlayerTwoController;
                        gameState = GameState.PAUSE;
                    }
                        BlockKeysAndMouseAndDefaultCurrentButton();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                #region Set Keyboard control for players
                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaKlawMysz) ? (ButtonSettingsTrybSterowaniaKlawMysz.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                {
                    PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                }
                else if (PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz.IsMouseOver = true;
                }

                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaKlawMysz2) ? (ButtonSettingsTrybSterowaniaKlawMysz2.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
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

                    if (AvailableGamepads.Count == 1)
                    {
                        if (PlayerOneController.IsConnectedTankController() && !PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                            PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                        else if (PlayerTwoController.IsConnectedTankController() && !PlayerTwoController.Equals(KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout))
                            PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                        else
                        {
                            PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                            PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                        }

                    }


                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaPad) ? (ButtonSettingsTrybSterowaniaPad.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        PlayerOneController = AvailableGamepads[0];
                        if (AvailableGamepads.Count == 1)
                            PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                    }
                    else if (!PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad.IsMouseOver = true;
                    }

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaPad2) ? (ButtonSettingsTrybSterowaniaPad2.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
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
                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPowrot) ? (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                {
                 
              
                    if (gameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        gameState = GameState.START_GAME;
                    }
                    else
                    {
                        tank1.TankActionProvider = PlayerOneController;
                        if (tank2 != null)
                            tank2.TankActionProvider = PlayerTwoController;
                        gameState = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }
            }

            //
            else if (gameState == GameState.SETTINGS_ANDROID)
            {


                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {

                    if (gameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        gameState = GameState.START_GAME;
                    }
                    else
                    {
                        tank1.TankActionProvider = PlayerOneController;
                        gameState = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (VirtualGamepad.DOM == VirtualGamepad.DirectionsOfMovement.BASIC)
                {
                    ButtonSettingsTrybSterowaniaBasic.IsMouseOver = true; 
                    
                }
                if (VirtualGamepad.DOM == VirtualGamepad.DirectionsOfMovement.ADVANCED)
                {
                    ButtonSettingsTrybSterowaniaAdvanced.IsMouseOver = true;              
                }

                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaBasic) ? (ButtonSettingsTrybSterowaniaBasic.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                {
                    VirtualGamepad.DOM = VirtualGamepad.DirectionsOfMovement.BASIC;
                }


                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaAdvanced) ? (ButtonSettingsTrybSterowaniaAdvanced.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                {
                    VirtualGamepad.DOM = VirtualGamepad.DirectionsOfMovement.ADVANCED;
                }




                ButtonPowrot.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 130, (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                ButtonPowrot.CenterHorizontal();
                if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPowrot) ? (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                {


                    if (gameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        gameState = GameState.START_GAME;
                    }
                    else
                    {
                        tank1.TankActionProvider = PlayerOneController;
                        gameState = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }
            }

            //-


            else if (gameState == GameState.CHOICE_OF_GAME_TYPE)
            {
                tank1 = new Tank(this, TankColors.GREEN, new Vector2(50, 50), new Vector2(3, 3), 1, 1, 1f, whiteRectangle, 1, 3, false, false, PlayerOneController);

                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    gameState = GameState.START_GAME;
                    gameReturn = GameState.START_GAME;
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }


                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;


                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (!LeftButtonStatus && !keysStatus)
                {

                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer1))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka1");
                        if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer1) ? (ButtonPlayer1.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                        {
                            if (tank2 != null)
                                tank2 = null;
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = GameState.GAME_RUNNING_PLAYER_1;
                        }
                    }
                    else if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer2))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka2");
                        if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer2) ? (ButtonPlayer2.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                        {
                            tank2 = new Tank(this, TankColors.RED, new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            map.WallBorder = randy.Next(7);
                            WallInside = true;
                            map.WallInside = randy.Next(5);
                            map.Reset();
                            settings.opponentsCPUClassic = 0;
                            settings.opponentsCPUKamikaze = 0;
                            sound.PauseSound(Sound.Sounds.MENU_SOUND);
                            gameState = GameState.GAME_RUNNING_PLAYERS_2;
                            gameReturn = GameState.GAME_RUNNING_PLAYERS_2;
                        }
                    }

                    else if(buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer3))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka3");
                        if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer3) ? (ButtonPlayer3.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                        {
                            tank2 = new Tank(this, TankColors.RED, new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = GameState.GAME_RUNNING_PLAYERS_2_AND_CPU;
                        }
                    }

                    else if(buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer4))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka4");
                        if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPlayer4) ? (ButtonPlayer4.IsClickedLeftButton(ref state) || (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                        {
                            tank2 = new Tank(this, TankColors.RED, new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            gameState = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE;
                            gameReturn = GameState.GAME_RUNNING_RACE;
                        }
                    }
                }
            }

            else if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
            {

                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {

                    gameState = GameState.CHOICE_OF_GAME_TYPE;
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                ButtondoBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 80, (int)ButtondoBoju.Width, (int)ButtondoBoju.Height);
                ButtondoBoju.CenterHorizontal();

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (!LeftButtonStatus && !keysStatus)
                {

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonCzas1Gry) ? (ButtonCzas1Gry.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.raceTime.Equals((float)RaceTime.Minutes_2))
                    {
                        ButtonCzas1Gry.IsMouseOver = true;
                        settings.raceTime = (float)RaceTime.Minutes_2;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonCzas2Gry) ? (ButtonCzas2Gry.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.raceTime.Equals((float)RaceTime.Minutes_5))
                    {
                        ButtonCzas2Gry.IsMouseOver = true;
                        settings.raceTime = (float)RaceTime.Minutes_5;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonCzas3Gry) ? (ButtonCzas3Gry.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.raceTime.Equals((float)RaceTime.Minutes_10))
                    {
                        ButtonCzas3Gry.IsMouseOver = true;
                        settings.raceTime = (float)RaceTime.Minutes_10;
                    }
                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtondoBoju) ? (ButtondoBoju.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        tank1.lives = 1;
                        tank2.lives = 1;
                        tank1.mines = 10;
                        tank2.mines = 10;
                        map.WallBorder = randy.Next(7);
                        WallInside = true;
                        map.WallInside = randy.Next(5);
                        map.Reset();
                        settings.opponentsCPUClassic = 0;
                        settings.opponentsCPUKamikaze = 0;
                        sound.PlaySound(Sound.Sounds.KLIK);
                        sound.PauseSound(Sound.Sounds.MENU_SOUND);
                        gameState = GameState.GAME_RUNNING_RACE;
                    }
                }      
            }

            else if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
            {

                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        gameState = GameState.CHOICE_OF_GAME_TYPE;
                    else
                    {
                        gameState = GameState.START_GAME;
                        gameReturn = GameState.START_GAME;
                    }
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (LeftButtonStatus == false)
                {

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPoziom1Trud) ? (ButtonPoziom1Trud.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.difficultyLevel.Equals(DifficultyLevel.Easy))
                    {
                        settings.difficultyLevel = DifficultyLevel.Easy;
                        ButtonPoziom1Trud.IsMouseOver = true;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPoziom2Trud) ? (ButtonPoziom2Trud.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.difficultyLevel.Equals(DifficultyLevel.Medium))
                    {
                        settings.difficultyLevel = DifficultyLevel.Medium;
                        ButtonPoziom2Trud.IsMouseOver = true;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPoziom3Trud) ? (ButtonPoziom3Trud.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.difficultyLevel.Equals(DifficultyLevel.Hard))
                    {
                        settings.difficultyLevel = DifficultyLevel.Hard;
                        ButtonPoziom3Trud.IsMouseOver = true;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonPoziom4Trud) ? (ButtonPoziom4Trud.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.difficultyLevel.Equals(DifficultyLevel.Impossible))
                    {
                        settings.difficultyLevel = DifficultyLevel.Impossible;
                        ButtonPoziom4Trud.IsMouseOver = true;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc0) ? (ButtonwyborCpuKlasykIlosc0.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUClassic == 0)
                    {
                        ButtonwyborCpuKlasykIlosc0.IsMouseOver = true;
                        settings.opponentsCPUClassic = 0;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc1) ? (ButtonwyborCpuKlasykIlosc1.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUClassic == 1)
                    {
                        ButtonwyborCpuKlasykIlosc1.IsMouseOver = true;
                        settings.opponentsCPUClassic = 1;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc2) ? (ButtonwyborCpuKlasykIlosc2.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUClassic == 2)
                    {
                        ButtonwyborCpuKlasykIlosc2.IsMouseOver = true;
                        settings.opponentsCPUClassic = 2;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc3) ? (ButtonwyborCpuKlasykIlosc3.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUClassic == 3)
                    {
                        ButtonwyborCpuKlasykIlosc3.IsMouseOver = true;
                        settings.opponentsCPUClassic = 3;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc0) ? (ButtonwyborCpuKlamikazeIlosc0.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUKamikaze == 0)
                    {
                        ButtonwyborCpuKlamikazeIlosc0.IsMouseOver = true;
                        settings.opponentsCPUKamikaze = 0;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc1) ? (ButtonwyborCpuKlamikazeIlosc1.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUKamikaze == 1)
                    {
                        ButtonwyborCpuKlamikazeIlosc1.IsMouseOver = true;
                        settings.opponentsCPUKamikaze = 1;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc2) ? (ButtonwyborCpuKlamikazeIlosc2.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUKamikaze == 2)
                    {
                        ButtonwyborCpuKlamikazeIlosc2.IsMouseOver = true;
                        settings.opponentsCPUKamikaze = 2;
                    }

                    if ((buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc3) ? (ButtonwyborCpuKlamikazeIlosc3.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false) || settings.opponentsCPUKamikaze == 3)
                    {
                        ButtonwyborCpuKlamikazeIlosc3.IsMouseOver = true;
                        settings.opponentsCPUKamikaze = 3;
                    }

                    ButtondoBoju.UIElementRectangle = new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 150, (int)ButtondoBoju.Width, (int)ButtondoBoju.Height);
                    ButtondoBoju.CenterHorizontal();
                    if (buttonsInMemu[gameState][currentButtonX, currentButtonY].Equals(ButtondoBoju) ? (ButtondoBoju.IsClickedLeftButton(ref state) ||  (IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && keysStatus == false)) : false)
                    {
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        Vector2 speedCPU;

                        if (settings.difficultyLevel.Equals(DifficultyLevel.Hard))
                            speedCPU = new Vector2(4, 4);
                        else
                            speedCPU = new Vector2(3, 3);

                        TankColors[] availableTankColors = new[] { TankColors.BLUE, TankColors.PINK, TankColors.YELLOW };

                        for (int i = 0; i < (settings.opponentsCPUClassic + settings.opponentsCPUKamikaze); i++)
                        {
                            enemyTanks.Add(new AI_Tank(this,
                                availableTankColors[i % availableTankColors.Length],
                                new Vector2(map.screenWidth / 2f, (int)map.screenHeight / 2f),
                                speedCPU, 0, 3 + i, 1f, whiteRectangle, 1, false, false,
                                MathHelper.WrapAngle(MathHelper.PiOver4 * 3 * i), settings.difficultyLevel, i >= settings.opponentsCPUClassic));
                        }


                        if (settings.difficultyLevel.Equals(DifficultyLevel.Hard))
                        {
                            tank1.mines = 1;
                            tank1.lives = 1;

                            if (gameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                tank2.mines = 1;
                                tank2.lives = 1;
                            }
                        }
                        if (settings.difficultyLevel.Equals(DifficultyLevel.Medium))
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


                        map.WallBorder = randy.Next(7);
                        WallInside = true;
                        map.WallInside = randy.Next(5);
                        map.Reset();
                        sound.PauseSound(Sound.Sounds.MENU_SOUND);

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

                if ((IsPressing(navigationKeys[FunctionsOfTheNavigationKeys.BACK]) && keysStatus == false) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                    sound.ResumeSound(Sound.Sounds.MENU_SOUND);
                    gameState = GameState.PAUSE;
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }

                if (gameState != GameState.PAUSE)
                {
                    map.Update(gameTime);

                    mines.ForEach(c => c.Update(gameTime));
                    mines.RemoveAll(d => !d.IsAlive);

                    bullets.ForEach(c => c.Update(gameTime));
                    bullets.RemoveAll(d => !d.IsAlive);

                    foreach (Tank tank in enemyTanks.Concat(gameReturn == Game1.GameState.GAME_RUNNING_PLAYER_1 ? new[] { tank1 } : new[] { tank1, tank2 }))
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

            if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU ||
              gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE ||
              gameState == GameState.CHOICE_OF_GAME_TYPE ||
              gameState == GameState.SETTINGS_WINDOWS ||
              gameState == GameState.SETTINGS_ANDROID ||
              gameState == GameState.START_GAME ||
              gameState == GameState.PAUSE || gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
                buttonsInMemu[gameState][currentButtonX, currentButtonY].IsMouseOver = true;


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
                Camera.Scale = 1;
                Camera.Position = Vector2.Zero;
                Camera.Center = false;
            }

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix());
            spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);

            map.Draw(spriteBatch, 0);                  
            map.Draw(spriteBatch, 1);

            if (gameState == GameState.CHOICE_OF_GAME_TYPE || gameState == GameState.PAUSE || gameState == GameState.START_GAME || gameState == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
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

                if (gameState == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED) {

                    LabelStatementAboutControllerDisconnected.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);

                }


                if (gameState == GameState.PAUSE)
                {

                    LabelprzerwaTexture.Draw(ref spriteBatch);
                    ButtonPowrot.Draw(ref spriteBatch);
                    ButtonNowaGra.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);
                    ButtonKoniec.Draw(ref spriteBatch);
                }
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }

            if (gameState == GameState.SETTINGS_WINDOWS || gameState == GameState.SETTINGS_ANDROID || gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU || gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
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

                if (gameState == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
                {
                    LabelwyborCzasGry.Draw(ref spriteBatch);
                    ButtonCzas1Gry.Draw(ref spriteBatch);
                    ButtonCzas2Gry.Draw(ref spriteBatch);
                    ButtonCzas3Gry.Draw(ref spriteBatch);

                    ButtondoBoju.Draw(ref spriteBatch);
                }

                if (gameState == GameState.SETTINGS_WINDOWS)
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

                if (gameState == GameState.SETTINGS_ANDROID)
                {
                    LabelSettingsTrybSterowania.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaBasic.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaAdvanced.Draw(ref spriteBatch);
                    ButtonPowrot.Draw(ref spriteBatch);
                }


                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);

            }

            if (gameState == GameState.GAME_WIN || gameState == GameState.GAME_LOSS)
            {
                spriteBatch.Draw(menuTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);
                ButtonNowaGra.Draw(ref spriteBatch);
                ButtonKoniec.Draw(ref spriteBatch);

                if (gameState == GameState.GAME_WIN)
                {
                    LabelwinTexture.Draw(ref spriteBatch);
                    if (gameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && tank2.lives == 0)
                        LabelSukcesPorazka1Gracza.Draw(ref spriteBatch);
                    if (tank1.lives == 0)
                        LabelSukcesPorazka2Gracza.Draw(ref spriteBatch);
                }

                if (gameState == GameState.GAME_LOSS)
                {
                    LabellossTexture.Draw(ref spriteBatch);
                }
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }




            if (gameState == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {

                map.Draw(spriteBatch, 0);

                if (RandomPowerUp.alive)
                    RandomPowerUp.Draw(spriteBatch);


                tank1.Draw(spriteBatch);
                tank2.Draw(spriteBatch);
                foreach (AI_Tank et in enemyTanks)
                {
                    et.Draw(spriteBatch);
                }
             

                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }

                foreach (Mine mine in mines)
                {
                    mine.Draw(spriteBatch);
                }

                map.Draw(spriteBatch, 1);

                scoreManager.Draw(spriteBatch);
            }
            if (gameState == GameState.GAME_RUNNING_PLAYERS_2 || gameState == GameState.GAME_RUNNING_RACE)
            {

                map.Draw(spriteBatch, 0);

                if (RandomPowerUp.alive)
                    RandomPowerUp.Draw(spriteBatch);         

                tank1.Draw(spriteBatch);
                tank2.Draw(spriteBatch);

       

                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }

                foreach (Mine mine in mines)
                {
                    mine.Draw(spriteBatch);
                }

                map.Draw(spriteBatch, 1);

                scoreManager.Draw(spriteBatch);


            }
            if (gameState == GameState.GAME_RUNNING_PLAYER_1)
            {
                Camera.Scale = Environment.OSVersion.Platform == PlatformID.Win32NT ? 1 : 2;
                Camera.Position = tank1.location;
                Camera.Center = true;
                Camera.MaxLeftTopCorner = new Point(0);
                Camera.MaxRightBottomCorner = new Point(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

                if (RandomPowerUp.alive)
                    RandomPowerUp.Draw(spriteBatch);

                map.Draw(spriteBatch, 0);

                tank1.Draw(spriteBatch);

                foreach (AI_Tank et in enemyTanks)
                {
                    et.Draw(spriteBatch);
                }

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }

                foreach (Mine mine in mines)
                {
                    mine.Draw(spriteBatch);
                }

                map.Draw(spriteBatch, 1);

                scoreManager.Draw(spriteBatch);

            }


            spriteBatch.End();
            if (Environment.OSVersion.Platform == PlatformID.Unix && gameState == GameState.GAME_RUNNING_PLAYER_1)
            {
                spriteBatch.Begin();
                VirtualGamepad.Draw(ref spriteBatch);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

