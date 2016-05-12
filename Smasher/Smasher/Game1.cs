using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Smasher
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D background;
        private Rectangle mainFrame;

        private Texture2D mainMenu;
        private Rectangle mainRect;

        private Texture2D pause;
        private Rectangle pauseRect;

        private Texture2D aboutMenu;
        private Rectangle aboutRect;
        private BoundingBox aboutBb;

        private Texture2D aboutBtn;
        private Rectangle aboutBtnRect;
        private BoundingBox aboutBtnBb;

        private Texture2D exit;
        private Rectangle exitRect;
        private BoundingBox exitBb;

        private Texture2D newGame;
        private Rectangle newGameRect;
        private BoundingBox newGameBb;
         
        private Animation sprite;
        private Animation sprite1;

        private bool menuState, aboutState;

        private bool paused = false;
        private bool pauseKeyDown = false;
        public static bool isKill;
        public static bool isKill1;
        private bool isKillProc;
        private bool isKillProc1;

        private MouseState lastMouseState;
        private MouseState mouseState;
        private int mouseX;
        private int mouseY;
        private BoundingBox bbMouse;

        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 864; //Resolution W
            graphics.PreferredBackBufferHeight = 486; //Resolution H
            //graphics.IsFullScreen = true;     //FullScreen
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            lastMouseState = Mouse.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Background");
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            mainMenu = Content.Load<Texture2D>("mainmenu");
            mainRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            pause = Content.Load<Texture2D>("pause");
            pauseRect = new Rectangle(243, 150, pause.Width, pause.Height);

            aboutMenu = Content.Load<Texture2D>("about");
            aboutRect = new Rectangle(80, 180, aboutMenu.Width, aboutMenu.Height);
            aboutBb.Min = new Vector3(0, 0, 0);
            aboutBb.Max = new Vector3(0, 0, 0);            

            aboutBtn = Content.Load<Texture2D>("aboutmenu");
            aboutBtnRect = new Rectangle(170, 290, aboutBtn.Width, aboutBtn.Height);
            aboutBtnBb.Min = new Vector3(aboutBtnRect.Left, aboutBtnRect.Top, 0);
            aboutBtnBb.Max = new Vector3(aboutBtnRect.Right, aboutBtnRect.Bottom, 0);

            exit = Content.Load<Texture2D>("exit");
            exitRect = new Rectangle(170, 360, exit.Width, exit.Height);
            exitBb.Min = new Vector3(exitRect.Left, exitRect.Top, 0);
            exitBb.Max = new Vector3(exitRect.Right, exitRect.Bottom, 0);

            newGame = Content.Load<Texture2D>("newgame");
            newGameRect = new Rectangle(170, 220, newGame.Width, newGame.Height);
            newGameBb.Min = new Vector3(newGameRect.Left, newGameRect.Top, 0);
            newGameBb.Max = new Vector3(newGameRect.Right, newGameRect.Bottom, 0);

            sprite = new Animation(Content.Load<Texture2D>("walk"), Content.Load<Texture2D>("kill"), new Vector2(100, 200), 109, 70, 109, 165, 10, 3, 20);
            sprite1 = new Animation(Content.Load<Texture2D>("walk"), Content.Load<Texture2D>("kill"), new Vector2(100, 310), 109, 70, 109, 165, 30, 1, 20);
            sprite.IsWalk = false;
            sprite1.IsWalk = false;
            isKill = false;
            isKill1 = false;
            isKillProc = false;
            isKillProc1 = false;
            aboutState = false;
            menuState = true;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
 
            if (menuState == false)
            {
                Pause();
                if (!paused)
                {
                    Kill(gameTime);

                    lastMouseState = Mouse.GetState();

                    if (isKill)
                    {
                        UpdateKill(gameTime, sprite, ref isKillProc, ref isKill, -100, random.Next(50, 200));
                    }

                    if (isKill1)
                    {
                        UpdateKill(gameTime, sprite1, ref isKillProc1, ref isKill1, -80, random.Next(320, 400));
                    }
                }
            }
            else
            {
                if (lastMouseState.LeftButton == ButtonState.Released &&
                    Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Collision();

                    if (bbMouse.Intersects(aboutBb))
                    {
                        aboutState = false;
                        aboutBtnBb.Min = new Vector3(aboutBtnRect.Left, aboutBtnRect.Top, 0);
                        aboutBtnBb.Max = new Vector3(aboutBtnRect.Right, aboutBtnRect.Bottom, 0);
                        exitBb.Min = new Vector3(exitRect.Left, exitRect.Top, 0);
                        exitBb.Max = new Vector3(exitRect.Right, exitRect.Bottom, 0);
                        newGameBb.Min = new Vector3(newGameRect.Left, newGameRect.Top, 0);
                        newGameBb.Max = new Vector3(newGameRect.Right, newGameRect.Bottom, 0);
                    }

                    if (bbMouse.Intersects(aboutBtnBb))
                    {
                        aboutState = true;
                        aboutBtnBb.Min = new Vector3(0, 0, 0);
                        aboutBtnBb.Max = new Vector3(0, 0, 0);
                        exitBb.Min = new Vector3(0, 0, 0);
                        exitBb.Max = new Vector3(0, 0, 0);
                        newGameBb.Min = new Vector3(0, 0, 0);
                        newGameBb.Max = new Vector3(0, 0, 0);

                        aboutBb.Min = new Vector3(aboutRect.Left + 80, aboutRect.Top + 240, 0);
                        aboutBb.Max = new Vector3(aboutRect.Right - 80, aboutRect.Bottom, 0);  
                    }

                    if (bbMouse.Intersects(exitBb))
                    {
                        Dispose();
                        Exit();
                    }

                    if (bbMouse.Intersects(newGameBb))
                    {
                        menuState = false;
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, mainFrame, Color.White);

            if (!menuState)
            {
                DrawSprite();
            }

            if (menuState)
            {
                spriteBatch.Draw(mainMenu, mainRect, Color.White);
                spriteBatch.Draw(aboutBtn, aboutBtnRect, Color.White);
                spriteBatch.Draw(exit, exitRect, Color.White);
                spriteBatch.Draw(newGame, newGameRect, Color.White);
            }

            if (aboutState)
            {
                //spriteBatch.Draw(mainMenu, mainRect, Color.White);
                spriteBatch.Draw(aboutMenu, aboutRect, Color.White);
            }

            if(paused)
                spriteBatch.Draw(pause, pauseRect, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Pause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                pauseKeyDown = true;
            else if (pauseKeyDown)
            {
                pauseKeyDown = false;
                paused = !paused;
            }
        }

        public void Collision()
        {
            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            bbMouse.Min = new Vector3(mouseX - 3, mouseY - 3, 0);
            bbMouse.Max = new Vector3(mouseX + 3, mouseY + 3, 0);
        }

        public void DrawSprite()
        {
            if (isKill && isKill1 == false)
            {
                sprite.DrawKill(spriteBatch);
                sprite1.Draw(spriteBatch);
            }
            if (isKill1 && isKill == false)
            {
                sprite1.DrawKill(spriteBatch);
                sprite.Draw(spriteBatch);
            }
            if (isKill && isKill1)
            {
                sprite.DrawKill(spriteBatch);
                sprite1.DrawKill(spriteBatch);
            }
            if (isKill == false && isKill1 == false)
            {
                sprite.Draw(spriteBatch);
                sprite1.Draw(spriteBatch);
            }
        }

        public void Kill(GameTime gameTime)
        {
            if (lastMouseState.LeftButton == ButtonState.Released &&
                Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Collision();
                if (bbMouse.Intersects(sprite.Bounding))
                {
                    if (isKillProc == false)
                    {
                        isKill = true;
                    }
                }
                if (bbMouse.Intersects(sprite1.Bounding))
                {
                    if (isKillProc1 == false)
                    {
                        isKill1 = true;
                    }
                }
            }
            if (isKill && isKill1 == false)
            {
                sprite1.Update(gameTime);
            }
            if (isKill1 && isKill == false)
            {
                sprite.Update(gameTime);
            }
            if (isKill == false && isKill1 == false)
            {
                sprite.Update(gameTime);
                sprite1.Update(gameTime);
            }
        }

        public void UpdateKill(GameTime gameTime, Animation sprite, ref bool isKillProc, ref bool isKill, int x, int y)
        {
            if (sprite.IsWalk == false)
            {
                isKillProc = true;
                sprite.UpdateKill(gameTime);
            }
            else
            {
                isKill = false;
                sprite.IsWalk = false;
                sprite.Position = new Vector2(x, y);
                isKillProc = false;
            }
        }
    }
}