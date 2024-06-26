using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;



namespace Sprint.Characters
{
    internal class BluebubbleEnemy : Enemy
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;

        public BluebubbleEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            timeAttack = new Timer(2);
            timeAttack.Start();

            health = CharacterConstants.LOW_HP;

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootBombC(itemFactory);

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Register a directional animation for BluebubbleEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for BluebubbleEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of BluebubbleEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update BluebubbleEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
            }

            // Calculate movement based on elapsed time for the random pattern
            MoveRandomly(gameTime);


            base.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {
            // Determine the direction and set the appropriate animation label
            if (Math.Abs(moveDirection.X) > Math.Abs(moveDirection.Y))
            {
                // Horizontal movement
                if (moveDirection.X > 0)
                    SetAnimation("rightFacing");
                else
                    SetAnimation("leftFacing");
            }
            else
            {
                // Vertical movement
                if (moveDirection.Y > 0)
                    SetAnimation("downFacing");
                else
                    SetAnimation("upFacing");
            }
        }

        // Move BluebubbleEnemy randomly within the game area
        private void MoveRandomly(GameTime gameTime)
        {
            float speed = 50; // Adjust the speed as needed
            float moveTime = 2; // Time before changing direction (in seconds)

            if (elapsedTime > moveTime)
            {
                // Change direction after the specified time
                RandomizeMoveDirection();
                elapsedTime = 0;
            }

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            physics.SetPosition(newPosition);
        }


        // Generate a random movement direction for BluebubbleEnemy
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            // Normalize the moveDirection vector
            moveDirection.Normalize();
        }

    }
}
