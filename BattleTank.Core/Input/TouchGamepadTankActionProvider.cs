using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace BattleTank.Core.Input
{
    class TouchGamepadTankActionProvider : ITankActionProvider
    {
        public float Size = 300;
        public Vector2 StartPosition = Vector2.Zero;
        public int Id = -1;

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            TankControllerState retVal = new TankControllerState(0, 0);
            var touchState = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();
            if (touchState.Count == 0)
            {
                Id = -1;
                StartPosition = Vector2.Zero;
                return retVal;
            }

            foreach (var touch in touchState)
            {
                if (touch.State == TouchLocationState.Moved)
                {
                    if (touch.Id == Id || Id == -1)
                    {
                        Id = touch.Id;
                        if (StartPosition == Vector2.Zero)
                        {
                            if (touch.TryGetPreviousLocation(out TouchLocation previousLocation))
                            {
                                StartPosition = previousLocation.Position;
                            }
                        }
                        retVal = new TankControllerState(StartPosition.X - touch.Position.X > 0 ? -1 : 1, StartPosition.Y - touch.Position.Y > 0 ? 1 : -1) ;

                    }

                }
            }
            return retVal;
        }
    }
}
