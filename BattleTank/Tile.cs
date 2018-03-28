using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleTank
{
    public class Tile
    {
        public const int AIR = 0;
        public const int WALL = 1;
        public const int BUSH = 2;
        public const int WATER = 3;
        public Rectangle collisionRect;
        public Texture2D texture;
 
        public int type;
        public Tile(int _type, Rectangle _collisionRect, Texture2D _texture)
        {
            collisionRect = _collisionRect;
            texture = _texture;
            type = _type;
        }
        public void Update(GameTime gameTime)
        {
            switch (type)
            {
                case AIR:
                    break;
                case WALL:
                    break;
                case BUSH:
                    break;
                case WATER:
                    break;

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (type)
            {
                case AIR:

                    break;
                case WALL:
                    spriteBatch.Draw(texture, new Vector2(collisionRect.X, collisionRect.Y), null, null);
                    break;
                case BUSH:
                    spriteBatch.Draw(texture, new Vector2(collisionRect.X, collisionRect.Y), null, null);
                    break;
                case WATER:
                    spriteBatch.Draw(texture, new Vector2(collisionRect.X, collisionRect.Y), null, null);
                    break;
            }
        }
        public Collision isColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, collisionRect);
            if (type == WALL || type ==WATER || type == BUSH)
            {
                if (intersect.Width > 0 || intersect.Height > 0)
                {
         
                     if (possibleCollisionRect.Top < collisionRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > collisionRect.Y)
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.TOP, depth);
                    }
                    if (possibleCollisionRect.Bottom > collisionRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.BOTTOM, depth);
                    }
                    if (possibleCollisionRect.Left < collisionRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > collisionRect.Right)
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.LEFT, depth);
                    }
                    if (possibleCollisionRect.Right > collisionRect.Right - collisionRect.Width && possibleCollisionRect.Right > collisionRect.Left && Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.RIGHT, depth);
                    }
                }
            }
            
                return new Collision();
        }
    }
}
