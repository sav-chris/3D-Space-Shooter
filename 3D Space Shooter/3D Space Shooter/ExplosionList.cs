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
    class ExplosionList
    {
        Explosion[] explosions;
        PhysicsEngine.Environment physics;
        Model explosionModel;
        Matrix[] explosionTransforms;
        int numExplosions = 0;

        /// <summary>
        /// The default constructor for a list of explosions.
        /// </summary>
        /// <param name="maxExplosions">The maximum number of explosions that can be displayed at any one time.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model to be used to draw a particle.</param>
        /// <param name="transforms">The graphical transforms to be applied to the particle.</param>
        public ExplosionList(int maxExplosions, PhysicsEngine.Environment environment, Model model, Matrix[] transforms)
        {
            this.explosions = new Explosion[maxExplosions];
            this.physics = environment;
            this.explosionModel = model;
            this.explosionTransforms = transforms;
        }

        /// <summary>
        /// Adds a new explosion to the explosion list.
        /// </summary>
        /// <param name="explosionPosition">The position that the explosion is to be added to the game world.</param>
        public void AddExplosion(Vector3 explosionPosition)
        {
            // Try to find an empty explosion slot in the array.
            int nextExplosionIndex = 0;
            while (nextExplosionIndex < explosions.Length && explosions[nextExplosionIndex] != null 
                   && explosions[nextExplosionIndex].Active)
            {
                nextExplosionIndex++;
            }

            // Add the new explosion provided an explosion slot could be found.
            if (nextExplosionIndex != explosions.Length)
            {
                explosions[nextExplosionIndex] = new Explosion(physics, explosionModel, explosionTransforms, explosionPosition);
                explosions[nextExplosionIndex].Active = true;
                numExplosions++;
            }
        }

        /// <summary>
        /// Draws all the active explosions to the screen.
        /// </summary>
        /// <param name="camera">A reference to the camera object.</param>
        /// <param name="aspectRatio">The aspect ratio of the screen.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            foreach (Explosion explosion in explosions)
            {
                if (explosion != null && explosion.Active)
                {
                    explosion.Draw(camera, aspectRatio);
                }
            }
        }

        /// <summary>
        /// Updates each explosion in the explosion array.
        /// </summary>
        /// <param name="gameTime">A structure that takes a snapshot of the games timing state.</param>
        public void Update(GameTime gameTime)
        {
            foreach (Explosion explosion in explosions)
            {
                // Start an explosion
                if (explosion != null && explosion.Active && explosion.LifeTime == 0.0f)
                {
                    explosion.LifeTime = GameConstants.timeToDisplayEffect;
                }
                // Decrement the life time of the explosion
                else if (explosion != null && explosion.Active && explosion.LifeTime != 0.0f)
                {
                    explosion.LifeTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                // Remove the explosion once it has displayed for its entire life time.
                if (explosion != null && explosion.Active && explosion.LifeTime < 0)
                {
                    physics.Remove(explosion.PhysicsReference);
                    numExplosions--;
                    explosion.Active = false;
                }
            }
        }

        /// <summary>
        /// Checks if the maximum number of explosions has been reached.
        /// </summary>
        /// <returns>True if the maximum number of explosions is reaches, false otherwise.</returns>
        public bool IsFull()
        {
            return numExplosions == explosions.Length;
        }
    }
}
