using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Particlecloud
    {
        private readonly int num = 15;
        public Particlecloud() { }


        public Particlecloud(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color, int maxSpeed)
        {
            Color = _color;
            Random rand = new Random();
            for (int i = 0; i < MAX_PARTICLES; ++i)
            {
                Vector2 speed;
                int a = rand.Next(-maxSpeed, maxSpeed);
                int b = rand.Next(-maxSpeed, maxSpeed);          
                speed = new Vector2(a, b);
       
                Particles[i] = new Particle(game, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(rand.Next(1, 20), rand.Next(1, 20))), speed, Color, player, (float)rand.NextDouble(), whiteRectangle, 1, true);
            }
        }

        public Particlecloud(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color, int maxSpeed, int _num)
        {
            num = _num;
            Particles = new Particle[num];
            Color = _color;
            Random rand = new Random();
            for (int i = 0; i < num; ++i)
            {
                Vector2 speed;
                int a = rand.Next(-maxSpeed, maxSpeed);
                int b = rand.Next(-maxSpeed, maxSpeed);        
                speed = new Vector2(a, b);
             
                Particles[i] = new Particle(game, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(rand.Next(1, 20), rand.Next(1, 20))), speed, Color, player, (float)rand.NextDouble(), whiteRectangle, 1, true);
            }
        }

        public static int MAX_PARTICLES { get; set; } = 15;
        public Color Color { get; }

        internal Particle[] Particles { get; set; } = new Particle[MAX_PARTICLES];

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < num; ++i)
            {
                Particles[i].Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < num; ++i)
            {
                Particles[i].Update(gameTime);
            }
        }
    }
}
