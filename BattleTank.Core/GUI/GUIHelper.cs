using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.GUI
{
    static class GUIHelper
    {
        public static Texture2D DrawStringOnTexture2D(string text, SpriteFont foreGroundFont, SpriteFont backgroundFont, GraphicsDevice graphicsDevice)
        {
            var stringDiamensions = UIElement.InActiveFont.MeasureString(text);
            RenderTarget2D renderTarget2D = new RenderTarget2D(graphicsDevice, (int)stringDiamensions.X, (int)stringDiamensions.Y);

            var spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget2D);

            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            if (backgroundFont != null)
                spriteBatch.DrawString(backgroundFont, text, Vector2.Zero, Color.White);
            spriteBatch.DrawString(foreGroundFont, text, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            return renderTarget2D;
        }
        public static void CenterHorizontal(this UIElement element)
            => element.Position = new Vector2(UIElement.GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - (float)element.Width / 2, element.Position.Y);
    }
}
