﻿using System.Collections.Generic;

namespace BattleTank.Core.Input
{
    static class GamePads
    {
        public static List<ITankActionProvider> GetAllAvailableGamepads()
        {
            List<ITankActionProvider> retVal = new List<ITankActionProvider>();
            if (XInputGamepadTankActionProvider.HowManyAvailable() > 0)
            {
                retVal.AddRange(XInputGamepadTankActionProvider.GetAllAvailable());
            }
#if WINDOWS
            if (GenericGamepadTankActionProvider.HowManyAvailable() > 0)
            {
                retVal.AddRange(GenericGamepadTankActionProvider.GetAllAvailable());
            }
#endif
            return retVal;
        }
    }
}