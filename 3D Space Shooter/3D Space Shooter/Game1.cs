using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PhysicsEngine;

namespace _D_Space_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Graphics components
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float aspectRatio;
        Texture2D stars;

        // Physics components
        PhysicsEngine.Environment physics;

        // Camera components
        Camera camera;

        // Game components
        AsteroidList asteroids;
        BulletList bullets;
        Ship ship;
        ExplosionList explosions;
        AsteroidPerimeter perimeter;

        float lastShot = 0;
        float lastAsteroid = 0;
        int nextLevelUpTime = GameConstants.secondsPerLevel;
        float gameStartTime = 0;
        bool tabDownLastUpdate = false;

        // Audio components
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue engineSound = null;

        // Text components
        SpriteFont lucidaConsole;

        /*************************************************************************************************************************/

        public Game1()
        {
            // Initialise graphics values
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            this.graphics.IsFullScreen = GameConstants.fullScreen;
            aspectRatio = (float)GraphicsDeviceManager.DefaultBackBufferWidth / GraphicsDeviceManager.DefaultBackBufferHeight;

            // Set default content directory
            Content.RootDirectory = "Content";

            // Set initial game values
            GameConstants.playfieldSizeX = (float)(GraphicsDeviceManager.DefaultBackBufferWidth * 1.2);
            GameConstants.playfieldSizeY = (float)(GraphicsDeviceManager.DefaultBackBufferHeight * 1.2);
            GameConstants.initialTimeBetweenAsteroids = GameConstants.timeBetweenAsteroids;
            GameConstants.initialAsteroidMinSpeed = GameConstants.asteroidMinSpeed;
            GameConstants.initialAsteroidMaxSpeed = GameConstants.asteroidMaxSpeed;
            GameConstants.initialPerimeterSpeedAdjustment = GameConstants.perimeterSpeedAdjustment;
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Update the screen size
            GameConstants.windowWidth = graphics.GraphicsDevice.Viewport.Width;
            GameConstants.windowHeight = graphics.GraphicsDevice.Viewport.Height;

            // Audio Initialisation
            audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            // Camera Initialisation
            camera = new Camera();
            camera.ViewMatrix = Matrix.CreateLookAt(camera.CameraPosition, camera.CameraFocusOn, Vector3.Up);
            camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(GameConstants.perspective),
                aspectRatio,
                1.0f,
                GameConstants.cameraMaxDistance);

            // Physics Initialisation
            PhysicsParameters parameters = new PhysicsParameters();
            parameters.Gravity = new Vector3(0.0f, 0.0f, GameConstants.gravity);
            parameters.WindSpeed = new Vector3(0.0f, 0.0f, 0.0f);
            physics = new PhysicsEngine.Environment();
            physics.PhysicsParameters = parameters;

            base.Initialize();
        }

        /*************************************************************************************************************************/

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load object models
            Model asteroidModel = Content.Load<Model>(@"Models\asteroid1");
            Model shipModel = Content.Load<Model>(@"Models\p1_wedge");
            Model bulletModel = Content.Load<Model>(@"Models\pea_proj");
            Model boundaryModel = Content.Load<Model>(@"Models\asteroid1");
            Model explosionModel = Content.Load<Model>(@"Models\Particle");

            // Set object graphical transforms
            Matrix[] asteroidTransforms = CommonFunctions.SetupEffectDefaults(asteroidModel, camera);
            Matrix[] shipTransforms = CommonFunctions.SetupEffectDefaults(shipModel, camera);
            Matrix[] bulletTransforms = CommonFunctions.SetupEffectDefaults(bulletModel, camera);
            Matrix[] boundaryTransforms = CommonFunctions.SetupEffectDefaults(boundaryModel, camera);
            Matrix[] explosionTransforms = CommonFunctions.SetupEffectDefaults(explosionModel, camera);

            // Load object hulls
            ConvexSegment asteroidHull = PhysicsEngine.CommonFunctions.LoadConvexHull(
                                                       new System.IO.StreamReader(@"..\..\..\Content\Hulls\Asteroid1.hull"));
            ConvexSegment shipHull = PhysicsEngine.CommonFunctions.LoadConvexHull(
                                                       new System.IO.StreamReader(@"..\..\..\Content\Hulls\Ship.hull"));
            ConvexSegment bulletHull = PhysicsEngine.CommonFunctions.LoadConvexHull(
                                                       new System.IO.StreamReader(@"..\..\..\Content\Hulls\Projectile.hull"));

            // Create all game elements
            asteroids = new AsteroidList(GameConstants.numAsteroids, 70.0f, asteroidHull, 
                                         physics, asteroidModel, asteroidTransforms);

            explosions = new ExplosionList(GameConstants.maxExplosions, physics, explosionModel, explosionTransforms);

            bullets = new BulletList(GameConstants.maxBullets, GameConstants.bulletSpeed, 25.0f, bulletHull, physics, bulletModel, 
                                     bulletTransforms, soundBank, asteroids, explosions);

            ship = new Ship(70.0f, shipHull, physics, shipModel, shipTransforms,
                            new Vector3(GameConstants.playfieldSizeX / 2, GameConstants.playfieldSizeY / 2, 0), 0, soundBank, explosions, asteroids);

            // Prepare the perimeter
            perimeter = new AsteroidPerimeter(boundaryModel, boundaryTransforms);

            // Load the background texture
            stars = Content.Load<Texture2D>("Textures/B1_stars");

            // Load the font
            lucidaConsole = Content.Load<SpriteFont>("Fonts/Lucida Console");
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if ((ship.IsAlive && ((float)gameTime.TotalGameTime.TotalSeconds - gameStartTime) > (Score.level * GameConstants.secondsPerLevel)))
            {
                Score.LevelUp();
            }
            if (!ship.IsAlive)
            {
                asteroids.Reset();
            }
            else
            {
                nextLevelUpTime = GameConstants.secondsPerLevel - ((int)(gameTime.TotalGameTime.TotalSeconds - gameStartTime) % GameConstants.secondsPerLevel);
            }

            // Update physics and audio engines
            physics.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            audioEngine.Update();

            // Update game constructs
            bullets.Update();
            asteroids.Update();
            perimeter.Update();
            explosions.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            bool noInput = true;

            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !tabDownLastUpdate)
            {
                GameConstants.drawScore = !GameConstants.drawScore;
                tabDownLastUpdate = true;
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                tabDownLastUpdate = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PlayEngineSound();
                ship.Left(GameConstants.movementAmount);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PlayEngineSound();
                ship.Right(GameConstants.movementAmount);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                PlayEngineSound();
                ship.Up(GameConstants.movementAmount);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                PlayEngineSound();
                ship.Down(GameConstants.movementAmount);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && !ship.IsAlive)
            {
                ship.Reset();
                Score.Reset();
                gameStartTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            if (noInput && engineSound != null && engineSound.IsPlaying)
            {
                engineSound.Pause();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && ship.IsAlive)
            {
                if (((float)gameTime.TotalGameTime.TotalMilliseconds - lastShot) > GameConstants.timeBetweenBullets)
                {
                    bullets.Shoot(new Vector3(ship.Position.X, ship.Position.Y, ship.Position.Z - 100.0f));
                    soundBank.PlayCue("tx0_fire1");
                    lastShot = (float)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (!asteroids.IsFull() && ((float)gameTime.TotalGameTime.TotalMilliseconds - lastAsteroid) > GameConstants.timeBetweenAsteroids)
            {
                asteroids.AddAsteroid();
                lastAsteroid = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }

            base.Update(gameTime);
        }

        /*************************************************************************************************************************/

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 0.0f, 0);

            // Draw Star Background
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Draw(stars, new Rectangle(0, 0, GameConstants.windowWidth, GameConstants.windowHeight), Color.White);
            spriteBatch.End();

            // Draw Boundary
            perimeter.Draw(camera, aspectRatio);

            // Draw Game objects
            asteroids.Draw(camera, aspectRatio);
            bullets.Draw(camera, aspectRatio);
            ship.Draw(camera, aspectRatio);
            explosions.Draw(camera, aspectRatio);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                              SpriteSortMode.Immediate, SaveStateMode.None);

            if (GameConstants.drawScore)
            {
                // Draw Score Information
                String line1 = "Level: " + Score.level + GameConstants.textTab + "Level Up In: " + nextLevelUpTime;
                String line2 = "Asteroids Hit: " + Score.asteroidsHit + GameConstants.textTab +
                               "Asteroids Passed: " + Score.asteroidsMissed + GameConstants.textTab +
                               "Shots Fired: " + Score.bulletsShot;
                String line3 = "Score: " + Score.score;
                Vector2 line1Size = lucidaConsole.MeasureString(line1);
                Vector2 line2Size = lucidaConsole.MeasureString(line2);
                Vector2 line3Size = lucidaConsole.MeasureString(line3);
                spriteBatch.DrawString(lucidaConsole, line1,
                                       new Vector2(GameConstants.windowWidth / 2 - line1Size.X / 2,
                                                   GameConstants.textLineSpacing),
                                       Color.OrangeRed);
                spriteBatch.DrawString(lucidaConsole, line2,
                                       new Vector2(GameConstants.windowWidth / 2 - line2Size.X / 2,
                                                   2 * GameConstants.textLineSpacing),
                                       Color.LightGreen);
                spriteBatch.DrawString(lucidaConsole, line3,
                                       new Vector2(GameConstants.windowWidth / 2 - line3Size.X / 2,
                                                   3 * GameConstants.textLineSpacing),
                                       Color.OrangeRed);
            }

            if (!ship.IsAlive)
            {
                Vector2 restartMessageSize = lucidaConsole.MeasureString(GameConstants.restartMessage);
                spriteBatch.DrawString(lucidaConsole, GameConstants.restartMessage,
                                       new Vector2(GameConstants.windowWidth / 2 - restartMessageSize.X / 2,
                                                   GameConstants.windowHeight / 2 - restartMessageSize.Y / 2),
                                       Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// Plays the sound of the ship moving.
        /// </summary>
        void PlayEngineSound()
        {
            if (engineSound == null)
            {
                engineSound = soundBank.GetCue("engine_2");
                engineSound.Play();
            }

            else if (engineSound.IsPaused)
            {
                engineSound.Resume();
            }
        }
    }
}
