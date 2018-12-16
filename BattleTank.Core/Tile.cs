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

        public TileType Type { get; set; }
        public Rectangle CollisionRect { get; set; }
        public Texture2D Texture { get; set; }

        public Tile(TileType _type, Rectangle _collisionRect, Texture2D _texture)
        {
            CollisionRect = _collisionRect;
            Texture = _texture;
            Type = _type;
        }
        public void Update(GameTime gameTime)
        {
            switch (Type)
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

            switch (Type)
            {
                case TileType.AIR:
                    break;
                case TileType.WALL:
                    spriteBatch.Draw(Texture, new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, CollisionRect.Height), Color.White);
                    break;
                case TileType.BUSH:
                    spriteBatch.Draw(Texture, new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, CollisionRect.Height), Color.White);
                    break;
                case TileType.WATER:
                    spriteBatch.Draw(Texture, new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, CollisionRect.Height), Color.White);                
                    break;                                                                                                                 
                case TileType.MUD:
                    Color colorMUD;
#if ANDROID
                    colorMUD = Color.Lerp(Color.Lerp(Color.Lerp(Color.Red, Color.Yellow, 0.55f), Color.OrangeRed, 0.3f), Color.Red, 0.6f);
#else
                    colorMUD = Color.Lerp(Color.Lerp(Color.Lerp(Color.Red, Color.Yellow, 0.55f), Color.OrangeRed, 0.3f), Color.Red, 0.8f);
#endif
                    spriteBatch.Draw(Texture, new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, CollisionRect.Height), colorMUD);
                    break;
            }
        }
        public Collision IsColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, CollisionRect);
            if ((Type == TileType.WALL || Type == TileType.WATER || Type == TileType.BUSH || Type == TileType.MUD) && (intersect.Width > 0 || intersect.Height > 0))
            {
       
                     if (possibleCollisionRect.Top < CollisionRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > CollisionRect.Y)
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.TOP, depth);
                    }
                    if (possibleCollisionRect.Bottom > CollisionRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Height;
                        return new Collision(Collision.Side.BOTTOM, depth);
                    }
                    if (possibleCollisionRect.Left < CollisionRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > CollisionRect.Right)
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.LEFT, depth);
                    }
                    if (possibleCollisionRect.Right > CollisionRect.Right - CollisionRect.Width && possibleCollisionRect.Right > CollisionRect.Left && Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                    {
                        float depth = intersect.Width;
                        return new Collision(Collision.Side.RIGHT, depth);
                    }
                
            }
            
                return new Collision();
        }
    }
}
