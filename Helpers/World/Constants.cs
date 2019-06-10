using System;


namespace HamstarHelpers.Helpers.World {
	public enum VanillaBiomes {
		Forest, Space, Ocean, Cave, Hell,
		Desert, Cold, Mushroom, Jungle, Corruption, Crimson, Hallow,
		Granite, Marble, SpiderNest, Dungeon, Temple
	}
	public enum VanillaSectionalBiomes {
		Forest, Space, Ocean, Cave, Hell
	}
	public enum VanillaSurfaceBiomes {
		Forest, Desert, Cold, Mushroom, Jungle, Ocean, Space, Corruption, Crimson, Hallow
	}
	public enum VanillaUndergroundBiomes {
		Cave, Desert, Cold, Mushroom, Granite, Marble, SpiderNest, Dungeon, Jungle, Temple, Corruption, Crimson, Hallow, Hell
	}
	public enum VanillaHardModeSurfaceBiomes {
		Corruption, Crimson, Hallow
	}
	public enum VanillaHardModeUndergroundBiomes {
		Temple, Corruption, Crimson, Hallow
	}
	public enum VanillaHardModeConvertibleBiomes {
		Cave, Desert, Cold
	}


	public enum WorldSize {
		SubSmall,
		Small,
		Medium,
		Large,
		SuperLarge
	}
}
