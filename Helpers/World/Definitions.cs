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
		Forest = 0,
		/// <summary></summary>
		Space = 1,
		/// <summary></summary>
		Ocean = 2,
		/// <summary></summary>
		Cave = 4,
		/// <summary></summary>
		RockCave = 8,
		/// <summary></summary>
		Hell = 16,
		/// <summary></summary>
		Desert = 32,
		/// <summary></summary>
		Snow = 64,
		[Obsolete("use Snow", true)]
		Cold = 64,
		/// <summary></summary>
		Mushroom = 128,
		/// <summary></summary>
		Jungle = 256,
		/// <summary></summary>
		Meteor = 512,
		/// <summary></summary>
		Corruption = 1024,
		/// <summary></summary>
		Crimson = 2048,
		/// <summary></summary>
		Hallow = 4096,
		/// <summary></summary>
		Granite = 8192,
		/// <summary></summary>
		Marble = 16384,
		/// <summary></summary>
		SpiderNest = 32768,
		/// <summary></summary>
		Dungeon = 65536,
		/// <summary></summary>
		Temple = 131072
	}
	/// <summary></summary>
	public enum VanillaSurfaceBiome {
		/// <summary></summary>
		Forest = 0,
		/// <summary></summary>
		Desert = 1,
		/// <summary></summary>
		Cold = 2,
		/// <summary></summary>
		Mushroom = 4,
		/// <summary></summary>
		Jungle = 8,
		/// <summary></summary>
		Ocean = 16,
		/// <summary></summary>
		Space = 32,
		/// <summary></summary>
		Meteor = 64,
		/// <summary></summary>
		Corruption = 128,
		/// <summary></summary>
		Crimson = 256,
		/// <summary></summary>
		Hallow = 512
	}
	/// <summary></summary>
	public enum VanillaUndergroundBiome {
		/// <summary></summary>
		Cave = 0,
		/// <summary></summary>
		Desert = 1,
		/// <summary></summary>
		Cold = 2,
		/// <summary></summary>
		Mushroom = 4,
		/// <summary></summary>
		Granite = 8,
		/// <summary></summary>
		Marble = 16,
		/// <summary></summary>
		SpiderNest = 32,
		/// <summary></summary>
		Dungeon = 64,
		/// <summary></summary>
		Jungle = 128,
		/// <summary></summary>
		Temple = 256,
		/// <summary></summary>
		Corruption = 512,
		/// <summary></summary>
		Crimson = 1024,
		/// <summary></summary>
		Hallow = 2048,
		/// <summary></summary>
		Hell = 4096
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
		Forest = 0,
		/// <summary></summary>
		Cave = 1,
		/// <summary></summary>
		Desert = 2,
		/// <summary></summary>
		Cold = 4
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
