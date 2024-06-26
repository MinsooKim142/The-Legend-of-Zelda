﻿using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Loader;
using Sprint.Sprite;
using System.Collections.Generic;


namespace Sprint.HUD
{
    internal class HUDFactory
    {
        private SpriteLoader spriteLoader;

        const string LOCATION = "HUD/HUDSprite";
        const string ITEM_LOCATION = "itemAnims";

        public HUDFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        public IHUD MakeHUDSprite(string spriteLabel, Vector2 position)
        {
            ISprite sprite = spriteLoader.BuildSprite(LOCATION, spriteLabel);

            return new HUDSprite(sprite, position);
        }

        public HUDPowerupArray MakeItemSprite(IPowerup powerup, Vector2 position)
        {

            HUDPowerupArray hpa = new(position, Vector2.Zero);
            hpa.SetSinglePowerup(powerup);
            return hpa;
        }

        public List<HUDAnimSprite> MakeNumber(string level, Vector2 position, int spriteSize)
        {
            List<HUDAnimSprite> nums = new List<HUDAnimSprite>();
            float x = position.X;
            for(int i = 0; i < 2; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(LOCATION, "Numbers");
                nums.Add(new HUDAnimSprite(sprite, new Vector2(x, position.Y)));
                x += spriteSize;
            }
            char[] arr = level.ToCharArray();
            nums[0].SetSprite(arr[0].ToString());
            nums[1].SetSprite(arr[1].ToString());

            return nums; 
        }

        // The game has a max of 16 hearts. For now only 8 will do.
        public List<HUDAnimSprite> MakeHearts(int amount, string spriteLabel, Vector2 position, int spriteSize)
        {
            List<HUDAnimSprite> hearts = new List<HUDAnimSprite>();

            float x = position.X;
            for (int i = 0; i < amount; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(LOCATION, spriteLabel);
                hearts.Add(new HUDAnimSprite(sprite, new Vector2(x, position.Y)));
                x += spriteSize;
            }

            return hearts;
        }

        public HUDSelector MakeSelector(string spriteLabel, Vector2 position, Vector2 padding)
        {
            return new HUDSelector(spriteLoader.BuildSprite(LOCATION, spriteLabel),
                new Rectangle((int)position.X, (int)position.Y, (int)padding.X, (int)padding.Y),
                "active", "inactive");
        }
    }
}
