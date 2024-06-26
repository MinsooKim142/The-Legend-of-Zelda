using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Collision;
using Sprint;

namespace Sprint.Characters
{
    public class MoveSpike : EnemyAI
    {
        private float elapsedTime;
        private Vector2 moveDirection; // Movement direction for the random pattern
        public Vector2 directionFace;


        Physics physics;


        public MoveSpike(Physics physics)
        {

            this.physics = physics;

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }




        // Move AI for MoveVert
        public override void MoveAI(GameTime gameTime)
        {
            float speed = 200; // Adjust the speed as needed
            float moveTime = 1.5f; // Time before changing direction (in seconds)

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > moveTime)
            {
                // Change direction after the specified time
                moveDirection = -moveDirection;
                elapsedTime = 0;
            }


            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            physics.SetPosition(newPosition);
        }



        // Choose a random direction to move
        public void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            int indDir = random.Next(4);
            directionFace = Directions.GetDirectionFromIndex(indDir);
            SetDirection(directionFace);


        }



        // Set the direction of the move AI
        public void SetDirection(Vector2 direction)
        {
            moveDirection = direction;

        }

        //reverses move direction when collides with wall
        public void ReverseHorDir()
        {
            moveDirection.X = -moveDirection.X;
        }

        public void ReverseVerDir()
        {
            moveDirection.Y = -moveDirection.Y;
        }
    }
}

