using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;
using System;
using Pig_Island.Tiles;

namespace Pig_Island
{
    class Generator : ModWorld
    {
        public static int pigGrassCount = 0;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            Main.statusText = "Adding The Pig Lands!";
            int floatingIslandIndex = tasks.FindIndex(genpas => genpas.Name.Equals("Floating Islands"));
            tasks.RemoveAt(floatingIslandIndex);
            if (floatingIslandIndex != -1)
                tasks.Insert(floatingIslandIndex, new PassLegacy("Floating Pig Islands", FloatingPigIslands));
        }

        private void FloatingPigIslands(GenerationProgress p)
        {
            Random random = new Random();
            int lastClusterX = 0;
            int maxClusters = 0;
            if (Main.maxTilesX <= 4200)
                maxClusters = 3;
            else if (Main.maxTilesX <= 6400)
                maxClusters = 5;
            else if (Main.maxTilesX <= 8400)
                maxClusters = 7;
            for (int cluster = 0; cluster < maxClusters; cluster++)
            {
                // cluster init
                int[] clusterStartingLocation = { 0, 0 };
                int runTracker = 0;
                int maxRun = 5;
                bool dontCreate = false;
                int lastCloudX = clusterStartingLocation[0]; // needs to be here
                while (true)
                { 
                    int[] temp = { random.Next(200, Main.maxTilesX), random.Next(80, 150) };
                    clusterStartingLocation = temp;
                    if (lastCloudX == 0 || (Math.Abs(clusterStartingLocation[0] - lastClusterX) > 200 && Math.Abs(lastCloudX - clusterStartingLocation[0]) > 200 && Math.Abs(clusterStartingLocation[0] + 200 - lastClusterX) > 200))
                        break;
                    if (runTracker++ >= maxRun)
                    {
                        dontCreate = true;
                        break;
                    }
                }

                if (dontCreate)
                    continue;

                lastClusterX = clusterStartingLocation[0];
                lastCloudX = clusterStartingLocation[0];

                int islandCount = random.Next(4, 8);
                for (int island = 0; island < islandCount; island++)
                {
                    // island init
                    int[] cloudSize = { random.Next(40, 60), 10 };
                    int[] startingLocation = { lastCloudX + cloudSize[0] + random.Next(5, 20), clusterStartingLocation[1] + random.Next(-30, 30) };
                    int[] grassSize = { cloudSize[0], 1 }; // should have same x as cloudSize
                    int[] airSize = { cloudSize[0], 10 }; // this is to get rid of any cloud that stays at the top of the island
                    int lengthModifier = 0;
                    int textureSize = 4;
                    lastCloudX = startingLocation[0] + cloudSize[0];
                    // cloud spawning - cloud layer
                    for (int i = 0; i < cloudSize[1]; i++)
                    {
                        lengthModifier += random.Next(1, 5);
                        for (int k = 0; k < cloudSize[0] - lengthModifier; k++)
                        {
                            int runnerSpeedX = 10 * ((k < (cloudSize[0] - lengthModifier) / 2) ? 1 : -1);
                            WorldGen.PlaceTile(startingLocation[0] + k + lengthModifier / 2, startingLocation[1] + i, TileID.Cloud);
                            WorldGen.TileRunner(startingLocation[0] + k + lengthModifier / 2, startingLocation[1] + i, textureSize + random.Next(-textureSize / 2, textureSize / 2), 10, TileID.Cloud, true, runnerSpeedX, 0, false, true);
                        }
                    }

                    // cloud spawning - grass layer
                    for (int i = 0; i < grassSize[1] + textureSize; i++)
                    {
                        for (int k = 0; k < grassSize[0]; k++)
                        {
                            WorldGen.PlaceTile(startingLocation[0] + k, startingLocation[1] - i, TileType<PigGrass>(), false, true);
                            WorldGen.PlaceTile(startingLocation[0] + k, startingLocation[1] - i, TileID.Grass, false, true);
                        }
                    }
                }
            }
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            pigGrassCount = tileCounts[TileType<PigGrass>()];
        }
    }
}
