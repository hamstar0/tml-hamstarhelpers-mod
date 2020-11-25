using System;
using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to world generation.
	/// </summary>
	public class WorldGenHelpers {
		/// <summary></summary>
		public static readonly string[] VanillaWorldGenTaskNames = new string[] {
			"Reset",
			"Terrain",
			"Tunnels",
			"Sand",
			"Mount Caves",
			"Dirt Wall Backgrounds",
			"Rocks In Dirt",
			"Dirt In Rocks",
			"Clay",
			"Small Holes",
			"Dirt Layer Caves",
			"Rock Layer Caves",
			"Surface Caves",
			"Slush Check",
			"Grass",
			"Jungle",
			"Marble",
			"Granite",
			"Mud Caves To Grass",
			"Full Desert",
			"Floating Islands",
			"Mushroom Patches",
			"Mud To Dirt",
			"Silt",
			"Shinies",
			"Webs",
			"Underworld",
			"Lakes",
			"Dungeon",
			"Corruption",
			"Slush",
			"Mud Caves To Grass",
			"Beaches",
			"Gems",
			"Gravitating Sand",
			"Clean Up Dirt",
			"Pyramids",
			"Dirt Rock Wall Runner",
			"Living Trees",
			"Wood Tree Walls",
			"Altars",
			"Wet Jungle",
			"Remove Water From Sand",
			"Jungle Temple",
			"Hives",
			"Jungle Chests",
			"Smooth World",
			"Settle Liquids",
			"Waterfalls",
			"Ice",
			"Wall Variety",
			"Traps",
			"Life Crystals",
			"Statues",
			"Buried Chests",
			"Surface Chests",
			"Jungle Chests Placement",
			"Water Chests",
			"Spider Caves",
			"Gem Caves",
			"Moss",
			"Temple",
			"Ice Walls",
			"Jungle Trees",
			"Floating Island Houses",
			"Quick Cleanup",
			"Pots",
			"Hellforge",
			"Spreading Grass",
			"Piles",
			"Moss",
			"Spawn Point",
			"Grass Wall",
			"Guide",
			"Sunflowers",
			"Planting Trees",
			"Herbs",
			"Dye Plants",
			"Webs And Honey",
			"Weeds",
			"Mud Caves To Grass",
			"Jungle Plants",
			"Vines",
			"Flowers",
			"Mushrooms",
			"Stalac",
			"Gems In Ice Biome",
			"Random Gems",
			"Moss Grass",
			"Muds Walls In Jungle",
			"Larva",
			"Settle Liquids Again",
			"Tile Cleanup",
			"Lihzahrd Altars",
			"Micro Biomes",
			"Final Cleanup",
		};



		////////////////

		/// <summary>
		/// Gets the full list of world gen passes.
		/// </summary>
		/// <returns></returns>
		public static IList<GenPass> GetWorldGenPasses() {
			if( !ReflectionHelpers.Get(typeof(WorldGen), null, "_generator", out WorldGenerator generator) ) {
				return null;
			}
			if( !ReflectionHelpers.Get(typeof(WorldGenerator), generator, "_passes", out IList<GenPass> passes) ) {
				return null;
			}
			return passes;
		}
	}
}
