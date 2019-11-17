using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile frames.
	/// </summary>
	public class TileFrameHelpers {
		/// <summary>
		/// Gets chest type names by frame.
		/// </summary>
		public readonly static IReadOnlyDictionary<int, string> VanillaChestTypeNamesByFrame;
		/// <summary>
		/// Gets chest frame by type names.
		/// </summary>
		public readonly static IReadOnlyDictionary<string, int> VanillaChestFramesByTypeName;


		////

		static TileFrameHelpers() {
			var chestTypesByFrame = new Dictionary<int, string> {
				{ 0, "Chest" },
				{ 1, "Gold Chest" },
				{ 2, "Locked Gold Chest" },
				{ 3, "Shadow Chest" },
				{ 4, "Locked Shadow Chest" },	// Shadow Chest?
				{ 5, "Barrel" },
				{ 6, "Trash Can" },
				{ 7, "Ebonwood Chest" },
				{ 8, "Rich Mahogany Chest" },	// Mushroom Chest?
				{ 9, "Pearlwood Chest" },
				{ 10, "Ivy Chest" },	//Rich Mahogany Chest?
				{ 11, "Ice Chest" },
				{ 12, "Living Wood Chest" },
				{ 13, "Skyware Chest" },
				{ 14, "Shadewood Chest" },
				{ 15, "Web Covered Chest" },
				{ 16, "Lihzahrd Chest" },
				{ 17, "Water Chest" },
				{ 18, "Jungle Chest" },
				{ 19, "Corruption Chest" },
				{ 20, "Crimson Chest" },
				{ 21, "Hallowed Chest" },
				{ 22, "Frozen Chest" },
				{ 23, "Locked Jungle Chest" },
				{ 24, "Locked Corruption Chest" },
				{ 25, "Locked Crimson Chest" },
				{ 26, "Locked Hallowed Chest" },
				{ 27, "Locked Frozen Chest" },
				{ 28, "Dynasty Chest" },
				{ 29, "Honey Chest" },
				{ 30, "Steampunk Chest" },
				{ 31, "Palm Wood Chest" },
				{ 32, "Mushroom Chest" },
				{ 33, "Boreal Wood Chest" },
				{ 34, "Slime Chest" },
				{ 35, "Green Dungeon Chest" },
				{ 36, "Locked Green Dungeon Chest" },
				{ 37, "Pink Dungeon Chest" },
				{ 38, "Locked Pink Dungeon Chest" },
				{ 39, "Blue Dungeon Chest" },
				{ 40, "Locked Blue Dungeon Chest" },
				{ 41, "Bone Chest" },
				{ 42, "Cactus Chest" },
				{ 43, "Flesh Chest" },
				{ 44, "Obsidian Chest" },
				{ 45, "Pumpkin Chest" },
				{ 46, "Spooky Chest" },
				{ 47, "Glass Chest" },
				{ 48, "Martian Chest" },
				{ 49, "Meteorite Chest" },
				{ 50, "Granite Chest" },
				{ 51, "Marble Chest" },
				{ 52, "Crystal Chest" },
				{ 53, "Golden Chest" },
				{ 50, "Granite Chest" },
				{ 51, "Marble Chest" },
				{ 23, "Jungle Chest" },
				{ 24, "Corruption Chest" },
				{ 25, "Crimson Chest" },
				{ 26, "Hallowed Chest" },
				{ 27, "Frozen Chest" },
			};

			TileFrameHelpers.VanillaChestTypeNamesByFrame = new ReadOnlyDictionary<int, string>( chestTypesByFrame );
			TileFrameHelpers.VanillaChestFramesByTypeName = new ReadOnlyDictionary<string, int>(
				chestTypesByFrame.ToDictionary( kv => kv.Value, kv => kv.Key )
			);
		}
	}
}
