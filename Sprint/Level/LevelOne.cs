﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Loader;
using Microsoft.Xna.Framework.Content;

namespace Sprint.Level
{
    internal class LevelOne
    {
        private List<IGameObject> tiles = new List<IGameObject>();
        private int currentRoomIndex = 0;

        PositionLoader pl;

        private const string ANIM_FILE = "XML/LevelOne";
        private const string POS_FILE = "XML/LevelOnePos";

        public LevelOne(Goober game, ContentManager content, SpriteLoader spriteLoader) 
        {
            pl = new PositionLoader(content);
            pl.LoadXML(POS_FILE);

            CreateRoom(game, "roomOneExterior", spriteLoader);
            CreateRoom(game, "roomOneTopDoor", spriteLoader);
            CreateRoom(game, "roomOneLeftDoor", spriteLoader);
            CreateRoom(game, "roomOneRightDoor", spriteLoader);
            CreateRoom(game, "roomOneDownDoor", spriteLoader);
            CreateRoom(game, "roomOneFloor", spriteLoader);
        }

        private void CreateRoom(Goober game, string roomName, SpriteLoader spriteLoader)
        {
            ISprite roomSprite = spriteLoader.BuildSprite(ANIM_FILE, roomName);

            Tiles roomPart = new(game, roomSprite, pl.GetPosition(roomName));

            tiles.Add(roomPart);
        }

        public void Update(GameTime gameTime)
        {
            // None For Now
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for(int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch, gameTime);
            }

        }
    }
}
