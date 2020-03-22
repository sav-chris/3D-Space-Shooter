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
    class PerimeterObject
    {
        Vector3 position;
        Vector3 velocity;

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        /// <summary>
        /// The default constructor for a perimeter object.
        /// </summary>
        /// <param name="initialPosition">The initial position of the object.</param>
        /// <param name="velocity">The velocity of the object.</param>
        public PerimeterObject(Vector3 initialPosition, Vector3 velocity)
        {
            this.position = initialPosition;
            this.velocity = velocity;
        }

        /// <summary>
        /// Updates the position of the perimeter object.
        /// </summary>
        public void Update()
        {
            position += GameConstants.perimeterSpeedAdjustment * velocity;

            // If the object goes past the camera, move it back to the start of its path.
            if (position.Z > GameConstants.cameraHeight)
            {
                position.Z = (float)(GameConstants.cameraHeight - 0.9 * GameConstants.cameraMaxDistance);
            }
        }
    }
}
