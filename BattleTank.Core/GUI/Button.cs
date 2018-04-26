using System;
using System.Linq;
using BattleTank.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Core.GUI
{
    class Button : UIElement
    {
        public static SoundEffect ClickSound { get; set; }
        Texture2D NonActiveTexture { get; set; }
        Texture2D ActiveTexture { get; set; }
        
        [Obsolete]
        public Button(Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle elementRectangle) :base(nonActiveTexture)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            Position = elementRectangle.Location.ToVector2();
            Width = elementRectangle.Width;
            Height = elementRectangle.Height;
        }
        /// <summary>
        /// Tworzy nowy przycisk na podstawie podanego tekstu w zadanym miejscu o dane szerokości i wysokosci.
        /// Jeżeli jeden z parametrów width lub height jest null a drugi liczbą przycisk zostanie automatycznie 
        /// wyskalowany proporcjonalnie.
        /// </summary>
        /// <param name="text">Tekst dla przycisku</param>
        /// <param name="position">Położenie przycisku</param>
        /// <param name="width">Szerokosć przyicsku</param>
        /// <param name="height">Wysokość przycisku</param>
        public Button(string text, Vector2 position, int? width = null, int? height = null) : base(null)
        {
            NonActiveTexture = GUIHelper.DrawStringOnTexture2D(text, UIElement.InActiveFont, null, GraphicsDevice);

            var newText = new String(text.ToCharArray().Select(d => Char.IsUpper(d) ? Char.ToLower(d) : Char.ToUpper(d))
                .ToArray());
                
            ActiveTexture = GUIHelper.DrawStringOnTexture2D(newText, UIElement.InActiveFont, UIElement.ActiveFont, GraphicsDevice);

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
            base.TextureToDraw = IsMouseOver ? ActiveTexture : NonActiveTexture;
            base.Draw(ref spriteBatch);
        }

        /// <inheritdoc />
        protected override void OnClickedRaised()
        {
            ClickSound?.Play();
            base.OnClickedRaised();
        }

        public void CenterHorizontal()
            => Position = new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth/ 2 - (float)Width /2 , Position.Y);
        
    }
}