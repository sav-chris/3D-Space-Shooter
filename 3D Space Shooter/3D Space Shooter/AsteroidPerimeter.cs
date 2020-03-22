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
    class AsteroidPerimeter
    {
        // When the object is created, it needs to be added to the physics engine under physicsReference
        Model asteroidModel;
        Matrix[] transforms;
        PerimeterObject[] topPerimeter;
        PerimeterObject[] bottomPerimeter;
        PerimeterObject[] leftPerimeter;
        PerimeterObject[] rightPerimeter;


        public Model ObjectModel
        {
            get
            {
                return asteroidModel;
            }
            set
            {
                asteroidModel = value;
            }
        }

        public Matrix[] Transforms
        {
            get
            {
                return transforms;
            }
            set
            {
                transforms = value;
            }
        }

        /// <summary>
        /// The default constructor for the perimeter asteroids.
        /// </summary>
        /// <param name="model">The model to use for the perimeter asteroids.</param>
        /// <param name="transforms">The graphical transforms to be applied to the perimeter asteroids.</param>
        public AsteroidPerimeter(Model model, Matrix[] transforms)
        {
            this.asteroidModel = model;
            this.transforms = transforms;

            // Equally split the number of asteroids between each side of the screen
            topPerimeter = new PerimeterObject[(int)(GameConstants.numPerimeterAsteroids / 4)];
            bottomPerimeter = new PerimeterObject[(int)(GameConstants.numPerimeterAsteroids / 4)];
            leftPerimeter = new PerimeterObject[(int)(GameConstants.numPerimeterAsteroids / 4)];
            rightPerimeter = new PerimeterObject[(int)(GameConstants.numPerimeterAsteroids / 4)];

            // Randomly assign an initial position and speed to each asteroid in the perimeter
            for (int i = 0; i < topPerimeter.Length; i++)
            {
                float randomX = CommonFunctions.GenerateRandom(0, (int)GameConstants.playfieldSizeX);
                float randomY = GameConstants.playfieldSizeY;
                float randomZ = 
                    CommonFunctions.GenerateRandom(((int)(GameConstants.cameraMaxDistance - GameConstants.cameraHeight)), 
                                                    (int)GameConstants.cameraHeight);
                Vector3 initialPosition = new Vector3(randomX, randomY, randomZ);
                float speed = CommonFunctions.GenerateRandom((int)GameConstants.perimeterMinSpeed, 
                                                             (int)GameConstants.perimeterMaxSpeed);
                Vector3 velocity = new Vector3(0, 0, speed);
                topPerimeter[i] = new PerimeterObject(initialPosition, velocity);
            }
            for (int i = 0; i < bottomPerimeter.Length; i++)
            {
                float randomX = CommonFunctions.GenerateRandom(0, (int)GameConstants.playfieldSizeX);
                float randomY = 0;
                float randomZ = 
                    CommonFunctions.GenerateRandom(((int)(GameConstants.cameraMaxDistance - GameConstants.cameraHeight)), 
                                                    (int)GameConstants.cameraHeight);
                Vector3 initialPosition = new Vector3(randomX, randomY, randomZ);
                float speed = CommonFunctions.GenerateRandom((int)GameConstants.perimeterMinSpeed, 
                                                             (int)GameConstants.perimeterMaxSpeed);
                Vector3 velocity = new Vector3(0, 0, speed);
                bottomPerimeter[i] = new PerimeterObject(initialPosition, velocity);
            }
            for (int i = 0; i < leftPerimeter.Length; i++)
            {
                float randomX = 0;
                float randomY = CommonFunctions.GenerateRandom(0, (int)GameConstants.playfieldSizeY);
                float randomZ = 
                    CommonFunctions.GenerateRandom(((int)(GameConstants.cameraMaxDistance - GameConstants.cameraHeight)), 
                                                    (int)GameConstants.cameraHeight);
                Vector3 initialPosition = new Vector3(randomX, randomY, randomZ);
                float speed = CommonFunctions.GenerateRandom((int)GameConstants.perimeterMinSpeed, 
                                                             (int)GameConstants.perimeterMaxSpeed);
                Vector3 velocity = new Vector3(0, 0, speed);
                leftPerimeter[i] = new PerimeterObject(initialPosition, velocity);
            }
            for (int i = 0; i < rightPerimeter.Length; i++)
            {
                float randomX = GameConstants.playfieldSizeY;
                float randomY = CommonFunctions.GenerateRandom(0, (int)GameConstants.playfieldSizeY);
                float randomZ = 
                    CommonFunctions.GenerateRandom(((int)(GameConstants.cameraMaxDistance - GameConstants.cameraHeight)), 
                                                    (int)GameConstants.cameraHeight);
                Vector3 initialPosition = new Vector3(randomX, randomY, randomZ);
                float speed = CommonFunctions.GenerateRandom((int)GameConstants.perimeterMinSpeed, 
                                                             (int)GameConstants.perimeterMaxSpeed);
                Vector3 velocity = new Vector3(0, 0, speed);
                rightPerimeter[i] = new PerimeterObject(initialPosition, velocity);
            }
        }

        /// <summary>
        /// Draws all the perimeter asteroids to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>   
        public void Draw(Camera camera, float aspectRatio)
        {
            foreach (PerimeterObject asteroid in topPerimeter)
            {
                Matrix tempTransforms = Matrix.CreateScale(GameConstants.perimeterScale) 
                                      * Matrix.CreateTranslation(asteroid.Position);
                CommonFunctions.DrawModel(asteroidModel, tempTransforms, transforms, camera, aspectRatio);
            }
            foreach (PerimeterObject asteroid in bottomPerimeter)
            {
                Matrix tempTransforms = Matrix.CreateScale(GameConstants.perimeterScale) 
                                      * Matrix.CreateTranslation(asteroid.Position);
                CommonFunctions.DrawModel(asteroidModel, tempTransforms, transforms, camera, aspectRatio);
            }
            foreach (PerimeterObject asteroid in leftPerimeter)
            {
                Matrix tempTransforms = Matrix.CreateScale(GameConstants.perimeterScale) 
                                      * Matrix.CreateTranslation(asteroid.Position);
                CommonFunctions.DrawModel(asteroidModel, tempTransforms, transforms, camera, aspectRatio);
            }
            foreach (PerimeterObject asteroid in rightPerimeter)
            {
                Matrix tempTransforms = Matrix.CreateScale(GameConstants.perimeterScale) 
                                      * Matrix.CreateTranslation(asteroid.Position);
                CommonFunctions.DrawModel(asteroidModel, tempTransforms, transforms, camera, aspectRatio);
            }
        }

        /// <summary>
        /// Updates the position of all perimeter asteroids.
        /// </summary>
        public void Update()
        {
            foreach (PerimeterObject asteroid in topPerimeter)
            {
                asteroid.Update();
            }
            foreach (PerimeterObject asteroid in bottomPerimeter)
            {
                asteroid.Update();
            }
            foreach (PerimeterObject asteroid in leftPerimeter)
            {
                asteroid.Update();
            }
            foreach (PerimeterObject asteroid in rightPerimeter)
            {
                asteroid.Update();
            }
        }
    }
}
