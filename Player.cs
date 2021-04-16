using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Pig_Island.Tiles;

namespace Pig_Island
{
    class Player : ModPlayer
    {
        public bool inPigIslands = false;
        public override void UpdateBiomes()
        {
            inPigIslands = Generator.pigGrassCount > 50;
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (inPigIslands)
            {
                return mod.GetTexture("Background");
            }
            return null;
        }
    }
}
