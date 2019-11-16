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
		public readonly static IReadOnlyDictionary<int, string> ChestTypeNamesByFrame;
		/// <summary>
		/// Gets chest frame by type names.
		/// </summary>
		public readonly static IReadOnlyDictionary<string, int> ChestFramesByTypeName;


		////

		static TileFrameHelpers() {
			var chestTypesByFrame = new Dictionary<int, string> {
				{ 0, "Chest" },
				{ 1, "Gold Chest" },
				{ 2, "Locked Gold Chest" },
				{ 4, "Shadow Chest" },
				{ 8, "Mushroom Chest" },	// ?
				{ 10, "Rich Mahogany Chest" },
				{ 11, "Ice Chest" },
				{ 12, "Living Wood Chest" },
				{ 13, "Skyware Chest" },
				{ 15, "Web Covered Chest" },
				{ 16, "Lihzahrd Chest" },
				{ 17, "Water Chest" },
				{ 50, "Granite Chest" },
				{ 51, "Marble Chest" },
				{ 23, "Jungle Chest" },
				{ 24, "Corruption Chest" },
				{ 25, "Crimson Chest" },
				{ 26, "Hallowed Chest" },
				{ 27, "Frozen Chest" },
			};

			TileFrameHelpers.ChestTypeNamesByFrame = new ReadOnlyDictionary<int, string>( chestTypesByFrame );
			TileFrameHelpers.ChestFramesByTypeName = new ReadOnlyDictionary<string, int>(
				chestTypesByFrame.ToDictionary( kv => kv.Value, kv => kv.Key )
			);
		}
	}
}
