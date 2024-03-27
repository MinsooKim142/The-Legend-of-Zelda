using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{

    //Code based on the BluebubbleEnemy.cs file
    public class BatEnemy : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;

        private Timer timeAttack;


        public BatEnemy(ISprite sprite, Vector2 initialPosition, SceneObjectManager objectManager, SpriteLoader spriteLoader)
            : base(sprite, initialPosition, objectManager)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, objectManager);

            projectileCommand = new ShootArrowCommand(itemFactory);


            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Update logic
        public override void Update(GameTime gameTime)
        {

            timeAttack.Update(gameTime);

            //uses timer to shoot arrows ever 3 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);

                itemFactory.SetDirection(moveDirection);

                projectileCommand.Execute();

                timeAttack.Start();

            }


            // Calculate movement based on elapsed time for the random pattern
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move randomly within a specified area
            MoveRandomly(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);


        }

        // Move Bat randomly within the game area
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

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            CheckBounds(newPosition, 3);

            physics.SetPosition(newPosition);
        }

        //ensures that the enemy always stays within windows of the game
        private void CheckBounds(Vector2 pos, float scale)
        {
            //int gameX = Goober.gameWidth;
            //int gameY = Goober.gameHeight;

            //makes the enemy go to the other direction when it reaches a certain distance so that it doesnt go over window
            //if (pos.X + scale > gameX)
            //{
            //    moveDirection.X = -moveDirection.X;

            //}

            //if (pos.Y + scale > gameY)
            //{
            //    moveDirection.Y = -moveDirection.Y;
            //}
        }

        // Generate a random movement direction
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

        }
    }
}