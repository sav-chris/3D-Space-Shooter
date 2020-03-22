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
    static class CommonFunctions
    {
        static Random randomGenerator = new Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// Sets up the default graphical effects for a given model.
        /// </summary>
        /// <param name="myModel">The model to generate effects for.</param>
        /// <param name="camera">A reference to the games camera.</param>
        /// <returns>The graphical transforms that need to be applied to the model when it is drawn.</returns>
        public static Matrix[] SetupEffectDefaults(Model myModel, Camera camera)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                }
            }
            return absoluteTransforms;
        }

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, Camera camera, float aspectRatio)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //Microsoft.Xna.Framework.Graphics.
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                    effect.View = Matrix.CreateLookAt(camera.CameraPosition, camera.CameraFocusOn, Vector3.Up); 
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(GameConstants.perspective),
                        aspectRatio, 1.0f, GameConstants.cameraMaxDistance);
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, Camera camera, float aspectRatio, Color tint)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = new Vector3(tint.R, tint.G, tint.B);
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                    effect.View = Matrix.CreateLookAt(camera.CameraPosition, camera.CameraFocusOn, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(GameConstants.perspective),
                        aspectRatio, 1.0f, GameConstants.cameraMaxDistance);
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        /// <summary>
        /// Generates a random float from a uniform distribution between two integer boundaries.
        /// </summary>
        /// <param name="lowerBound">The lowest value to return.</param>
        /// <param name="upperBound">The highest value to return.</param>
        /// <returns>A random float between the two input parameters.</returns>
        public static float GenerateRandom(int lowerBound, int upperBound)
        {
            return (float)(upperBound - lowerBound) * (float)randomGenerator.NextDouble() + lowerBound;
        }
    }
}
