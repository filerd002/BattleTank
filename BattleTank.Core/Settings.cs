using System;
using System.Collections.Generic;
using System.Text;

namespace BattleTank.Core
{
    public class Settings
    {
        public DifficultyLevel DifficultyLevelCurrent { get; set; } = DifficultyLevel.Easy;
        public float RaceTimeCurrent { get; set; } = (float)RaceTime.Minutes_2;

        public int ElementsOnTheHeight { get; set; } = 23;
        public int ElementsOnTheWidth { get; set; } = 41;
        public int OpponentsCPUKamikaze { get; set; } = 1;
        public int OpponentsCPUClassic { get; set; } = 1;

        public double RatioOfWidthtOfFrameToScreen { get; set; } = 0.75;
        public double RatioOfHeightOfFrameToScreen { get; set; } = 1.3;

        public bool Widescreen { get; set; } = true;

        public enum RaceTime
        {
            Minutes_2 = 120,
            Minutes_5 = 300,
            Minutes_10 = 600,
        }

        public enum DifficultyLevel
        {
            Easy = 1,
            Medium = 2,
            Hard = 3,
            Impossible = 4
        }

    }
}
