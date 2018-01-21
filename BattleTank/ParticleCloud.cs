﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleTank
{
    public class Particlecloud
    {
        public static int MAX_PARTICLES = 15;
        private Color color;
        private Particle[] particles = new Particle[MAX_PARTICLES];
        private int num = 15;
        public Particlecloud() { }


        public Particlecloud(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color, int maxSpeed)
        {
            color = _color;
            Random rand = new Random();
            for (int i = 0; i < MAX_PARTICLES; ++i)
            {
                Vector2 speed = new Vector2();
                int a = rand.Next(-maxSpeed, maxSpeed);
                int b = rand.Next(-maxSpeed, maxSpeed);          
                speed = new Vector2(a, b);
       
                particles[i] = new Particle(game, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(rand.Next(1, 20), rand.Next(1, 20))), speed, color, player, (float)rand.NextDouble(), whiteRectangle, 1, true);
            }
        }

        public Particlecloud(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color, int maxSpeed, int _num)
        {
            num = _num;
            particles = new Particle[num];
            color = _color;
            Random rand = new Random();
            for (int i = 0; i < num; ++i)
            {
                Vector2 speed = new Vector2();
                int a = rand.Next(-maxSpeed, maxSpeed);
                int b = rand.Next(-maxSpeed, maxSpeed);        
                speed = new Vector2(a, b);
             
                particles[i] = new Particle(game, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(rand.Next(1, 20), rand.Next(1, 20))), speed, color, player, (float)rand.NextDouble(), whiteRectangle, 1, true);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < num; ++i)
            {
                particles[i].Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < num; ++i)
            {
                particles[i].Update(gameTime);
            }
        }
    }
}
