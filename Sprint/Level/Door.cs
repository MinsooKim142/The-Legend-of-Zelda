﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint
{
    internal class Door : IGameObject, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;

        public Door(ISprite sprite, Vector2 position, Vector2 size)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)(position.X), (int)(position.Y), (int)(size.X), (int)(size.Y));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
