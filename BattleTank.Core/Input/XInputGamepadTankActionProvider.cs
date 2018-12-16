using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Core.Input
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
            {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new Exception(message: $"Wybrany pad ({padNumber}) nie jest podłączony do komputera!");
#pragma warning restore S112 // General exceptions should never be thrown
            }

        }

        public bool IsConnectedTankController()
        {
            if (GamePad.GetState(PadNumber).IsConnected)
                return true;
            else
                return false;
        }

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            GamePadState state;

            state = GamePad.GetState(PadNumber);
        
               float moveX = state.ThumbSticks.Left.X - (float)(state.DPad.Left) + (float)(state.DPad.Right);
              float moveY = state.ThumbSticks.Left.Y - (float)(state.DPad.Down) + (float)(state.DPad.Up);

                if (Math.Abs(moveX) > 1)
                    moveX = 1;
                if (Math.Abs(moveY) > 1)
                    moveY = 1;    

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
