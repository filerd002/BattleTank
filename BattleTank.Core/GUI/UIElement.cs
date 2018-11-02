using System;
using System.Collections.Generic;
using System.Text;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Core.GUI
{
    public abstract class UIElement
    {
        public Vector2 Position
        {
            get => UIElementRectangle.Location.ToVector2();
            set => UIElementRectangle.Location = value.ToPoint();
        }
        public double Width
        {
            get => UIElementRectangle.Width;
            set => UIElementRectangle.Width = (int)value;
        }
        public double Height
        {
            get => UIElementRectangle.Height;
            set => UIElementRectangle.Height = (int)value;
        }

        public bool IsMouseOver { get; set; }
        public bool IsEnabled { get; set; } = true;
        
        protected Texture2D TextureToDraw { get; set; }
        internal Rectangle UIElementRectangle;

        public EventHandler<UIElement> Clicked;
        protected virtual void OnClickedRaised() => Clicked?.Invoke(this, this);

        public EventHandler<UIElement> MouseOver;
        protected virtual void OnMouseOverRaised() => MouseOver?.Invoke(this, this);

        public static Effect Effect { get; set; }
        public static SpriteFont InActiveFont { get; set; }
        public static SpriteFont ActiveFont { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
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

        public bool IsClickedLeftButton(ref PointerState mouseState)
        {
            if (!CheckIsMouseOver(ref mouseState)) return false;
            if (mouseState.LeftButtonAction != ButtonState.Pressed) return false;

            OnClickedRaised();
            return true;
        }
    }
}
