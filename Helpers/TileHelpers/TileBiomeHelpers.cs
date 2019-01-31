using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileBiomeHelpers {
		public static ISet<int> VanillaHolyTiles = new ReadOnlySet<int>( new HashSet<int> { 109, 110, 113, 117, 116, 164, 403, 402 } );
		public static ISet<int> VanillaCorruptionTiles = new ReadOnlySet<int>( new HashSet<int> { 23, 24, 25, 32, 112, 163, 400, 398 } ); //-5 * screenTileCounts[27];
		public static ISet<int> VanillaCrimsonTiles = new ReadOnlySet<int>( new HashSet<int> { 199, 203, 200, 401, 399, 234, 352 } ); //-5 * screenTileCounts[27];
		public static ISet<int> VanillaSnowTiles = new ReadOnlySet<int>( new HashSet<int> { 147, 148, 161, 162, 164, 163, 200 } );
		public static ISet<int> VanillaJungleTiles = new ReadOnlySet<int>( new HashSet<int> { 60, 61, 62, 74, 226 } );
		public static ISet<int> VanillaShroomTiles = new ReadOnlySet<int>( new HashSet<int> { 70, 71, 72 } );
		public static ISet<int> VanillaMeteorTiles = new ReadOnlySet<int>( new HashSet<int> { 37 } );
		public static ISet<int> VanillaDungeonTiles = new ReadOnlySet<int>( new HashSet<int> { 41, 43, 44 } );
		public static ISet<int> VanillaDesertTiles = new ReadOnlySet<int>( new HashSet<int> { 53, 112, 116, 234, 397, 398, 402, 399, 396, 400, 403, 401 } );
	}
}
