using System;
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
        

        public Button(Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle elementRectangle) :base(nonActiveTexture)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            Position = elementRectangle.Location.ToVector2();
            Width = elementRectangle.Width;
            Height = elementRectangle.Height;
        }

        public Button(string text, Vector2 position, int? width, int? height) : base(null)
        {
            var foo = UIElement.InActiveFont.MeasureString(text);
                RenderTarget2D renderTarget = new RenderTarget2D(
                GraphicsDevice,
                (int)foo.X,
                (int)foo.Y);

            var spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin();
            spriteBatch.DrawString(InActiveFont, text, Vector2.Zero, Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            NonActiveTexture = renderTarget;

            UIElementRectangle.Location = position.ToPoint();
            base.Width = width ?? NonActiveTexture.Width;
            base.Height = height ?? NonActiveTexture.Height;
        }

        public Button(string text, Vector2 position, int width) : this(text, position, null, null)
        {
            var proportion = width / base.Width;
            base.Width *= proportion;
            base.Height *= proportion;
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
    }
}