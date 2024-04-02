﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;
using System.Diagnostics;

namespace Sprint.Factory.Door
{
    internal class WallDoor: Door
    {
        
        public WallDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 sideOfRoom, Point roomIndices, DungeonState dungeon) :
            base(sprite, false, position, size, Vector2.Zero, sideOfRoom, roomIndices, Vector2.Zero, dungeon)
        {

        }

        public override void SetOpen(bool open)
        {
            Debug.Assert(!open); // cant close this door
            isOpen = false;
        }

    }
}
