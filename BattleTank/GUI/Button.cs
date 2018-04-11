﻿using Microsoft.Xna.Framework;
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
        Texture2D NonActiveTexture { get; set; }
        Texture2D ActiveTexture { get; set; }
        Rectangle DestinationRectangle { get; set; }
        bool IsActive { get; set; }

        EventHandler Clicked;

        void OnClickedRaised()
            => Clicked?.Invoke(this, EventArgs.Empty);

        Button (Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle destinationRectangle)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            DestinationRectangle = destinationRectangle;
        }

        void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(IsActive ?
                ActiveTexture : NonActiveTexture, DestinationRectangle, Color.White);
        }

        bool IsMouseOver(ref MouseState mouseState)
        {
            var mousePosition = new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1);

            if (mousePosition.Intersects(DestinationRectangle))
                IsActive = true;
            else
                IsActive = false;

            return IsActive;
        }

        bool IsClicked(ref MouseState mouseState)
        {
            if (IsMouseOver(ref mouseState))
            {
               if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    OnClickedRaised();
                    return true;
                }
            }
            return false;
        }
    }
}
