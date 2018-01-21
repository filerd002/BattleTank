﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleTank
{
    public class Map
    {
        private Game1 game;
        public int screenWidth;
        public int rowWidth;
        public int screenHeight;
        public int columnHeight;
        public int WallBorder;
        public Tile[][] map;
      
        public Texture2D wallTexture;
        public Map(Game1 _game, int _screenWidth, int _screenHeight, int _WallBorder)
        {
            game = _game;
            screenWidth = _screenWidth;
            screenHeight = _screenHeight;
            rowWidth = screenWidth / 48;
            columnHeight = screenHeight / 48;
            WallBorder = _WallBorder;
            map = new Tile[rowWidth][];
       
            for (int i=0; i < map.Length; ++i)
            {
                map[i] = new Tile[columnHeight];
            }
        
            wallTexture = game.Content.Load<Texture2D>("Graphics//wall");


            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {
          
                        map[i][e] = new Tile(Tile.AIR, new Rectangle(e * 48, i * 48, 48, 48), null);


                }
            }
            DrawWallBorder(this.WallBorder);
            if(game.WallInside)
            DrawWallInside();
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {
               
                        map[i][e].Update(gameTime);

              
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for(int e = 0; e < map[i].Length; ++e)
                {
            
                        map[i][e].Draw(spriteBatch);

                }
            }
        }
        public void DrawWallBorder(int _WallBorder)
        {
          switch(_WallBorder)
            {
                case 0:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        map[i][0] = new Tile(Tile.WALL, new Rectangle(i * 48, 0, 48, 48), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        map[0][i] = new Tile(Tile.WALL, new Rectangle(0, i * 48, 48, 48), wallTexture);
                        map[rowWidth - 1][i] = new Tile(Tile.WALL, new Rectangle((rowWidth - 1) * 48, i * 48, 48, 48), wallTexture);

                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        map[i][columnHeight - 1] = new Tile(Tile.WALL, new Rectangle(i * 48, (columnHeight - 1) * 48, 48, 48), wallTexture);
                    }   
            break;
                case 1:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != 6 && i != map.Length - 7)
                            map[i][0] = new Tile(Tile.WALL, new Rectangle(i * 48, 0, 48, 48), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i != 5 && i != 10)
                        {
                            map[0][i] = new Tile(Tile.WALL, new Rectangle(0, i * 48, 48, 48), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.WALL, new Rectangle((rowWidth - 1) * 48, i * 48, 48, 48), wallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != 6 && i != map.Length - 7)
                            map[i][columnHeight - 1] = new Tile(Tile.WALL, new Rectangle(i * 48, (columnHeight - 1) * 48, 48, 48), wallTexture);
                    }

                    break;
                case 2:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != map.Length / 2 && i != (map.Length / 2) - 1)
                            map[i][0] = new Tile(Tile.WALL, new Rectangle(i * 48, 0, 48, 48), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1)
                        {
                            map[0][i] = new Tile(Tile.WALL, new Rectangle(0, i * 48, 48, 48), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.WALL, new Rectangle((rowWidth - 1) * 48, i * 48, 48, 48), wallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != map.Length / 2 && i != (map.Length / 2) - 1)
                            map[i][columnHeight - 1] = new Tile(Tile.WALL, new Rectangle(i * 48, (columnHeight - 1) * 48, 48, 48), wallTexture);
                    }

                    break;
                case 3:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i <= 1 || i >= map.Length - 2)
                            map[i][0] = new Tile(Tile.WALL, new Rectangle(i * 48, 0, 48, 48), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i <= 1 || i >= map[0].Length - 2)
                        {
                            map[0][i] = new Tile(Tile.WALL, new Rectangle(0, i * 48, 48, 48), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.WALL, new Rectangle((rowWidth - 1) * 48, i * 48, 48, 48), wallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i <= 1 || i >= map.Length - 2)
                            map[i][columnHeight - 1] = new Tile(Tile.WALL, new Rectangle(i * 48, (columnHeight - 1) * 48, 48, 48), wallTexture);
                    }

                    break;
                case 4:

                    break;
              
            }


        }


        public void DrawWallInside()
        {
            Random randy = new Random();
            //Middle rows
            for (int i = 0; i < 60; ++i)
            {
               

                int x = randy.Next(2,  map.Length - 2);
                int y = randy.Next(2, columnHeight - 2);

                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                        map[x][y] = new Tile(Tile.WALL, new Rectangle(x * 48, y * 48, 48, 48), wallTexture);
               
            }


          //  map[map.Length/2][map.Length/2] = new Tile(Tile.WALL, new Rectangle((screenWidth/2)-24,(screenHeight/2)-24, 48, 48), wallTexture);
          //     map[map.Length / 2][(map.Length / 2)-1] = new Tile(Tile.WALL, new Rectangle((screenWidth / 2) - 72, (screenHeight / 2) - 24, 48, 48), wallTexture);
           //   map[map.Length / 2][(map.Length / 2) - 2] = new Tile(Tile.WALL, new Rectangle((screenWidth / 2) + 24, (screenHeight / 2) - 24, 48, 48), wallTexture);
         //    map[map.Length / 2][(map.Length / 2) - 3] = new Tile(Tile.WALL, new Rectangle((screenWidth / 2) - 24, (screenHeight / 2) + 24, 48, 48), wallTexture);
         //   map[map.Length / 2][(map.Length / 2) - 4] = new Tile(Tile.WALL, new Rectangle((screenWidth / 2) - 24, (screenHeight / 2) - 72, 48, 48), wallTexture);
        }
    }
}
