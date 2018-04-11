using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using BattleTank.Input;
using BattleTank.Tanks;
using BattleTank.GUI;

namespace BattleTank
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

        private float tank1TimeToBackAlive = 2f;
        private float tank2TimeToBackAlive = 2f;
        private float tankAITimeToBackAlive = 2f;
        private const float BACK_ALIVE_DELAY = 2f;

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
        Texture2D ButtonPlayer1;
        Texture2D ButtonPlayer2;
        Texture2D ButtonPlayer3;
        Texture2D ButtonPlayer4;
        Texture2D ButtonNowaGra;
        Texture2D ButtonPowrot;
        Texture2D ButtonKoniec;
        Button ButtonZagraj;
        Texture2D ButtonSettings;
        Texture2D SettingsTrybSterowania;
        Texture2D ButtonSettingsTrybSterowaniaKlawMysz;
        Texture2D ButtonSettingsTrybSterowaniaPad;
        Texture2D ButtonSettingsTrybSterowaniaKlawMysz2;
        Texture2D ButtonSettingsTrybSterowaniaPad2;
        Texture2D wyborPoziomTrud;
        Texture2D Poziom1Trud;
        Texture2D Poziom2Trud;
        Texture2D Poziom3Trud;
        Texture2D Poziom4Trud;
        Texture2D wyborCpuKlasyk;
        Texture2D wyborCpuKlasykIlosc0;
        Texture2D wyborCpuKlasykIlosc1;
        Texture2D wyborCpuKlasykIlosc2;
        Texture2D wyborCpuKlasykIlosc3;
        Texture2D wyborCpuKlamikaze;
        Texture2D wyborCpuKlamikazeIlosc0;
        Texture2D wyborCpuKlamikazeIlosc1;
        Texture2D wyborCpuKlamikazeIlosc2;
        Texture2D wyborCpuKlamikazeIlosc3;
        Texture2D Czas1Gry;
        Texture2D Czas2Gry;
        Texture2D Czas3Gry;
        Texture2D wyborCzasGry;
        Texture2D SukcesPorazka1Gracza;
        Texture2D SukcesPorazka2Gracza;
        Texture2D doBoju;

        Vector2 positionMouse;

        public Sound menuSound;
        public Sound sound;

        public PowerUp RandomPowerUp;
        string PowerUpSpriteName;
        public float timerPowerUp = 10f;
      
        Random randy = new Random();

        bool keysStatus = false;
        bool LeftButtonStatus = false;


        int typePowerUp;
       
 

        int soundOnOff = 0;

        public int poziomTrudnosci = 2;
        public int iloscCPUKlasyk = 1;
        public int iloscCPUKamikaze = 1;
        public float czasWyscigu = 300f;

        public int gameState, gameReturn;
        public int START_GAME = 0, SETTINGS = 1, CHOICE_OF_GAME_TYPE = 2, CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU = 3, CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG = 4, gameRunningPlayer1 = 5, gameRunningPlayers2 = 6, gameRunningPlayers2andCPU = 7, gameRunningWyscig = 8, pause = 9, gameWin = 10, gameLoss = 11;

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
            gameState = START_GAME;
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
            background = Content.Load<Texture2D>("Graphics//Background");
            menuTexture = Content.Load<Texture2D>("Graphics//Ramka");

            menuWinAndLossTexture = Content.Load<Texture2D>("Graphics//MenuWinAndLoss");
            BattleTankTexture = Content.Load<Texture2D>("Graphics//battleTank");
            wyborTrybGryTexture = Content.Load<Texture2D>("Graphics//wyborTrybGry");
            winTexture = Content.Load<Texture2D>("Graphics//sukces");
            lossTexture = Content.Load<Texture2D>("Graphics//przegrana");
            przerwaTexture = Content.Load<Texture2D>("Graphics//przerwa");

            cursorTexture = Content.Load<Texture2D>("Graphics//cursor");

            scoreManager = new Score(this, 10);
            debugRect = new Rectangle();
            tank2DebugRect = new Rectangle();
            sound = new Sound(this);

            menuSound = new Sound(this);
            soundEffectInstance = menuSound.deploySound(Sound.Sounds.MENU_SOUND).CreateInstance();
            soundOnOff = 0;
            AvailableGamepads = GamePads.GetAllAvailableGamepads();
            base.Initialize();
            positionMouse = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                                    graphics.GraphicsDevice.Viewport.Height / 2);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ButtonPlayer1 = this.Content.Load<Texture2D>("Graphics//playerVScpu");
            ButtonPlayer2 = this.Content.Load<Texture2D>("Graphics//playerVSplayer");
            ButtonPlayer3 = this.Content.Load<Texture2D>("Graphics//player2VScpu");
            ButtonPlayer4 = this.Content.Load<Texture2D>("Graphics//wyscig");
            ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot");
            ButtonNowaGra = this.Content.Load<Texture2D>("Graphics//nowagra");
            ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec");
            ButtonZagraj = new Button(
                Content.Load<Texture2D>("Graphics//zagraj"),
                Content.Load<Texture2D>("Graphics//zagraj1"),
                new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50));
            ButtonSettings = this.Content.Load<Texture2D>("Graphics//settings");
            SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics//trybSterowania");
            ButtonSettingsTrybSterowaniaKlawMysz = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz");
            ButtonSettingsTrybSterowaniaPad = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad");
            ButtonSettingsTrybSterowaniaKlawMysz2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz");
            ButtonSettingsTrybSterowaniaPad2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad");
            wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics//wyborPoziomTrud");
            Poziom1Trud = this.Content.Load<Texture2D>("Graphics//Poziom1Trud");
            Poziom2Trud = this.Content.Load<Texture2D>("Graphics//Poziom2Trud");
            Poziom3Trud = this.Content.Load<Texture2D>("Graphics//Poziom3Trud");
            Poziom4Trud = this.Content.Load<Texture2D>("Graphics//Poziom4Trud");
            wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics//wyborCpuKlasyk");
            wyborCpuKlasykIlosc0 = this.Content.Load<Texture2D>("Graphics//0");
            wyborCpuKlasykIlosc1 = this.Content.Load<Texture2D>("Graphics//1");
            wyborCpuKlasykIlosc2 = this.Content.Load<Texture2D>("Graphics//2");
            wyborCpuKlasykIlosc3 = this.Content.Load<Texture2D>("Graphics//3");
            wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics//wyborCpuKlamikaze");
            wyborCpuKlamikazeIlosc0 = this.Content.Load<Texture2D>("Graphics//0");
            wyborCpuKlamikazeIlosc1 = this.Content.Load<Texture2D>("Graphics//1");
            wyborCpuKlamikazeIlosc2 = this.Content.Load<Texture2D>("Graphics//2");
            wyborCpuKlamikazeIlosc3 = this.Content.Load<Texture2D>("Graphics//3");
            wyborCzasGry = this.Content.Load<Texture2D>("Graphics//wyborCzasGry");
            Czas1Gry = this.Content.Load<Texture2D>("Graphics//Czas1Gry");
            Czas2Gry = this.Content.Load<Texture2D>("Graphics//Czas2Gry");
            Czas3Gry = this.Content.Load<Texture2D>("Graphics//Czas3Gry");
            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka1Gracza");
            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka2Gracza");
            doBoju = this.Content.Load<Texture2D>("Graphics//doBoju");


            // TODO: use this.Content to load your game content here

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
            MouseState state = Mouse.GetState();



            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {

                keysStatus = false;
            }

            if (state.LeftButton == ButtonState.Released)
            {
                LeftButtonStatus = false;
            }



            if (gameState == START_GAME || gameState == CHOICE_OF_GAME_TYPE || gameState == SETTINGS)
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

                    typePowerUp = randy.Next(6);
                    switch (typePowerUp)
                    {
                        case PowerUp.HEART:
                            PowerUpSpriteName = "Graphics//PowerUpHeart";
                            break;
                        case PowerUp.ARMOR:
                            PowerUpSpriteName = "Graphics//PowerUpArmor";
                            break;
                        case PowerUp.BARRIER:
                            PowerUpSpriteName = "Graphics//PowerUpBarrier";
                            break;
                        case PowerUp.AMMO:
                            PowerUpSpriteName = "Graphics//PowerUpAmmo";
                            break;
                        case PowerUp.MINE:
                            PowerUpSpriteName = "Graphics//PowerUpMine";
                            break;
                        case PowerUp.MATRIX:
                            PowerUpSpriteName = "Graphics//PowerUpMatrix";
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

            if (gameState == gameRunningPlayer1 || gameState == gameRunningPlayers2andCPU)
            {
                if (gameState == gameRunningPlayer1 && tank1.lives <= 0)
                {
                    gameState = gameLoss;
                }
                else if (gameState == gameRunningPlayers2andCPU && tank1.lives <= 0 && tank2.lives <= 0)
                {
                    gameState = gameLoss;
                }
                else if (enemyTanks.All((tank) => !tank.alive && tank.lives <= 0))
                {
                    gameState = gameWin;
                }
            }

            if (gameState == gameRunningPlayers2)
            {
                if (tank1.lives == 0)
                {
                    gameState = gameWin;
                }
                if (tank2.lives == 0)
                {
                    gameState = gameWin;
                }
            }


            if (gameState == gameRunningWyscig)
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
                    gameState = gameWin;
                }
            }






            if (gameState == pause || gameState == gameWin || gameState == gameLoss)
            {


                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (gameState == pause)
                {

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 145, 320, 80)))
                    {

                        przerwaTexture = this.Content.Load<Texture2D>("Graphics//przerwa1");

                    }
                    else
                    {
                        przerwaTexture = this.Content.Load<Texture2D>("Graphics//przerwa");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50)))
                    {


                        ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            sound.PlaySound(Sound.Sounds.KLIK);
                            soundOnOff = 1;
                            gameState = gameReturn;
                        }
                    }
                    else
                    {
                        ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot");
                    }
                }

                else if (gameState == gameWin)
                {

                    soundOnOff = 0;

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 250, 50)))
                    {

                        winTexture = this.Content.Load<Texture2D>("Graphics//sukces1");

                    }
                    else
                    {
                        winTexture = this.Content.Load<Texture2D>("Graphics//sukces");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70)))
                    {

                        if (tank2.lives == 0)
                            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka1Gracza1");
                        if (tank1.lives == 0)
                            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka2Gracza1");

                    }
                    else
                    {

                        if (tank2.lives == 0)
                            SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka1Gracza");
                        if (tank1.lives == 0)
                            SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka2Gracza");



                    }


                }
                else if (gameState == gameLoss)
                {
                    soundOnOff = 0;

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 250, 50)))
                    {

                        lossTexture = this.Content.Load<Texture2D>("Graphics//przegrana1");

                    }
                    else
                    {
                        lossTexture = this.Content.Load<Texture2D>("Graphics//przegrana");
                    }
                }




                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50)))


                {

                    ButtonNowaGra = this.Content.Load<Texture2D>("Graphics//nowagra1");
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        sound.PlaySound(Sound.Sounds.KLIK);
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
                }
                else
                {
                    ButtonNowaGra = this.Content.Load<Texture2D>("Graphics//nowagra");
                }







                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50)))

                {

                    ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec1");
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        sound.PlaySound(Sound.Sounds.KLIK);
                        Exit();
                    }
                }
                else
                {
                    ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec");
                }


            }

            else if (gameState == START_GAME)
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

                    BattleTankTexture = this.Content.Load<Texture2D>("Graphics//battleTank1");

                }
                else
                {
                    BattleTankTexture = this.Content.Load<Texture2D>("Graphics//battleTank");
                }

                if (ButtonZagraj.IsClicked(ref state))
                {
                    
                    sound.PlaySound(Sound.Sounds.KLIK);
                    gameState = CHOICE_OF_GAME_TYPE;
                    LeftButtonStatus = true;
                        
                }





                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50)))


                    {

                        ButtonSettings = this.Content.Load<Texture2D>("Graphics//settings1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            AvailableGamepads = GamePads.GetAllAvailableGamepads();
                            menuTexture = Content.Load<Texture2D>("Graphics//RamkaXL");
                            gameState = SETTINGS;
                            sound.PlaySound(Sound.Sounds.KLIK);


                        }
                    }
                    else
                    {
                        ButtonSettings = this.Content.Load<Texture2D>("Graphics//settings");
                    }







                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50)))

                    {

                        ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            sound.PlaySound(Sound.Sounds.KLIK);
                            Exit();
                        }
                    }
                    else
                    {
                        ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec");
                    }


            }

            else if (gameState == SETTINGS)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    menuTexture = Content.Load<Texture2D>("Graphics//Ramka");
                    gameState = START_GAME;
                    keysStatus = true;


                }






                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                #region Headers
                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 195, 300, 70)))
                {
                    SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics//trybSterowania1");
                }
                else
                {
                    SettingsTrybSterowania = this.Content.Load<Texture2D>("Graphics//trybSterowania");
                }

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 130, 300, 60)))
                {
                    SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka1Gracza1");
                }
                else
                {
                    SukcesPorazka1Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka1Gracza");
                }

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 3, 300, 60)))
                {
                    SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka2Gracza1");
                }
                else
                {
                    SukcesPorazka2Gracza = this.Content.Load<Texture2D>("Graphics//SukcesPorazka2Gracza");
                }
                #endregion

                #region Set Keyboard control for players
                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) - 60, 250, 50)))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz1");
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        sound.PlaySound(Sound.Sounds.KLIK);
                        PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                    }
                }
                else if (PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz1");
                }
                else
                {
                    ButtonSettingsTrybSterowaniaKlawMysz = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz");
                }

                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) + 67, 250, 50)))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz1");
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        sound.PlaySound(Sound.Sounds.KLIK);
                        PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                    }
                }
                else if (PlayerTwoController.Equals(KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout))
                {
                    ButtonSettingsTrybSterowaniaKlawMysz2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz1");
                }
                else
                {
                    ButtonSettingsTrybSterowaniaKlawMysz2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaKlawMysz");
                }
                #endregion

                if (AvailableGamepads.Count > 0)
                {
                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 60, 250, 50)))
                    {
                        ButtonSettingsTrybSterowaniaPad = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad1");

                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            sound.PlaySound(Sound.Sounds.KLIK);
                            PlayerOneController = AvailableGamepads[0];
                            if (AvailableGamepads.Count == 1)
                                PlayerTwoController = KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout;
                        }
                    }
                    else if (!PlayerOneController.Equals(KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad1");
                    }
                    else
                    {
                        ButtonSettingsTrybSterowaniaPad = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad");
                    }
                
                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 67, 250, 50)))
                    {
                        ButtonSettingsTrybSterowaniaPad2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad1");

                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            sound.PlaySound(Sound.Sounds.KLIK);
                            if (AvailableGamepads.Count > 1)
                                PlayerTwoController = AvailableGamepads[1];
                            else if (AvailableGamepads.Count == 1)
                            {
                                PlayerOneController = KeyboardTankActionProvider.DefaultPlayerOneKeybordLayout;
                                PlayerTwoController = AvailableGamepads[0];
                            }
                        }
                    }
                    else if (!PlayerTwoController.Equals(KeyboardTankActionProvider.DefaultPlayerTwoKeybordLayout))
                    {
                        ButtonSettingsTrybSterowaniaPad2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad1");
                    }
                    else
                    {
                        ButtonSettingsTrybSterowaniaPad2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad");
                    }
                }
                else
                {
                    ButtonSettingsTrybSterowaniaPad = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad-unavailble");
                    ButtonSettingsTrybSterowaniaPad2 = this.Content.Load<Texture2D>("Graphics//trybSterowaniaPad-unavailble");
                }




                if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 130, 250, 50)))

                {

                    ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot1");
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        menuTexture = Content.Load<Texture2D>("Graphics//Ramka");
                        LeftButtonStatus = true;
                        sound.PlaySound(Sound.Sounds.KLIK);
                        gameState = START_GAME;
                    }
                }
                else
                {
                    ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot");
                }




            }


            else if (gameState == CHOICE_OF_GAME_TYPE)
            {
                // To też nie jest najlepsze miejsce na tworzenie czołgów. Ale jest to
                // tuż po wybraniu ustawień przez użytkownika. Ze względu na to aby robić
                // jak najmniej komplikacji tutaj będą one tworzone. Jednak w przyszłości należy
                // przenieśc je jeszcze bliżej samej rozgrywki
                tank1 = new Tank(this, "Graphics//GreenTank", new Vector2(50, 50), new Vector2(3, 3), 1, 1, 1f, whiteRectangle, 1, 3, false,false, PlayerOneController);
                tank2 = new Tank(this, "Graphics//RedTank", new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, 1, 3, false,false, PlayerTwoController);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && keysStatus == false)
                {

                    gameState = START_GAME;
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

                        wyborTrybGryTexture = this.Content.Load<Texture2D>("Graphics//wyborTrybGry1");

                    }
                    else
                    {
                        wyborTrybGryTexture = this.Content.Load<Texture2D>("Graphics//wyborTrybGry");
                    }






                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 60, 250, 50)))
                    {

                        ButtonPlayer1 = this.Content.Load<Texture2D>("Graphics//playerVScpu1");
                        menuTexture = Content.Load<Texture2D>("Graphics//Ramka1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            tank2.lives = 0;
                            tank2.armor = 0;
                            tank2.alive = false;
                            LeftButtonStatus = true;
                            menuTexture = Content.Load<Texture2D>("Graphics//RamkaXXL");
                            gameState = CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = gameRunningPlayer1;
                        }
                    }
                    else
                    {
                        ButtonPlayer1 = this.Content.Load<Texture2D>("Graphics//playerVScpu");
                    }
                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) - 10, 250, 50)))
                    {

                        ButtonPlayer2 = this.Content.Load<Texture2D>("Graphics//playerVSplayer1");
                        menuTexture = Content.Load<Texture2D>("Graphics//Ramka2");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            LeftButtonStatus = true;
                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
                            iloscCPUKlasyk = 0;
                            iloscCPUKamikaze = 0;
                            sound.PlaySound(Sound.Sounds.KLIK);
                            soundOnOff = 1;
                            gameState = gameRunningPlayers2;
                            gameReturn = gameRunningPlayers2;
                        }
                    }
                    else
                    {
                        ButtonPlayer2 = this.Content.Load<Texture2D>("Graphics//playerVSplayer");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 40, 250, 50)))
                    {

                        ButtonPlayer3 = this.Content.Load<Texture2D>("Graphics//player2VScpu1");
                        menuTexture = Content.Load<Texture2D>("Graphics//Ramka3");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            LeftButtonStatus = true;
                            menuTexture = Content.Load<Texture2D>("Graphics//RamkaXXL");
                            gameState = CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU;
                            gameReturn = gameRunningPlayers2andCPU;
                        }
                    }
                    else
                    {
                        ButtonPlayer3 = this.Content.Load<Texture2D>("Graphics//player2VScpu");
                    }


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 90, 250, 50)))
                    {

                        ButtonPlayer4 = this.Content.Load<Texture2D>("Graphics//wyscig1");
                        menuTexture = Content.Load<Texture2D>("Graphics//Ramka4");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            LeftButtonStatus = true;
                            gameState = CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG;
                            gameReturn = gameRunningWyscig;


                        }
                    }
                    else
                    {
                        ButtonPlayer4 = this.Content.Load<Texture2D>("Graphics//wyscig");
                    }



                }

            }

            else if (gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                    gameState = CHOICE_OF_GAME_TYPE;
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

                        wyborCzasGry = this.Content.Load<Texture2D>("Graphics//wyborCzasGry1");

                    }
                    else
                    {
                        wyborCzasGry = this.Content.Load<Texture2D>("Graphics//wyborCzasGry");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 90, 250, 50)) || czasWyscigu == 120)
                    {

                        Czas1Gry = this.Content.Load<Texture2D>("Graphics//Czas1Gry1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            czasWyscigu = 120;
                        }
                    }
                    else
                    {
                        Czas1Gry = this.Content.Load<Texture2D>("Graphics//Czas1Gry");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 35, 250, 50)) || czasWyscigu == 300)
                    {

                        Czas2Gry = this.Content.Load<Texture2D>("Graphics//Czas2Gry1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            czasWyscigu = 300;
                        }
                    }
                    else
                    {
                        Czas2Gry = this.Content.Load<Texture2D>("Graphics//Czas2Gry");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 20, 250, 50)) || czasWyscigu == 600)
                    {

                        Czas3Gry = this.Content.Load<Texture2D>("Graphics//Czas3Gry1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            czasWyscigu = 600;
                        }
                    }
                    else
                    {
                        Czas3Gry = this.Content.Load<Texture2D>("Graphics//Czas3Gry");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 80, 300, 60)))
                    {

                        doBoju = this.Content.Load<Texture2D>("Graphics//doBoju1");
                        if (state.LeftButton == ButtonState.Pressed)
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
                            gameState = gameRunningWyscig;


                        }

                    }
                    else
                    {
                        doBoju = this.Content.Load<Texture2D>("Graphics//doBoju");
                    }




                }

            }


            //

            else if (gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
            {





                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                    gameState = CHOICE_OF_GAME_TYPE;
                    keysStatus = true;
                    menuTexture = Content.Load<Texture2D>("Graphics//Ramka");


                }



                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;




                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);

                if (LeftButtonStatus == false)
                {


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 220, 300, 60)))
                    {

                        wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics//wyborPoziomTrud1");

                    }
                    else
                    {
                        wyborPoziomTrud = this.Content.Load<Texture2D>("Graphics//wyborPoziomTrud");
                    }


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 400, (map.screenHeight / 2) - 155, 250, 50)) || poziomTrudnosci == 1)
                    {

                        Poziom1Trud = this.Content.Load<Texture2D>("Graphics//Poziom1Trud1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            poziomTrudnosci = 1;
                        }

                    }
                    else
                    {
                        Poziom1Trud = this.Content.Load<Texture2D>("Graphics//Poziom1Trud");
                    }


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 210, (map.screenHeight / 2) - 155, 250, 50)) || poziomTrudnosci == 2)
                    {

                        Poziom2Trud = this.Content.Load<Texture2D>("Graphics//Poziom2Trud1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            poziomTrudnosci = 2;
                        }
                    }
                    else
                    {
                        Poziom2Trud = this.Content.Load<Texture2D>("Graphics//Poziom2Trud");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 15, (map.screenHeight / 2) - 155, 250, 50)) || poziomTrudnosci == 3)
                    {

                        Poziom3Trud = this.Content.Load<Texture2D>("Graphics//Poziom3Trud1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            poziomTrudnosci = 3;
                        }
                    }
                    else
                    {
                        Poziom3Trud = this.Content.Load<Texture2D>("Graphics//Poziom3Trud");
                    }


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 155, (map.screenHeight / 2) - 155, 250, 50)) || poziomTrudnosci == 4)
                    {

                        Poziom4Trud = this.Content.Load<Texture2D>("Graphics//Poziom4Trud1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            poziomTrudnosci = 4;
                        }
                    }
                    else
                    {
                        Poziom4Trud = this.Content.Load<Texture2D>("Graphics//Poziom4Trud");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 100, 300, 60)))
                    {

                        wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics//wyborCpuKlasyk1");

                    }
                    else
                    {
                        wyborCpuKlasyk = this.Content.Load<Texture2D>("Graphics//wyborCpuKlasyk");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) - 30, 60, 50)) || iloscCPUKlasyk == 0)
                    {

                        wyborCpuKlasykIlosc0 = this.Content.Load<Texture2D>("Graphics//01");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKlasyk = 0;
                        }
                    }
                    else
                    {
                        wyborCpuKlasykIlosc0 = this.Content.Load<Texture2D>("Graphics//0");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) - 30, 60, 50)) || iloscCPUKlasyk == 1)
                    {

                        wyborCpuKlasykIlosc1 = this.Content.Load<Texture2D>("Graphics//11");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKlasyk = 1;
                        }
                    }
                    else
                    {
                        wyborCpuKlasykIlosc1 = this.Content.Load<Texture2D>("Graphics//1");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 30, 60, 50)) || iloscCPUKlasyk == 2)
                    {

                        wyborCpuKlasykIlosc2 = this.Content.Load<Texture2D>("Graphics//21");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKlasyk = 2;
                        }
                    }
                    else
                    {
                        wyborCpuKlasykIlosc2 = this.Content.Load<Texture2D>("Graphics//2");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) - 30, 60, 50)) || iloscCPUKlasyk == 3)
                    {

                        wyborCpuKlasykIlosc3 = this.Content.Load<Texture2D>("Graphics//31");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKlasyk = 3;
                        }
                    }
                    else
                    {
                        wyborCpuKlasykIlosc3 = this.Content.Load<Texture2D>("Graphics//3");
                    }

                    //
                    //
                    //

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 20, 300, 60)))
                    {

                        wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics//wyborCpuKlamikaze1");

                    }
                    else
                    {
                        wyborCpuKlamikaze = this.Content.Load<Texture2D>("Graphics//wyborCpuKlamikaze");
                    }



                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) + 90, 60, 50)) || iloscCPUKamikaze == 0)
                    {

                        wyborCpuKlamikazeIlosc0 = this.Content.Load<Texture2D>("Graphics//01");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKamikaze = 0;
                        }
                    }
                    else
                    {
                        wyborCpuKlamikazeIlosc0 = this.Content.Load<Texture2D>("Graphics//0");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) + 90, 60, 50)) || iloscCPUKamikaze == 1)
                    {

                        wyborCpuKlamikazeIlosc1 = this.Content.Load<Texture2D>("Graphics//11");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKamikaze = 1;
                        }
                    }
                    else
                    {
                        wyborCpuKlamikazeIlosc1 = this.Content.Load<Texture2D>("Graphics//1");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 90, 60, 50)) || iloscCPUKamikaze == 2)
                    {

                        wyborCpuKlamikazeIlosc2 = this.Content.Load<Texture2D>("Graphics//21");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKamikaze = 2;
                        }
                    }
                    else
                    {
                        wyborCpuKlamikazeIlosc2 = this.Content.Load<Texture2D>("Graphics//2");
                    }

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) + 90, 60, 50)) || iloscCPUKamikaze == 3)
                    {

                        wyborCpuKlamikazeIlosc3 = this.Content.Load<Texture2D>("Graphics//31");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            iloscCPUKamikaze = 3;
                        }
                    }
                    else
                    {
                        wyborCpuKlamikazeIlosc3 = this.Content.Load<Texture2D>("Graphics//3");
                    }


                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 150, 300, 60)))
                    {

                        doBoju = this.Content.Load<Texture2D>("Graphics//doBoju1");

                        if (state.LeftButton == ButtonState.Pressed && ((iloscCPUKamikaze + iloscCPUKlasyk) > 0))
                        {
                            LeftButtonStatus = true;
                            menuTexture = Content.Load<Texture2D>("Graphics//Ramka");
                            Vector2 speedCPU;

                            if (poziomTrudnosci == 3)
                                speedCPU = new Vector2(4, 4);
                            else
                                speedCPU = new Vector2(3, 3);




                            if (iloscCPUKlasyk == 1 && iloscCPUKamikaze == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false,false, MathHelper.Pi, poziomTrudnosci, false));
                            }
                            else if (iloscCPUKlasyk == 1 && iloscCPUKamikaze == 1)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 4, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 1 && iloscCPUKamikaze == 2)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 4, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 5, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 1 && iloscCPUKamikaze == 3)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 4, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 5, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 6, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, true));
                            }

                            else if (iloscCPUKlasyk == 2 && iloscCPUKamikaze == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                            }
                            else if (iloscCPUKlasyk == 2 && iloscCPUKamikaze == 1)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 2 && iloscCPUKamikaze == 2)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 6, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 2 && iloscCPUKamikaze == 3)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 6, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 7, 1f, whiteRectangle, 1, false, false, -(MathHelper.Pi - MathHelper.PiOver4), poziomTrudnosci, true));
                            }

                            else if (iloscCPUKlasyk == 3 && iloscCPUKamikaze == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                            }
                            else if (iloscCPUKlasyk == 3 && iloscCPUKamikaze == 1)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 6, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 3 && iloscCPUKamikaze == 2)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 6, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 7, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver4, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKlasyk == 3 && iloscCPUKamikaze == 3)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, MathHelper.Pi - MathHelper.PiOver4, poziomTrudnosci, false));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 6, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 7, 1f, whiteRectangle, 1, false, false, 0, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 8, 1f, whiteRectangle, 1, false, false, -(MathHelper.Pi - MathHelper.PiOver4), poziomTrudnosci, true));
                            }
                            else if (iloscCPUKamikaze == 3 && iloscCPUKlasyk == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 5, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKamikaze == 2 && iloscCPUKlasyk == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, -MathHelper.PiOver2, poziomTrudnosci, true));
                                enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, 0, 4, 1f, whiteRectangle, 1, false, false, MathHelper.PiOver2, poziomTrudnosci, true));
                            }
                            else if (iloscCPUKamikaze == 1 && iloscCPUKlasyk == 0)
                            {
                                enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), speedCPU, MathHelper.Pi, 3, 1f, whiteRectangle, 1, false, false, MathHelper.Pi, poziomTrudnosci, true));
                            }


                            if (poziomTrudnosci == 3)
                            {
                                tank1.mines = 1;
                                tank1.lives = 1;

                                if (gameReturn != gameRunningPlayer1)
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
                                if (gameReturn != gameRunningPlayer1)
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
                                if (gameReturn != gameRunningPlayer1)
                                {
                                    tank2.mines = 5;
                                    tank2.lives = 3;
                                    tank2.armor = 3;
                                }
                            }





                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
                            sound.PlaySound(Sound.Sounds.KLIK);
                            soundOnOff = 1;

                            if (gameReturn == gameRunningPlayer1)
                            {
                                gameState = gameRunningPlayer1;

                            }


                            if (gameReturn == gameRunningPlayers2andCPU)
                            {
                                gameState = gameRunningPlayers2andCPU;

                            }

                        }
                    }
                    else
                    {
                        doBoju = this.Content.Load<Texture2D>("Graphics//doBoju");
                    }

                }

            }

            else
            {




                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    soundOnOff = 0;
                    gameState = pause;

                }
                if (gameState != pause)
                {
                    map.Update(gameTime);

                    //Update delays
                    float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

                    //if tanks are dead, decrease their time until they respawn
                    if (!tank1.alive)
                    {
                        tank1TimeToBackAlive -= timer;
                        if (tank1TimeToBackAlive < 0)
                        {

                            tank1.colliding = true;
                            while (tank1.colliding)
                            {
                                int startingLocationX = randy.Next(100, graphics.PreferredBackBufferWidth - 100);
                                int startingLocationY = randy.Next(100, graphics.PreferredBackBufferHeight - 100);

                                tank1.startingLocation = new Vector2(startingLocationX, startingLocationY);

                                Rectangle startingtankRect = new Rectangle(startingLocationX, startingLocationY, 32, 32);




                                tank1.colliding = false;
                                foreach (Tile[] tiles in map.map)
                                {
                                    foreach (Tile tile in tiles)
                                    {
                                        if (tile != null)
                                        {
                                            if ((tile.isColliding(startingtankRect).depth > 0))
                                            {
                                                tank1.colliding = true;


                                            }

                                        }
                                        else { continue; }

                                    }
                                }


                            }
                            tank1.Respawn(tank1.startingLocation);
                            tank1TimeToBackAlive = BACK_ALIVE_DELAY;
                        }
                    }
                    if (!tank2.alive)
                    {
                        tank2TimeToBackAlive -= timer;
                        if (tank2TimeToBackAlive < 0)
                        {

                            tank2.colliding = true;
                            while (tank2.colliding)
                            {
                                int startingLocationX = randy.Next(100, graphics.PreferredBackBufferWidth - 100);
                                int startingLocationY = randy.Next(100, graphics.PreferredBackBufferHeight - 100);

                                tank2.startingLocation = new Vector2(startingLocationX, startingLocationY);

                                Rectangle startingtankRect = new Rectangle(startingLocationX, startingLocationY, 32, 32);




                                tank2.colliding = false;
                                foreach (Tile[] tiles in map.map)
                                {
                                    foreach (Tile tile in tiles)
                                    {
                                        if (tile != null)
                                        {
                                            if ((tile.isColliding(startingtankRect).depth > 0))
                                            {
                                                tank1.colliding = true;


                                            }

                                        }
                                        else { continue; }

                                    }
                                }


                            }
                            tank2.Respawn(tank2.startingLocation);
                            tank2TimeToBackAlive = BACK_ALIVE_DELAY;
                        }
                    }



                    foreach (AI_Tank et in enemyTanks)
                    {



                        if (!et.alive && et.lives > 0)
                        {
                            tankAITimeToBackAlive -= timer;
                            if (tankAITimeToBackAlive < 0)
                            {

                                et.colliding = true;
                                while (et.colliding)
                                {
                                    int startingLocationX = randy.Next(100, graphics.PreferredBackBufferWidth - 100);
                                    int startingLocationY = randy.Next(100, graphics.PreferredBackBufferHeight - 100);

                                    et.startingLocation = new Vector2(startingLocationX, startingLocationY);

                                    Rectangle startingtankRect = new Rectangle(startingLocationX, startingLocationY, 32, 32);




                                    et.colliding = false;
                                    foreach (Tile[] tiles in map.map)
                                    {
                                        foreach (Tile tile in tiles)
                                        {
                                            if (tile != null)
                                            {
                                                if ((tile.isColliding(startingtankRect).depth > 0))
                                                {
                                                    et.colliding = true;


                                                }

                                            }
                                            else { continue; }

                                        }
                                    }


                                }

                                et.Respawn(et.startingLocation);
                                tankAITimeToBackAlive = BACK_ALIVE_DELAY;
                            }
                        }
                    }

                    // TODO: Add your update logic here
                    tank1.Update(gameTime);
                    tank2.Update(gameTime);

                    enemyTanks.ForEach(c => c.Update(gameTime));

                    mines.ForEach(c => c.Update());
                    bullets.ForEach(c => c?.Update());

                    if (tank1.TryFire(out Bullet[] newBullets))
                    {
                        bullets.AddRange(newBullets);
                    }
                    if (tank2.TryFire(out newBullets))
                    {
                        bullets.AddRange(newBullets);
                    }

                    if (tank1.TryPlantMine(out Mine mine))
                    {
                        mines.Add(mine);
                    }
                    if (tank2.TryPlantMine(out mine))
                    {
                        mines.Add(mine);
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

            if (gameState == CHOICE_OF_GAME_TYPE || gameState == pause || gameState == START_GAME)
            {

                spriteBatch.Draw(menuTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);

                if (gameState == START_GAME)
                {
                    spriteBatch.Draw(BattleTankTexture, new Rectangle((map.screenWidth / 2) - 195, (map.screenHeight / 2) - 145, 380, 100), Color.White);

                    ButtonZagraj.Draw(ref spriteBatch);
                    spriteBatch.Draw(ButtonSettings, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonKoniec, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50), Color.White);
                }


                if (gameState == CHOICE_OF_GAME_TYPE)
                {
                    spriteBatch.Draw(wyborTrybGryTexture, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 145, 300, 70), Color.White);

                    spriteBatch.Draw(ButtonPlayer1, new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 60, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonPlayer2, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) - 10, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonPlayer3, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 40, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonPlayer4, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 90, 250, 50), Color.White);
                }



                if (gameState == pause)
                {

                    spriteBatch.Draw(przerwaTexture, new Rectangle((map.screenWidth / 2) - 170, (map.screenHeight / 2) - 145, 320, 80), Color.White);

                    spriteBatch.Draw(ButtonPowrot, new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonNowaGra, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonKoniec, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50), Color.White);
                }
                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }

            if (gameState == SETTINGS || gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU || gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
            {
                spriteBatch.Draw(menuTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);


                if (gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_CPU)
                {


                    spriteBatch.Draw(wyborPoziomTrud, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 220, 300, 60), Color.White);
                    spriteBatch.Draw(Poziom1Trud, new Rectangle((map.screenWidth / 2) - 400, (map.screenHeight / 2) - 155, 250, 50), Color.White);
                    spriteBatch.Draw(Poziom2Trud, new Rectangle((map.screenWidth / 2) - 210, (map.screenHeight / 2) - 155, 250, 50), Color.White);
                    spriteBatch.Draw(Poziom3Trud, new Rectangle((map.screenWidth / 2) - 15, (map.screenHeight / 2) - 155, 250, 50), Color.White);
                    spriteBatch.Draw(Poziom4Trud, new Rectangle((map.screenWidth / 2) + 155, (map.screenHeight / 2) - 155, 250, 50), Color.White);

                    spriteBatch.Draw(wyborCpuKlasyk, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 100, 300, 60), Color.White);

                    spriteBatch.Draw(wyborCpuKlasykIlosc0, new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) - 30, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlasykIlosc1, new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) - 30, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlasykIlosc2, new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 30, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlasykIlosc3, new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) - 30, 60, 50), Color.White);

                    spriteBatch.Draw(wyborCpuKlamikaze, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 20, 300, 60), Color.White);

                    spriteBatch.Draw(wyborCpuKlamikazeIlosc0, new Rectangle((map.screenWidth / 2) - 225, (map.screenHeight / 2) + 90, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlamikazeIlosc1, new Rectangle((map.screenWidth / 2) - 90, (map.screenHeight / 2) + 90, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlamikazeIlosc2, new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 90, 60, 50), Color.White);
                    spriteBatch.Draw(wyborCpuKlamikazeIlosc3, new Rectangle((map.screenWidth / 2) + 175, (map.screenHeight / 2) + 90, 60, 50), Color.White);



                    spriteBatch.Draw(doBoju, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 150, 300, 60), Color.White);


                }

                if (gameState == CHOICE_OF_BATTLE_SETTINGS_GAME_TYPE_WYSCIG)
                {

                    spriteBatch.Draw(wyborCzasGry, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 150, 300, 60), Color.White);

                    spriteBatch.Draw(Czas1Gry, new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 90, 250, 50), Color.White);
                    spriteBatch.Draw(Czas2Gry, new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) - 35, 250, 50), Color.White);
                    spriteBatch.Draw(Czas3Gry, new Rectangle((map.screenWidth / 2) - 125, (map.screenHeight / 2) + 20, 250, 50), Color.White);

                    spriteBatch.Draw(doBoju, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) + 80, 300, 60), Color.White);

                }

                if (gameState == SETTINGS)
                {
                    spriteBatch.Draw(SettingsTrybSterowania, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 195, 300, 70), Color.White);
                    spriteBatch.Draw(SukcesPorazka1Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 130, 300, 60), Color.White);

                    spriteBatch.Draw(ButtonSettingsTrybSterowaniaKlawMysz, new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) - 60, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonSettingsTrybSterowaniaPad, new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) - 60, 250, 50), Color.White);

                    spriteBatch.Draw(SukcesPorazka2Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 3, 300, 60), Color.White);

                    spriteBatch.Draw(ButtonSettingsTrybSterowaniaKlawMysz2, new Rectangle((map.screenWidth / 2) - 240, (map.screenHeight / 2) + 67, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonSettingsTrybSterowaniaPad2, new Rectangle((map.screenWidth / 2) + 40, (map.screenHeight / 2) + 67, 250, 50), Color.White);



                    spriteBatch.Draw(ButtonPowrot, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 130, 250, 50), Color.White);

                }


                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);

            }

            if (gameState == gameWin || gameState == gameLoss)
            {
                spriteBatch.Draw(menuWinAndLossTexture, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);
                spriteBatch.Draw(ButtonNowaGra, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50), Color.White);
                spriteBatch.Draw(ButtonKoniec, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50), Color.White);

                if (gameState == gameWin)
                {
                    spriteBatch.Draw(winTexture, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 300, 70), Color.White);
                    if (tank2.lives == 0)
                        spriteBatch.Draw(SukcesPorazka1Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70), Color.White);
                    if (tank1.lives == 0)
                        spriteBatch.Draw(SukcesPorazka2Gracza, new Rectangle((map.screenWidth / 2) - 160, (map.screenHeight / 2) - 60, 300, 70), Color.White);

                }

                if (gameState == gameLoss)
                {
                    spriteBatch.Draw(lossTexture, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 140, 300, 70), Color.White);

                }
                spriteBatch.Draw(cursorTexture, new Vector2(positionMouse.X - 8, positionMouse.Y - 20), Color.White);
            }




            if (gameState == gameRunningPlayers2andCPU)
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
            if (gameState == gameRunningPlayers2 || gameState == gameRunningWyscig)
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
            if (gameState == gameRunningPlayer1)
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

