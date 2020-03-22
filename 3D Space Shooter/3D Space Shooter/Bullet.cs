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
    class Bullet
    {
        // When the object is created, it needs to be added to the physics engine under physicsReference
        Model bulletModel;
        Entity physicsReference;
        Matrix[] transforms;
        bool active = false;

        public Model BulletModel
        {
            get
            {
                return bulletModel;
            }
            set
            {
                bulletModel = value;
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
        /// The default constructor for a bullet.
        /// </summary>
        /// <param name="radius">A radius that encloses the entire bullet model, centred at (0,0,0) in object space.</param>
        /// <param name="hulls">The convex hull segments that enclose the bullet.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model used for the bullets.</param>
        /// <param name="transforms">The graphical transforms that are applied to the bullet.</param>
        /// <param name="initialPosition">The initial position of the bullet.</param>
        /// <param name="initialVelocity">The initial velocity of the bullet.</param>
        /// <param name="soundBank">A reference to the SoundBank used for the game's sound effects.</param>
        /// <param name="bullets">A reference to the list of bullets.</param>
        /// <param name="asteroids">A reference to the list of asteroids.</param>
        /// <param name="explosions">A reference to the list of explosions.</param>
        public Bullet(float radius, ConvexHull[] hulls, PhysicsEngine.Environment environment, Model model, Matrix[] transforms,
                        Vector3 initialPosition, Vector3 initialVelocity, SoundBank soundBank, BulletList bullets, AsteroidList asteroids, ExplosionList explosions)
        {
            physicsReference = new NonForceEntity(initialPosition, Vector3.Up, 25, radius, new Hull(hulls), new BulletAsteroidCollision(1.0f, soundBank, bullets, asteroids, explosions));
            environment.Add(physicsReference);
            this.bulletModel = model;
            this.transforms = transforms;
            physicsReference.Velocity = initialVelocity;
        }

        /// <summary>
        /// Draws the bullet to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            Matrix bulletTransforms = Matrix.CreateScale(GameConstants.bulletScale) * physicsReference.Transform;
            CommonFunctions.DrawModel(bulletModel, bulletTransforms, transforms, camera, aspectRatio);
        }
    }
}
