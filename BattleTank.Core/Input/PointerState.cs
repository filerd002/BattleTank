using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SlimDX.DirectSound;

namespace BattleTank.Core.Input
{
    public class PointerState
    {
        public float X => Position.X;
        public float Y => Position.Y;
        public Vector2 Position { get; }
        public Point Location => Position.ToPoint();
        public ButtonState LeftButtonAction { get; }
        public ButtonState RightButtonAction { get; }

        private static bool _isTouchAvailble;

        public PointerState(float x, float y, ButtonState leftButtonAction = ButtonState.Released, ButtonState rightButtonAction = ButtonState.Released)
        {
            Position = new Vector2(x, y);
            LeftButtonAction = leftButtonAction;
            RightButtonAction = rightButtonAction;

            var touch = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetCapabilities();
            _isTouchAvailble = touch.IsConnected;
        }

        public PointerState(Vector2 position, ButtonState leftButtonAction = ButtonState.Released, ButtonState rightButtonAction = ButtonState.Released)
            : this(position.X, position.Y, leftButtonAction, rightButtonAction)
        {
        }

        public static PointerState GetState()
        {
            MouseState mouseState = Mouse.GetState();
            PointerState retVal = new PointerState(mouseState.X, mouseState.Y, mouseState.LeftButton, mouseState.RightButton);

            if (!_isTouchAvailble) return retVal;

            TouchCollection touchState = TouchPanel.GetState();
            foreach (TouchLocation touch in touchState)
            {
                retVal = new PointerState(touch.Position, ButtonState.Pressed);
            }

            return retVal;
        }

        public Vector2 ToVector2()
            => new Vector2(X, Y);
        
        public Point ToPoint()
            => new Point((int)X, (int)Y);
    }
}
