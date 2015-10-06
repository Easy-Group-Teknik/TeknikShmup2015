﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class SpawnManager
    {
        const int AMOUNT_OF_ENEMIES = 3;

        private int currentLevel;
        private int nextLevelCount;

        public int MaxNextLevelCount
        {
            get
            {
                return currentLevel * 128*2;
            }
        }

        private int sides;

        int[] enemySpawnCounts;
        int[] maxEnemySpawnCounts;

        public SpawnManager()
        {
            enemySpawnCounts = new int[AMOUNT_OF_ENEMIES];
            maxEnemySpawnCounts = new int[AMOUNT_OF_ENEMIES];

            maxEnemySpawnCounts[0] = 128;
        }

        public void EnemySpawnUpdate()
        {
            for (int i = 0; i < AMOUNT_OF_ENEMIES; i++)
            {
                enemySpawnCounts[i] += 1;

                if (enemySpawnCounts[i] >= maxEnemySpawnCounts[i])
                {
                    int side = Globals.Randomizer.Next(0, sides+1);
                    
                    float spawnAngle = Globals.DegreesToRadian(-270);

                    if(side == 2) spawnAngle = Globals.DegreesToRadian(-180);
                    else if(side == 1) spawnAngle = 0;

                    switch (i)
                    {
                        case 0:
                            GameObjectManager.Add(new Ship(GetSpawnPosition(side), spawnAngle, Globals.Randomizer.Next(2, 5), Globals.Randomizer.Next(64, 128)));
                            break;
                    }
                    enemySpawnCounts[i] = 0;
                }
            }
        }

        public void Update()
        {
            sides = (currentLevel >= 3) ? 1 : sides;
            sides = (currentLevel >= 6) ? 2 : sides;

            nextLevelCount += 1;

            if (nextLevelCount >= MaxNextLevelCount)
            {
                currentLevel += 1;
                nextLevelCount = 0;
            }

            EnemySpawnUpdate();
        }

        public Vector2 GetSpawnPosition(int side)
        {
            Vector2 tmp = Vector2.Zero;

            switch (side)
            {
                case 0:
                    tmp = new Vector2(Globals.Randomizer.Next(16, 800 - 16), Globals.Randomizer.Next(-500, -100));
                    break;
                case 1:
                    tmp = new Vector2(Globals.Randomizer.Next(-500, -200), Globals.Randomizer.Next(16, 480 - 16));
                    break;
                case 2:
                    tmp = new Vector2(Globals.Randomizer.Next(800 + 200, 800 + 500), Globals.Randomizer.Next(16, 480 - 16));
                    break;
            }

            return tmp;
        }
    }
}
