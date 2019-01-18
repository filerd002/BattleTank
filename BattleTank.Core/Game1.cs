
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
     

        public SpriteBatch spriteBatch;
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
         (items.Item2.Item2[1] != -2 && (!genericControllersAvailable.TrueForAll(c => !c._joystick.GetCurrentState().GetPointOfViewControllers()[0].Equals(items.Item2.Item2[1])))) ||
          (items.Item2.Item2[0] != -1 && (!genericControllersAvailable.TrueForAll(c => !c._joystick.GetCurrentState().GetButtons()[items.Item2.Item2[0]])));

#else
            return false;

#endif
        }

        int currentButtonX = 0;
        int currentButtonY = 0;

        public ITankActionProvider PlayerOneController { get; set; } = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
        public ITankActionProvider PlayerTwoController { get; set; } = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
        public List<ITankActionProvider> AvailableGamepads { get; set; } = new List<ITankActionProvider>();
        internal VirtualGamepad VirtualGamepad { get; private set; }
        public Tank Tank1 { get; set; }
        public Tank Tank2 { get; set; }
        public Map Map { get; set; }
        public bool WallInside { get; set; } = false;
        public Settings Settings { get; set; }
        public List<AI_Tank> EnemyTanks { get; set; } = new List<AI_Tank>();
        public List<Mine> Mines { get; set; } = new List<Mine>();
        public List<Bullet> Bullets { get; set; } = new List<Bullet>();
        public GraphicsDeviceManager Graphics { get; set; }
        public Dictionary<FunctionsOfTheNavigationKeys, Tuple<List<Keys>, Tuple<List<Buttons>, int[]>>> NavigationKeys { get; set; } = new Dictionary<FunctionsOfTheNavigationKeys, Tuple<List<Keys>, Tuple<List<Buttons>, int[]>>>
        {
             { FunctionsOfTheNavigationKeys.UP, Tuple.Create( new List<Keys>(){Keys.Up,Keys.W },  Tuple.Create( new List<Buttons>(){ Buttons.DPadUp, Buttons.LeftThumbstickUp }, new int[] { -1, 0 } ) )  },
             { FunctionsOfTheNavigationKeys.DOWN, Tuple.Create( new List<Keys>(){Keys.Down,Keys.S },   Tuple.Create(new List<Buttons>(){ Buttons.DPadDown, Buttons.LeftThumbstickDown }, new int[] {-1, 18000 } )  )  },
             { FunctionsOfTheNavigationKeys.LEFT, Tuple.Create( new List<Keys>(){Keys.Left,Keys.A },   Tuple.Create(new List<Buttons>(){ Buttons.DPadLeft, Buttons.LeftThumbstickLeft }, new int[] {-1, 27000 } )  )  },
             { FunctionsOfTheNavigationKeys.RIGHT, Tuple.Create( new List<Keys>(){Keys.Right, Keys.D },  Tuple.Create( new List<Buttons>(){ Buttons.DPadRight, Buttons.LeftThumbstickRight }, new int[] {-1, 9000 } )  )  },
             { FunctionsOfTheNavigationKeys.CONFIRM, Tuple.Create( new List<Keys>(){Keys.Enter },   Tuple.Create(new List<Buttons>(){ Buttons.Start, Buttons.RightStick }, new int[] { 9, -2 } )  )  },
             { FunctionsOfTheNavigationKeys.BACK, Tuple.Create( new List<Keys>(){Keys.Escape, Keys.Back },   Tuple.Create(new List<Buttons>(){ Buttons.Back, Buttons.LeftStick }, new int[] { 8, -2 } )  )  },

        };
        public Dictionary<GameState, Button[,]> ButtonsInMemu { get; set; }
        public GameState GameStateCurrent { get; set; }
        public GameState GameReturn { get; set; }
        public Camera2D Camera { get; set; }
        public Score ScoreManager { get; set; }
        public Sound SoundMenu { get; set; }
        public Sound[] SoundsTanks { get; set; }
        public PowerUp RandomPowerUp { get; set; }
        public Random Randy { get; set; } = new Random();
        public Texture2D WhiteRectangle { get; set; }

        void BlockKeysAndMouseAndDefaultCurrentButton()
        {
            keysStatus = true;
            LeftButtonStatus = true;
            RightButtonStatus = true;
            currentButtonX = 0;
            currentButtonY = 0;
        }

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
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
            Settings = new Settings();

            IsMouseVisible = false;
            GameStateCurrent = GameState.START_GAME;
            GameReturn = GameState.START_GAME;
            WhiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % Settings.ElementsOnTheWidth;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % Settings.ElementsOnTheHeight;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            Camera = new Camera2D(GraphicsDevice.PresentationParameters);

            Map = new Map(this, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight, 0, 0);
            WhiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Graphics/Background");
            menuTexture = Content.Load<Texture2D>("Graphics/Ramka");

            cursorTexture = Content.Load<Texture2D>("Graphics/cursor");

            ScoreManager = new Score(this, 10);
            SoundMenu = new Sound(this);
            SoundsTanks = new Sound[9];

            RandomPowerUp = new PowerUp(this);

            // Zainicjalizuj odłgos kliknięcia
            Button.ClickSound = Content.Load<SoundEffect>("Sounds\\klik");
            Button.Effect = Content.Load<Effect>(@"Shaders\GreyscaleEffect");



            SoundMenu.PlaySound(Sound.Sounds.MENU_SOUND);
            AvailableGamepads = GamePads.GetAllAvailableGamepads();
            base.Initialize();
            positionMouse = new Vector2(Graphics.GraphicsDevice.Viewport.Width / 2,
                                    Graphics.GraphicsDevice.Viewport.Height / 2);

            Settings.Widescreen = true;
            if (GraphicsDevice.DisplayMode.Width / (double) (GraphicsDevice.DisplayMode.Width + GraphicsDevice.DisplayMode.Height) == 4 / 7d ||
                GraphicsDevice.DisplayMode.Width / (double)(GraphicsDevice.DisplayMode.Width + GraphicsDevice.DisplayMode.Height) == 3 / 5d ||
                GraphicsDevice.DisplayMode.Width / (double)(GraphicsDevice.DisplayMode.Width + GraphicsDevice.DisplayMode.Height) == 5 / 8d ||
                GraphicsDevice.DisplayMode.Width / (double)(GraphicsDevice.DisplayMode.Width + GraphicsDevice.DisplayMode.Height) == 5 / 9d) {
                Settings.Widescreen = false;
            }

            if (Settings.Widescreen == true && Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Settings.RatioOfWidthtOfFrameToScreen = 0.75;
            }
            else {
                Settings.RatioOfWidthtOfFrameToScreen = 1;
            }
         

                LabelBattleTank = new Label("BaTtLeTaNk", new Vector2((Map.ScreenWidth / 2) - 195, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.175)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.08) );
            LabelBattleTank.CenterHorizontal();
            LabelwyborPoziomTrud = new Label("PoZiOm TrUdNoScI", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.286)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelwyborPoziomTrud.CenterHorizontal();
            LabelwyborCpuKlasyk = new Label("CpU kLaSyCzNyCh", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.13)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelwyborCpuKlasyk.CenterHorizontal();
            LabelwyborCpuKlamikaze = new Label("CpU kAmIkAzE", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.026)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelwyborCpuKlamikaze.CenterHorizontal();
            LabelwyborCzasGry = new Label("CzAs RoZgRyWkI", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.195)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelwyborCzasGry.CenterHorizontal();
            LabelSukcesPorazka1Gracza = new Label("GrAcZa 1", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.07));
            LabelSukcesPorazka1Gracza.CenterHorizontal();
            LabelSukcesPorazka2Gracza = new Label("GrAcZa 2", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.07));
            LabelSukcesPorazka2Gracza.CenterHorizontal();
            LabelSettingsTrybSterowania = new Label("TrYb StErOwAnIa", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.254)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.07));
            LabelSettingsTrybSterowania.CenterHorizontal();
            LabelTrybSterowania1Gracza = new Label("GrAcZ 1", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.17)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelTrybSterowania1Gracza.CenterHorizontal();
            LabelTrybSterowania2Gracza = new Label("GrAcZ 2", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.006)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            LabelTrybSterowania2Gracza.CenterHorizontal();
            LabelwyborTrybGryTexture = new Label("WyBoR TrYbU\n        gRy", new Vector2((Map.ScreenWidth / 2) - 160, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.208)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.1));
            LabelwyborTrybGryTexture.CenterHorizontal();
            LabelwinTexture = new Label("SuKcEs", new Vector2((Map.ScreenWidth / 2) - 150, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.182)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.075));
            LabelwinTexture.CenterHorizontal();
            LabellossTexture = new Label("PrZeGrAnA", new Vector2((Map.ScreenWidth / 2) - 150, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.182)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.075));
            LabellossTexture.CenterHorizontal();
            LabelprzerwaTexture = new Label("PrZeRwA", new Vector2((Map.ScreenWidth / 2) - 170, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.240)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.075));
            LabelprzerwaTexture.CenterHorizontal();
            LabelStatementAboutControllerDisconnected = new Label("KoNtRoLeR zOsTaL oDlAcZoNy\n       PrZeJdZ dO UsTaWiEn,\n      AbY zMiEnIc StErOwAnIe", new Vector2((Map.ScreenWidth / 2) - 300, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.208)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.13));
            LabelStatementAboutControllerDisconnected.CenterHorizontal();
            ButtonKoniec = new Button("KoNiEc", new Vector2(), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonKoniec.CenterHorizontal();
            ButtonZagraj = new Button("ZaGrAj", new Vector2(0, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.052)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonZagraj.CenterHorizontal();
            ButtonSettings = new Button("UsTaWiEnIa", new Vector2(0, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.026)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonSettings.CenterHorizontal();
            ButtonPlayer1 = new Button("GrAcZ vS cPu", new Vector2((Map.ScreenWidth / 2) - 140, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPlayer1.CenterHorizontal();
            ButtonPlayer2 = new Button("GrAcZ vS gRaCz", new Vector2((Map.ScreenWidth / 2) - 135, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.013)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPlayer2.CenterHorizontal();
            ButtonPlayer3 = new Button("2 GrAcZy Vs CpU", new Vector2((Map.ScreenWidth / 2) - 135, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.052)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPlayer3.CenterHorizontal();
            ButtonPlayer4 = new Button("WyScIg Z cZaSeM", new Vector2((Map.ScreenWidth / 2) - 135, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPlayer4.CenterHorizontal();
            ButtonSettingsTrybSterowaniaKlawMysz = new Button("KlAwIaTuRa/MySz", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * (Settings.Widescreen  ? 0.197 : 0.27)), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonSettingsTrybSterowaniaPad = new Button("GaMePaD", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * (Settings.Widescreen ? 0.073 : 0.1) ), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonSettingsTrybSterowaniaKlawMysz2 = new Button("KlAwIaTuRa/MySz", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * (Settings.Widescreen ? 0.197 : 0.27)), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.087)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonSettingsTrybSterowaniaPad2 = new Button("GaMePaD", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * (Settings.Widescreen ? 0.073 : 0.1) ), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.087)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonSettingsTrybSterowaniaBasic = new Button("PoDsTaWoWy", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.197), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.091)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonSettingsTrybSterowaniaBasic.CenterHorizontal();
            ButtonSettingsTrybSterowaniaAdvanced = new Button("ZaAwAnSoWaNy", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.197), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.026)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonSettingsTrybSterowaniaAdvanced.CenterHorizontal();
            ButtonPowrot = new Button("PoWrOt", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.091), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.052)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonNowaGra = new Button("NoWa GrA", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.091), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.026)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtonNowaGra.CenterHorizontal();
            ButtonPoziom1Trud = new Button("BaNaLnY", new Vector2((Map.ScreenWidth / 2) -  (int)(Map.ScreenWidth * (Settings.Widescreen && Environment.OSVersion.Platform == PlatformID.Win32NT ? 0.281 : 0.365)), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.201)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPoziom2Trud = new Button("PrZyZwOiTy", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * (Settings.Widescreen && Environment.OSVersion.Platform == PlatformID.Win32NT ? 0.153 : 0.200) ), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.201)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPoziom3Trud = new Button("ZaBoJcZy", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * (Settings.Widescreen && Environment.OSVersion.Platform == PlatformID.Win32NT ? 0.025 : 0.035) ), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.201)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonPoziom4Trud = new Button("SzAlOnY", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * (Settings.Widescreen && Environment.OSVersion.Platform == PlatformID.Win32NT ? 0.164 : 0.219) ), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.201)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlasykIlosc0 = new Button("0", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.164), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.04)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlasykIlosc1 = new Button("1", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.065), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.04)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlasykIlosc2 = new Button("2", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.029), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.04)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlasykIlosc3 = new Button("3", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.128), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.04)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlamikazeIlosc0 = new Button("0", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.164), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlamikazeIlosc1 = new Button("1", new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.065), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlamikazeIlosc2 = new Button("2", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.029), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonwyborCpuKlamikazeIlosc3 = new Button("3", new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.128), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonCzas1Gry = new Button("2 MiNuTy", new Vector2((Map.ScreenWidth / 2) - 125, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.117)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonCzas1Gry.CenterHorizontal();
            ButtonCzas2Gry = new Button("5 MiNuT", new Vector2((Map.ScreenWidth / 2) - 125, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.045)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonCzas2Gry.CenterHorizontal();
            ButtonCzas3Gry = new Button("10 MiNuT", new Vector2((Map.ScreenWidth / 2) - 125, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.026)), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.05));
            ButtonCzas3Gry.CenterHorizontal();
            ButtondoBoju = new Button("Do BoJu!!!", new Vector2(), null, (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen * 0.06));
            ButtondoBoju.CenterHorizontal();

            ButtonsInMemu = new Dictionary<GameState, Button[,]>
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
            // Method intentionally left empty.
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
                !(IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.UP]) ||
                IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.DOWN]) ||
                IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.LEFT]) ||
                IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.RIGHT]) ||
                IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) ||
                IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM])))

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


            if (GameStateCurrent == GameState.START_GAME || GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE || GameStateCurrent == GameState.SETTINGS_WINDOWS || GameStateCurrent == GameState.SETTINGS_ANDROID)
                WallInside = false;
            else
            {
                WallInside = true;
            }

            if (RandomPowerUp.Alive)
            {
                RandomPowerUp.Update(gameTime);
            }
            else
            {
                RandomPowerUp = new PowerUp(this);
                RandomPowerUp.Random();
            }


            foreach (Tank tankInGame in EnemyTanks.Concat(new List<Tank> { Tank1, Tank2 }).Where(d => !(d is null)))
            {
                if ((tankInGame.location.X) < 0)
                {
                    tankInGame.location.X = Map.ScreenWidth;
                }
                if ((tankInGame.location.X) > Map.ScreenWidth)
                {
                    tankInGame.location.X = 0;
                }
                if ((tankInGame.location.Y) < 0)
                {
                    tankInGame.location.Y = Map.ScreenHeight;
                }
                if ((tankInGame.location.Y) > Map.ScreenHeight)
                {
                    tankInGame.location.Y = 0;
                }

            }

            if (GameStateCurrent == GameState.GAME_RUNNING_PLAYER_1 || GameStateCurrent == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix && Tank1.Alive)
                    Camera.Scale = 2;
                else
                    Camera.Scale = 1;


                if (GameStateCurrent == GameState.GAME_RUNNING_PLAYER_1 && Tank1.Lives <= 0)
                {
                    GameStateCurrent = GameState.GAME_LOSS;
                }
                else if (GameStateCurrent == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU && Tank1.Lives <= 0 && Tank2.Lives <= 0)
                {
                    GameStateCurrent = GameState.GAME_LOSS;
                }
                else if (EnemyTanks.All((tank) => !tank.Alive && tank.Lives <= 0))
                {
                    GameStateCurrent = GameState.GAME_WIN;
                }
            }

            if (GameStateCurrent == GameState.GAME_RUNNING_PLAYERS_2)
            {
                if (Tank1.Lives == 0)
                {
                    GameStateCurrent = GameState.GAME_WIN;
                }
                if (Tank2.Lives == 0)
                {
                    GameStateCurrent = GameState.GAME_WIN;
                }
            }


            if (GameStateCurrent == GameState.GAME_RUNNING_RACE)
            {
                float timerWyscig = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                Settings.RaceTimeCurrent -= timerWyscig;
                if (Settings.RaceTimeCurrent < 0)
                {
                    if (ScoreManager.GetScore(0) > ScoreManager.GetScore(1))
                    {
                        Tank2.Lives = 0;

                    }
                    if (ScoreManager.GetScore(0) < ScoreManager.GetScore(1))
                    {
                        Tank1.Lives = 0;
                    }
                    GameStateCurrent = GameState.GAME_WIN;
                }
            }

            if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU ||
              GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE ||
              GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE ||
              GameStateCurrent == GameState.SETTINGS_WINDOWS ||
              GameStateCurrent == GameState.SETTINGS_ANDROID ||
              GameStateCurrent == GameState.START_GAME ||
              GameStateCurrent == GameState.PAUSE || 
              GameStateCurrent == GameState.GAME_WIN || 
              GameStateCurrent == GameState.GAME_LOSS ||
              GameStateCurrent == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)

            {

                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.DOWN]) || IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.UP]) || IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.RIGHT]) || IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.LEFT])) && !keysStatus)
                {

                    if (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.DOWN]))
                    {
                        if (currentButtonX == ButtonsInMemu[GameStateCurrent].GetLength(0) - 1)
                            currentButtonX = 0;
                        else
                            currentButtonX++;
                    }
                    if (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.UP]))
                    {
                        if (currentButtonX == 0)
                            currentButtonX = ButtonsInMemu[GameStateCurrent].GetLength(0) - 1;
                        else
                            currentButtonX--;
                    }
                    if (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.RIGHT]))

                    {
                        if (currentButtonY == ButtonsInMemu[GameStateCurrent].GetLength(1) - 1)
                            currentButtonY = 0;
                        else
                            currentButtonY++;
                    }
                    if (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.LEFT]))
                    {
                        if (currentButtonY == 0)
                            currentButtonY = ButtonsInMemu[GameStateCurrent].GetLength(1) - 1;
                        else
                            currentButtonY--;
                    }
                    keysStatus = true;
                }

                if (!LeftButtonStatus && !keysStatus)
                {
                    for (int x = 0; x < ButtonsInMemu[GameStateCurrent].GetLength(0); x++)
                    {
                        for (int y = 0; y < ButtonsInMemu[GameStateCurrent].GetLength(1); y++)
                        {

                            if (ButtonsInMemu[GameStateCurrent][x, y].CheckIsMouseOver(ref state))
                            {
                                currentButtonX = x;
                                currentButtonY = y;
                            }
                            else
                                ButtonsInMemu[GameStateCurrent][x, y].IsMouseOver = false;
                        }
                    }
                }
            }




            if (GameStateCurrent == GameState.PAUSE || GameStateCurrent == GameState.GAME_WIN || GameStateCurrent == GameState.GAME_LOSS || GameStateCurrent == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
            {

                SoundsTanks.ToList<Sound>().ForEach((i) => { if (null != i) i.PauseSound(Sound.Sounds.ENGINE); });

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                if (GameStateCurrent == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");

                    ButtonSettings.CenterHorizontal();

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettings) && (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            GameStateCurrent = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            GameStateCurrent = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                }


                if (GameStateCurrent == GameState.PAUSE)
                {

                    ButtonPowrot.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * (Settings.Widescreen ? 0.172 : 0.222) ), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078), (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                    ButtonNowaGra.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.018), (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * 0.078), (int)ButtonNowaGra.Width, (int)ButtonNowaGra.Height);
                    ButtonSettings.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * (Settings.Widescreen ? 0.194 : 0.244) ), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.078), (int)ButtonSettings.Width, (int)ButtonSettings.Height);

                    ButtonKoniec.Position = new Vector2((Map.ScreenWidth / 2) + (int)(Map.ScreenWidth * 0.040), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.078));
              

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonNowaGra) && (ButtonNowaGra.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        WallInside = false;
                        Map.Reset();
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        RandomPowerUp.Alive = false;
                        Settings.RaceTimeCurrent = (float)RaceTime.Minutes_5;
                          
                            SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);
                            EnemyTanks.Clear();
                            Mines.Clear();
                            Bullets.Clear();
                            Tank1.Lives = 0;
                            if (GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1)
                                Tank2.Lives = 0;
                            Initialize();
                        
                    }

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettings) && (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            GameStateCurrent = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            GameStateCurrent = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonKoniec) && (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        Exit();
                    }

                    if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus) || (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPowrot) && (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))))
                    {
                        SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);
                        GameStateCurrent = GameReturn;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }


                }

                else if (GameStateCurrent == GameState.GAME_WIN || GameStateCurrent == GameState.GAME_LOSS)
                {

                    if (GameReturn.Equals(GameState.GAME_RUNNING_PLAYER_1))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka1");
                    else if (GameReturn.Equals(GameState.GAME_RUNNING_PLAYERS_2))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka2");
                    else if (GameReturn.Equals(GameState.GAME_RUNNING_PLAYERS_2_AND_CPU))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka3");
                    else if (GameReturn.Equals(GameState.GAME_RUNNING_RACE))
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka4");

                    SoundMenu.ResumeSound(Sound.Sounds.MENU_SOUND);



                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonNowaGra) && (ButtonNowaGra.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        WallInside = false;
                        Map.Reset();
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        RandomPowerUp.Alive = false;
                        Settings.RaceTimeCurrent = (float)RaceTime.Minutes_5;
                        if (LeftButtonStatus)
                        {
                            SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);
                            EnemyTanks.Clear();
                            Mines.Clear();
                            Bullets.Clear();
                            Tank1.Lives = 0;
                            if (GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1)
                                Tank2.Lives = 0;
                            Initialize();
                        }
                    }



                    ButtonKoniec.Position = new Vector2((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.099), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.104));
                    ButtonKoniec.CenterHorizontal();

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonKoniec) && (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        Exit();
                    }

                }
            }

            else if (GameStateCurrent == GameState.START_GAME)
            {


                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    Exit();
                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;



                if (!LeftButtonStatus && !keysStatus)
                {

                    ButtonKoniec.Position = new Vector2((Map.ScreenWidth / 2) - (float)(ButtonKoniec.Width / 2), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.104));
                    ButtonKoniec.CenterHorizontal();


                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonZagraj) && (ButtonZagraj.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            GameStateCurrent = GameState.CHOICE_OF_GAME_TYPE;
                        else
                        {
                            Tank1 = new Tank(this, TankColors.GREEN, new Vector2(50, 50), new Vector2(6, 6), 1, 1, 1f, WhiteRectangle, 1, 3, false, false, PlayerOneController);
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            GameStateCurrent = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            GameReturn = GameState.GAME_RUNNING_PLAYER_1;
                        }
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }

                    else if(ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettings) && (ButtonSettings.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        AvailableGamepads = GamePads.GetAllAvailableGamepads();
                        menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            GameStateCurrent = GameState.SETTINGS_WINDOWS;
                        else if (Environment.OSVersion.Platform == PlatformID.Unix)
                            GameStateCurrent = GameState.SETTINGS_ANDROID;
                        BlockKeysAndMouseAndDefaultCurrentButton();
                    }


                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonKoniec) && (ButtonKoniec.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        Exit();
                    }
                }
            }

            else if (GameStateCurrent == GameState.SETTINGS_WINDOWS)
            {


                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus ))
                {

                    if (GameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        GameStateCurrent = GameState.START_GAME;
                    }
                    else
                    {
                        Tank1.TankActionProvider = PlayerOneController;
                        if(Tank2 != null)
                        {
                            Tank2.TankActionProvider = PlayerTwoController;
                        }

                        GameStateCurrent = GameState.PAUSE;
                    }
                        BlockKeysAndMouseAndDefaultCurrentButton();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;



                #region Set Keyboard control for players
                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaKlawMysz) && (ButtonSettingsTrybSterowaniaKlawMysz.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                {
                    PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                }
                else if (PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz.IsMouseOver = true;
                }

                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaKlawMysz2) && (ButtonSettingsTrybSterowaniaKlawMysz2.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
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


                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaPad) && (ButtonSettingsTrybSterowaniaPad.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        PlayerOneController = AvailableGamepads[0];
                        if (AvailableGamepads.Count == 1)
                            PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                    }
                    else if (!PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad.IsMouseOver = true;
                    }

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaPad2) && (ButtonSettingsTrybSterowaniaPad2.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
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

                ButtonPowrot.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - 125, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.169), (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                ButtonPowrot.CenterHorizontal();
                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPowrot) && (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                {
                 
              
                    if (GameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        GameStateCurrent = GameState.START_GAME;
                    }
                    else
                    {
                        Tank1.TankActionProvider = PlayerOneController;
                        if (Tank2 != null)
                            Tank2.TankActionProvider = PlayerTwoController;
                        GameStateCurrent = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }
            }

            //
            else if (GameStateCurrent == GameState.SETTINGS_ANDROID)
            {


                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {

                    if (GameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        GameStateCurrent = GameState.START_GAME;
                    }
                    else
                    {
                        Tank1.TankActionProvider = PlayerOneController;
                        GameStateCurrent = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }


                positionMouse.X = state.X;
                positionMouse.Y = state.Y;


                if (VirtualGamepad.DOM == VirtualGamepad.DirectionsOfMovement.BASIC)
                {
                    ButtonSettingsTrybSterowaniaBasic.IsMouseOver = true; 
                    
                }
                if (VirtualGamepad.DOM == VirtualGamepad.DirectionsOfMovement.ADVANCED)
                {
                    ButtonSettingsTrybSterowaniaAdvanced.IsMouseOver = true;              
                }

                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaBasic) && (ButtonSettingsTrybSterowaniaBasic.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                {
                    VirtualGamepad.DOM = VirtualGamepad.DirectionsOfMovement.BASIC;
                }


                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonSettingsTrybSterowaniaAdvanced) && (ButtonSettingsTrybSterowaniaAdvanced.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                {
                    VirtualGamepad.DOM = VirtualGamepad.DirectionsOfMovement.ADVANCED;
                }




                ButtonPowrot.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - 125, (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.169), (int)ButtonPowrot.Width, (int)ButtonPowrot.Height);
                ButtonPowrot.CenterHorizontal();
                if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPowrot) && (ButtonPowrot.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                {


                    if (GameReturn == GameState.START_GAME)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        GameStateCurrent = GameState.START_GAME;
                    }
                    else
                    {
                        Tank1.TankActionProvider = PlayerOneController;
                        GameStateCurrent = GameState.PAUSE;
                    }
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }
            }

            //-


            else if (GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE)
            {
                Tank1 = new Tank(this, TankColors.GREEN, new Vector2(50, 50), new Vector2(3, 3), 1, 1, 1f, WhiteRectangle, 1, 3, false, false, PlayerOneController);

                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    GameStateCurrent = GameState.START_GAME;
                    GameReturn = GameState.START_GAME;
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }


                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;



                if (!LeftButtonStatus && !keysStatus)
                {

                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer1))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka1");
                        if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer1) && (ButtonPlayer1.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                        {
                            if (Tank2 != null)
                                Tank2 = null;
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            GameStateCurrent = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            GameReturn = GameState.GAME_RUNNING_PLAYER_1;
                        }
                    }
                    else if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer2))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka2");
                        if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer2) && (ButtonPlayer2.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                        {
                            Tank2 = new Tank(this, TankColors.RED, new Vector2(Graphics.PreferredBackBufferWidth - 50, Graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, WhiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            Map.WallBorder = Randy.Next(7);
                            WallInside = true;
                            Map.WallInside = Randy.Next(5);
                            Map.Reset();
                            Settings.OpponentsCPUClassic = 0;
                            Settings.OpponentsCPUKamikaze = 0;
                            SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);
                            GameStateCurrent = GameState.GAME_RUNNING_PLAYERS_2;
                            GameReturn = GameState.GAME_RUNNING_PLAYERS_2;
                        }
                    }

                    else if(ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer3))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka3");
                        if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer3) && (ButtonPlayer3.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                        {
                            Tank2 = new Tank(this, TankColors.RED, new Vector2(Graphics.PreferredBackBufferWidth - 50, Graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, WhiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            menuTexture = Content.Load<Texture2D>("Graphics/RamkaXXL");
                            GameStateCurrent = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            GameReturn = GameState.GAME_RUNNING_PLAYERS_2_AND_CPU;
                        }
                    }

                    else if(ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer4))
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka4");
                        if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPlayer4) && (ButtonPlayer4.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                        {
                            Tank2 = new Tank(this, TankColors.RED, new Vector2(Graphics.PreferredBackBufferWidth - 50, Graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, WhiteRectangle, 1, 3, false, false, PlayerTwoController);
                            BlockKeysAndMouseAndDefaultCurrentButton();
                            GameStateCurrent = GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE;
                            GameReturn = GameState.GAME_RUNNING_RACE;
                        }
                    }
                }
            }

            else if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
            {

                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {

                    GameStateCurrent = GameState.CHOICE_OF_GAME_TYPE;
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                ButtondoBoju.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.044), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.104), (int)ButtondoBoju.Width, (int)ButtondoBoju.Height);
                ButtondoBoju.CenterHorizontal();



                if (!LeftButtonStatus && !keysStatus)
                {

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonCzas1Gry) && (ButtonCzas1Gry.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.RaceTimeCurrent.Equals((float)RaceTime.Minutes_2))
                    {
                        ButtonCzas1Gry.IsMouseOver = true;
                        Settings.RaceTimeCurrent = (float)RaceTime.Minutes_2;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonCzas2Gry) && (ButtonCzas2Gry.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.RaceTimeCurrent.Equals((float)RaceTime.Minutes_5))
                    {
                        ButtonCzas2Gry.IsMouseOver = true;
                        Settings.RaceTimeCurrent = (float)RaceTime.Minutes_5;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonCzas3Gry) && (ButtonCzas3Gry.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.RaceTimeCurrent.Equals((float)RaceTime.Minutes_10))
                    {
                        ButtonCzas3Gry.IsMouseOver = true;
                        Settings.RaceTimeCurrent = (float)RaceTime.Minutes_10;
                    }
                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtondoBoju) && (ButtondoBoju.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        Tank1.Lives = 1;
                        Tank2.Lives = 1;
                        Tank1.Mines = 10;
                        Tank2.Mines = 10;
                        Map.WallBorder = Randy.Next(7);
                        WallInside = true;
                        Map.WallInside = Randy.Next(5);
                        Map.Reset();
                        Settings.OpponentsCPUClassic = 0;
                        Settings.OpponentsCPUKamikaze = 0;
                        SoundMenu.PlaySound(Sound.Sounds.KLIK);
                        SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);
                        GameStateCurrent = GameState.GAME_RUNNING_RACE;
                    }
                }      
            }

            else if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
            {

                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        GameStateCurrent = GameState.CHOICE_OF_GAME_TYPE;
                    else
                    {
                        GameStateCurrent = GameState.START_GAME;
                        GameReturn = GameState.START_GAME;
                    }
                    menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                    BlockKeysAndMouseAndDefaultCurrentButton();

                }

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                if (!LeftButtonStatus)
                {

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPoziom1Trud) && (ButtonPoziom1Trud.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Easy))
                    {
                        Settings.DifficultyLevelCurrent = DifficultyLevel.Easy;
                        ButtonPoziom1Trud.IsMouseOver = true;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPoziom2Trud) && (ButtonPoziom2Trud.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Medium))
                    {
                        Settings.DifficultyLevelCurrent = DifficultyLevel.Medium;
                        ButtonPoziom2Trud.IsMouseOver = true;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPoziom3Trud) && (ButtonPoziom3Trud.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Hard))
                    {
                        Settings.DifficultyLevelCurrent = DifficultyLevel.Hard;
                        ButtonPoziom3Trud.IsMouseOver = true;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonPoziom4Trud) && (ButtonPoziom4Trud.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Impossible))
                    {
                        Settings.DifficultyLevelCurrent = DifficultyLevel.Impossible;
                        ButtonPoziom4Trud.IsMouseOver = true;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc0) && (ButtonwyborCpuKlasykIlosc0.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUClassic == 0)
                    {
                        ButtonwyborCpuKlasykIlosc0.IsMouseOver = true;
                        Settings.OpponentsCPUClassic = 0;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc1) && (ButtonwyborCpuKlasykIlosc1.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUClassic == 1)
                    {
                        ButtonwyborCpuKlasykIlosc1.IsMouseOver = true;
                        Settings.OpponentsCPUClassic = 1;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc2) && (ButtonwyborCpuKlasykIlosc2.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUClassic == 2)
                    {
                        ButtonwyborCpuKlasykIlosc2.IsMouseOver = true;
                        Settings.OpponentsCPUClassic = 2;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlasykIlosc3) && (ButtonwyborCpuKlasykIlosc3.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUClassic == 3)
                    {
                        ButtonwyborCpuKlasykIlosc3.IsMouseOver = true;
                        Settings.OpponentsCPUClassic = 3;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc0) && (ButtonwyborCpuKlamikazeIlosc0.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUKamikaze == 0)
                    {
                        ButtonwyborCpuKlamikazeIlosc0.IsMouseOver = true;
                        Settings.OpponentsCPUKamikaze = 0;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc1) && (ButtonwyborCpuKlamikazeIlosc1.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUKamikaze == 1)
                    {
                        ButtonwyborCpuKlamikazeIlosc1.IsMouseOver = true;
                        Settings.OpponentsCPUKamikaze = 1;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc2) && (ButtonwyborCpuKlamikazeIlosc2.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUKamikaze == 2)
                    {
                        ButtonwyborCpuKlamikazeIlosc2.IsMouseOver = true;
                        Settings.OpponentsCPUKamikaze = 2;
                    }

                    if ((ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtonwyborCpuKlamikazeIlosc3) && (ButtonwyborCpuKlamikazeIlosc3.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus))) || Settings.OpponentsCPUKamikaze == 3)
                    {
                        ButtonwyborCpuKlamikazeIlosc3.IsMouseOver = true;
                        Settings.OpponentsCPUKamikaze = 3;
                    }

                    ButtondoBoju.UIElementRectangle = new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * 0.117), (Map.ScreenHeight / 2) + (int)(Map.ScreenHeight * 0.195), (int)ButtondoBoju.Width, (int)ButtondoBoju.Height);
                    ButtondoBoju.CenterHorizontal();
                    if (ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].Equals(ButtondoBoju) && (ButtondoBoju.IsClickedLeftButton(ref state) || (IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.CONFIRM]) && !keysStatus)))
                    {
                        BlockKeysAndMouseAndDefaultCurrentButton();
                        menuTexture = Content.Load<Texture2D>("Graphics/Ramka");
                        Vector2 speedCPU;

                        if (Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Hard))
                            speedCPU = new Vector2(4, 4);
                        else
                            speedCPU = new Vector2(3, 3);

                        TankColors[] availableTankColors = new[] { TankColors.BLUE, TankColors.PINK, TankColors.YELLOW };

                        for (int i = 0; i < (Settings.OpponentsCPUClassic + Settings.OpponentsCPUKamikaze); i++)
                        {
                            EnemyTanks.Add(new AI_Tank(this,
                                availableTankColors[i % availableTankColors.Length],
                                new Vector2(Map.ScreenWidth / 2f, Map.ScreenHeight / 2f),
                                speedCPU, 0, 3 + i, 1f, WhiteRectangle, 1, false, false,
                                MathHelper.WrapAngle(MathHelper.PiOver4 * 3 * i), Settings.DifficultyLevelCurrent, i >= Settings.OpponentsCPUClassic));
                        }


                        if (Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Hard))
                        {
                            Tank1.Mines = 1;
                            Tank1.Lives = 1;

                            if (GameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                Tank2.Mines = 1;
                                Tank2.Lives = 1;
                            }
                        }
                        if (Settings.DifficultyLevelCurrent.Equals(DifficultyLevel.Medium))
                        {
                            Tank1.Mines = 3;
                            Tank1.Lives = 2;
                            Tank1.Armor = 2;
                            if (GameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                Tank2.Mines = 3;
                                Tank2.Lives = 2;
                                Tank2.Armor = 2;
                            }
                        }
                        else
                        {
                            Tank1.Mines = 5;
                            Tank1.Lives = 3;
                            Tank1.Armor = 3;
                            if (GameReturn != GameState.GAME_RUNNING_PLAYER_1)
                            {
                                Tank2.Mines = 5;
                                Tank2.Lives = 3;
                                Tank2.Armor = 3;
                            }
                        }


                        Map.WallBorder = Randy.Next(7);
                        WallInside = true;
                        Map.WallInside = Randy.Next(5);
                        Map.Reset();
                        SoundMenu.PauseSound(Sound.Sounds.MENU_SOUND);

                        if (GameReturn == GameState.GAME_RUNNING_PLAYER_1)
                        {
                            GameStateCurrent = GameState.GAME_RUNNING_PLAYER_1;

                        }


                        if (GameReturn == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
                        {
                            GameStateCurrent = GameState.GAME_RUNNING_PLAYERS_2_AND_CPU;

                        }

                    }
                }
            }



            else
            {

                if ((IsPressing(NavigationKeys[FunctionsOfTheNavigationKeys.BACK]) && !keysStatus) || (state.RightButtonAction == ButtonState.Pressed && !RightButtonStatus))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics/RamkaXL");
                    SoundMenu.ResumeSound(Sound.Sounds.MENU_SOUND);
                    GameStateCurrent = GameState.PAUSE;
                    BlockKeysAndMouseAndDefaultCurrentButton();
                }

                if (GameStateCurrent != GameState.PAUSE)
                {
                    Map.Update(gameTime);

                    Mines.ForEach(c => c.Update(gameTime));
                    Mines.RemoveAll(d => !d.IsAlive);

                    Bullets.ForEach(c => c.Update(gameTime));
                    Bullets.RemoveAll(d => !d.IsAlive);

                    foreach (Tank tank in EnemyTanks.Concat(GameReturn == Game1.GameState.GAME_RUNNING_PLAYER_1 ? new[] { Tank1 } : new[] { Tank1, Tank2 }))
                    {
                        tank.Update(gameTime);

                        if (!tank.Alive) continue;

                        if (tank.TryFire(out Bullet[] newBullets))
                        {
                            Bullets.AddRange(newBullets);
                        }

                        if (tank.TryPlantMine(out Mine mine))
                        {
                            Mines.Add(mine);
                        }
                    }
                }
            }

            if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU ||
              GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE ||
              GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE ||
              GameStateCurrent == GameState.SETTINGS_WINDOWS ||
              GameStateCurrent == GameState.SETTINGS_ANDROID ||
              GameStateCurrent == GameState.START_GAME ||
              GameStateCurrent == GameState.PAUSE || GameStateCurrent == GameState.GAME_WIN || GameStateCurrent == GameState.GAME_LOSS)
                ButtonsInMemu[GameStateCurrent][currentButtonX, currentButtonY].IsMouseOver = true;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (GameStateCurrent != GameState.GAME_RUNNING_PLAYER_1)
            {
                Camera.Scale = 1;
                Camera.Position = Vector2.Zero;
                Camera.Center = false;
            }

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix());
            spriteBatch.Draw(background, new Rectangle(0, 0, Map.ScreenWidth, Map.ScreenHeight), Color.White);

            Map.Draw(spriteBatch, 0);                  
            Map.Draw(spriteBatch, 1);

            if (GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE || GameStateCurrent == GameState.PAUSE || GameStateCurrent == GameState.START_GAME || GameStateCurrent == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED)
            {

                spriteBatch.Draw(menuTexture, new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen) /2, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen) /2, (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen), (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen)), Color.White);

                if (GameStateCurrent == GameState.START_GAME)
                {
                    LabelBattleTank.Draw(ref spriteBatch);

                    ButtonZagraj.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);
                    ButtonKoniec.Draw(ref spriteBatch);
                }


                if (GameStateCurrent == GameState.CHOICE_OF_GAME_TYPE)
                {
                    LabelwyborTrybGryTexture.Draw(ref spriteBatch);
                    ButtonPlayer1.Draw(ref spriteBatch);
                    ButtonPlayer2.Draw(ref spriteBatch);
                    ButtonPlayer3.Draw(ref spriteBatch);
                    ButtonPlayer4.Draw(ref spriteBatch);
                }

                if (GameStateCurrent == GameState.STATEMENT_ABOUT_CONTROLLER_DISCONNECTED) {

                    LabelStatementAboutControllerDisconnected.Draw(ref spriteBatch);
                    ButtonSettings.Draw(ref spriteBatch);

                }


                if (GameStateCurrent == GameState.PAUSE)
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

            if (GameStateCurrent == GameState.SETTINGS_WINDOWS || GameStateCurrent == GameState.SETTINGS_ANDROID || GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU || GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
            {
                spriteBatch.Draw(menuTexture, new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen) / 2, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen) / 2, (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen), (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen)), Color.White);


                if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
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

                if (GameStateCurrent == GameState.CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_RACE)
                {
                    LabelwyborCzasGry.Draw(ref spriteBatch);
                    ButtonCzas1Gry.Draw(ref spriteBatch);
                    ButtonCzas2Gry.Draw(ref spriteBatch);
                    ButtonCzas3Gry.Draw(ref spriteBatch);

                    ButtondoBoju.Draw(ref spriteBatch);
                }

                if (GameStateCurrent == GameState.SETTINGS_WINDOWS)
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

                if (GameStateCurrent == GameState.SETTINGS_ANDROID)
                {
                    LabelSettingsTrybSterowania.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaBasic.Draw(ref spriteBatch);
                    ButtonSettingsTrybSterowaniaAdvanced.Draw(ref spriteBatch);
                    ButtonPowrot.Draw(ref spriteBatch);
                }


                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);

            }

            if (GameStateCurrent == GameState.GAME_WIN || GameStateCurrent == GameState.GAME_LOSS)
            {
                spriteBatch.Draw(menuTexture, new Rectangle((Map.ScreenWidth / 2) - (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen) / 2, (Map.ScreenHeight / 2) - (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen) / 2, (int)(Map.ScreenWidth * Settings.RatioOfWidthtOfFrameToScreen), (int)(Map.ScreenHeight * Settings.RatioOfHeightOfFrameToScreen)), Color.White);
                ButtonNowaGra.Draw(ref spriteBatch);
                ButtonKoniec.Draw(ref spriteBatch);

                if (GameStateCurrent == GameState.GAME_WIN)
                {
                    LabelwinTexture.Draw(ref spriteBatch);
                    if (GameReturn != Game1.GameState.GAME_RUNNING_PLAYER_1 && Tank2.Lives == 0)
                        LabelSukcesPorazka1Gracza.Draw(ref spriteBatch);
                    if (Tank1.Lives == 0)
                        LabelSukcesPorazka2Gracza.Draw(ref spriteBatch);
                }

                if (GameStateCurrent == GameState.GAME_LOSS)
                {
                    LabellossTexture.Draw(ref spriteBatch);
                }
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }




            if (GameStateCurrent == GameState.GAME_RUNNING_PLAYERS_2_AND_CPU)
            {

                Map.Draw(spriteBatch, 0);

                if (RandomPowerUp.Alive)
                    RandomPowerUp.Draw(spriteBatch);


                Tank1.Draw(spriteBatch);
                Tank2.Draw(spriteBatch);
                foreach (AI_Tank et in EnemyTanks)
                {
                    et.Draw(spriteBatch);
                }
             

                foreach (Bullet bullet in Bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }

                foreach (Mine mine in Mines)
                {
                    mine.Draw(spriteBatch);
                }

                Map.Draw(spriteBatch, 1);

                ScoreManager.Draw(spriteBatch);
            }
            if (GameStateCurrent == GameState.GAME_RUNNING_PLAYERS_2 || GameStateCurrent == GameState.GAME_RUNNING_RACE)
            {

                Map.Draw(spriteBatch, 0);

                if (RandomPowerUp.Alive)
                    RandomPowerUp.Draw(spriteBatch);         

                Tank1.Draw(spriteBatch);
                Tank2.Draw(spriteBatch);

       

                foreach (Bullet bullet in Bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }

                foreach (Mine mine in Mines)
                {
                    mine.Draw(spriteBatch);
                }

                Map.Draw(spriteBatch, 1);

                ScoreManager.Draw(spriteBatch);


            }
            if (GameStateCurrent == GameState.GAME_RUNNING_PLAYER_1)
            {
                Camera.Scale = Environment.OSVersion.Platform == PlatformID.Win32NT ? 1 : 2;
                Camera.Position = Tank1.location;
                Camera.Center = true;
                Camera.MaxLeftTopCorner = new Point(0);
                Camera.MaxRightBottomCorner = new Point(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

                if (RandomPowerUp.Alive)
                    RandomPowerUp.Draw(spriteBatch);

                Map.Draw(spriteBatch, 0);

                Tank1.Draw(spriteBatch);

                foreach (AI_Tank et in EnemyTanks)
                {
                    et.Draw(spriteBatch);
                }

                foreach (Bullet bullet in Bullets)
                {
                    bullet.Draw(spriteBatch);
                }

                foreach (Mine mine in Mines)
                {
                    mine.Draw(spriteBatch);
                }

                Map.Draw(spriteBatch, 1);

                ScoreManager.Draw(spriteBatch);

            }


            spriteBatch.End();
            if (Environment.OSVersion.Platform == PlatformID.Unix && GameStateCurrent == GameState.GAME_RUNNING_PLAYER_1)
            {
                spriteBatch.Begin();
                VirtualGamepad.Draw(ref spriteBatch);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

