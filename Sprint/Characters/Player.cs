using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Projectile;
using Sprint.Levels;
using Sprint.Collision;
using System;
using Sprint.Music.Sfx;
using Sprint.Items;
using Sprint.Functions.States;

namespace Sprint.Characters
{

    internal class Player : Character, IMovingCollidable
    {
        private Inventory inventory;

        private SfxFactory sfxFactory;

        private ISprite sprite;
        private SpriteLoader spriteLoader;
        private ISprite damagedSprite;

        public event EventHandler OnPlayerDied;


        public delegate void HealthUpdateDelegate(double prev, double next);
        public event HealthUpdateDelegate OnPlayerHealthChange;
        public delegate void MaxHealthUpdateDelegate(int prev, int next, double health);
        public event MaxHealthUpdateDelegate OnPlayerMaxHealthChange;

        private Physics physics;

        // Player variables
        private int sideLength = CharacterConstants.DEFAULT_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE;
        private int maxHealth = CharacterConstants.STARTING_HEALTH;
        private double health = CharacterConstants.STARTING_HEALTH;

        private ProjectileSystem secondaryItems;
        private SwordCollision swordCollision;
        private const int swordWidth = CharacterConstants.SWORD_WIDTH, swordLength = CharacterConstants.SWORD_LENGTH;

        public Vector2 Facing { get; private set; }

        public Rectangle BoundingBox => new((int)(physics.Position.X - sideLength / 2.0),
                (int) (physics.Position.Y - sideLength / 2.0),
                sideLength,
                sideLength);

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PLAYER, CollisionTypes.CHARACTER };

        private float speed = CharacterConstants.PLAYER_SPEED;

        private Timer attackTimer;
        private Timer castTimer;
        private Timer damageTimer;
        private Room room;
        private OpenDeath gameOver;

        // TODO: replace this with state machine
        // Animation to return to as base after a played animation ends
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;


        //declares the move systems for the main character sprite
        public Player(SpriteLoader spriteLoader, DungeonState dungeon)
        {
            //Initialize SFX player
            sfxFactory = SfxFactory.GetInstance();

            //Initialize physics and objectManager
            physics = new Physics(Vector2.Zero);
            this.spriteLoader = spriteLoader;

            inventory = new Inventory();

            //Loads sprite for link
            sprite = spriteLoader.BuildSprite("playerAnims" , "player");
            damagedSprite = spriteLoader.BuildSprite("playerDamagedAnims" , "player");

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.5);
            castTimer = new Timer(0.5);
            // Duration of the damage state
            damageTimer = new Timer(0.5);

            room = null;

