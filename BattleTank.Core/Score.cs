using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Score
    {
        private Game1 Game { get; set; }
        private readonly int[][] score;
        private int NumOfPlayers { get; set; }

        private SpriteFont spriteFont;
        private SpriteFont spriteFontBig;
        public Score() { }

        public Score(Game1 _game, int _numOfPlayers)
        {
            Game = _game;
            NumOfPlayers = _numOfPlayers;
            score = new int[NumOfPlayers][];
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
            spriteFont = Game.Content.Load<SpriteFont>("Arial");
            spriteFontBig = Game.Content.Load<SpriteFont>("ArialBig");
        }
        public int GetScore(int playerIndex)
        {
            return score[playerIndex][3]*1000 + score[playerIndex][2] * 100 + score[playerIndex][1] * 10 + score[playerIndex][0];
        }
        public void AddScore(int playerIndex, int pointsToAdd)
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

        private Texture2D rect;


        public void Draw(SpriteBatch spriteBatch)
        {

            void DrawRectangle(Rectangle coords, Color color)
            {
                if (rect == null)
                {
                    rect = new Texture2D(Game.Graphics.GraphicsDevice, 1, 1);
                    rect.SetData(new[] { Color.White });
                }
                spriteBatch.Draw(rect, coords, color);
            }


            


            DrawRectangle(new Rectangle((int)Game.Tank1.location.X-20, (int)Game.Tank1.location.Y - 22, (int)(16 * (Game.Tank1.Lives)), 4), new Color(138, 7, 7));
            DrawRectangle(new Rectangle((int)Game.Tank1.location.X - 20, (int)Game.Tank1.location.Y - 22, 1, 4), Color.Yellow);
            for (int i = 0; i < Game.Tank1.Lives-1; i++)
            {
                if (i == 0)
                {
                    DrawRectangle(new Rectangle((int)Game.Tank1.location.X - 4, (int)Game.Tank1.location.Y - 22, 1, 4), Color.Yellow);
                }
                else
                    DrawRectangle(new Rectangle((int)Game.Tank1.location.X - 4 + (16 * i), (int)Game.Tank1.location.Y - 22, 1, 4), Color.Yellow);

            }
            DrawRectangle(new Rectangle((int)Game.Tank1.location.X-20, (int)Game.Tank1.location.Y - 27, (int)(16 * (Game.Tank1.Armor)), 4), Color.Gray);

            if (Game.Tank1.Mines < 4)
            {
                if (Game.Tank1.Mines >= 1)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineGreen"), new Rectangle((int)Game.Tank1.location.X-22, (int)Game.Tank1.location.Y - 50, 12, 12), Color.White);
                if (Game.Tank1.Mines >= 2)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineGreen"), new Rectangle((int)Game.Tank1.location.X-8, (int)Game.Tank1.location.Y - 50, 12, 12), Color.White);
                if (Game.Tank1.Mines >= 3)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineGreen"), new Rectangle((int)Game.Tank1.location.X+6, (int)Game.Tank1.location.Y - 50, 12, 12), Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, Game.Tank1.Mines + "x", new Vector2((int)Game.Tank1.location.X -19, (int)Game.Tank1.location.Y - 50), Color.White);

                spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineGreen"), new Rectangle((int)Game.Tank1.location.X -2, (int)Game.Tank1.location.Y - 50, 12, 12), Color.White);
            }
            if (Game.Tank1.Strong < 4)
            {
                if (Game.Tank1.Strong >= 1)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank1.location.X-22, (int)Game.Tank1.location.Y - 40, 12, 12), Color.White);
                if (Game.Tank1.Strong >= 2)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank1.location.X -8, (int)Game.Tank1.location.Y - 40, 12, 12), Color.White);
                if (Game.Tank1.Strong >= 3)
                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank1.location.X + 6, (int)Game.Tank1.location.Y - 40, 12, 12), Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, Game.Tank1.Strong + "x", new Vector2((int)Game.Tank1.location.X - 19, (int)Game.Tank1.location.Y - 40), Color.White);

                spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank1.location.X -2, (int)Game.Tank1.location.Y - 40, 12, 12), Color.White);
            }

            


                if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYERS_2 || Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_PLAYERS_2_AND_CPU || Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_RACE)
            {

              



                    if (Game.GameStateCurrent == Game1.GameState.GAME_RUNNING_RACE)
                {

                    TimeSpan time = TimeSpan.FromSeconds(Game.Settings.RaceTimeCurrent);
                    spriteBatch.DrawString(spriteFontBig, time.ToString("mm':'ss"), new Vector2((Game.Map.ScreenWidth / 2) - 27, 65), Color.White);
                    spriteBatch.DrawString(spriteFontBig, score[0][3].ToString() + "" + score[0][2].ToString() + "" + score[0][1].ToString() + "" + score[0][0].ToString(), new Vector2((Game.Map.ScreenWidth / 2) - 97, 65), Color.Green);
                    spriteBatch.DrawString(spriteFontBig, score[1][3].ToString() + "" + score[1][2].ToString() + "" + score[1][1].ToString() + "" + score[1][0].ToString(), new Vector2((Game.Map.ScreenWidth / 2) + 50, 65), Color.Red);



                }


                DrawRectangle(new Rectangle((int)Game.Tank2.location.X - 20, (int)Game.Tank2.location.Y - 22, (int)(16 * (Game.Tank2.Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.Tank2.location.X - 20, (int)Game.Tank2.location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.Tank2.Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.Tank2.location.X - 4, (int)Game.Tank2.location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.Tank2.location.X - 4 + (16 * i), (int)Game.Tank2.location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.Tank2.location.X - 20, (int)Game.Tank2.location.Y - 27, (int)(16 * (Game.Tank2.Armor)), 4), Color.Gray);

                if (Game.Tank2.Mines < 4)
                {
                    if (Game.Tank2.Mines >= 1)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineRed"), new Rectangle((int)Game.Tank2.location.X - 22, (int)Game.Tank2.location.Y - 50, 12, 12), Color.White);
                    if (Game.Tank2.Mines >= 2)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineRed"), new Rectangle((int)Game.Tank2.location.X - 8, (int)Game.Tank2.location.Y - 50, 12, 12), Color.White);
                    if (Game.Tank2.Mines >= 3)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineRed"), new Rectangle((int)Game.Tank2.location.X + 6, (int)Game.Tank2.location.Y - 50, 12, 12), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, Game.Tank2.Mines + "x", new Vector2((int)Game.Tank2.location.X - 19, (int)Game.Tank2.location.Y - 50), Color.White);

                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/mineRed"), new Rectangle((int)Game.Tank2.location.X - 2, (int)Game.Tank2.location.Y - 50, 12, 12), Color.White);
                }
                if (Game.Tank2.Strong < 4)
                {
                    if (Game.Tank2.Strong >= 1)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank2.location.X - 22, (int)Game.Tank2.location.Y - 40, 12, 12), Color.White);
                    if (Game.Tank2.Strong >= 2)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank2.location.X - 8, (int)Game.Tank2.location.Y - 40, 12, 12), Color.White);
                    if (Game.Tank2.Strong >= 3)
                        spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank2.location.X + 6, (int)Game.Tank2.location.Y - 40, 12, 12), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, Game.Tank2.Strong + "x", new Vector2((int)Game.Tank2.location.X - 19, (int)Game.Tank2.location.Y - 40), Color.White);

                    spriteBatch.Draw(Game.Content.Load<Texture2D>("Graphics/Ammo"), new Rectangle((int)Game.Tank2.location.X - 2, (int)Game.Tank2.location.Y - 40, 12, 12), Color.White);
                }



            }

            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 1)
            {
             
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[0].location.X - 20, (int)Game.EnemyTanks[0].location.Y - 22, (int)(16 * (Game.EnemyTanks[0].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[0].location.X - 20, (int)Game.EnemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[0].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[0].location.X - 4, (int)Game.EnemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[0].location.X - 4 + (16 * i), (int)Game.EnemyTanks[0].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[0].location.X - 20, (int)Game.EnemyTanks[0].location.Y - 27, (int)(16 * (Game.EnemyTanks[0].Armor)), 4), Color.Gray);



            }
            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 2)
            {

               
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[1].location.X - 20, (int)Game.EnemyTanks[1].location.Y - 22, (int)(16 * (Game.EnemyTanks[1].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[1].location.X - 20, (int)Game.EnemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[1].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[1].location.X - 4, (int)Game.EnemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[1].location.X - 4 + (16 * i), (int)Game.EnemyTanks[1].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[1].location.X - 20, (int)Game.EnemyTanks[1].location.Y - 27, (int)(16 * (Game.EnemyTanks[1].Armor)), 4), Color.Gray);


            }

            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 3)

            {

               

                DrawRectangle(new Rectangle((int)Game.EnemyTanks[2].location.X - 20, (int)Game.EnemyTanks[2].location.Y - 22, (int)(16 * (Game.EnemyTanks[2].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[2].location.X - 20, (int)Game.EnemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[2].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[2].location.X - 4, (int)Game.EnemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[2].location.X - 4 + (16 * i), (int)Game.EnemyTanks[2].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[2].location.X - 20, (int)Game.EnemyTanks[2].location.Y - 27, (int)(16 * (Game.EnemyTanks[2].Armor)), 4), Color.Gray);



            }


            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 4)

            {

              
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[3].location.X - 20, (int)Game.EnemyTanks[3].location.Y - 22, (int)(16 * (Game.EnemyTanks[3].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[3].location.X - 20, (int)Game.EnemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[3].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[3].location.X - 4, (int)Game.EnemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[3].location.X - 4 + (16 * i), (int)Game.EnemyTanks[3].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[3].location.X - 20, (int)Game.EnemyTanks[3].location.Y - 27, (int)(16 * (Game.EnemyTanks[3].Armor)), 4), Color.Gray);



            }

            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 5)

            {

              

                DrawRectangle(new Rectangle((int)Game.EnemyTanks[4].location.X - 20, (int)Game.EnemyTanks[4].location.Y - 22, (int)(16 * (Game.EnemyTanks[4].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[4].location.X - 20, (int)Game.EnemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[4].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[4].location.X - 4, (int)Game.EnemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[4].location.X - 4 + (16 * i), (int)Game.EnemyTanks[4].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[4].location.X - 20, (int)Game.EnemyTanks[4].location.Y - 27, (int)(16 * (Game.EnemyTanks[4].Armor)), 4), Color.Gray);



            }

            if (Game.Settings.OpponentsCPUClassic + Game.Settings.OpponentsCPUKamikaze >= 6)

            {

             

                DrawRectangle(new Rectangle((int)Game.EnemyTanks[5].location.X - 20, (int)Game.EnemyTanks[5].location.Y - 22, (int)(16 * (Game.EnemyTanks[5].Lives)), 4), new Color(138, 7, 7));
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[5].location.X - 20, (int)Game.EnemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);
                for (int i = 0; i < Game.EnemyTanks[5].Lives - 1; i++)
                {
                    if (i == 0)
                    {
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[5].location.X - 4, (int)Game.EnemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);
                    }
                    else
                        DrawRectangle(new Rectangle((int)Game.EnemyTanks[5].location.X - 4 + (16 * i), (int)Game.EnemyTanks[5].location.Y - 22, 1, 4), Color.Yellow);

                }
                DrawRectangle(new Rectangle((int)Game.EnemyTanks[5].location.X - 20, (int)Game.EnemyTanks[5].location.Y - 27, (int)(16 * (Game.EnemyTanks[5].Armor)), 4), Color.Gray);



            }
        }
    }
}


