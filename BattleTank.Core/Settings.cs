using System;
using System.Collections.Generic;
using System.Text;

namespace BattleTank.Core
{
    public class Settings
    {
        public DifficultyLevel difficultyLevel = DifficultyLevel.Easy;
        public float raceTime = (float)RaceTime.Minutes_2;


        public int opponentsCPUClassic = 1;
        public int opponentsCPUKamikaze = 1;

        //map
        public int elementsOnTheWidth = 41;
        public int elementsOnTheHeight = 23;
        

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