            // Start out idle
            Facing = Directions.STILL;
            baseAnim = AnimationCycle.Idle;

            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, spriteLoader);

            this.gameOver = new OpenDeath(dungeon);
        }

        public SimpleProjectileFactory GetProjectileFactory()
        {
            return secondaryItems.ProjectileFactory;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        // Moves the player from current scene into a new one
        public void SetRoom(Room room)
        {
            if (this.room != null)
            {
                this.room.GetScene().Remove(this);
            }

            this.room = room;
            secondaryItems.SetRoom(this.room);
            this.room.GetScene().Add(this);
            StopMoving();
        }

        //Melee attack according to direction
        public void Attack()
        {
            Rectangle swordRec  = new Rectangle();


            // Only attack if not already attacking
            if (!attackTimer.Ended)
            {
                
                return;
            }

            // Player shouldn't move while attacking
            StopMoving();

            // Start timer for attack
            attackTimer.Start();
            castTimer.Start();

            //Creates animations and bounds for the sword for collision
            if (Facing == Directions.DOWN)
            {
                sprite.SetAnimation("swordDown");
                damagedSprite.SetAnimation("swordDown");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y, swordWidth, swordLength);
            }
            else if (Facing == Directions.LEFT)
            {
                sprite.SetAnimation("swordLeft");
                damagedSprite.SetAnimation("swordLeft");
                swordRec = new Rectangle((int)physics.Position.X - swordLength, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
            }
            else if (Facing == Directions.UP)
            {
                sprite.SetAnimation("swordUp");
                damagedSprite.SetAnimation("swordUp");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y - swordLength, swordWidth, swordLength);
            }
            else if (Facing == Directions.RIGHT)
            {
                sprite.SetAnimation("swordRight");
                damagedSprite.SetAnimation("swordRight");
                swordRec = new Rectangle((int)physics.Position.X, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
            }

            swordCollision = new SwordCollision(swordRec, this);
            
            room.GetScene().Add(swordCollision);
            
            
        }

        //Cast according to direction
        public void Cast()
        {

            // Only attack if not already attacking
            if (!castTimer.Ended)
            {
                return;
            }

            // Player shouldn't move while attacking
            StopMoving();

            // Start timer for attack
            castTimer.Start();

            if (Facing == Directions.DOWN)
            {
                sprite.SetAnimation("castDown");
                damagedSprite.SetAnimation("castDown");
            }
            else if (Facing == Directions.LEFT)
            {
                sprite.SetAnimation("castLeft");
                damagedSprite.SetAnimation("castLeft");
            }
            else if (Facing == Directions.UP)
            {
                sprite.SetAnimation("castUp");
                damagedSprite.SetAnimation("castUp");
            }
            else if (Facing == Directions.RIGHT)
            {
                sprite.SetAnimation("castRight");
                damagedSprite.SetAnimation("castRight");
            }
        }

        public void WinPose()
        {
            sprite.SetAnimation("holdItem");
        }

        // Removes velocity and changes animation to match lack of movement
        public void StopMoving()
        {
            physics.SetVelocity(new Vector2(0, 0));
            baseAnim = AnimationCycle.Idle;
            returnToBaseAnim();
        }

        // Return to base animation cycle based on states and facing dir
        // TODO: replace with a state machine
        private void returnToBaseAnim()
        {
            if (baseAnim == AnimationCycle.Idle)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("downStill");
                    damagedSprite.SetAnimation("downStill");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("leftStill");
                    damagedSprite.SetAnimation("leftStill");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("upStill");
                    damagedSprite.SetAnimation("upStill");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("rightStill");
                    damagedSprite.SetAnimation("rightStill");
                }
            }
            else if (baseAnim == AnimationCycle.Walk)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("down");
                    damagedSprite.SetAnimation("down");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("left");
                    damagedSprite.SetAnimation("left");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("up");
                    damagedSprite.SetAnimation("up");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("right");
                    damagedSprite.SetAnimation("right");
                }
            }

        }

        public void MoveLeft()
        {
            // Sets velocity towards left
            physics.SetVelocity(new Vector2(-speed, 0));

            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveRight()
        {
            // Sets velocity towards right
            physics.SetVelocity(new Vector2(speed, 0));

            sprite.SetAnimation("right");
            Facing = Directions.RIGHT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveUp()
        {
            // Sets velocity towards up
            physics.SetVelocity(new Vector2(0, -speed));

            sprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveDown()
        {
            // Sets velocity towards down
            physics.SetVelocity(new Vector2(0, speed));
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;
        }

        public Physics GetPhysic()
        {
            return physics;
        }

        public override void TakeDamage(double dmg)
        {
            // Invincible until timer goes down
            if (!damageTimer.Ended)
            {
                return;
            }
            // sound playing
            sfxFactory.PlaySoundEffect("Player Hurt");
            // switching sprites
            sprite = damagedSprite;
            damageTimer.Start();
            double prevHealth = health;
            health -= dmg;

            // If negative damage (healing), don't go over max health
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            OnPlayerHealthChange?.Invoke(prevHealth, health);

            // Trigger death when health is at or below 0
            if (health <= 0.0)
            {
                this.Die();
            }


        }


        public override void Update(GameTime gameTime)
        {

            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                room.GetScene().Remove(swordCollision);
                returnToBaseAnim();
            }
            castTimer.Update(gameTime);
            if (castTimer.JustEnded)
            {
                returnToBaseAnim();
            }

            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

            // Checks for damage state
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                sprite = spriteLoader.BuildSprite("playerAnims", "player");
                returnToBaseAnim();

            }
            physics.Update(gameTime);
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Draws sprite animation using AnimationSprite class
            sprite.Draw(spriteBatch, physics.Position, gameTime);

        }

        // Moves the player by a set distance
        public void Move(Vector2 distance)
        {
            // teleport player in displacement specified
            physics.SetPosition(physics.Position + distance);
        }
        
        // Moves player to set position
        // Should be in Characters?
        public void MoveTo(Vector2 pos)
        {
            physics.SetPosition(pos);
        }

        /// <summary>
        /// Pickup Item off the ground
        /// </summary>
        /// <param name="item"> ItemType to pickup</param>
        public void PickupItem(Item item)
        {
            if(item.CanPickup(inventory))
            {
                item.GetPowerup().Apply(this);
                room.GetScene().Remove(item);
            }
        }

        // Send to a game over
        public override void Die()
        {
            OnPlayerDied?.Invoke(this, EventArgs.Empty);
            gameOver.Execute();
        }

        /*public void OnInventoryEvent(ItemType it, int prev, int next, List<ItemType> ownedUpgrades)
        {
            switch (it)
            {
                case ItemType.HeartPiece:
                    int prevMax = maxHealth;
                    if(maxHealth < 16)
                        maxHealth += 1;
                    OnPlayerMaxHealthChange?.Invoke(prevMax, maxHealth, health);
                    break;
                case ItemType.Heart:
                    double prevHealth = health;
                    health += 1;
                    if(health > maxHealth)
                    {
                        health = maxHealth;
                    }
                    OnPlayerHealthChange?.Invoke(prevHealth, health);
                    break;
                default:
                    break;
            }
        }*/

    }
}

