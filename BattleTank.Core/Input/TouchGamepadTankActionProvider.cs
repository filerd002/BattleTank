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
            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count == 0)
            {
                Id = -1;
                StartPosition = Vector2.Zero;
                return retVal;
            }

            foreach (TouchLocation touch in touchState)
            {
                if (touch.State != TouchLocationState.Moved) continue;
                if (touch.Id != Id && Id != -1) continue;

                Id = touch.Id;
                if (StartPosition == Vector2.Zero)
                {
                    if (touch.TryGetPreviousLocation(out TouchLocation previousLocation))
                    {
                        StartPosition = previousLocation.Position;
                    }
                }

                float xMove = StartPosition.X - touch.Position.X;
                xMove = (Math.Abs(xMove) > Size ? 1 * Math.Sign(xMove) : xMove / Size);

                float yMove = StartPosition.Y - touch.Position.Y;
                yMove = (Math.Abs(yMove) > Size ? 1 * Math.Sign(yMove) : yMove / Size);

                retVal = new TankControllerState(-xMove, yMove);
            }
            return retVal;
        }
    }
}
