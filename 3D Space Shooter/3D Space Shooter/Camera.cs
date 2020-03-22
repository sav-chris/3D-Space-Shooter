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

namespace _D_Space_Shooter
{
    class Camera
    {
        //Camera/View information
        Vector3 cameraPosition = new Vector3(GameConstants.playfieldSizeX / 2, 
                                             GameConstants.playfieldSizeY / 2, 
                                             GameConstants.cameraHeight);
        Vector3 cameraFocusOn = new Vector3(GameConstants.playfieldSizeX / 2, 
                                            GameConstants.playfieldSizeY / 2, 
                                            0);
        Matrix viewMatrix;
        Matrix projectionMatrix;

        public Vector3 CameraPosition
        {
            get
            {
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
            }
        }

        public Vector3 CameraFocusOn
        {
            get
            {
                return cameraFocusOn;
            }
            set
            {
                cameraFocusOn = value;
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return viewMatrix;
            }
            set
            {
                viewMatrix = value;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
            set
            {
                projectionMatrix = value;
            }
        }
    }
}
