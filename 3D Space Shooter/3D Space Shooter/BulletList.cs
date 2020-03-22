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
    class BulletList
    {
        Bullet[] bullets;
        Vector3 bulletVelocity;
        float bulletRadius;
        ConvexHull[] hulls;
        PhysicsEngine.Environment physics;
        Model bulletModel;
        Matrix[] bulletTransforms;
        SoundBank soundBank;
        AsteroidList asteroids;
        ExplosionList explosions;

        /// <summary>
        /// The default constructor for a list of bullets.
        /// </summary>
        /// <param name="maxBullets">The maximum number of bullets that can be active at any time.</param>
        /// <param name="bulletSpeed">The speed that each bullet moves.</param>
        /// <param name="radius">A radius that encloses the entire bullet model, centred at (0,0,0) in object space.</param>
        /// <param name="hull">The convex hull segment that encloses the bullet.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        /// <param name="model">The model used for the bullets.</param>
        /// <param name="transforms">The graphical transforms that are applied to the bullet.</param>
        /// <param name="soundBank">A reference to the SoundBank used for the game's sound effects.</param>
        /// <param name="asteroids">A reference to the list of asteroids.</param>
        /// <param name="explosions">A reference to the list of explosions.</param>
        public BulletList(int maxBullets, float bulletSpeed, float radius, ConvexSegment hull, 
                          PhysicsEngine.Environment environment, Model model, Matrix[] transforms,
                          SoundBank soundBank, AsteroidList asteroids, ExplosionList explosions)
        {
            this.bullets = new Bullet[maxBullets];
            this.bulletVelocity = new Vector3(0, 0, (-1) * bulletSpeed);
            this.bulletRadius = radius;
            this.hulls = new ConvexHull[] { new ConvexHull(hull, Matrix.CreateScale(GameConstants.bulletScale)) };
            this.physics = environment;
            this.bulletModel = model;
            this.bulletTransforms = transforms;
            this.soundBank = soundBank;
            this.asteroids = asteroids;
            this.explosions = explosions;
        }

        /// <summary>
        /// Shoots a new bullet.
        /// </summary>
        /// <param name="initialPosition">The position that the bullet shoots from.</param>
        public void Shoot(Vector3 initialPosition)
        {
            // Find an empty bullet slot if one exists
            int nextBulletIndex = 0;
            while (nextBulletIndex < bullets.Length && bullets[nextBulletIndex] != null && bullets[nextBulletIndex].Active)
            {
                nextBulletIndex++;
            }

            // Add the bullet uf a bullet slot is available
            if (nextBulletIndex != bullets.Length)
            {
                bullets[nextBulletIndex] = new Bullet(bulletRadius, hulls, physics, bulletModel, bulletTransforms, 
                                                      initialPosition, bulletVelocity, soundBank, this, asteroids, explosions);
                bullets[nextBulletIndex].Active = true;
                Score.BulletShot();
            }
        }

        /// <summary>
        /// Draw each bullet to the screen.
        /// </summary>
        /// <param name="camera">A reference to the game camera.</param>
        /// <param name="aspectRatio">The aspect ratio the game is running in.</param>
        public void Draw(Camera camera, float aspectRatio)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null && bullet.Active)
                {
                    bullet.Draw(camera, aspectRatio);
                }
            }
        }

        /// <summary>
        /// Removes bullets from the system if they have gone past all the asteroids.
        /// </summary>
        public void Update()
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null && bullet.Active && bullet.Position.Z < ((-1) * (GameConstants.cameraMaxDistance - GameConstants.cameraHeight)))
                {
                    physics.Remove(bullet.PhysicsReference);
                    bullet.Active = false;
                }
            }
        }
    }
}
