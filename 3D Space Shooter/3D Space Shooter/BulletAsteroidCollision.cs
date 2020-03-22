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
    class BulletAsteroidCollision : ElasticCollision
    {
        SoundBank soundBank;
        BulletList bullets;
        AsteroidList asteroids;
        ExplosionList explosions;

        /// <summary>
        /// Handles collisions between a bullet and an asteroid.
        /// </summary>
        /// <param name="left">The first object involved in the collision.</param>
        /// <param name="right">The second object involved in the collision.</param>
        /// <param name="point">The point that has been detected inside a hull.</param>
        /// <param name="normal">The normal to the closest surface to the point.</param>
        /// <param name="dist">The distance to the closest surface.</param>
        /// <param name="environment">A reference to the physics engine.</param>
        public override void Collide(Entity left, Entity right, Vector3 point, Vector3 normal, float dist, PhysicsEngine.Environment environment)
        {
            // Increase the score
            Score.AsteroidHit();

            base.Collide(left, right, point, normal, dist, environment);

            // Add an explosion to the list of explosions
            explosions.AddExplosion(left.Position);
            soundBank.PlayCue("explosion2");
        }

        /// <summary>
        /// The default constructor for a collision between a bullet and an asteroid.
        /// </summary>
        /// <param name="elasticity">The elasticity that will be used for the collision.</param>
        /// <param name="soundBank">A reference to the game's SoundBank.</param>
        /// <param name="bullets">A reference to the list of bullets.</param>
        /// <param name="asteroids">A reference to the list of asteroids.</param>
        /// <param name="explosions">A reference to the list of explosions.</param>
        public BulletAsteroidCollision(float elasticity, SoundBank soundBank, BulletList bullets, AsteroidList asteroids, ExplosionList explosions) : base(elasticity)
        {
            this.soundBank = soundBank;
            this.bullets = bullets;
            this.asteroids = asteroids;
            this.explosions = explosions;
        }
    }
}
