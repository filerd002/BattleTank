using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Timers;

namespace BattleTank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Map map;
        public bool WallInside = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D whiteRectangle;
        public Tank tank1;
        public Tank tank2;
		public List<AI_Tank> enemyTanks = new List<AI_Tank>();
        List<Bullet> bullets = new List<Bullet>();
        public Score scoreManager;
        Rectangle debugRect;
        Rectangle tank2DebugRect;
        private float tank1FireDelay = 0f;
        private float tank2FireDelay = 0f;
        private const float FIRE_DELAY = 0.5f;
        private float tank1TimeToBackAlive = 2f;
        private float tank2TimeToBackAlive = 2f;
        private float tankAITimeToBackAlive = 2f;
     
        private const float BACK_ALIVE_DELAY = 2f;
     
        int timer1control = 0;
        public int timer2control = 0;
        Texture2D background;
        Texture2D menu;
  
        Texture2D menuWinAndLoss;
        Texture2D win;
        Texture2D loss;
        public Sound menuSound;
        public Sound sound;
        Vector2 positionMouse;
        Texture2D ButtonPlayer1;
        Texture2D ButtonPlayer2;
        Texture2D ButtonPlayer3;
        Texture2D ButtonNowaGra;
        Texture2D ButtonPowrot;
        Texture2D ButtonKoniec;

        public PowerUp RandomPowerUp;
        string PowerUpSpriteName;
        public  float timerPowerUp = 10f;
        private float timerBarrier = 10f;

        Random randy = new Random();
       

    
     
        int typePowerUp;
        public int barrierPlayer;


        int soundOnOff = 0;

        public int gameState, gameReturn,newGame=0;
        public  int introMenu = 0, gameRunningPlayer1 = 1, gameRunningPlayers2 = 2, gameRunningPlayers2andCPU = 3, pause = 4, gameWin=5,gameLoss=6;

        SoundEffectInstance soundEffectInstance = null;

       
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
            IsMouseVisible = true;
            gameState = introMenu;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
           // UNCOMMENT NEXT THREE COMMENTS FOR FULLSCREEN
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % 48; //Makes the window size a divisor of 48 so the tiles fit more cleanly.
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % 48;
          //  graphics.PreferredBackBufferWidth = 48 * 20;
           // graphics.PreferredBackBufferHeight = 48 * 16;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight,0);
            whiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Graphics//Background");
            menu = Content.Load<Texture2D>("Graphics//BattleTank");
         
            menuWinAndLoss = Content.Load<Texture2D>("Graphics//MenuWinAndLoss");
            win = Content.Load<Texture2D>("Graphics//zwyciestwo");
            loss = Content.Load<Texture2D>("Graphics//przegrana");

     
            tank1 = new Tank(this, "Graphics//GreenTank", new Vector2(50,50), new Vector2(3, 3), 0, 1, 1f, whiteRectangle, 1, false, Keys.W, Keys.A, Keys.S, Keys.D, Keys.B);
            tank2 = new Tank(this, "Graphics//RedTank", new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle,1, false, Keys.Up, Keys.Left, Keys.Down, Keys.Right,  Keys.Decimal);
            enemyTanks.Add(new AI_Tank(this, "Graphics//PinkTank", new Vector2(graphics.PreferredBackBufferWidth - 50, 50), new Vector2(3, 3), MathHelper.Pi, 3, 1f, whiteRectangle,1, false, -MathHelper.PiOver2));
            enemyTanks.Add(new AI_Tank(this, "Graphics//YellowTank", new Vector2(50, graphics.PreferredBackBufferHeight - 50), new Vector2(3, 3), 0, 4, 1f, whiteRectangle,1, false, MathHelper.PiOver2));           
             enemyTanks.Add(new AI_Tank(this, "Graphics//BlueTank", new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 100), new Vector2(3, 3), MathHelper.Pi, 5, 1f, whiteRectangle,1, false, MathHelper.PiOver2));



       



           scoreManager = new Score(this, 10);
            debugRect = new Rectangle();
            tank2DebugRect = new Rectangle();
            sound = new Sound(this);

            menuSound = new Sound(this);       
            soundEffectInstance = menuSound.deploySound(Sound.Sounds.MENU_SOUND).CreateInstance();
            soundOnOff = 0; 


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
            ButtonPowrot = this.Content.Load<Texture2D>("Graphics//powrot");
            ButtonNowaGra = this.Content.Load<Texture2D>("Graphics//nowagra");
            ButtonKoniec = this.Content.Load<Texture2D>("Graphics//koniec");
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
          

            if (gameState == introMenu)
                WallInside = false;
            else
            {
                WallInside = true;
               


            }

            if (gameReturn == gameRunningPlayer1)
            {
                if (tank1.barrier == true|| enemyTanks[0].barrier == true || enemyTanks[1].barrier == true || enemyTanks[2].barrier == true)
                {
                    float timer2 = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    timerBarrier -= timer2;
                    if (timerBarrier < 0)
                    {

                        timer2control = 0;
                        tank1.barrier = false;
                        enemyTanks[0].barrier = false;
                        enemyTanks[1].barrier = false;
                        enemyTanks[2].barrier = false;


                        timerBarrier = 10;
                    }
                }
            }
            else if (gameReturn == gameRunningPlayers2andCPU)
            {
                if (tank1.barrier == true || tank2.barrier == true || enemyTanks[0].barrier == true || enemyTanks[1].barrier == true )
                {
                    float timer2 = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    timerBarrier -= timer2;
                    if (timerBarrier < 0)
                    {

                        timer2control = 0;

                        tank1.barrier = false;
                        tank2.barrier = false;
                        enemyTanks[0].barrier = false;
                        enemyTanks[1].barrier = false;
                     

                        timerBarrier = 10;
                    }
                }
            }
            else if (gameReturn == gameRunningPlayers2)
            {
                if (tank1.barrier == true || tank2.barrier == true )
                {
                    float timer2 = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    timerBarrier -= timer2;
                    if (timerBarrier < 0)
                    {

                        timer2control = 0;

                        tank1.barrier = false;
                        tank2.barrier = false;


                        timerBarrier = 10;
                    }
                }
            }
           



            if (timer2control == 1)
                {
                    if (barrierPlayer == 1)
                    {
                        if (gameReturn == gameRunningPlayer1)
                        {
                            enemyTanks[0].barrier = false;
                            enemyTanks[1].barrier = false;
                            enemyTanks[2].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2andCPU)
                        {
                            tank2.barrier = false;
                            enemyTanks[0].barrier = false;
                            enemyTanks[1].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2)
                            tank2.barrier = false;
                        tank1.Barrier();
                    }
                    else if (barrierPlayer == 2)
                    {
                        if (gameReturn == gameRunningPlayers2andCPU)
                        {
                            tank1.barrier = false;
                            enemyTanks[0].barrier = false;
                            enemyTanks[1].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2)
                            tank1.barrier = false;
                        tank2.Barrier();

                    }
                    else if (barrierPlayer == 3)
                    {
                        if (gameReturn == gameRunningPlayer1)
                        {
                            tank1.barrier = false;
                            enemyTanks[1].barrier = false;
                            enemyTanks[2].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2andCPU)
                        {
                            tank1.barrier = false;
                            tank2.barrier = false;
                            enemyTanks[1].barrier = false;
                            enemyTanks[2].barrier = false;
                        }
                        enemyTanks[0].Barrier();
                    }
                    else if (barrierPlayer == 4)
                    {
                        if (gameReturn == gameRunningPlayer1)
                        {
                            tank1.barrier = false;
                            enemyTanks[0].barrier = false;
                            enemyTanks[2].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2andCPU)
                        {
                            tank1.barrier = false;
                            tank2.barrier = false;
                            enemyTanks[0].barrier = false;
                            enemyTanks[2].barrier = false;
                        }
                        enemyTanks[1].Barrier();
                    }
                    else if (barrierPlayer == 5)
                    {
                        if (gameReturn == gameRunningPlayer1)
                        {
                            tank1.barrier = false;
                            enemyTanks[1].barrier = false;
                            enemyTanks[0].barrier = false;
                        }
                        else if (gameReturn == gameRunningPlayers2andCPU)
                        {
                            tank1.barrier = false;
                            tank2.barrier = false;
                            enemyTanks[1].barrier = false;
                            enemyTanks[0].barrier = false;
                        }
                        enemyTanks[2].Barrier();
                    }

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

                    typePowerUp = randy.Next(4);
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
                    }

                    RandomPowerUp = new PowerUp(this, typePowerUp, positionPowerUp, PowerUpSpriteName, whiteRectangle);
                  

                    timer1control = 1;
                }
                else if (timer1control == 1)
                {
                    RandomPowerUp.controlSound = 1;
                    RandomPowerUp =null;
                  
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
            

                MouseState state = Mouse.GetState();

            if((tank1.location.X) < 0){
                tank1.location.X = map.screenWidth;
            }
            if ((tank1.location.X) > map.screenWidth)
            {
                tank1.location.X =0;
            }
            if ((tank2.location.X) < 0)
            {
                tank2.location.X = map.screenWidth;
            }
            if ((tank2.location.X) > map.screenWidth)
            {
                tank2.location.X = 0;
            }


            if ((tank1.location.Y) < 0)
            {
                tank1.location.Y = map.screenHeight;
            }
            if ((tank1.location.Y) > map.screenHeight)
            {
                tank1.location.Y = 0;
            }
            if ((tank2.location.Y) < 0)
            {
                tank2.location.Y = map.screenHeight;
            }
            if ((tank2.location.Y) > map.screenHeight)
            {
                tank2.location.Y = 0;
            }


            foreach (AI_Tank et in enemyTanks)
            {

                if ((et.location.X) < 0)
                {
                    et.location.X = map.screenWidth;
                }
                if ((et.location.X) > map.screenWidth)
                {
                    et.location.X = 0;
                }
                if ((et.location.Y) < 0)
                {
                    et.location.Y = map.screenHeight;
                }
                if ((et.location.Y) > map.screenHeight)
                {
                    et.location.Y = 0;
                }
              
            }

           

            //gameRunningPlayer1 = 1, gameRunningPlayers2 = 2, gameRunningPlayers2andCPU


            if (gameState == gameRunningPlayer1)
            {
                if (tank1.lives == 0)
                {
                    gameState = gameLoss;
                }
                if (enemyTanks[0].lives == 0 && enemyTanks[1].lives == 0 && enemyTanks[2].lives == 0)
                    gameState = gameWin;

            }
            if (gameState == gameRunningPlayers2)
            {
                if (tank1.lives == 0)
                {
                    gameState = gameLoss;
                }
                if (tank2.lives == 0)
                {
                    gameState = gameWin;
                }
            }
            if (gameState == gameRunningPlayers2andCPU)
            {
                if (tank1.lives == 0 && tank2.lives == 0)
                {
                    gameState = gameLoss;
                }
                if (enemyTanks[0].lives == 0 && enemyTanks[1].lives == 0)
                    gameState = gameWin;

            }





            if (gameState == pause || gameState == gameWin || gameState == gameLoss) {




                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;

                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (gameState == pause)
                {



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

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 120, 250, 50)))
                    {
                     
                        win = this.Content.Load<Texture2D>("Graphics//zwyciestwo1");

                    }
                    else
                    {
                        win = this.Content.Load<Texture2D>("Graphics//zwyciestwo");
                    }
                }
                else if (gameState == gameLoss)
                {
                    soundOnOff = 0;

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 120, 250, 50)))
                    {
                       
                        loss = this.Content.Load<Texture2D>("Graphics//przegrana1");

                    }
                    else
                    {
                        loss = this.Content.Load<Texture2D>("Graphics//przegrana");
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
                        newGame = 1;
                        timerPowerUp = 10f;
                        timer1control = 0;
                        if (newGame == 1)
                        {
                            soundEffectInstance.Stop();
                            enemyTanks.Clear();
                            tank2.Die();
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

            else if (gameState == introMenu)
            {
                
               
                


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

          

                // Update our sprites position to the current cursor location

                positionMouse.X = state.X;
                positionMouse.Y = state.Y;




                var positionMouseXY = new Rectangle((int)positionMouse.X, (int)positionMouse.Y, 1, 1);


                if (newGame>7){
                 
                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50)))
                    {
                 
                        ButtonPlayer1 = this.Content.Load<Texture2D>("Graphics//playerVScpu1");
                        menu = Content.Load<Texture2D>("Graphics//BattleTank1");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
                            sound.PlaySound(Sound.Sounds.KLIK);
                            soundOnOff = 1;
                            gameState = gameRunningPlayer1;
                            gameReturn = gameRunningPlayer1;

                        }
                    }
                    else
                    {
                        ButtonPlayer1 = this.Content.Load<Texture2D>("Graphics//playerVScpu");
                    }
                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50)))
                    {
                       
                        ButtonPlayer2 = this.Content.Load<Texture2D>("Graphics//playerVSplayer1");
                        menu = Content.Load<Texture2D>("Graphics//BattleTank2");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
 
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

                    if (positionMouseXY.Intersects(new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50)))
                    {
                     
                        ButtonPlayer3 = this.Content.Load<Texture2D>("Graphics//player2VScpu1");
                        menu = Content.Load<Texture2D>("Graphics//BattleTank3");
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            map.WallBorder = randy.Next(5);
                            WallInside = true;
                            map.Reset();
                            sound.PlaySound(Sound.Sounds.KLIK);
                            soundOnOff = 1;
                            gameState = gameRunningPlayers2andCPU;
                            gameReturn = gameRunningPlayers2andCPU;
                        }
                    }
                    else
                    {
                        ButtonPlayer3 = this.Content.Load<Texture2D>("Graphics//player2VScpu");
                    }




                    newGame = 0;
                }
                newGame++;
            }
            else
            {




                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    soundOnOff = 0;
                    gameState = pause;

                }
                if (gameState != pause)
                {
                    map.Update(gameTime);

                    //Update delays
                    float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    tank1FireDelay -= timer;
                    tank2FireDelay -= timer;



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



                        if (!et.alive)
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
                    KeyboardState stateKey = Keyboard.GetState();

                    tank1.Update(stateKey, gameTime);
                    tank2.Update(stateKey, gameTime);
                    foreach (AI_Tank et in enemyTanks)
                    {
                        et.Update(stateKey, gameTime);
                    }
                    debugRect = new Rectangle((int)tank1.location.X - (tank1.tankTexture.Width / 2), (int)tank1.location.Y - (tank1.tankTexture.Height / 2), tank1.tankTexture.Width, tank1.tankTexture.Height);
                    tank2DebugRect = new Rectangle((int)tank2.location.X - (tank2.tankTexture.Width / 2), (int)tank2.location.Y - (tank2.tankTexture.Height / 2), tank2.tankTexture.Width, tank2.tankTexture.Height);

                    if (stateKey.IsKeyDown(Keys.Space) && tank1FireDelay <= 0)
                    {
                        tank1FireDelay = FIRE_DELAY;
                        for( int i = 0;i<tank1.strong; i++)
                        bullets.Add(tank1.Fire());
                
                    }
                    if (stateKey.IsKeyDown(Keys.NumPad0) && tank2FireDelay <= 0)
                    {
                        
                        tank2FireDelay = FIRE_DELAY;
                        for (int i = 0; i < tank2.strong; i++)
                            bullets.Add(tank2.Fire());
                    }


                    foreach (Bullet bullet in bullets)
                    {
                        if (bullet != null)
                        {
                            bullet.Update();
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
            if (gameState == introMenu || gameState > 0 || gameState == pause)
            {

                GraphicsDevice.Clear(Color.WhiteSmoke);

                // TODO: Add your drawing code here
                spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);

               

                map.Draw(spriteBatch);
                if (gameState == introMenu || gameState == pause)
                {
               
                    spriteBatch.Draw(menu, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);
                    if (gameState == introMenu){
                        spriteBatch.Draw(ButtonPlayer1, new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50), Color.White);
                        spriteBatch.Draw(ButtonPlayer2, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50), Color.White);
                        spriteBatch.Draw(ButtonPlayer3, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50), Color.White);
                    }
                    if (gameState == pause )
                    {
                        spriteBatch.Draw(ButtonPowrot, new Rectangle((map.screenWidth / 2) - 140, (map.screenHeight / 2) - 40, 250, 50), Color.White);
                        spriteBatch.Draw(ButtonNowaGra, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 20, 250, 50), Color.White);
                        spriteBatch.Draw(ButtonKoniec, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) + 80, 250, 50), Color.White);

                    }
                }
                if (gameState == gameWin || gameState == gameLoss) {
                    spriteBatch.Draw(menuWinAndLoss, new Rectangle((map.screenWidth / 2) - 500, (map.screenHeight / 2) - 500, 1000, 1000), Color.White);
                    spriteBatch.Draw(ButtonNowaGra, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2)+ 20, 250, 50), Color.White);
                    spriteBatch.Draw(ButtonKoniec, new Rectangle((map.screenWidth / 2) - 135, (map.screenHeight / 2) +80, 250, 50), Color.White);

                    if (gameState == gameWin) {
                        spriteBatch.Draw(win, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 120, 300, 70), Color.White);

                    }

                    if (gameState == gameLoss)
                    {
                        spriteBatch.Draw(loss, new Rectangle((map.screenWidth / 2) - 150, (map.screenHeight / 2) - 120, 300, 70), Color.White);

                    }
                }
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

                enemyTanks[2].alive = false;
                enemyTanks[2].lives = 0;
                scoreManager.Draw(spriteBatch);

                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        bullet.Draw(spriteBatch);
                    }
                }
           }
            if (gameState == gameRunningPlayers2)
            {
                //
                if(timer1control==1)
                    RandomPowerUp.Draw(spriteBatch);




                tank1.Draw(spriteBatch);
                tank2.Draw(spriteBatch);

                enemyTanks.Clear();


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
                tank2.alive = false;
                tank2.lives = 0;
                foreach (AI_Tank et in enemyTanks)
                {
                    et.Draw(spriteBatch);
                }
                scoreManager.Draw(spriteBatch);

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
    
