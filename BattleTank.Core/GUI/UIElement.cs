using System;
using System.Collections.Generic;
using System.Text;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.GUI
{
    abstract class UIElement
    {
        public Vector2 Position
        {
            get => UIElementRectangle.Location.ToVector2();
            set => UIElementRectangle.Location = value.ToPoint();
        }
        public int Width
        {
            get => UIElementRectangle.Width;
            set => UIElementRectangle.Width = value;
        }
        public int Height
        {
            get => UIElementRectangle.Height;
            set => UIElementRectangle.Height = value;
        }

        public static Effect Effect { get; set; }
        public bool IsMouseOver { get; set; }
        public bool IsEnabled { get; set; } = true;

        internal Rectangle UIElementRectangle;
        protected Texture2D TextureToDraw { get; set; }

        EventHandler<UIElement> Clicked;

        protected virtual void OnClickedRaised()
            => Clicked?.Invoke(this, this);

        private EventHandler<UIElement> MouseOver;

        protected virtual void OnMouseOverRaised()
            => MouseOver?.Invoke(this, this);

        protected UIElement(Texture2D textureToDraw)
        {
            TextureToDraw = textureToDraw;
        }

        public bool CheckIsMouseOver(ref PointerState mouseState)
        {
            if (!IsEnabled) return false;
            
            IsMouseOver = UIElementRectangle.Contains(mouseState.ToVector2());

            if (IsMouseOver) OnMouseOverRaised();
            return IsMouseOver;
        }

        public virtual void Draw(ref SpriteBatch spriteBatch)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(TextureToDraw, UIElementRectangle, Color.White);
            }
            else
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Effect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(TextureToDraw, UIElementRectangle, Color.White);
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

    }
}
