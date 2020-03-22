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
    class Explosion
    {
        float lifeTime;
        ParticleSystem physicsReference;
        bool active = false;
        Model objectModel;
        Matrix[] transforms;

        public float LifeTime
        {
            get
            {
                return lifeTime;
            }
            set
            {
                lifeTime = value;
            }
        }

        public ParticleSystem PhysicsReference
        {
            get
            {
                return physicsReference;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// The default constructor for an explosion.
        /// </summary>
        /// <param name="physics">A reference to the physics engine.</param>
        /// <param name="explosionModel">The model to use for a particle in the explosion.</param>
        /// <param name="explosionTransforms">The graphical transforms to apply to the model.</param>
        /// <param name="explosionPosition">The position of the explosion.</param>
        public Explosion(PhysicsEngine.Environment physics, Model explosionModel, 
                         Matrix[] explosionTransforms, Vector3 explosionPosition)
        {
            this.objectModel = explosionModel;
            this.transforms = explosionTransforms;
            
            // Set up the particle parameters based on game constants
            ParticleSystemParameters psp = new ParticleSystemParameters();
            psp.BirthTime = GameConstants.particleBirthTime;
            psp.BirthTimeVariance = 0;
            psp.Colour = GameConstants.explosionColour;
            psp.ColourVariance = GameConstants.colourVariance;
            psp.Direction = GameConstants.particleBaseDirection;
            psp.DirectionVariance = GameConstants.particleDirectionVariance;
            psp.InitialParticles = GameConstants.initialNumParticles;
            psp.LifeTime = GameConstants.particleLifeTime;
            psp.LifeTimeVariance = GameConstants.particleLifeTimeVariance;
            psp.Mass = 1;
            psp.MassVariance = 0.1f;
            psp.MaxParticles = GameConstants.explosionNumParticles;
            psp.Position = explosionPosition;
            psp.Speed = GameConstants.explosionParticleSpeed;
            psp.SpeedVariance = 0;

            // Create the particle system and add it to the physics engine.
            this.physicsReference = new ParticleSystem(psp);
            physics.Add(physicsReference);
        }

        /// <summary>
        /// Draws an explosion effect to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            // Loop through each particle and draw it to the screen.
            foreach (Particle particle in physicsReference.Particles)
            {
                Matrix tempTransforms = Matrix.CreateScale(GameConstants.particleScale) * Matrix.CreateTranslation(particle.Position);
                CommonFunctions.DrawModel(objectModel, tempTransforms, transforms, camera, aspectRatio, particle.Colour);

                // Ensure we are not trying to draw too many particle effects at the same time.
                if (physicsReference.Particles.Count > 5)
                {
                    int i = 5;
                    i++;
                }
            }
        }
    }
}
