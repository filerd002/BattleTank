using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank
{
    public class Score
    {
        private Game1 game { get; set; }
        private int[][] score;
        private int numOfPlayers { get; set; }
     
        private SpriteFont spriteFont;
        private SpriteFont spriteFontBig;
        public Score() { }



        String locationScoreTank1 = "TOP";
        String locationScoreTank2 = "RIGHT";
        String locationScoreenemyTanks0 = "TOP";
        String locationScoreenemyTanks1 = "TOP";
        String locationScoreenemyTanks2 = "TOP";
        String locationScoreenemyTanks3 = "TOP";
        String locationScoreenemyTanks4 = "TOP";
        String locationScoreenemyTanks5 = "TOP";
        public Score(Game1 _game, int _numOfPlayers)
        {
            game = _game;
            numOfPlayers = _numOfPlayers;
            score = new int[numOfPlayers][];
            for (int i = 0; i < score.Length; ++i)
            {
                score[i] = new int[4];
                for (int j = 0; j < 4; ++j)                 
                score[i][j] = 0;
            }
            Load();
        }
        public void Load()
        {
            spriteFont = game.Content.Load<SpriteFont>("Arial");
            spriteFontBig = game.Content.Load<SpriteFont>("ArialBig");
        }
        public int getScore(int playerIndex)
        {
            return score[playerIndex][0];
        }
        public void addScore(int playerIndex, int pointsToAdd)
        {

            if (score[playerIndex][2] == 9)
            {
                score[playerIndex][2] = 0;
                score[playerIndex][3] += pointsToAdd;
            }
            else if (score[playerIndex][1] == 9)
            {
                score[playerIndex][1] = 0 ;
                score[playerIndex][2] += pointsToAdd;           
            }
            else
                score[playerIndex][1] += pointsToAdd;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            




            if (locationScoreTank1.Equals("TOP"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 32), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 47), Color.Green);
                if(game.gameState == game.gameRunningWyscig)
                spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 62), Color.Green);
                else
                spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 77), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 92), Color.Green);


            }
            else if (locationScoreTank1.Equals("BOTTOM"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 92), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 77), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                else
                spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 47), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 32), Color.Green);


            }
            else if (locationScoreTank1.Equals("LEFT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 92), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 77), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 47), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 32), Color.Green);
            }
            else if (locationScoreTank1.Equals("RIGHT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 32), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 47), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 77), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 92), Color.Green);

            }

            else if (locationScoreTank1.Equals("TOP_LEFT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 92), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 77), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 47), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y - 32), Color.Green);
            }
            else if (locationScoreTank1.Equals("TOM_RIGHT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 92), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 77), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 47), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y - 32), Color.Green);

            }
            else if (locationScoreTank1.Equals("BOTTOM_LEFT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 32), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 47), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 77), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X - 108, game.tank1.location.Y + 92), Color.Green);

            }
            else if (locationScoreTank1.Equals("BOTTOM_RIGHT"))
            {
                spriteBatch.DrawString(spriteFont, "  Player 1: " + score[0], new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 32), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Armor P1: " + game.tank1.armor, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 47), Color.Green);
                if (game.gameState == game.gameRunningWyscig)
                    spriteBatch.DrawString(spriteFont, "  Lives P1: Niesmiertelny", new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 62), Color.Green);
                else
                    spriteBatch.DrawString(spriteFont, "  Lives P1: " + game.tank1.lives, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 62), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Strong P1: " + game.tank1.strong, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 77), Color.Green);
                spriteBatch.DrawString(spriteFont, "  Mines P1: " + game.tank1.mines, new Vector2(game.tank1.location.X + 14, game.tank1.location.Y + 92), Color.Green);

            }


            game.tank1.colliding = false;
            foreach (Tile[] tiles in game.map.map)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile != null)
                    {
                        if ((tile.isColliding(game.tank1.tankRect).depth > 0))
                        {
                            game.tank1.colliding = true;
                            Collision collision = tile.isColliding(game.tank1.tankRect);
                            switch (collision.side)
                            {
                                case Collision.Side.TOP:
                                    if (locationScoreTank1.Equals("LEFT"))
                                        locationScoreTank1 = "TOP_LEFT";
                                    else if (locationScoreTank1.Equals("RIGHT"))
                                        locationScoreTank1 = "TOP_RIGHT";
                                    else
                                        locationScoreTank1 = "TOP";
                                    break;
                                case Collision.Side.BOTTOM:
                                    if (locationScoreTank1.Equals("LEFT"))
                                        locationScoreTank1 = "BOTTOM_LEFT";
                                    else if (locationScoreTank1.Equals("RIGHT"))
                                        locationScoreTank1 = "BOTTOM_RIGHT";
                                    else
                                        locationScoreTank1 = "BOTTOM";

                                    break;
                                case Collision.Side.LEFT:
                                    if (locationScoreTank1.Equals("TOP"))
                                        locationScoreTank1 = "TOP_LEFT";
                                    else if (locationScoreTank1.Equals("BOTTOM"))
                                        locationScoreTank1 = "BOTTOM_LEFT";
                                    else
                                        locationScoreTank1 = "LEFT";

                                    break;
                                case Collision.Side.RIGHT:
                                    if (locationScoreTank1.Equals("TOP"))
                                        locationScoreTank1 = "TOP_RIGHT";
                                    else if (locationScoreTank1.Equals("BOTTOM"))
                                        locationScoreTank1 = "BOTTOM_RIGHT";
                                    else
                                        locationScoreTank1 = "RIGHT";

                                    break;

                            }
                        }
                    }
                    else { continue; }

                }
            }



            if (game.gameState == game.gameRunningPlayers2 || game.gameState == game.gameRunningPlayers2andCPU || game.gameState == game.gameRunningWyscig)
            {

                //   spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.graphics.PreferredBackBufferWidth - 170, game.graphics.PreferredBackBufferHeight - 70), Color.Red);


                if (game.gameState == game.gameRunningWyscig)
                {

                   
                    TimeSpan time = TimeSpan.FromSeconds(game.czasWyscigu);
                    spriteBatch.DrawString(spriteFontBig, time.ToString("mm':'ss"), new Vector2((game.map.screenWidth / 2)-27,65), Color.White);
                    spriteBatch.DrawString(spriteFontBig, score[0][3].ToString() +""+ score[0][2].ToString() + ""+score[0][1].ToString() + ""+score[0][0].ToString(), new Vector2((game.map.screenWidth / 2) - 97, 65), Color.Green);
                    spriteBatch.DrawString(spriteFontBig, score[1][3].ToString() + "" + score[1][2].ToString() + "" + score[1][1].ToString() + "" + score[1][0].ToString(), new Vector2((game.map.screenWidth / 2) +57, 65), Color.Red);


                }


                if (locationScoreTank2.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 32), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 47), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 77), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 92), Color.Red);


                }
                else if (locationScoreTank2.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 92), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 77), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 47), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 32), Color.Red);


                }
                else if (locationScoreTank2.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 92), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 77), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 47), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 32), Color.Red);
                }
                else if (locationScoreTank2.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 32), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 47), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 77), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 92), Color.Red);

                }

                else if (locationScoreTank2.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 92), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 77), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 47), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y - 32), Color.Red);
                }
                else if (locationScoreTank2.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 92), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 77), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 47), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y - 32), Color.Red);

                }
                else if (locationScoreTank2.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 32), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 47), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 77), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X - 108, game.tank2.location.Y + 92), Color.Red);

                }
                else if (locationScoreTank2.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Player 2: " + score[1], new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 32), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Armor P2: " + game.tank2.armor, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 47), Color.Red);
                    if (game.gameState == game.gameRunningWyscig)
                        spriteBatch.DrawString(spriteFont, "  Lives P2: Niesmiertelny", new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 62), Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont, "  Lives P2: " + game.tank2.lives, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 62), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Strong P2: " + game.tank2.strong, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 77), Color.Red);
                    spriteBatch.DrawString(spriteFont, "  Mines P2: " + game.tank2.mines, new Vector2(game.tank2.location.X + 14, game.tank2.location.Y + 92), Color.Red);

                }


                game.tank2.colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.tank2.tankRect).depth > 0))
                            {
                                game.tank2.colliding = true;
                                Collision collision2 = tile.isColliding(game.tank2.tankRect);
                                switch (collision2.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreTank2.Equals("LEFT"))
                                            locationScoreTank2 = "TOP_LEFT";
                                        else if (locationScoreTank2.Equals("RIGHT"))
                                            locationScoreTank2 = "TOP_RIGHT";
                                        else
                                            locationScoreTank2 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreTank2.Equals("LEFT"))
                                            locationScoreTank2 = "BOTTOM_LEFT";
                                        else if (locationScoreTank2.Equals("RIGHT"))
                                            locationScoreTank2 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreTank2 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreTank2.Equals("TOP"))
                                            locationScoreTank2 = "TOP_LEFT";
                                        else if (locationScoreTank2.Equals("BOTTOM"))
                                            locationScoreTank2 = "BOTTOM_LEFT";
                                        else
                                            locationScoreTank2 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreTank2.Equals("TOP"))
                                            locationScoreTank2 = "TOP_RIGHT";
                                        else if (locationScoreTank2.Equals("BOTTOM"))
                                            locationScoreTank2 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreTank2 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }


                }


               

            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 1)
            {
                //game.enemyTanks[0]

                if (locationScoreenemyTanks0.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y + 32), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y + 47), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 47), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 32), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 47), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 32), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y + 32), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y + 47), Color.Pink);

                }

                else if (locationScoreenemyTanks0.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y - 47), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y - 32), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 47), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y - 32), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y + 32), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X - 108, game.enemyTanks[0].location.Y + 47), Color.Pink);

                }
                else if (locationScoreenemyTanks0.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU1: " + game.enemyTanks[0].armor, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y + 32), Color.Pink);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU1: " + game.enemyTanks[0].lives, new Vector2(game.enemyTanks[0].location.X + 14, game.enemyTanks[0].location.Y + 47), Color.Pink);

                }


                game.enemyTanks[0].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[0].tankRect).depth > 0))
                            {
                                game.enemyTanks[0].colliding = true;
                                Collision collisionenemyTanks0 = tile.isColliding(game.enemyTanks[0].tankRect);
                                switch (collisionenemyTanks0.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks0.Equals("LEFT"))
                                            locationScoreenemyTanks0 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks0.Equals("RIGHT"))
                                            locationScoreenemyTanks0 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks0 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks0.Equals("LEFT"))
                                            locationScoreenemyTanks0 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks0.Equals("RIGHT"))
                                            locationScoreenemyTanks0 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks0 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks0.Equals("TOP"))
                                            locationScoreenemyTanks0 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks0.Equals("BOTTOM"))
                                            locationScoreenemyTanks0 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks0 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks0.Equals("TOP"))
                                            locationScoreenemyTanks0 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks0.Equals("BOTTOM"))
                                            locationScoreenemyTanks0 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks0 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }
            }
            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 2)
            {

                //game.enemyTanks[1]

                if (locationScoreenemyTanks1.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y + 32), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y + 47), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 47), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 32), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 47), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 32), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y + 32), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y + 47), Color.Yellow);

                }

                else if (locationScoreenemyTanks1.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y - 47), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y - 32), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 47), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y - 32), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y + 32), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X - 108, game.enemyTanks[1].location.Y + 47), Color.Yellow);

                }
                else if (locationScoreenemyTanks1.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU2: " + game.enemyTanks[1].armor, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y + 32), Color.Yellow);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU2: " + game.enemyTanks[1].lives, new Vector2(game.enemyTanks[1].location.X + 14, game.enemyTanks[1].location.Y + 47), Color.Yellow);

                }


                game.enemyTanks[1].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[1].tankRect).depth > 1))
                            {
                                game.enemyTanks[1].colliding = true;
                                Collision collisionenemyTanks1 = tile.isColliding(game.enemyTanks[1].tankRect);
                                switch (collisionenemyTanks1.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks1.Equals("LEFT"))
                                            locationScoreenemyTanks1 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks1.Equals("RIGHT"))
                                            locationScoreenemyTanks1 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks1 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks1.Equals("LEFT"))
                                            locationScoreenemyTanks1 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks1.Equals("RIGHT"))
                                            locationScoreenemyTanks1 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks1 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks1.Equals("TOP"))
                                            locationScoreenemyTanks1 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks1.Equals("BOTTOM"))
                                            locationScoreenemyTanks1 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks1 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks1.Equals("TOP"))
                                            locationScoreenemyTanks1 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks1.Equals("BOTTOM"))
                                            locationScoreenemyTanks1 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks1 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }

            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 3)

            {

                //game.enemyTanks[2]

                if (locationScoreenemyTanks2.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y + 47), Color.Blue);
                }
                else if (locationScoreenemyTanks2.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks2.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks2.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y + 47), Color.Blue);

                }

                else if (locationScoreenemyTanks2.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks2.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks2.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X - 108, game.enemyTanks[2].location.Y + 47), Color.Blue);

                }
                else if (locationScoreenemyTanks2.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU3: " + game.enemyTanks[2].armor, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU3: " + game.enemyTanks[2].lives, new Vector2(game.enemyTanks[2].location.X + 14, game.enemyTanks[2].location.Y + 47), Color.Blue);

                }


                game.enemyTanks[2].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[2].tankRect).depth > 1))
                            {
                                game.enemyTanks[2].colliding = true;
                                Collision collisionenemyTanks2 = tile.isColliding(game.enemyTanks[2].tankRect);
                                switch (collisionenemyTanks2.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks2.Equals("LEFT"))
                                            locationScoreenemyTanks2 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks2.Equals("RIGHT"))
                                            locationScoreenemyTanks2 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks2 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks2.Equals("LEFT"))
                                            locationScoreenemyTanks2 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks2.Equals("RIGHT"))
                                            locationScoreenemyTanks2 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks2 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks2.Equals("TOP"))
                                            locationScoreenemyTanks2 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks2.Equals("BOTTOM"))
                                            locationScoreenemyTanks2 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks2 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks2.Equals("TOP"))
                                            locationScoreenemyTanks2 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks2.Equals("BOTTOM"))
                                            locationScoreenemyTanks2 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks2 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }



            }


            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 4)

            {

                //game.enemyTanks[3]

                if (locationScoreenemyTanks3.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y + 47), Color.Blue);
                }
                else if (locationScoreenemyTanks3.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks3.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks3.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y + 47), Color.Blue);

                }

                else if (locationScoreenemyTanks3.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks3.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks3.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X - 108, game.enemyTanks[3].location.Y + 47), Color.Blue);

                }
                else if (locationScoreenemyTanks3.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU4: " + game.enemyTanks[3].armor, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU4: " + game.enemyTanks[3].lives, new Vector2(game.enemyTanks[3].location.X + 14, game.enemyTanks[3].location.Y + 47), Color.Blue);

                }


                game.enemyTanks[3].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[3].tankRect).depth > 1))
                            {
                                game.enemyTanks[3].colliding = true;
                                Collision collisionenemyTanks3 = tile.isColliding(game.enemyTanks[3].tankRect);
                                switch (collisionenemyTanks3.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks3.Equals("LEFT"))
                                            locationScoreenemyTanks3 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks3.Equals("RIGHT"))
                                            locationScoreenemyTanks3 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks3 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks3.Equals("LEFT"))
                                            locationScoreenemyTanks3 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks3.Equals("RIGHT"))
                                            locationScoreenemyTanks3 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks3 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks3.Equals("TOP"))
                                            locationScoreenemyTanks3 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks3.Equals("BOTTOM"))
                                            locationScoreenemyTanks3 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks3 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks3.Equals("TOP"))
                                            locationScoreenemyTanks3 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks3.Equals("BOTTOM"))
                                            locationScoreenemyTanks3 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks3 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }



            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 5)

            {

                //game.enemyTanks[4]

                if (locationScoreenemyTanks3.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y + 47), Color.Blue);
                }
                else if (locationScoreenemyTanks4.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks4.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks4.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y + 47), Color.Blue);

                }

                else if (locationScoreenemyTanks4.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks4.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks4.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X - 108, game.enemyTanks[4].location.Y + 47), Color.Blue);

                }
                else if (locationScoreenemyTanks4.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU5: " + game.enemyTanks[4].armor, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU5: " + game.enemyTanks[4].lives, new Vector2(game.enemyTanks[4].location.X + 14, game.enemyTanks[4].location.Y + 47), Color.Blue);

                }


                game.enemyTanks[4].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[4].tankRect).depth > 1))
                            {
                                game.enemyTanks[4].colliding = true;
                                Collision collisionenemyTanks4 = tile.isColliding(game.enemyTanks[4].tankRect);
                                switch (collisionenemyTanks4.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks4.Equals("LEFT"))
                                            locationScoreenemyTanks4 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks4.Equals("RIGHT"))
                                            locationScoreenemyTanks4 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks4 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks4.Equals("LEFT"))
                                            locationScoreenemyTanks4 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks4.Equals("RIGHT"))
                                            locationScoreenemyTanks4 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks4 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks4.Equals("TOP"))
                                            locationScoreenemyTanks4 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks4.Equals("BOTTOM"))
                                            locationScoreenemyTanks4 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks4 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks4.Equals("TOP"))
                                            locationScoreenemyTanks4 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks4.Equals("BOTTOM"))
                                            locationScoreenemyTanks4 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks4 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }



            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 6)

            {

                //game.enemyTanks[5]

                if (locationScoreenemyTanks5.Equals("TOP"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y + 47), Color.Blue);
                }
                else if (locationScoreenemyTanks5.Equals("BOTTOM"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks5.Equals("LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks5.Equals("RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y + 47), Color.Blue);

                }

                else if (locationScoreenemyTanks5.Equals("TOP_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks5.Equals("TOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 47), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y - 32), Color.Blue);

                }
                else if (locationScoreenemyTanks5.Equals("BOTTOM_LEFT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X - 108, game.enemyTanks[5].location.Y + 47), Color.Blue);

                }
                else if (locationScoreenemyTanks5.Equals("BOTTOM_RIGHT"))
                {
                    spriteBatch.DrawString(spriteFont, "  Armor CPU6: " + game.enemyTanks[5].armor, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y + 32), Color.Blue);
                    spriteBatch.DrawString(spriteFont, "  Lives CPU6: " + game.enemyTanks[5].lives, new Vector2(game.enemyTanks[5].location.X + 14, game.enemyTanks[5].location.Y + 47), Color.Blue);

                }


                game.enemyTanks[5].colliding = false;
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            if ((tile.isColliding(game.enemyTanks[5].tankRect).depth > 1))
                            {
                                game.enemyTanks[5].colliding = true;
                                Collision collisionenemyTanks5 = tile.isColliding(game.enemyTanks[5].tankRect);
                                switch (collisionenemyTanks5.side)
                                {
                                    case Collision.Side.TOP:
                                        if (locationScoreenemyTanks5.Equals("LEFT"))
                                            locationScoreenemyTanks5 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks5.Equals("RIGHT"))
                                            locationScoreenemyTanks5 = "TOP_RIGHT";
                                        else
                                            locationScoreenemyTanks5 = "TOP";
                                        break;
                                    case Collision.Side.BOTTOM:
                                        if (locationScoreenemyTanks5.Equals("LEFT"))
                                            locationScoreenemyTanks5 = "BOTTOM_LEFT";
                                        else if (locationScoreenemyTanks5.Equals("RIGHT"))
                                            locationScoreenemyTanks5 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks5 = "BOTTOM";

                                        break;
                                    case Collision.Side.LEFT:
                                        if (locationScoreenemyTanks5.Equals("TOP"))
                                            locationScoreenemyTanks5 = "TOP_LEFT";
                                        else if (locationScoreenemyTanks5.Equals("BOTTOM"))
                                            locationScoreenemyTanks5 = "BOTTOM_LEFT";
                                        else
                                            locationScoreenemyTanks5 = "LEFT";

                                        break;
                                    case Collision.Side.RIGHT:
                                        if (locationScoreenemyTanks5.Equals("TOP"))
                                            locationScoreenemyTanks5 = "TOP_RIGHT";
                                        else if (locationScoreenemyTanks5.Equals("BOTTOM"))
                                            locationScoreenemyTanks5 = "BOTTOM_RIGHT";
                                        else
                                            locationScoreenemyTanks5 = "RIGHT";

                                        break;

                                }
                            }
                        }
                        else { continue; }

                    }
                }



            }
        }
    }
}
    

