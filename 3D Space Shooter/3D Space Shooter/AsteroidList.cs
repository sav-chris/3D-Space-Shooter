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
    class AsteroidList
    {
        Asteroid[] asteroids;
        float asteroidRadius;
        ConvexHull[] hulls;
        PhysicsEngine.Environment physics;
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        int numAsteroids = 0;

        /// <summary>
        /// The default constructor for the list of asteroids.
        /// </summary>
        /// <param name="maxAsteroids">The maximum number of asteroids that can be active at any time.</param>
        /// <param name="radius">A radius that encloses the entire asteroid model, centred at (0,0,0) in object space.</param>
        /// <param name="hull">The convex hull segment that enclose the asteroid.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model used for the asteroids.</param>
        /// <param name="transforms">The graphical transforms that are applied to the asteroid.</param>
        public AsteroidList(int maxAsteroids, float radius, ConvexSegment hull, PhysicsEngine.Environment environment,
                            Model model, Matrix[] transforms)
        {
            this.asteroids = new Asteroid[maxAsteroids];
            this.asteroidRadius = radius;
            this.hulls = new ConvexHull[] { new ConvexHull(hull, Matrix.CreateScale(GameConstants.asteroidScale)) };
            this.physics = environment;
            this.asteroidModel = model;
            this.asteroidTransforms = transforms;
        }

        /// <summary>
        /// Adds an asteroid to the game world.
        /// </summary>
        public void AddAsteroid()
        {
            // Search for an empty asteroid slot
            int nextAsteroidIndex = 0;
            while (nextAsteroidIndex < asteroids.Length && asteroids[nextAsteroidIndex] != null 
                   && asteroids[nextAsteroidIndex].Active)
            {
                nextAsteroidIndex++;
            }
            // Insert the asteroid at a random position at a random initial velocity
            if (nextAsteroidIndex != asteroids.Length)
            {
                Vector3 asteroidPosition = new Vector3(CommonFunctions.GenerateRandom((int)(GameConstants.playfieldSizeX * 0.1), 
                                                       (int)(GameConstants.playfieldSizeX * 0.9)),
                                                       CommonFunctions.GenerateRandom((int)(GameConstants.playfieldSizeX * 0.1), 
                                                       (int)(GameConstants.playfieldSizeY * 0.9)),
                                                       -1 * (float)(GameConstants.cameraMaxDistance * 0.8));
                Vector3 asteroidVelocity = new Vector3(0, 0, CommonFunctions.GenerateRandom(GameConstants.asteroidMinSpeed, 
                                                                                            GameConstants.asteroidMaxSpeed));
                asteroids[nextAsteroidIndex] = new Asteroid(asteroidRadius, hulls, physics, asteroidModel, asteroidTransforms, 
                                                            asteroidPosition, asteroidVelocity);
                asteroids[nextAsteroidIndex].Active = true;
                numAsteroids++;
            }
        }

        /// <summary>
        /// Draws the list of asteroids to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid != null && asteroid.Active)
                {
                    asteroid.Draw(camera, aspectRatio);
                }
            }
        }

        /// <summary>
        /// Removes asteroids from the system if they have gone outside of the playing field.
        /// </summary>
        public void Update()
        {
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid != null && asteroid.Active && (   asteroid.Position.Z > GameConstants.cameraHeight
                                                            || asteroid.Position.Z < -GameConstants.cameraMaxDistance
                                                            || asteroid.Position.Y > GameConstants.playfieldSizeY
                                                            || asteroid.Position.Y < -GameConstants.playfieldSizeY
                                                            || asteroid.Position.X > GameConstants.playfieldSizeX
                                                            || asteroid.Position.X < -GameConstants.playfieldSizeX))
                {
                    physics.Remove(asteroid.PhysicsReference);
                    numAsteroids--;
                    if (asteroid.Position.Z > GameConstants.cameraHeight)
                    {
                        Score.AsteroidPassed();
                    }
                    asteroid.Active = false;
                }
            }
        }

        /// <summary>
        /// Determines whether the maximum number of asteroids has been reached.
        /// </summary>
        /// <returns>Returns true if the list of asteroids is full, false otherwise.</returns>
        public bool IsFull()
        {
            return numAsteroids == asteroids.Length;
        }

        /// <summary>
        /// Resets the asteroid list to be empty.
        /// </summary>
        public void Reset()
        {
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid != null && asteroid.Active)
                {
                    physics.Remove(asteroid.PhysicsReference);
                    asteroid.Active = false;
                }
            }
            numAsteroids = 0;
        }
    }
}
