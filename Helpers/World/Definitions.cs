using System;


namespace HamstarHelpers.Helpers.World {
	/// <summary></summary>
	public enum WorldRegionFlags {
		/// <summary></summary>
		Overworld = 1,
		/// <summary></summary>
		Sky = 2,
		/// <summary></summary>
		Ocean = 4,
		/// <summary></summary>
		OceanEast = 8 + 2,
		/// <summary></summary>
		OceanWest = 16 + 2,
		/// <summary></summary>
		Cave = 32,
		/// <summary></summary>
		CaveDirt = 64 + 16,
		/// <summary></summary>
		CavePreRock = 128 + 16,
		/// <summary></summary>
		CaveRock = 256 + 16,
		/// <summary></summary>
		CaveLava = 512 + 16,
		/// <summary></summary>
		Hell = 1024
	}


	/// <summary></summary>
	public enum VanillaBiome {
		/// <summary></summary>
		Forest,
		/// <summary></summary>
		Space,
		/// <summary></summary>
		Ocean,
		/// <summary></summary>
		Cave,
		/// <summary></summary>
		RockCave,
		/// <summary></summary>
		Hell,
		/// <summary></summary>
		Desert,
		/// <summary></summary>
		Snow=7,
		[Obsolete("use Snow", true)]
		Cold=7,
		/// <summary></summary>
		Mushroom,
		/// <summary></summary>
		Jungle,
		/// <summary></summary>
		Meteor,
		/// <summary></summary>
		Corruption,
		/// <summary></summary>
		Crimson,
		/// <summary></summary>
		Hallow,
		/// <summary></summary>
		Granite,
		/// <summary></summary>
		Marble,
		/// <summary></summary>
		SpiderNest,
		/// <summary></summary>
		Dungeon,
		/// <summary></summary>
		Temple
	}
	/// <summary></summary>
	public enum VanillaSurfaceBiome {
		/// <summary></summary>
		Forest,
		/// <summary></summary>
		Desert,
		/// <summary></summary>
		Cold,
		/// <summary></summary>
		Mushroom,
		/// <summary></summary>
		Jungle,
		/// <summary></summary>
		Ocean,
		/// <summary></summary>
		Space,
		/// <summary></summary>
		Meteor,
		/// <summary></summary>
		Corruption,
		/// <summary></summary>
		Crimson,
		/// <summary></summary>
		Hallow
	}
	/// <summary></summary>
	public enum VanillaUndergroundBiome {
		/// <summary></summary>
		Cave,
		/// <summary></summary>
		Desert,
		/// <summary></summary>
		Cold,
		/// <summary></summary>
		Mushroom,
		/// <summary></summary>
		Granite,
		/// <summary></summary>
		Marble,
		/// <summary></summary>
		SpiderNest,
		/// <summary></summary>
		Dungeon,
		/// <summary></summary>
		Jungle,
		/// <summary></summary>
		Temple,
		/// <summary></summary>
		Corruption,
		/// <summary></summary>
		Crimson,
		/// <summary></summary>
		Hallow,
		/// <summary></summary>
		Hell
	}
	/// <summary></summary>
	public enum VanillaHardModeSurfaceBiome {
		/// <summary></summary>
		Corruption,
		/// <summary></summary>
		Crimson,
		/// <summary></summary>
		Hallow
	}
	/// <summary></summary>
	public enum VanillaHardModeUndergroundBiome {
		/// <summary></summary>
		Temple,
		/// <summary></summary>
		Corruption,
		/// <summary></summary>
		Crimson,
		/// <summary></summary>
		Hallow
	}
	/// <summary></summary>
	public enum VanillaHardModeConvertibleBiome {
		/// <summary></summary>
		Forest,
		/// <summary></summary>
		Cave,
		/// <summary></summary>
		Desert,
		/// <summary></summary>
		Cold
	}


	/// <summary></summary>
	public enum WorldSize {
		/// <summary></summary>
		SubSmall,
		/// <summary></summary>
		Small,
		/// <summary></summary>
		Medium,
		/// <summary></summary>
		Large,
		/// <summary></summary>
		SuperLarge
	}
}
