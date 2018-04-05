using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Input
{
    class XInputGamepadTankActionProvider : ITankActionProvider
    {
        public Buttons SpeedBoostButton { get; set; }
        public Buttons PlantMineButton { get; set; }
        public Buttons FireButton { get; set; }
        public PlayerIndex PadNumber { get; set; }

        public XInputGamepadTankActionProvider(PlayerIndex padNumber, Buttons speedBoostButton = Buttons.A, Buttons plantMineButton = Buttons.X, Buttons fireButton = Buttons.RightShoulder)
        {
            PadNumber = padNumber;
            SpeedBoostButton = speedBoostButton;
            PlantMineButton = plantMineButton;
            FireButton = fireButton;

            if (!GamePad.GetState(PadNumber).IsConnected)
                throw new Exception($"Wybrany pad ({padNumber}) nie jest podłączony do komputera!");
        }
        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);

            if (!state.IsConnected)
            {
                // TODO: Polepszyć obsługę wyjątków.
                throw new Exception($"Próba pobrania danych z kontrolera ({PadNumber}), który najwidoczniej został odłączony!");
            }

            float moveX = state.ThumbSticks.Left.X;
            float moveY = state.ThumbSticks.Left.Y;

            bool speedBost = state.IsButtonDown(SpeedBoostButton);
            bool plantMine = state.IsButtonDown(PlantMineButton);
            bool fire = state.IsButtonDown(FireButton);

            return new TankControllerState(
                moveX: moveX,
                moveY: moveY,
                speedBoost: speedBost,
                plantMine: plantMine,
                fire: fire);
        }

        public void Vibrate(float power)
        {
            GamePad.SetVibration(PadNumber, power, power);
        }

        public static bool IsXGamePadAvailable(PlayerIndex padNo)
        {
            return GamePad.GetState(padNo).IsConnected;
        }

        public static int HowManyAvailable()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!IsXGamePadAvailable((PlayerIndex)i))
                {
                    return i;
                }
            }
            return 0;
        }

        public static List<ITankActionProvider> GetAllAvailable()
        {
            var retVal = new List<ITankActionProvider>();
            for (int i = 0; i < HowManyAvailable(); i++)
            {
                retVal.Add(new XInputGamepadTankActionProvider((PlayerIndex)i));
            }
            return retVal;
        }

    }
}
