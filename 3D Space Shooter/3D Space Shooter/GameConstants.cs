using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _D_Space_Shooter
{
    static class GameConstants
    {
        // Physics Constants
        public static float elasticityFactor = 1.0f;
        static public float gravity = 10;

        // Camera Adjustment Constants
        public static float cameraHeight = 1000f;
        public static float playfieldSizeX = 800f;
        public static float playfieldSizeY = 600f;
        public const float cameraMaxDistance = 6000f;
        public const float perspective = 40.0f;

        // Movement Constants
        static public float movementAmount = 10;

        // Asteroid Constants
        static public int asteroidMinSpeed = 40;
        static public int asteroidMaxSpeed = 120;
        static public int numAsteroids = 200;
        static public float timeBetweenAsteroids = 1500.0f;

        // Bullet Constants
        static public int maxBullets = 200;
        static public float bulletSpeed = 200;
        static public float timeBetweenBullets = 100; //in milliseconds

        // Level constants
        static public int secondsPerLevel = 10;
        static public int asteroidTimeDecrease = 150;
        static public int minTimeBetweenAsteroids = 300;
        static public int asteroidSpeedIncrease = 20;
        static public float perimeterSpeedAdjustment = 1.0f;
        static public float perimeterSpeedIncrease = 0.075f;
        static public int asteroidPassedPenalty = 20;
        static public int asteroidHitBonus = 50;
        static public int shotFiredPenalty = 1;

        // Perimeter Asteroids
        static public float perimeterMinSpeed = 10;
        static public float perimeterMaxSpeed = 50;
        static public int numPerimeterAsteroids = 40;
        static public float perimeterScale = 0.009f;

        // Particle Constants
        static public float timeToDisplayEffect = 1000.0f; //in milliseconds
        static public int explosionNumParticles = 75;
        static public int initialNumParticles = 75;
        static public float explosionParticleSpeed = 250.0f;
        static public float particleLifeTime = 0.5f; // in seconds
        static public float particleLifeTimeVariance = 0.5f;
        static public float particleBirthTime = 0.0f; // in seconds
        static public float particleScale = 0.15f;
        static public Vector3 particleBaseDirection = Vector3.Zero;
        static public float particleDirectionVariance = 100.0f;
        static public Color explosionColour = Color.Red;
        static public byte colourVariance = 1;
        static public int maxExplosions = 10;

        // Text Stuff
        static public string restartMessage = "You Lose! Press the R key to restart.";

        static public int textLineSpacing = 25;
        static public string textTab = "        ";
        static public bool drawScore = true;

        // Scaling Constants
        static public float asteroidScale = 0.06f;
        static public float shipScale = 0.07f;
        static public float bulletScale = 0.07f;

        // Window Size
        public static int windowWidth = 1600;
        public static int windowHeight = 900;
        public static bool fullScreen = true;

        // Handy Stuff that shouldn't be touched
        static public float initialTimeBetweenAsteroids;
        static public float initialPerimeterSpeedAdjustment;
        static public int initialAsteroidMinSpeed;
        static public int initialAsteroidMaxSpeed;
    }
}