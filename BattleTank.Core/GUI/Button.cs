using System;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Core.GUI
{
    class Button
    {
        public static SoundEffect ClickSound { get; set; }
        Texture2D NonActiveTexture { get; set; }
        Texture2D ActiveTexture { get; set; }
        public static Effect Effect;
        public Rectangle Position { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; } = true;

        EventHandler Clicked;

        void OnClickedRaised()
            => Clicked?.Invoke(this, EventArgs.Empty);

        public Button (Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle position)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            Position = position;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(IsActive ?
                    ActiveTexture : NonActiveTexture, Position, Color.White);
            }
            else
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Effect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(NonActiveTexture, Position, Color.White);
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        public bool IsMouseOver(ref PointerState mouseState)
        {
            if (!IsEnabled) return false;

            var mousePosition = new Rectangle(mouseState.Location, new Point(1));

            if (mousePosition.Intersects(Position))
                IsActive = true;
            else
                IsActive = false;

            return IsActive;
        }

        public bool IsClicked(ref PointerState mouseState)
        {
            if (IsMouseOver(ref mouseState))
            {
               if (mouseState.MainAction == ButtonState.Pressed)
                {
                    ClickSound?.Play();
                    OnClickedRaised();
                    return true;
                }
            }
            return false;
        }
    }
}
