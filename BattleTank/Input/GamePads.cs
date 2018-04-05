using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleTank.Input
{
    static class GamePads
    {

        public static List<ITankActionProvider> GetNextAvailableGamepad()
        {
            List<ITankActionProvider> retVal = new List<ITankActionProvider>();
            if (XInputGamepadTankActionProvider.HowManyAvailable() > 0)
            {
                retVal.AddRange(
                    XInputGamepadTankActionProvider.GetAllAvailable());
            }
            if (GenericGamepadTankActionProvider.HowManyAvailable() > 0)
            {
                retVal.AddRange(
                    GenericGamepadTankActionProvider.GetAllAvailable());
            }
            return retVal;
        }
    }
}
