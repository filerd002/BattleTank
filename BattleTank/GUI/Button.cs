using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleTank.GUI
{
    class Button
    {
        public static SoundEffect ClickSound { get; set; }
        Texture2D NonActiveTexture { get; set; }
        Texture2D ActiveTexture { get; set; }
        public static Effect Effect;
        Rectangle DestinationRectangle { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; } = true;

        EventHandler Clicked;

        void OnClickedRaised()
            => Clicked?.Invoke(this, EventArgs.Empty);

        public Button (Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle destinationRectangle)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            DestinationRectangle = destinationRectangle;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(IsActive ?
                    ActiveTexture : NonActiveTexture, DestinationRectangle, Color.White);
            }
            else
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Effect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(NonActiveTexture, DestinationRectangle, Color.White);
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        public bool IsMouseOver(ref MouseState mouseState)
        {
            if (!IsEnabled) return false;

            var mousePosition = new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1);

            if (mousePosition.Intersects(DestinationRectangle))
                IsActive = true;
            else
                IsActive = false;

            return IsActive;
        }

        public bool IsClicked(ref MouseState mouseState)
        {
            if (IsMouseOver(ref mouseState))
            {
               if (mouseState.LeftButton == ButtonState.Pressed)
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
