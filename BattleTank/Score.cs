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
                score[playerIndex][1] = 0;
                score[playerIndex][2] += pointsToAdd;
            }
            else
                score[playerIndex][1] += pointsToAdd;
        }

        private static Texture2D rect;


        public void Draw(SpriteBatch spriteBatch)
        {

            void DrawRectangle(Rectangle coords, Color color)
            {
                if (rect == null)
                {
                    rect = new Texture2D(game.graphics.GraphicsDevice, 1, 1);
                    rect.SetData(new[] { Color.White });
                }
                spriteBatch.Draw(rect, coords, color);
            }


            


            DrawRectangle(new Rectangle((int)game.tank1.location.X-20, (int)game.tank1.location.Y - 22, (int)(16 * (game.tank1.lives)), 4), new Color(138, 7, 7));
            DrawRectangle(new Rectangle((int)game.tank1.location.X - 20, (int)game.tank1.location.Y - 22, 1, 4), Color.Yellow);
            for (int i = 0; i < game.tank1.lives-1; i++)
            {
                if (i == 0)
                {
                    DrawRectangle(new Rectangle((int)game.tank1.location.X - 4, (int)game.tank1.location.Y - 22, 1, 4), Color.Yellow);
                }
                else
                    DrawRectangle(new Rectangle((int)game.tank1.location.X - 4 + (16 * i), (int)game.tank1.location.Y - 22, 1, 4), Color.Yellow);

            }
            DrawRectangle(new Rectangle((int)game.tank1.location.X-20, (int)game.tank1.location.Y - 27, (int)(16 * (game.tank1.armor)), 4), Color.Gray);

            if (game.tank1.mines < 4)
            {
                if (game.tank1.mines >= 1)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineGreen"), new Rectangle((int)game.tank1.location.X-22, (int)game.tank1.location.Y - 50, 12, 12), Color.White);
                if (game.tank1.mines >= 2)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineGreen"), new Rectangle((int)game.tank1.location.X-8, (int)game.tank1.location.Y - 50, 12, 12), Color.White);
                if (game.tank1.mines >= 3)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineGreen"), new Rectangle((int)game.tank1.location.X+6, (int)game.tank1.location.Y - 50, 12, 12), Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, game.tank1.mines + "x", new Vector2((int)game.tank1.location.X -19, (int)game.tank1.location.Y - 50), Color.White);

                spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineGreen"), new Rectangle((int)game.tank1.location.X -2, (int)game.tank1.location.Y - 50, 12, 12), Color.White);
            }
            if (game.tank1.strong < 4)
            {
                if (game.tank1.strong >= 1)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank1.location.X-22, (int)game.tank1.location.Y - 40, 12, 12), Color.White);
                if (game.tank1.strong >= 2)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank1.location.X -8, (int)game.tank1.location.Y - 40, 12, 12), Color.White);
                if (game.tank1.strong >= 3)
                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank1.location.X + 6, (int)game.tank1.location.Y - 40, 12, 12), Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, game.tank1.strong + "x", new Vector2((int)game.tank1.location.X - 19, (int)game.tank1.location.Y - 40), Color.White);

                spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank1.location.X -2, (int)game.tank1.location.Y - 40, 12, 12), Color.White);
            }

            


                if (game.gameState == game.gameRunningPlayers2 || game.gameState == game.gameRunningPlayers2andCPU || game.gameState == game.gameRunningWyscig)
            {

              



                    if (game.gameState == game.gameRunningWyscig)
                {

                    TimeSpan time = TimeSpan.FromSeconds(game.czasWyscigu);
                    spriteBatch.DrawString(spriteFontBig, time.ToString("mm':'ss"), new Vector2((game.map.screenWidth / 2) - 27, 65), Color.White);
                    spriteBatch.DrawString(spriteFontBig, score[0][3].ToString() + "" + score[0][2].ToString() + "" + score[0][1].ToString() + "" + score[0][0].ToString(), new Vector2((game.map.screenWidth / 2) - 97, 65), Color.Green);
                    spriteBatch.DrawString(spriteFontBig, score[1][3].ToString() + "" + score[1][2].ToString() + "" + score[1][1].ToString() + "" + score[1][0].ToString(), new Vector2((game.map.screenWidth / 2) + 50, 65), Color.Red);



                }


                DrawRectangle(new Rectangle((int)game.tank2.location.X - 20, (int)game.tank2.location.Y - 22, (int)(16 * (game.tank2.lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.tank2.location.X - 20, (int)game.tank2.location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.tank2.lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.tank2.location.X - 4, (int)game.tank2.location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.tank2.location.X - 4 + (16 * i), (int)game.tank2.location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.tank2.location.X - 20, (int)game.tank2.location.Y - 27, (int)(16 * (game.tank2.armor)), 4), Color.Gray);

                if (game.tank2.mines < 4)
                {
                    if (game.tank2.mines >= 1)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineRed"), new Rectangle((int)game.tank2.location.X - 22, (int)game.tank2.location.Y - 50, 12, 12), Color.White);
                    if (game.tank2.mines >= 2)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineRed"), new Rectangle((int)game.tank2.location.X - 8, (int)game.tank2.location.Y - 50, 12, 12), Color.White);
                    if (game.tank2.mines >= 3)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineRed"), new Rectangle((int)game.tank2.location.X + 6, (int)game.tank2.location.Y - 50, 12, 12), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, game.tank2.mines + "x", new Vector2((int)game.tank2.location.X - 19, (int)game.tank2.location.Y - 50), Color.White);

                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//mineRed"), new Rectangle((int)game.tank2.location.X - 2, (int)game.tank2.location.Y - 50, 12, 12), Color.White);
                }
                if (game.tank2.strong < 4)
                {
                    if (game.tank2.strong >= 1)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank2.location.X - 22, (int)game.tank2.location.Y - 40, 12, 12), Color.White);
                    if (game.tank2.strong >= 2)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank2.location.X - 8, (int)game.tank2.location.Y - 40, 12, 12), Color.White);
                    if (game.tank2.strong >= 3)
                        spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank2.location.X + 6, (int)game.tank2.location.Y - 40, 12, 12), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, game.tank2.strong + "x", new Vector2((int)game.tank2.location.X - 19, (int)game.tank2.location.Y - 40), Color.White);

                    spriteBatch.Draw(game.Content.Load<Texture2D>("Graphics//Ammo"), new Rectangle((int)game.tank2.location.X - 2, (int)game.tank2.location.Y - 40, 12, 12), Color.White);
                }



            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 1)
            {
             
                DrawRectangle(new Rectangle((int)game.enemyTanks[0].location.X - 20, (int)game.enemyTanks[0].location.Y - 22, (int)(16 * (game.enemyTanks[0].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[0].location.X - 20, (int)game.enemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[0].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[0].location.X - 4, (int)game.enemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[0].location.X - 4 + (16 * i), (int)game.enemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[0].location.X - 20, (int)game.enemyTanks[0].location.Y - 27, (int)(16 * (game.enemyTanks[0].armor)), 4), Color.Gray);



            }
            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 2)
            {

               
                DrawRectangle(new Rectangle((int)game.enemyTanks[1].location.X - 20, (int)game.enemyTanks[1].location.Y - 22, (int)(16 * (game.enemyTanks[1].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[1].location.X - 20, (int)game.enemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[1].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[1].location.X - 4, (int)game.enemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[1].location.X - 4 + (16 * i), (int)game.enemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[1].location.X - 20, (int)game.enemyTanks[1].location.Y - 27, (int)(16 * (game.enemyTanks[1].armor)), 4), Color.Gray);


            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 3)

            {

               

                DrawRectangle(new Rectangle((int)game.enemyTanks[2].location.X - 20, (int)game.enemyTanks[2].location.Y - 22, (int)(16 * (game.enemyTanks[2].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[2].location.X - 20, (int)game.enemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[2].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[2].location.X - 4, (int)game.enemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[2].location.X - 4 + (16 * i), (int)game.enemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[2].location.X - 20, (int)game.enemyTanks[2].location.Y - 27, (int)(16 * (game.enemyTanks[2].armor)), 4), Color.Gray);



            }


            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 4)

            {

              
                DrawRectangle(new Rectangle((int)game.enemyTanks[3].location.X - 20, (int)game.enemyTanks[3].location.Y - 22, (int)(16 * (game.enemyTanks[3].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[3].location.X - 20, (int)game.enemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[3].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[3].location.X - 4, (int)game.enemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[3].location.X - 4 + (16 * i), (int)game.enemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[3].location.X - 20, (int)game.enemyTanks[3].location.Y - 27, (int)(16 * (game.enemyTanks[3].armor)), 4), Color.Gray);



            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 5)

            {

              

                DrawRectangle(new Rectangle((int)game.enemyTanks[4].location.X - 20, (int)game.enemyTanks[4].location.Y - 22, (int)(16 * (game.enemyTanks[4].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[4].location.X - 20, (int)game.enemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[4].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[4].location.X - 4, (int)game.enemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[4].location.X - 4 + (16 * i), (int)game.enemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[4].location.X - 20, (int)game.enemyTanks[4].location.Y - 27, (int)(16 * (game.enemyTanks[4].armor)), 4), Color.Gray);



            }

            if (game.iloscCPUKlasyk + game.iloscCPUKamikaze >= 6)

            {

             

                DrawRectangle(new Rectangle((int)game.enemyTanks[5].location.X - 20, (int)game.enemyTanks[5].location.Y - 22, (int)(16 * (game.enemyTanks[5].lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)game.enemyTanks[5].location.X - 20, (int)game.enemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < game.enemyTanks[5].lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)game.enemyTanks[5].location.X - 4, (int)game.enemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)game.enemyTanks[5].location.X - 4 + (16 * i), (int)game.enemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)game.enemyTanks[5].location.X - 20, (int)game.enemyTanks[5].location.Y - 27, (int)(16 * (game.enemyTanks[5].armor)), 4), Color.Gray);



            }
        }
    }
}


