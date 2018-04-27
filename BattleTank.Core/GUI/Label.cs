using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.GUI
{
    class Label : UIElement
    {
        /// <summary>
        /// Tworzy nową etykieta na podstawie podanego tekstu w zadanym miejscu o dane szerokości i wysokosci.
        /// Jeżeli jeden z parametrów width lub height jest null a drugi liczbą etykieta zostanie automatycznie 
        /// wyskalowany proporcjonalnie.
        /// </summary>
        /// <param name="text">Tekst dla etykiety</param>
        /// <param name="position">Położenie etykiety</param>
        /// <param name="width">Szerokosć etykiety</param>
        /// <param name="height">Wysokość etykiety</param>
        public Label(string text, Vector2 position, int? width = null, int? height = null) : base(null)
        {
            TextureToDraw = GUIHelper.DrawStringOnTexture2D(text, UIElement.InActiveFont, null, GraphicsDevice);
            UIElementRectangle.Location = position.ToPoint();

            double proportion = 1;
            if (width != null && height == null)
                proportion = (double)width / TextureToDraw.Width;
            else if (width == null && height != null)
                proportion = (double)height / TextureToDraw.Height;

            base.Width = width ?? TextureToDraw.Width * proportion;
            base.Height = height ?? TextureToDraw.Height * proportion;
        }
    }
}