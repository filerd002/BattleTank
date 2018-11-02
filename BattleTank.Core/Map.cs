using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Map
    {
        private Game1 game;
        public int screenWidth;
        public int rowWidth;
        public int screenHeight;
        public int columnHeight;
        public int tileWidth;
        public int tileHeight;
        public int WallBorder;
        public int WallInside;
        public Tile[][] map;

        public Texture2D wallTexture;
        public Texture2D bushTexture;
   

        public Dictionary<WaterTextureType, List<Texture2D>> waterTextures = new Dictionary<WaterTextureType, List<Texture2D>>();


        public enum WaterTextureType
        {
            FULL,
            BAY,
            CORNER,
            ONE
        }

        public Map(Game1 _game, int _screenWidth, int _screenHeight, int _WallBorder, int _WallInside)
        {
            game = _game;
            screenWidth = _screenWidth;
            screenHeight = _screenHeight;
            rowWidth = game.settings.elementsOnTheWidth;
            columnHeight = game.settings.elementsOnTheHeight;
            tileWidth = screenWidth / rowWidth;
            tileHeight = screenHeight / columnHeight;
            WallBorder = _WallBorder;
            WallInside = _WallInside;
            map = new Tile[rowWidth][];

            for (int i = 0; i < map.Length; ++i)
            {
                map[i] = new Tile[columnHeight];
            }

            wallTexture = game.Content.Load<Texture2D>("Graphics/wall");
            bushTexture = game.Content.Load<Texture2D>("Graphics/bush");



            waterTextures.Add(WaterTextureType.FULL, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterFull") });
            waterTextures.Add(WaterTextureType.CORNER, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterCornerHorizontalRight"), game.Content.Load<Texture2D>("Graphics/waterCornerVerticalLeft"), game.Content.Load<Texture2D>("Graphics/waterCornerHorizontalLeft"), game.Content.Load<Texture2D>("Graphics/waterCornerVerticalRight") });
            waterTextures.Add(WaterTextureType.BAY, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterBayHorizontalLeft"), game.Content.Load<Texture2D>("Graphics/waterBayHorizontalRight"), game.Content.Load<Texture2D>("Graphics/waterBayVertical") });
            waterTextures.Add(WaterTextureType.ONE, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterOne_1"), game.Content.Load<Texture2D>("Graphics/waterOne_2"), game.Content.Load<Texture2D>("Graphics/waterOne_3"), game.Content.Load<Texture2D>("Graphics/waterOne_4") });

            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {

                    map[i][e] = new Tile(Tile.TileType.AIR, new Rectangle(e * tileWidth, i * tileHeight, tileWidth, tileHeight), null);


                }
            }
            DrawWallBorder(this.WallBorder);

            if (game.WallInside)
                DrawWallInside(this.WallInside);
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
        public void Draw(SpriteBatch spriteBatch, int level)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {
                    if(!map[i][e].type.Equals(Tile.TileType.MUD) && level == 1)
                        map[i][e].Draw(spriteBatch);
                    else if (map[i][e].type.Equals(Tile.TileType.MUD) && level == 0)
                        map[i][e].Draw(spriteBatch);
                    else
                        continue;

                }
            }
        }


        public Texture2D ReverseTexture(Texture2D texture, int reverse)
        {
            Texture2D newTexture = new Texture2D(game.GraphicsDevice, texture.Width, texture.Height);
            Color[] newTexturePixels = new Color[texture.Width * texture.Height];

            newTexturePixels = GetPixels(texture);
            if (reverse == 1)
                System.Array.Reverse(newTexturePixels, 0, newTexturePixels.Length);


            newTexture.SetData(newTexturePixels);

            return newTexture;
        }

        public static Color[] GetPixels(Texture2D texture)
        {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixels);
            return pixels;
        }

        public void DrawWallBorder(int _WallBorder)
        {
            switch (_WallBorder)
            {
                case 0:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                        map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);

                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }
                    break;
                case 1:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != map.Length / 2 && i != (map.Length / 2) - 1 && i != 8 && i != 9 && i != (map.Length / 2) - 1 && i != map.Length - 9 && i != map.Length - 10)
                            map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1 && i != (map[0].Length - 5) && i != (map[0].Length - 6) && i != 4 && i != 5)
                        {
                            map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);

                        }


                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {

                        if (i != map.Length / 2 && i != (map.Length / 2) - 1 && i != 8 && i != 9 && i != (map.Length / 2) - 1 && i != map.Length - 9 && i != map.Length - 10)
                            map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }

                    break;
                case 2:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != map.Length / 2 && i != (map.Length / 2) - 1)
                            map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1)
                        {
                            map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i != map.Length / 2 && i != (map.Length / 2) - 1)
                            map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }

                    break;
                case 3:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i <= 1 || i >= map.Length - 2)
                            map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i <= 1 || i >= map[0].Length - 2)
                        {
                            map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i <= 1 || i >= map.Length - 2)
                            map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }

                    break;
                case 4:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i % 2 == 0)

                            map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 0; i < map[0].Length - 1; ++i)
                    {

                        if (i % 2 == 0)
                        {
                            map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);
                        }


                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i % 2 == 0)
                            map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }

                    break;
                case 5:
                    //First row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i % 4 == 0)

                            map[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), wallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < map[0].Length - 1; ++i)
                    {

                        if (i % 2 == 0)
                        {
                            map[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            map[rowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((rowWidth - 1) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);

                        }


                    }
                    //Bottom row
                    for (int i = 0; i < map.Length; ++i)
                    {
                        if (i % 4 == 0)
                            map[i][columnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, (columnHeight - 1) * tileHeight, tileWidth, tileHeight), wallTexture);
                    }

                    break;
                case 6:

                    break;

            }


        }

        public void MatchTextureLiquid(Tile.TileType type) {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {

                    if (map[i][e].type.Equals(type))
                    {
                        int decisionReverse = game.randy.Next(0, 2);

                        if (!map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {
                            int typeTexture = game.randy.Next(0, 4);

                            map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.ONE][typeTexture], decisionReverse);
                        }
                        else if (map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {
                            if (decisionReverse == 1)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][1], 0);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][0], 1);
                        }
                        else if (!map[i - 1][e].type.Equals(type) && map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {
                            if (decisionReverse == 1)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][0], 0);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][1], 1);

                        }
                        else if (!map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {
                            map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][2], 1);
                        }
                        else if (!map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && map[i][e + 1].type.Equals(type))
                        {
                            map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.BAY][2], 0);
                        }
                        else if (map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][0], decisionReverse);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][1], decisionReverse);


                        }
                        else if (!map[i - 1][e].type.Equals(type) && map[i + 1][e].type.Equals(type) && map[i][e - 1].type.Equals(type) && !map[i][e + 1].type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][2], decisionReverse);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][3], decisionReverse);

                        }
                        else if (map[i - 1][e].type.Equals(type) && !map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && map[i][e + 1].type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][3], decisionReverse);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][2], decisionReverse);

                        }
                        else if (!map[i - 1][e].type.Equals(type) && map[i + 1][e].type.Equals(type) && !map[i][e - 1].type.Equals(type) && map[i][e + 1].type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][1], decisionReverse);
                            else
                                map[i][e].texture = ReverseTexture(waterTextures[WaterTextureType.CORNER][0], decisionReverse);

                        }

                    }
                }
            }
        }


        public void DrawWallInside(int _WallInside)
        {
            _WallInside = 3;
            switch (_WallInside)
            {
                case 0:
                    break;
                case 1:
                    {
                        //Middle rows
                        for (int i = 0; i < 120; ++i)
                        {


                            int x = game.randy.Next(2, map.Length - 2);
                            int y = game.randy.Next(2, columnHeight - 2);
                            if (i < 30)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), wallTexture);
                            }
                            else if (i < 60)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.BUSH, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), bushTexture);
                            }
                            else if (i < 90)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.WATER, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }
                            else if (i < 120)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.MUD, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }


                        }

                        MatchTextureLiquid(Tile.TileType.WATER);
                        MatchTextureLiquid(Tile.TileType.MUD);

                    }
                    break;
                case 2:
                    {
                        //Middle rows

                        for (int x = 2; x < map.Length - 2; ++x)
                        {
                            for (int y = 2; y < (map[0].Length) - 2; y++)
                            {
                                if (x % 2 == 0 && y % 2 == 0)
                                    map[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), wallTexture);

                            }
                        }
                    }
                    break;

                case 3:
                    {
                        //Middle rows
                        for (int i = 0; i < 360; ++i)
                        {


                            int x = game.randy.Next(2, map.Length - 2);
                            int y = game.randy.Next(2, columnHeight - 2);
                            if (i < 90)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), wallTexture);
                            }
                            else if (i < 180)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.BUSH, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), bushTexture);
                            }
                            else if (i < 270)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.WATER, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }
                            else if (i < 360)
                            {
                                if (x != map.Length / 2 && x != (map.Length / 2) - 1 && y != (map[0].Length / 2) && y != (map[0].Length / 2) - 1)
                                    map[x][y] = new Tile(Tile.TileType.MUD, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }


                        }

                        MatchTextureLiquid(Tile.TileType.WATER);
                        MatchTextureLiquid(Tile.TileType.MUD);
                    }
                    break;

                case 4:
                    {
                        //Middle rows

                        //WALL
                        for (int i = 5; i < map.Length - 5; i++)
                        {
                            if (i != (map.Length / 2) && i != (map.Length / 2) - 1)
                            {
                                map[i][5] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, 5 * tileHeight, tileWidth, tileHeight), wallTexture);
                                map[i][(map[0].Length) - 6] = new Tile(Tile.TileType.WALL, new Rectangle(i * tileWidth, ((map[0].Length) - 6) * tileHeight, tileWidth, tileHeight), wallTexture);
                            }

                        }
                        for (int i = 5; i < (map[0].Length) - 5; i++)
                        {
                            if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1)
                            {
                                map[5][i] = new Tile(Tile.TileType.WALL, new Rectangle(5 * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);
                                map[map.Length - 6][i] = new Tile(Tile.TileType.WALL, new Rectangle((map.Length - 6) * tileWidth, i * tileHeight, tileWidth, tileHeight), wallTexture);
                            }

                        }
                        for (int i = 0; i < 50; ++i)
                        {

                            int x = game.randy.Next(7, map.Length - 7);
                            int y = game.randy.Next(7, columnHeight - 7);

                            map[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), wallTexture);

                        }


                        //WALL


                        //WATER
                        for (int i = 3; i < map.Length - 3; i++)
                        {
                            if (i != (map.Length / 2) && i != (map.Length / 2) - 1)
                            {
                                map[i][3] = new Tile(Tile.TileType.WATER, new Rectangle(i * tileWidth, 3 * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                                map[i][(map[0].Length) - 4] = new Tile(Tile.TileType.WATER, new Rectangle(i * tileWidth, ((map[0].Length) - 4) * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }

                        }
                        for (int i = 3; i < (map[0].Length) - 3; i++)
                        {
                            if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1)
                            {
                                map[3][i] = new Tile(Tile.TileType.WATER, new Rectangle(3 * tileWidth, i * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                                map[map.Length - 4][i] = new Tile(Tile.TileType.WATER, new Rectangle((map.Length - 4) * tileWidth, i * tileHeight, tileWidth, tileHeight), waterTextures[WaterTextureType.FULL][0]);
                            }

                        }
                        //WATER

                        //BUSH
                        for (int i = 1; i < map.Length - 1; i++)
                        {
                            if (i != (map.Length / 2) && i != (map.Length / 2) - 1)
                            {
                                map[i][1] = new Tile(Tile.TileType.BUSH, new Rectangle(i * tileWidth, 1 * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[i][(map[0].Length) - 2] = new Tile(Tile.TileType.BUSH, new Rectangle(i * tileWidth, ((map[0].Length) - 2) * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[i][2] = new Tile(Tile.TileType.BUSH, new Rectangle(i * tileWidth, 2 * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[i][(map[0].Length) - 3] = new Tile(Tile.TileType.BUSH, new Rectangle(i * tileWidth, ((map[0].Length) - 3) * tileHeight, tileWidth, tileHeight), bushTexture);

                            }

                        }
                        for (int i = 1; i < (map[0].Length) - 1; i++)
                        {
                            if (i != (map[0].Length / 2) && i != (map[0].Length / 2) - 1)
                            {
                                map[1][i] = new Tile(Tile.TileType.BUSH, new Rectangle(1 * tileWidth, i * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[map.Length - 2][i] = new Tile(Tile.TileType.BUSH, new Rectangle((map.Length - 2) * tileWidth, i * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[2][i] = new Tile(Tile.TileType.BUSH, new Rectangle(2 * tileWidth, i * tileHeight, tileWidth, tileHeight), bushTexture);
                                map[map.Length - 3][i] = new Tile(Tile.TileType.BUSH, new Rectangle((map.Length - 3) * tileWidth, i * tileHeight, tileWidth, tileHeight), bushTexture);

                            }

                        }
                        //BUSH


                        MatchTextureLiquid(Tile.TileType.WATER);
                     

                    }
                    break;


            }

        }


        public Vector2 FindNonColidingPosition(int width, int height)
        {
            Random randy = new Random();

            Vector2 respawnLocation;
            bool colliding = false;
            do
            {
                int startingLocationX = randy.Next(100, screenWidth - 100);
                int startingLocationY = randy.Next(100, screenHeight - 100);

                respawnLocation = new Vector2(startingLocationX, startingLocationY);

                Rectangle startingtankRect = new Rectangle(startingLocationX, startingLocationY, width, height);

                colliding = false;
                foreach (Tile[] tiles in map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile is null) continue;
                        if ((!(tile.isColliding(startingtankRect).depth > 0))) continue;

                        colliding = true;
                    }
                }
            } while (colliding);
            return respawnLocation;
        }
    }
}
