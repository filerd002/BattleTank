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

        public Button (Texture2D nonActiveTexture, Texture2D activeTexture, Vector2 position, int? width = null, int? height = null) : base(nonActiveTexture)
        {
            NonActiveTexture = nonActiveTexture;
            ActiveTexture = activeTexture;
            Position = position;
            Width = width ?? NonActiveTexture.Width;
            Height = height ?? NonActiveTexture.Height;
        }

        public Button(Texture2D nonActiveTexture, Texture2D activeTexture, Rectangle elementRectangle) : this (nonActiveTexture, activeTexture, elementRectangle.Location.ToVector2(), elementRectangle.Width, elementRectangle.Height)
        { }

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