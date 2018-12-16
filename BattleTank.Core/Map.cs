using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Map
    {
        private readonly Game1 game;

        public int ScreenWidth { get; set; }
        public int RowWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int ColumnHeight { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int WallBorder { get; set; }
        public int WallInside { get; set; }
        public Tile[][] MapCurrent { get; set; }
        public Texture2D WallTexture { get; set; }
        public Texture2D BushTexture { get; set; }
        public Dictionary<WaterTextureType, List<Texture2D>> WaterTextures { get; set; } = new Dictionary<WaterTextureType, List<Texture2D>>();

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
            ScreenWidth = _screenWidth;
            ScreenHeight = _screenHeight;
            RowWidth = game.Settings.ElementsOnTheWidth;
            ColumnHeight = game.Settings.ElementsOnTheHeight;
            TileWidth = ScreenWidth / RowWidth;
            TileHeight = ScreenHeight / ColumnHeight;
            WallBorder = _WallBorder;
            WallInside = _WallInside;
            MapCurrent = new Tile[RowWidth][];

            for (int i = 0; i < MapCurrent.Length; ++i)
            {
                MapCurrent[i] = new Tile[ColumnHeight];
            }

            WallTexture = game.Content.Load<Texture2D>("Graphics/wall");
            BushTexture = game.Content.Load<Texture2D>("Graphics/bush");



            WaterTextures.Add(WaterTextureType.FULL, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterFull") });
            WaterTextures.Add(WaterTextureType.CORNER, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterCornerHorizontalRight"), game.Content.Load<Texture2D>("Graphics/waterCornerVerticalLeft"), game.Content.Load<Texture2D>("Graphics/waterCornerHorizontalLeft"), game.Content.Load<Texture2D>("Graphics/waterCornerVerticalRight") });
            WaterTextures.Add(WaterTextureType.BAY, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterBayHorizontalLeft"), game.Content.Load<Texture2D>("Graphics/waterBayHorizontalRight"), game.Content.Load<Texture2D>("Graphics/waterBayVertical") });
            WaterTextures.Add(WaterTextureType.ONE, new List<Texture2D> { game.Content.Load<Texture2D>("Graphics/waterOne_1"), game.Content.Load<Texture2D>("Graphics/waterOne_2"), game.Content.Load<Texture2D>("Graphics/waterOne_3"), game.Content.Load<Texture2D>("Graphics/waterOne_4") });

            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < MapCurrent.Length; ++i)
            {
                for (int e = 0; e < MapCurrent[i].Length; ++e)
                {

                    MapCurrent[i][e] = new Tile(Tile.TileType.AIR, new Rectangle(e * TileWidth, i * TileHeight, TileWidth, TileHeight), null);


                }
            }
            DrawWallBorder(this.WallBorder);

            if (game.WallInside)
                DrawWallInside(this.WallInside);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < MapCurrent.Length; ++i)
            {
                for (int e = 0; e < MapCurrent[i].Length; ++e)
                {

                    MapCurrent[i][e].Update(gameTime);


                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, int level)
        {
            for (int i = 0; i < MapCurrent.Length; ++i)
            {
                for (int e = 0; e < MapCurrent[i].Length; ++e)
                {
                    if(!MapCurrent[i][e].Type.Equals(Tile.TileType.MUD) && level == 1)
                        MapCurrent[i][e].Draw(spriteBatch);
                    else if (MapCurrent[i][e].Type.Equals(Tile.TileType.MUD) && level == 0)
                        MapCurrent[i][e].Draw(spriteBatch);
             

                }
            }
        }


        public Texture2D ReverseTexture(Texture2D texture, int reverse)
        {
            Texture2D newTexture = new Texture2D(game.GraphicsDevice, texture.Width, texture.Height);
            Color[] newTexturePixels;

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
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < MapCurrent[0].Length - 1; ++i)
                    {

                        MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                        MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);

                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }
                    break;
                case 1:
                    //First row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i != MapCurrent.Length / 2 && i != (MapCurrent.Length / 2) - 1 && i != 8 && i != 9 && i != (MapCurrent.Length / 2) - 1 && i != MapCurrent.Length - 9 && i != MapCurrent.Length - 10)
                            MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < MapCurrent[0].Length - 1; ++i)
                    {

                        if (i != (MapCurrent[0].Length / 2) && i != (MapCurrent[0].Length / 2) - 1 && i != (MapCurrent[0].Length - 5) && i != (MapCurrent[0].Length - 6) && i != 4 && i != 5)
                        {
                            MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);

                        }


                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {

                        if (i != MapCurrent.Length / 2 && i != (MapCurrent.Length / 2) - 1 && i != 8 && i != 9 && i != (MapCurrent.Length / 2) - 1 && i != MapCurrent.Length - 9 && i != MapCurrent.Length - 10)
                            MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }

                    break;
                case 2:
                    //First row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i != MapCurrent.Length / 2 && i != (MapCurrent.Length / 2) - 1)
                            MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < MapCurrent[0].Length - 1; ++i)
                    {

                        if (i != (MapCurrent[0].Length / 2) && i != (MapCurrent[0].Length / 2) - 1)
                        {
                            MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i != MapCurrent.Length / 2 && i != (MapCurrent.Length / 2) - 1)
                            MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }

                    break;
                case 3:
                    //First row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i <= 1 || i >= MapCurrent.Length - 2)
                            MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < MapCurrent[0].Length - 1; ++i)
                    {

                        if (i <= 1 || i >= MapCurrent[0].Length - 2)
                        {
                            MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);
                        }
                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i <= 1 || i >= MapCurrent.Length - 2)
                            MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }

                    break;
                case 4:
                    //First row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i % 2 == 0)

                            MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 0; i < MapCurrent[0].Length - 1; ++i)
                    {

                        if (i % 2 == 0)
                        {
                            MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);
                        }


                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i % 2 == 0)
                            MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }

                    break;
                case 5:
                    //First row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i % 4 == 0)

                            MapCurrent[i][0] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 0, TileWidth, TileHeight), WallTexture);
                    }
                    //Middle rows
                    for (int i = 1; i < MapCurrent[0].Length - 1; ++i)
                    {

                        if (i % 2 == 0)
                        {
                            MapCurrent[0][i] = new Tile(Tile.TileType.WALL, new Rectangle(0, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            MapCurrent[RowWidth - 1][i] = new Tile(Tile.TileType.WALL, new Rectangle((RowWidth - 1) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);

                        }


                    }
                    //Bottom row
                    for (int i = 0; i < MapCurrent.Length; ++i)
                    {
                        if (i % 4 == 0)
                            MapCurrent[i][ColumnHeight - 1] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, (ColumnHeight - 1) * TileHeight, TileWidth, TileHeight), WallTexture);
                    }

                    break;
                case 6:

                    break;

            }


        }

        public void MatchTextureLiquid(Tile.TileType type) {
            for (int i = 0; i < MapCurrent.Length; ++i)
            {
                for (int e = 0; e < MapCurrent[i].Length; ++e)
                {

                    if (MapCurrent[i][e].Type.Equals(type))
                    {
                        int decisionReverse = game.Randy.Next(0, 2);

                        if (!MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {
                            int typeTexture = game.Randy.Next(0, 4);

                            MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.ONE][typeTexture], decisionReverse);
                        }
                        else if (MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {
                            if (decisionReverse == 1)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][1], 0);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][0], 1);
                        }
                        else if (!MapCurrent[i - 1][e].Type.Equals(type) && MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {
                            if (decisionReverse == 1)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][0], 0);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][1], 1);

                        }
                        else if (!MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {
                            MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][2], 1);
                        }
                        else if (!MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && MapCurrent[i][e + 1].Type.Equals(type))
                        {
                            MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.BAY][2], 0);
                        }
                        else if (MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][0], decisionReverse);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][1], decisionReverse);


                        }
                        else if (!MapCurrent[i - 1][e].Type.Equals(type) && MapCurrent[i + 1][e].Type.Equals(type) && MapCurrent[i][e - 1].Type.Equals(type) && !MapCurrent[i][e + 1].Type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][2], decisionReverse);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][3], decisionReverse);

                        }
                        else if (MapCurrent[i - 1][e].Type.Equals(type) && !MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && MapCurrent[i][e + 1].Type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][3], decisionReverse);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][2], decisionReverse);

                        }
                        else if (!MapCurrent[i - 1][e].Type.Equals(type) && MapCurrent[i + 1][e].Type.Equals(type) && !MapCurrent[i][e - 1].Type.Equals(type) && MapCurrent[i][e + 1].Type.Equals(type))
                        {

                            if (decisionReverse == 0)
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][1], decisionReverse);
                            else
                                MapCurrent[i][e].Texture = ReverseTexture(WaterTextures[WaterTextureType.CORNER][0], decisionReverse);

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


                            int x = game.Randy.Next(2, MapCurrent.Length - 2);
                            int y = game.Randy.Next(2, ColumnHeight - 2);
                            if (i < 30 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                    MapCurrent[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WallTexture);
                            }
                            else if (i < 60 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                    MapCurrent[x][y] = new Tile(Tile.TileType.BUSH, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), BushTexture);
                            }
                            else if (i < 90 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                    MapCurrent[x][y] = new Tile(Tile.TileType.WATER, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                            }
                            else if (i < 120 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            {
                                    MapCurrent[x][y] = new Tile(Tile.TileType.MUD, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                            }
                            


                        }

                        MatchTextureLiquid(Tile.TileType.WATER);
                        MatchTextureLiquid(Tile.TileType.MUD);

                    }
                    break;
                case 2:
                    {
                        //Middle rows

                        for (int x = 2; x < MapCurrent.Length - 2; ++x)
                        {
                            for (int y = 2; y < (MapCurrent[0].Length) - 2; y++)
                            {
                                if (x % 2 == 0 && y % 2 == 0)
                                    MapCurrent[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WallTexture);

                            }
                        }
                    }
                    break;

                case 3:
                    {
                        //Middle rows
                        for (int i = 0; i < 360; ++i)
                        {


                            int x = game.Randy.Next(2, MapCurrent.Length - 2);
                            int y = game.Randy.Next(2, ColumnHeight - 2);
                            if (i < 90 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                MapCurrent[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WallTexture);
                            }
                            else if (i < 180 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                MapCurrent[x][y] = new Tile(Tile.TileType.BUSH, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), BushTexture);
                            }
                            else if (i < 270 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            { 
                                MapCurrent[x][y] = new Tile(Tile.TileType.WATER, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                            }
                            else if (i < 360 && (x != MapCurrent.Length / 2 && x != (MapCurrent.Length / 2) - 1 && y != (MapCurrent[0].Length / 2) && y != (MapCurrent[0].Length / 2) - 1))
                            {                         
                                MapCurrent[x][y] = new Tile(Tile.TileType.MUD, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);                              
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
                        for (int i = 5; i < MapCurrent.Length - 5; i++)
                        {
                            if (i != (MapCurrent.Length / 2) && i != (MapCurrent.Length / 2) - 1)
                            {
                                MapCurrent[i][5] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, 5 * TileHeight, TileWidth, TileHeight), WallTexture);
                                MapCurrent[i][(MapCurrent[0].Length) - 6] = new Tile(Tile.TileType.WALL, new Rectangle(i * TileWidth, ((MapCurrent[0].Length) - 6) * TileHeight, TileWidth, TileHeight), WallTexture);
                            }

                        }
                        for (int i = 5; i < (MapCurrent[0].Length) - 5; i++)
                        {
                            if (i != (MapCurrent[0].Length / 2) && i != (MapCurrent[0].Length / 2) - 1)
                            {
                                MapCurrent[5][i] = new Tile(Tile.TileType.WALL, new Rectangle(5 * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);
                                MapCurrent[MapCurrent.Length - 6][i] = new Tile(Tile.TileType.WALL, new Rectangle((MapCurrent.Length - 6) * TileWidth, i * TileHeight, TileWidth, TileHeight), WallTexture);
                            }

                        }
                        for (int i = 0; i < 50; ++i)
                        {

                            int x = game.Randy.Next(7, MapCurrent.Length - 7);
                            int y = game.Randy.Next(7, ColumnHeight - 7);

                            MapCurrent[x][y] = new Tile(Tile.TileType.WALL, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), WallTexture);

                        }


                        //WALL


                        //WATER
                        for (int i = 3; i < MapCurrent.Length - 3; i++)
                        {
                            if (i != (MapCurrent.Length / 2) && i != (MapCurrent.Length / 2) - 1)
                            {
                                MapCurrent[i][3] = new Tile(Tile.TileType.WATER, new Rectangle(i * TileWidth, 3 * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                                MapCurrent[i][(MapCurrent[0].Length) - 4] = new Tile(Tile.TileType.WATER, new Rectangle(i * TileWidth, ((MapCurrent[0].Length) - 4) * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                            }

                        }
                        for (int i = 3; i < (MapCurrent[0].Length) - 3; i++)
                        {
                            if (i != (MapCurrent[0].Length / 2) && i != (MapCurrent[0].Length / 2) - 1)
                            {
                                MapCurrent[3][i] = new Tile(Tile.TileType.WATER, new Rectangle(3 * TileWidth, i * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                                MapCurrent[MapCurrent.Length - 4][i] = new Tile(Tile.TileType.WATER, new Rectangle((MapCurrent.Length - 4) * TileWidth, i * TileHeight, TileWidth, TileHeight), WaterTextures[WaterTextureType.FULL][0]);
                            }

                        }
                        //WATER

                        //BUSH
                        for (int i = 1; i < MapCurrent.Length - 1; i++)
                        {
                            if (i != (MapCurrent.Length / 2) && i != (MapCurrent.Length / 2) - 1)
                            {
                                MapCurrent[i][1] = new Tile(Tile.TileType.BUSH, new Rectangle(i * TileWidth, 1 * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[i][(MapCurrent[0].Length) - 2] = new Tile(Tile.TileType.BUSH, new Rectangle(i * TileWidth, ((MapCurrent[0].Length) - 2) * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[i][2] = new Tile(Tile.TileType.BUSH, new Rectangle(i * TileWidth, 2 * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[i][(MapCurrent[0].Length) - 3] = new Tile(Tile.TileType.BUSH, new Rectangle(i * TileWidth, ((MapCurrent[0].Length) - 3) * TileHeight, TileWidth, TileHeight), BushTexture);

                            }

                        }
                        for (int i = 1; i < (MapCurrent[0].Length) - 1; i++)
                        {
                            if (i != (MapCurrent[0].Length / 2) && i != (MapCurrent[0].Length / 2) - 1)
                            {
                                MapCurrent[1][i] = new Tile(Tile.TileType.BUSH, new Rectangle(1 * TileWidth, i * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[MapCurrent.Length - 2][i] = new Tile(Tile.TileType.BUSH, new Rectangle((MapCurrent.Length - 2) * TileWidth, i * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[2][i] = new Tile(Tile.TileType.BUSH, new Rectangle(2 * TileWidth, i * TileHeight, TileWidth, TileHeight), BushTexture);
                                MapCurrent[MapCurrent.Length - 3][i] = new Tile(Tile.TileType.BUSH, new Rectangle((MapCurrent.Length - 3) * TileWidth, i * TileHeight, TileWidth, TileHeight), BushTexture);

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
                int startingLocationX = randy.Next(100, ScreenWidth - 100);
                int startingLocationY = randy.Next(100, ScreenHeight - 100);

                respawnLocation = new Vector2(startingLocationX, startingLocationY);

                Rectangle startingtankRect = new Rectangle(startingLocationX, startingLocationY, width, height);

                colliding = false;
                foreach (Tile[] tiles in MapCurrent)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile is null) continue;
                        if ((tile.IsColliding(startingtankRect).Depth <= 0)) continue;

                        colliding = true;
                    }
                }
            } while (colliding);
            return respawnLocation;
        }
    }
}
