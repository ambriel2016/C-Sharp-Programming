using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SuperMarioRecreation.Worlds;

namespace SuperMarioRecreation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix pos;
        BaseWorld currentWorld;
        BaseWorld[] worldList;
        Boolean firstTry = true;
        Texture2D mario;

        Viewport v;
        Point backPos;
        Vector2 marioPos;

        SpriteFont myFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphics.PreferredBackBufferHeight = 696;       //Defines the size of the window (Height).
            graphics.PreferredBackBufferWidth = 765;        //Defines the size of the window (Width).
            graphics.ApplyChanges();                        //Actually sets the window to the size we defined.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backPos = new Point(0, 0);
            marioPos = new Vector2(120, 552);
            mario = Content.Load<Texture2D>("mario");
            myFont = Content.Load<SpriteFont>("myFont");
            worldList = new BaseWorld[] { new World1_1() };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            v = graphics.GraphicsDevice.Viewport;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            if (firstTry)
            {
                currentWorld = worldList[0];
                currentWorld.initWorld();
                firstTry = false;
            }
            else
            {
                checkMovement();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Main draw method.  All drawing that is not part of the scoreboard goes here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, pos);
            currentWorld.draw(gameTime, spriteBatch);
            spriteBatch.Draw(mario, marioPos, Color.White);
            spriteBatch.End();

            //Will be used for scoreboard area.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            print();
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Boolean keyDown(Keys key)
        { 
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void print()
        {
            spriteBatch.DrawString(myFont, "View Port  X: " + v.X + "  Y: " + v.Y, new Vector2(10, 24), Color.White);
            spriteBatch.DrawString(myFont, "Camera Pos  X: " + backPos.X + "  Y: " + backPos.Y, new Vector2(10, 10), Color.White);
        }

        private void checkMovement()
        {
            if (keyDown(Keys.Escape))
                this.Exit();
            if (keyDown(Keys.Right))
            {
                marioPos.X += 4;
                if (marioPos.X >= backPos.X + v.Width / 2)
                    backPos.X += 4;
            }
            if (keyDown(Keys.Left))
            {
                if (!(marioPos.X <= backPos.X))
                {
                    marioPos.X -= 4;
                }
            }
            if (keyDown(Keys.Up))
                marioPos.Y -= 4;
            if (keyDown(Keys.Down))
                marioPos.Y += 4;
            pos = Matrix.CreateTranslation(-backPos.X, backPos.Y, 0); 
        }
    }
}
