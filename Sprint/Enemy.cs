﻿// Enemy.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint
{
    public class Enemy : Character
    {
        protected ISprite sprite;
        protected Physics physics;

        public Enemy(Game1 game, ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            physics = new Physics(game, position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, physics.Position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);
        }
    }
}