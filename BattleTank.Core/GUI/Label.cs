using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.GUI
{
    class Label : UIElement
    {
      
        Texture2D NonActiveTexture { get; set; }
 
        [Obsolete]
        public Label(Texture2D nonActiveTexture, Rectangle elementRectangle) : base(nonActiveTexture)
        {
            NonActiveTexture = nonActiveTexture;
            Position = elementRectangle.Location.ToVector2();
            Width = elementRectangle.Width;
            Height = elementRectangle.Height;
        }
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
            NonActiveTexture = GUIHelper.DrawStringOnTexture2D(text, UIElement.InActiveFont, null, GraphicsDevice);
            UIElementRectangle.Location = position.ToPoint();

            double proportion = 1;
            if (width != null && height == null)
                proportion = (double)width / NonActiveTexture.Width;
            else if (width == null && height != null)
                proportion = (double)height / NonActiveTexture.Height;

            base.Width = width ?? NonActiveTexture.Width * proportion;
            base.Height = height ?? NonActiveTexture.Height * proportion;
        }

        /// <inheritdoc />
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            base.TextureToDraw = NonActiveTexture;
            base.Draw(ref spriteBatch);
        }

        public void CenterHorizontal()
            => Position = new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - (float)Width / 2, Position.Y);

    }
}