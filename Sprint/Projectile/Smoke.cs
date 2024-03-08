﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class Smoke : SimpleProjectile
    {

        private Timer smokeTimer;

        public override CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PARTICLE };

        public Smoke(ISprite sprite, Vector2 startPos, GameObjectManager objManager) : base(sprite, startPos, objManager)
        {
            smokeTimer = new Timer(0.5);
        }

        public override void Create()
        {
            smokeTimer.Start();
            base.Create();
        }

        public override void Update(GameTime gameTime)
        {
            smokeTimer.Update(gameTime);

            base.Update(gameTime);

            if (smokeTimer.JustEnded)
            {
                Delete();
            }
        }
    }
}
