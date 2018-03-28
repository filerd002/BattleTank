using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BattleTank
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
        public Keys keyUp;
        public Keys keyLeft;
        public Keys keyDown;
        public Keys keyRight;
        public Keys keyBoost;
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

        //generic constructor
        public Tank()
        {

        }

        //overloaded constructor(s)
        public Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle,int _strong,int _mines, bool _barrier, Keys _keyUp, Keys _keyLeft, Keys _keyDown, Keys _keyRight, Keys _keyBoost)
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
            keyUp = _keyUp;
            keyLeft = _keyLeft;
            keyDown = _keyDown;
            keyRight = _keyRight;
            keyBoost = _keyBoost;     
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
            if(barrier)
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
        public virtual void Update(KeyboardState state, GameTime gameTime)
        {
            if (alive)
            {
                if(!frozen)
               { 
                Move(state);
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

          
            } else
			{
              
            }
            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null)
            {
                hitParticles.Update(gameTime);
            }


        }
        public virtual void Move(KeyboardState state)
        {
           
            bool RIGHT_down = state.IsKeyDown(keyRight);
            bool DOWN_down = state.IsKeyDown(keyDown);
            bool LEFT_down = state.IsKeyDown(keyLeft);
            bool UP_down = state.IsKeyDown(keyUp);
            bool BOOST_down = state.IsKeyDown(keyBoost);
            bool isPressedLeft = false;
            bool isPressedRight = false;
            bool isDrifting = false;

            //isPressed keyUp statements
            if (!RIGHT_down)
            {
                isPressedRight = false;
            }

            if (!LEFT_down)
            {
                isPressedLeft = false;
            }

            if (!LEFT_down || !RIGHT_down)
            {
                isDrifting = false;
            }

        
            if (isPressedRight && UP_down && LEFT_down)
            {
                isDrifting = true;
                Rotate(UP_RIGHT);
                MoveUp(BOOST_down);

            } else if (isPressedRight && DOWN_down && LEFT_down)
            {
                isDrifting = true;
                Rotate(DOWN_RIGHT);
                MoveDown(BOOST_down);
            }

            if (isPressedLeft && UP_down)
            {
                isDrifting = true;
                Rotate(UP_LEFT);
                MoveUp(BOOST_down);

            } else if (isPressedLeft && DOWN_down)
            {
                isDrifting = true;
                Rotate(DOWN_LEFT);
                MoveDown(BOOST_down);
            }

    
            if (UP_down && !isDrifting)
            {
                Rotate(UP);
                MoveUp(BOOST_down);
                if (RIGHT_down && !BOOST_down)
                {
                    Rotate(UP_RIGHT);
                    MoveRight(BOOST_down);
                }
                if (LEFT_down && !BOOST_down)
                {
                    Rotate(UP_LEFT);
                    MoveLeft(BOOST_down);
                }
            }
            else if (DOWN_down && !isDrifting)
            {
                Rotate(DOWN);
                MoveDown(BOOST_down);
                if (RIGHT_down && !BOOST_down)
                {
                    Rotate(DOWN_RIGHT);
                    MoveRight(BOOST_down);
                }
                if (LEFT_down && !BOOST_down)
                {
                    Rotate(DOWN_LEFT);
                    MoveLeft(BOOST_down);
                }
            }
            else if (RIGHT_down && !isDrifting)
            {
                Rotate(RIGHT);
                MoveRight(BOOST_down);
                    if (!isPressedLeft)
                    {
                        isPressedRight = true;
                    }
            }
            else if (LEFT_down && !isDrifting)
            {
                Rotate(LEFT);
                MoveLeft(BOOST_down);
                if (!isPressedRight)
                {
                        isPressedLeft = true;
                }
            }
            
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
        public Bullet Fire()
        {
            game.sound.PlaySound(Sound.Sounds.SHOT);
            if (alive)
            {
                Color color = Color.Blue;

           

                if (player == 1)
                    color = Color.Green;
                if(player == 2)
                    color = Color.Red;



          
                if (rotation == UP)
                {

                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y, 5, 5), new Vector2(0, -20), color, player, UP, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y, 5, 20));
                }
                else if (rotation == UP_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 2, 5, 5), new Vector2(10, -10), color, player, UP_RIGHT, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 2, 20, 20));
                }
                else if (rotation == RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 5, (int)location.Y - 2, 5, 5), new Vector2(20, 0), color, player, RIGHT, whiteRectangle, new Rectangle((int)location.X - 5, (int)location.Y - 2, 20, 5));
                }
                else if (rotation == DOWN_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y, 5, 5), new Vector2(10, 10), color, player, DOWN_RIGHT, whiteRectangle, new Rectangle((int)location.X, (int)location.Y, 20, 20));
                }
                else if (rotation == DOWN)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 5, 5, 5), new Vector2(0, 20), color, player, DOWN, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 5, 5, 20));
                }
                else if (rotation == DOWN_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 2, (int)location.Y - 2, 5, 5), new Vector2(-10, 10), color, player, DOWN_LEFT, whiteRectangle, new Rectangle((int)location.X - 2, (int)location.Y - 2, 20, 20));
                }
                else if (rotation == LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y - 2, 5, 5), new Vector2(-20, 0), color, player, LEFT, whiteRectangle, new Rectangle((int)location.X, (int)location.Y - 2, 20, 5));
                }
                else if (rotation == UP_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X - 3, (int)location.Y - 3, 5, 5), new Vector2(-10, -10), color, player, UP, whiteRectangle, new Rectangle((int)location.X - 3, (int)location.Y - 3, 20, 20));
                }
                else
                {
                    return null;
                }
            
            }
            return null;
            
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
            if(armor <  1)
            {
                Die();
            } else {
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
         
            if (!alive&&lives>0)
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
