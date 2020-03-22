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
    class Ship
    {
        // When the object is created, it needs to be added to the physics engine under physicsReference
        Model objectModel;
        Entity physicsReference;
        Matrix[] transforms;
        SoundBank soundBank;
        bool isAlive = true;

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

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;
            }
        }

        /// <summary>
        /// The default constructor for the ship class.
        /// </summary>
        /// <param name="radius">An estimate to the radius that encloses the ship, measured from (0,0,0) in object space.</param>
        /// <param name="hull">The convex hull for the ship model.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model used to draw the ship.</param>
        /// <param name="transforms">The graphics transforms to be applied to the model.</param>
        /// <param name="initialPosition">The initial position of the ship.</param>
        /// <param name="initialSpeed">The initial speed of the ship.</param>
        /// <param name="soundBank">A reference to the game's SoundBank.</param>
        /// <param name="explosions">A reference to the list of explosions.</param>
        /// <param name="asteroids">A reference to the list of asteroids.</param>
        public Ship(float radius, ConvexSegment hull, PhysicsEngine.Environment environment, Model model, Matrix[] transforms,
                        Vector3 initialPosition, float initialSpeed, SoundBank soundBank, ExplosionList explosions, AsteroidList asteroids)
        {
            ConvexHull[] hulls = new ConvexHull[] { new ConvexHull(hull, Matrix.CreateScale(GameConstants.shipScale)) };
            physicsReference = new NonForceEntity(initialPosition, Vector3.Up, 25.0f, radius, new Hull(hulls), 
                                                new ShipAsteroidCollision(soundBank, explosions, this, asteroids));
            environment.Add(physicsReference);
            this.objectModel = model;
            this.transforms = transforms;

            // Set the initial speed of the object
            physicsReference.Velocity = Vector3.UnitZ * initialSpeed;
            this.soundBank = soundBank;
        }

        /// <summary>
        /// Resets the ship to begin a new game.
        /// </summary>
        public void Reset()
        {
            isAlive = true;

            // Move ship to the centre of the screen
            physicsReference.Position = new Vector3(GameConstants.playfieldSizeX / 2, GameConstants.playfieldSizeY / 2, 0);

            soundBank.PlayCue("hyperspace_activate");
        }

        /// <summary>
        /// Moves the ship up.
        /// </summary>
        /// <param name="distance">The distance to move the ship.</param>
        public void Up(float distance)
        {
            physicsReference.Position += Vector3.UnitY * distance;

            // If the ship goes off the top of the screen, move it to the bottom of the screen and play a sound.
            if (physicsReference.Position.Y > GameConstants.playfieldSizeY)
            {
                physicsReference.Position = new Vector3(physicsReference.Position.X, distance, physicsReference.Position.Z);
                soundBank.PlayCue("hyperspace_activate");
            }
        }

        /// <summary>
        /// Moves the ship down.
        /// </summary>
        /// <param name="distance">The distance to move the ship.</param>
        public void Down(float distance)
        {
            physicsReference.Position -= Vector3.UnitY * distance;

            // If the ship goes off the bottom of the screen, move it to the top of the screen and play a sound.
            if (physicsReference.Position.Y < 0)
            {
                physicsReference.Position = new Vector3(physicsReference.Position.X, GameConstants.playfieldSizeY - distance, physicsReference.Position.Z);
                soundBank.PlayCue("hyperspace_activate");
            }
        }

        /// <summary>
        /// Moves the ship left.
        /// </summary>
        /// <param name="distance">The distance to move the ship.</param>
        public void Left(float distance)
        {
            physicsReference.Position -= Vector3.UnitX * distance;

            // If the ship goes off the left of the screen, move it to the right of the screen and play a sound.
            if (physicsReference.Position.X < 0)
            {
                physicsReference.Position = new Vector3(GameConstants.playfieldSizeX - distance, physicsReference.Position.Y, physicsReference.Position.Z);
                soundBank.PlayCue("hyperspace_activate");
            }
        }

        /// <summary>
        /// Moves the ship right.
        /// </summary>
        /// <param name="distance">The distance to move the ship.</param>
        public void Right(float distance)
        {
            physicsReference.Position += Vector3.UnitX * distance;

            // If the ship goes off the right of the screen, move it to the left of the screen and play a sound.
            if (physicsReference.Position.X > GameConstants.playfieldSizeX)
            {
                physicsReference.Position = new Vector3(distance, physicsReference.Position.Y, physicsReference.Position.Z);
                soundBank.PlayCue("hyperspace_activate");
            }
        }

        /// <summary>
        /// Draws the ship to the screen
        /// </summary>
        /// <param name="camera">A reference to the camera object.</param>
        /// <param name="aspectRatio">The aspect ratio of the screen.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            if (isAlive)
            {
                Matrix objectTransforms = Matrix.CreateScale(GameConstants.shipScale)
                                      * Matrix.CreateRotationY(300 * MathHelper.Pi / 180) 
                                      * physicsReference.Transform;
                CommonFunctions.DrawModel(objectModel, objectTransforms, transforms, camera, aspectRatio);
            }
        }
    }
}
