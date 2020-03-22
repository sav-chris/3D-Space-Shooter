///

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
    class Asteroid
    {
        // When the object is created, it needs to be added to the physics engine under physicsReference
        Model objectModel;
        Entity physicsReference;
        Matrix[] transforms;
        bool active = false;

        public Model ObjectModel
        {
            get
            {
                return objectModel;
            }
            set
            {
                objectModel = value;
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

        public Entity PhysicsReference
        {
            get
            {
                return physicsReference;
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

        public Vector3 Position
        {
            get
            {
                return physicsReference.Position;
            }
            set
            {
                physicsReference.Position = value;
            }
        }

        /// <summary>
        /// The default constructor for an asteroid.
        /// </summary>
        /// <param name="radius">A radius that encloses the entire asteroid model, centred at (0,0,0) in object space.</param>
        /// <param name="hulls">The convex hull segments that enclose the asteroid.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model used for the asteroids.</param>
        /// <param name="transforms">The graphical transforms that are applied to the asteroid.</param>
        /// <param name="initialPosition">The initial position of the asteroid.</param>
        /// <param name="initialVelocity">The initial velocity of the asteroid.</param>
        public Asteroid(float radius, ConvexHull[] hulls, PhysicsEngine.Environment environment, Model model, Matrix[] transforms, 
                        Vector3 initialPosition, Vector3 initialVelocity)
        {
            physicsReference = new DefaultEntity(initialPosition, new Vector3(CommonFunctions.GenerateRandom(-1, 1),
                                                                              CommonFunctions.GenerateRandom(-1, 1),
                                                                              CommonFunctions.GenerateRandom(-1, 1)),
                                                 40, radius, new Hull(hulls), new ElasticCollision(GameConstants.elasticityFactor), 
                                                 environment, 0.0f);
            environment.Add(physicsReference);
            this.objectModel = model;
            this.transforms = transforms;
            physicsReference.Position = initialPosition;
            physicsReference.Velocity = initialVelocity;
        }

        /// <summary>
        /// Draws the asteroid to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>        
        public void Draw(Camera camera, float aspectRatio)
        {
            Matrix asteroidTransforms = Matrix.CreateScale(GameConstants.asteroidScale) * physicsReference.Transform;
            CommonFunctions.DrawModel(objectModel, asteroidTransforms, transforms, camera, aspectRatio);
        }
    }
}
