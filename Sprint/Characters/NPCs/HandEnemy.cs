using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;



namespace Sprint.Characters
{
    internal class HandEnemy : Enemy
    {

        private string lastAnimationName;
        private MoveHand moveHand;

        public HandEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader, Player player)
            : base(sprite, damagedSprite, initialPosition, room)
        {


            health = CharacterConstants.MID_HP;



            // Initialize the move direction randomly
            moveHand = new MoveHand(physics, player);


        }

        // Register a directional animation for HandEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for HandEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of HandEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update HandEnemy logic
        public override void Update(GameTime gameTime)
        {
            // Calculate movement based on elapsed time for the random pattern
            moveHand.MoveAI(gameTime);

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();


            base.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {

            string newAnim = "leftFacing";  

            if (moveHand.moveDirection.X > 0)
                newAnim = "rightFacing";
            else
                newAnim = "leftFacing";

            if(newAnim != lastAnimationName)
            {
                lastAnimationName = newAnim;
                SetAnimation(newAnim);
            }


        }



    }
}
