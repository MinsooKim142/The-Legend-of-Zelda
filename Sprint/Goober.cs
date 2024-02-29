﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Commands;
using Sprint.Characters;
using Sprint.Sprite;
using Sprint.Level;

namespace Sprint
{
    public class Goober : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;

        private CycleItem items;
        private CycleEnemy enemies;
        private CycleTile tiles;
        private SpriteFont font;
        private Vector2 characterLoc = new Vector2(20, 20);
        private bool resetGame = false;

        private IInputMap inputTable;
        private GameObjectManager objectManager;
        private SpriteLoader spriteLoader;

        private LevelOne levelOne;

        public Goober()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();

            objectManager = new GameObjectManager();
            inputTable = new InputTable();
            spriteLoader = new SpriteLoader(Content);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Uncomment in order to write an XML file
            //SpriteGroupSaver.WriteFile();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            items = new CycleItem(this, new Vector2(500, 100), spriteLoader);
            enemies = new CycleEnemy(this, new Vector2(500, 300), objectManager, spriteLoader);
            tiles = new CycleTile(this, new Vector2(500, 200), spriteLoader);
            levelOne = new LevelOne(this, spriteLoader);

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new NextItem(items));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.U), new BackItem(items));

            font = Content.Load<SpriteFont>("Font");

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            player = new Player(this, characterLoc, inputTable, objectManager, spriteLoader);
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new MoveDown(player));

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Left), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Right), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Up), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveDown(player));

            Keys[] moveKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            inputTable.RegisterMapping(new MultipleKeyReleaseTrigger(moveKeys), new StopMoving(player));

            //Enemy cycling
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.O), new PreviousEnemyCommand(enemies));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.P), new NextEnemyCommand(enemies));

            //Tile Cycling
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.T), new PreviousTileCommand(tiles));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Y), new NextTileCommand(tiles));

            //Melee Regular Sword Attack
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new Melee(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.N), new Melee(player));

            //Player uses a cast move
            // TODO: shouldnt bind separately from shoot commands
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D7), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D8), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D9), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D0), new Cast(player));


            //Take Damage
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.E), new TakeDamageCommand(player));

            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));
        }

        //clears input dictionary and object manager
        public void ResetGame()
        {
            inputTable.ClearDictionary();
            objectManager.ClearObjects();
        }


        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        protected override void Update(GameTime gameTime)
        {
            
            //resets the game when user request a reset
            if(resetGame)
            {
                ResetGame();
                LoadContent();
                resetGame=false;
            }


            inputTable.Update(gameTime);
            objectManager.Update(gameTime);

            enemies.Update(gameTime);
            tiles.Update(gameTime);
            items.Update(gameTime);
            player.Update(gameTime);

            levelOne.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Gets the vector coordinates (spriteLocation) from MoveSystems.cs and draws main character sprite
            levelOne.Draw(_spriteBatch, gameTime);

            player.Draw(_spriteBatch, gameTime);
            enemies.Draw(_spriteBatch, gameTime);
            tiles.Draw(_spriteBatch, gameTime);
            items.Draw(_spriteBatch, gameTime);

            objectManager.Draw(_spriteBatch, gameTime);

            //Remove
            //_spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            //_spriteBatch.DrawString(font, "Program Made By: Team Goobers", new Vector2(10, 330), Color.Black);
            //_spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/the_legend_of_zelda_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
