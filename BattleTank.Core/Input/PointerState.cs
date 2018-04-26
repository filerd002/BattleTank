using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SlimDX.DirectSound;

namespace BattleTank.Core.Input
{
    class PointerState
    {
        public float X => Position.X;
        public float Y => Position.Y;
        public Vector2 Position { get; }
        public Point Location => Position.ToPoint();
        public ButtonState MainAction { get; }

        private static bool _isTouchAvailble;

        public PointerState(float x, float y, ButtonState mainAction = ButtonState.Released)
        {
            Position = new Vector2(x, y);
            MainAction = mainAction;

            var touch = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetCapabilities();
            _isTouchAvailble = touch.IsConnected;
        }

        public PointerState(Vector2 position, ButtonState mainAction = ButtonState.Released)
            : this(position.X, position.Y, mainAction)
        {
        }

        public static PointerState GetState()
        {
            MouseState mouseState = Mouse.GetState();
            PointerState retVal = new PointerState(mouseState.X, mouseState.Y, mouseState.LeftButton);

            if (!_isTouchAvailble) return retVal;

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count > 0)
            {
                retVal = new PointerState(touchState[0].Position, ButtonState.Pressed);
            }

            return retVal;
        }

        public Vector2 ToVector2()
            => new Vector2(X, Y);
        
        public Point ToPoint()
            => new Point((int)X, (int)Y);
    }
}
