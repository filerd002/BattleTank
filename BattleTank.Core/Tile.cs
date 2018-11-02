using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Tile
    {


       public  enum TileType
        {
            AIR,
            WALL,
            BUSH,
            WATER,
            MUD
        }

        public Rectangle collisionRect;
        public Texture2D texture;
 
         public TileType type;
         public Tile(TileType _type, Rectangle _collisionRect, Texture2D _texture)
        {
            collisionRect = _collisionRect;
            texture = _texture;
            type = _type;
        }
        public void Update(GameTime gameTime)
        {
            switch (type)
            {
                case TileType.AIR:
                    break;
                case TileType.WALL:
                    break;
                case TileType.BUSH:
                    break;
                case TileType.WATER:
                    break;
                case TileType.MUD:
                    break;

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            switch (type)
            {
                case TileType.AIR:
                    break;
                case TileType.WALL:
                    spriteBatch.Draw(texture, new Rectangle(collisionRect.X, collisionRect.Y, collisionRect.Width, collisionRect.Height), Color.White);
                    break;
                case TileType.BUSH:
                    spriteBatch.Draw(texture, new Rectangle(collisionRect.X, collisionRect.Y, collisionRect.Width, collisionRect.Height), Color.White);
                    break;
                case TileType.WATER:
                    spriteBatch.Draw(texture, new Rectangle(collisionRect.X, collisionRect.Y, collisionRect.Width, collisionRect.Height), Color.White);                
                    break;                                                                                                                 
                case TileType.MUD:
                    Color colorMUD;
#if ANDROID
                    colorMUD = Color.Lerp(Color.Lerp(Color.Lerp(Color.Red, Color.Yellow, 0.55f), Color.OrangeRed, 0.3f), Color.Red, 0.6f);
#else
                    colorMUD = Color.Lerp(Color.Lerp(Color.Lerp(Color.Red, Color.Yellow, 0.55f), Color.OrangeRed, 0.3f), Color.Red, 0.8f);
#endif
                    spriteBatch.Draw(texture, new Rectangle(collisionRect.X, collisionRect.Y, collisionRect.Width, collisionRect.Height), colorMUD);
                    break;
            }
        }
        public Collision isColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, collisionRect);
            if (type == TileType.WALL || type == TileType.WATER || type == TileType.BUSH || type == TileType.MUD)
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
