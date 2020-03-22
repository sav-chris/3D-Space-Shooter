using System;
using System.Collections.Generic;
using System.Text;

namespace _D_Space_Shooter
{
    static class Score
    {
        public static int asteroidsHit = 0;
        public static int asteroidsMissed = 0;
        public static int bulletsShot = 0;
        public static int level = 1;
        public static int score = 0;

        /// <summary>
        /// Resets all score values and game variables.
        /// </summary>
        public static void Reset()
        {
            asteroidsHit = 0;
            asteroidsMissed = 0;
            bulletsShot = 0;
            score = 0;
            level = 1;
            GameConstants.timeBetweenAsteroids = GameConstants.initialTimeBetweenAsteroids;
            GameConstants.asteroidMinSpeed = GameConstants.initialAsteroidMinSpeed;
            GameConstants.asteroidMaxSpeed = GameConstants.initialAsteroidMaxSpeed;
            GameConstants.perimeterSpeedAdjustment = GameConstants.initialPerimeterSpeedAdjustment;
        }

        /// <summary>
        /// Increases the difficulty of the game.
        /// </summary>
        public static void LevelUp()
        {
            if (GameConstants.timeBetweenAsteroids - GameConstants.asteroidTimeDecrease > GameConstants.minTimeBetweenAsteroids)
            {
                GameConstants.timeBetweenAsteroids -= GameConstants.asteroidTimeDecrease;
            }
            GameConstants.asteroidMinSpeed += GameConstants.asteroidSpeedIncrease;
            GameConstants.asteroidMaxSpeed += GameConstants.asteroidSpeedIncrease;
            GameConstants.perimeterSpeedAdjustment += GameConstants.perimeterSpeedIncrease;
            level++;
        }

        /// <summary>
        /// Increases the number of asteroids that have passed and applies a penalty to the score.
        /// </summary>
        public static void AsteroidPassed()
        {
            asteroidsMissed++;
            if (score - GameConstants.asteroidPassedPenalty >= 0)
            {
                score -= GameConstants.asteroidPassedPenalty;
            }
            else
            {
                score = 0;
            }
        }

        /// <summary>
        /// Increases the number of asteroids hit and adds to the score.
        /// </summary>
        public static void AsteroidHit()
        {
            asteroidsHit++;
            score += GameConstants.asteroidHitBonus;
        }

        /// <summary>
        /// Increases the number of shots fired and decreases the score.
        /// </summary>
        public static void BulletShot()
        {
            bulletsShot++;
            if (score - GameConstants.shotFiredPenalty >= 0)
            {
                score -= GameConstants.shotFiredPenalty;
            }
            else
            {
                score = 0;
            }
        }
    }
}
