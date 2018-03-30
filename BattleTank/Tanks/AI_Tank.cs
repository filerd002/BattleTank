using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Tanks
{
    public class AI_Tank : Tank
    {
        public int Level_AI;  //Można wykorzystać do ustawienia poziomu trudności
        Vector2 initSpeed = new Vector2();
        public float targetDirection;
        public new bool enemy = true;
        public List<Bullet> enemyBullets = new List<Bullet>();
        private float delayOfFire = 1;
        private const float FIRE_DELAY = 1;
        public bool KamikazeMode =false;


        public AI_Tank() { }
        public AI_Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle,int _strong, bool _barrier, int _Level_AI, float _targetDirection)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            location = _location;
            startingLocation = _location;
            speed = _speed;
            initSpeed = speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = _scale;
            whiteRectangle = _whiteRectangle;
            strong = _strong;
            barrier = _barrier;
            alive = true;
            lives = 3;
            armor = 3;
            respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
            targetDirection = _targetDirection;
            Level_AI = _Level_AI;

        }
        public AI_Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle, int _strong, bool _barrier, float _targetDirection, int _Level_AI, bool _KamikazeMode)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            location = _location;
            startingLocation = _location;
            speed = _speed;
            initSpeed = speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = _scale;
           

            whiteRectangle = _whiteRectangle;
            strong = _strong;
            barrier = _barrier;
            alive = true;
            lives = 3;
            armor = 3;
            respawnParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            deathParticles = new Particlecloud(location, game, player, whiteRectangle, Color.Gray, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
            targetDirection = _targetDirection;
            KamikazeMode = _KamikazeMode;
            if (KamikazeMode) {
                initSpeed = new Vector2(4, 4);
            }
            Level_AI = _Level_AI;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {


            base.Draw(spriteBatch);
            foreach (Bullet b in enemyBullets)
            {
                if (b != null && b.alive)
                {
                    b.Draw(spriteBatch);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {

            float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            delayOfFire -= timer;
            if (delayOfFire <= 0)
            {
                if (!frozen)
                {
                    enemyBullets.AddRange(Fire());
                }
                delayOfFire = FIRE_DELAY;
            }
            base.Update(gameTime);
            foreach (Bullet b in enemyBullets)
            {
                if (b != null && b.alive)
                {
                    b.Update();
                }
            }
        }
        public override void Move()
        {
            if (colliding)
            {
                switch ((int)targetDirection)
                {
                    case (int)UP:

                        targetDirection = RIGHT;
                        break;
                    case (int)RIGHT:
                        targetDirection = DOWN;
                        break;
                    case (int)LEFT:
                        targetDirection = UP;
                        break;
                    case (int)DOWN:
                        targetDirection = LEFT;
                        break;        
                    case (int)UP_LEFT:
                        targetDirection = LEFT;
                        break;
                    case (int)DOWN_LEFT:
                        targetDirection = LEFT;
                        break;
                    default:
                        break;
                }
            }
            switch ((int)targetDirection)
            {
                case (int)UP:
                    MoveUp(false);
                    Rotate(UP);
                    break;
                case (int)RIGHT:
                    MoveRight(false);
                    Rotate(RIGHT);
                    break;
                case (int)LEFT:
                    MoveLeft(false);
                    Rotate(LEFT);
                    break;
                case (int)DOWN:
                    MoveDown(false);
                    Rotate(DOWN);
                    break;             
                case (int)UP_LEFT:
                    MoveUp(false);
                    MoveLeft(false);
                    Rotate(UP_LEFT);
                    break;
                case (int)DOWN_LEFT:
                    MoveDown(false);
                    MoveLeft(false);
                    Rotate(DOWN_LEFT);
                    break;
                default:
                    break;
            }
            speed = initSpeed;

            if (KamikazeMode) {

                if ((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && (location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI))
                {
                    Explode();
                    if (game.tank1.barrier == false)
                    {
                        game.tank1.Die();
                    }
                }
                if ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && (location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI))
                {
                    Explode();
                    if (game.tank2.barrier == false)
                    {
                        game.tank2.Die();
                    }

                }

            }

            if (game.gameState == game.gameRunningPlayers2andCPU)
            {

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y > game.tank1.location.Y) || ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && location.Y > game.tank2.location.Y))
                {


                    targetDirection = UP;

                }

                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y < game.tank1.location.Y) || ((location.X >= game.tank2.location.X - Level_AI && location.X <= game.tank2.location.X + Level_AI) && location.Y < game.tank2.location.Y))
                {

                    targetDirection = DOWN;

                }
   
                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X > game.tank1.location.X) || ((location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI) && location.X > game.tank2.location.X))
                {


                    targetDirection = LEFT;

                }
         
                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X < game.tank1.location.X) || ((location.Y >= game.tank2.location.Y - Level_AI && location.Y <= game.tank2.location.Y + Level_AI) && location.X < game.tank2.location.X))
                {


                    targetDirection = RIGHT;


                }
            }
            else {
          
                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y > game.tank1.location.Y))
                {


                    targetDirection = UP;

                }
           
                if (((location.X >= game.tank1.location.X - Level_AI && location.X <= game.tank1.location.X + Level_AI) && location.Y < game.tank1.location.Y) )
                {

                    targetDirection = DOWN;

                }
             
                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X > game.tank1.location.X))
                {


                    targetDirection = LEFT;

                }    
                if (((location.Y >= game.tank1.location.Y - Level_AI && location.Y <= game.tank1.location.Y + Level_AI) && location.X < game.tank1.location.X) )
                {


                    targetDirection = RIGHT;


                }

            }


        }
        public override void Die()
        {
            base.Die();
         
        }
        public override void Hit()
        {
            base.Hit();
         
        }
        public override void Barrier()
        {
            base.Barrier();

        }


        public override void Respawn(Vector2 _location)
        {
            base.Respawn(_location);
        }

        }
}
