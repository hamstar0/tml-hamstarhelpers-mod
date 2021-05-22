using System;


namespace HamstarHelpers.Libraries.World {
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
		CaveDirt = 64 + 32,
		/// <summary></summary>
		CavePreRock = 128 + 32,
		/// <summary></summary>
		CaveRock = 256 + 32,
		/// <summary></summary>
		CaveLava = 512 + 32,
		/// <summary></summary>
		Hell = 1024
	}


	/// <summary></summary>
	public enum VanillaBiome {
		/// <summary></summary>
		Forest = 1,
		/// <summary></summary>
		Space = 2,
		/// <summary></summary>
		Ocean = 4,
		/// <summary></summary>
		Cave = 8,
		/// <summary></summary>
		RockCave = 16,
		/// <summary></summary>
		Hell = 32,
		/// <summary></summary>
		Desert = 64,
		/// <summary></summary>
		Snow = 128,
		/// @private
		[Obsolete("use Snow", true)]
		Cold = 128,
		/// <summary></summary>
		Mushroom = 256,
		/// <summary></summary>
		Jungle = 512,
		/// <summary></summary>
		Meteor = 1024,
		/// <summary></summary>
		Corruption = 2048,
		/// <summary></summary>
		Crimson = 4096,
		/// <summary></summary>
		Hallow = 8192,
		/// <summary></summary>
		Granite = 16384,
		/// <summary></summary>
		Marble = 32768,
		/// <summary></summary>
		SpiderNest = 65536,
		/// <summary></summary>
		Dungeon = 131072,
		/// <summary></summary>
		Temple = 262144
	}
	/// <summary></summary>
	public enum VanillaSurfaceBiome {
		/// <summary></summary>
		Forest = 1,
		/// <summary></summary>
		Desert = 2,
		/// <summary></summary>
		Cold = 4,
		/// <summary></summary>
		Mushroom = 8,
		/// <summary></summary>
		Jungle = 16,
		/// <summary></summary>
		Ocean = 32,
		/// <summary></summary>
		Space = 64,
		/// <summary></summary>
		Meteor = 128,
		/// <summary></summary>
		Corruption = 256,
		/// <summary></summary>
		Crimson = 512,
		/// <summary></summary>
		Hallow = 1024
	}
	/// <summary></summary>
	public enum VanillaUndergroundBiome {
		/// <summary></summary>
		Cave = 1,
		/// <summary></summary>
		Desert = 2,
		/// <summary></summary>
		Cold = 4,
		/// <summary></summary>
		Mushroom = 8,
		/// <summary></summary>
		Granite = 16,
		/// <summary></summary>
		Marble = 32,
		/// <summary></summary>
		SpiderNest = 64,
		/// <summary></summary>
		Dungeon = 128,
		/// <summary></summary>
		Jungle = 256,
		/// <summary></summary>
		Temple = 512,
		/// <summary></summary>
		Corruption = 1024,
		/// <summary></summary>
		Crimson = 2048,
		/// <summary></summary>
		Hallow = 4096,
		/// <summary></summary>
		Hell = 8192
	}
	/// <summary></summary>
	public enum VanillaHardModeSurfaceBiome {
		/// <summary></summary>
		Corruption = 1,
		/// <summary></summary>
		Crimson = 2,
		/// <summary></summary>
		Hallow = 4
	}
	/// <summary></summary>
	public enum VanillaHardModeUndergroundBiome {
		/// <summary></summary>
		Temple = 1,
		/// <summary></summary>
		Corruption = 2,
		/// <summary></summary>
		Crimson = 4,
		/// <summary></summary>
		Hallow = 8
	}
	/// <summary></summary>
	public enum VanillaHardModeConvertibleBiome {
		/// <summary></summary>
		Forest = 1,
		/// <summary></summary>
		Cave = 2,
		/// <summary></summary>
		Desert = 4,
		/// <summary></summary>
		Cold = 8
	}


	////////////////

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
