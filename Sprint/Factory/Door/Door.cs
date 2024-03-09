﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;
using System.Diagnostics;

namespace Sprint.Factory.Door
{
    internal abstract class Door: IDoor
    {
        protected ISprite sprite;
        protected Vector2 position;
        protected Rectangle bounds;
        protected Rectangle openBounds;
        protected int otherSide;
        protected bool isOpen;
        protected Vector2 spawnPosition;

        GameObjectManager objManager;

        // Bounds depends on if this door is open
        public Rectangle BoundingBox => isOpen ? openBounds : bounds;

        public virtual CollisionTypes[] CollisionType
        {
            get
            {
                if (isOpen)
                {
                    return new CollisionTypes[] { CollisionTypes.OPEN_DOOR, CollisionTypes.DOOR };
                }
                else
                {
                    return new CollisionTypes[] { CollisionTypes.CLOSED_DOOR, CollisionTypes.DOOR };
                }
            }
        }

        public Door(ISprite sprite, bool isOpen, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, Vector2 spawnPosition, GameObjectManager objManager)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            openBounds = new Rectangle((int)(position.X + (size.X - openSize.X)/2), (int)(position.Y + (size.Y - openSize.Y) / 2), 
                (int)openSize.X, (int)openSize.Y);
            this.otherSide = otherSide;
            this.isOpen = isOpen;
            this.objManager = objManager;
            this.spawnPosition = spawnPosition;
        }

        public void SwitchRoom()
        {
            if (otherSide >= 0)
                objManager.SwitchRoom(PlayerSpawnPosition(), otherSide);
        }

        public Vector2 PlayerSpawnPosition()
        {
            return spawnPosition;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        // Returns index in Level's room array of the Room this leads to
        public int GetAdjacentRoomIndex()
        {
            return otherSide;
        }

        public void SetOpen(bool open)
        {
            isOpen = open;
            if (isOpen)
            {
                sprite.SetAnimation("open");
            }
            else
            {
                sprite.SetAnimation("closed");
            }
        }
    }
}