using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileBiomeHelpers {
		public readonly static ISet<int> VanillaHolyTiles = new ReadOnlySet<int>( new HashSet<int> { 109, 110, 113, 117, 116, 164, 403, 402 } );
		public readonly static ISet<int> VanillaCorruptionTiles = new ReadOnlySet<int>( new HashSet<int> { 23, 24, 25, 32, 112, 163, 400, 398 } ); //-5 * screenTileCounts[27];
		public readonly static ISet<int> VanillaCrimsonTiles = new ReadOnlySet<int>( new HashSet<int> { 199, 203, 200, 401, 399, 234, 352 } ); //-5 * screenTileCounts[27];
		public readonly static ISet<int> VanillaSnowTiles = new ReadOnlySet<int>( new HashSet<int> { 147, 148, 161, 162, 164, 163, 200 } );
		public readonly static ISet<int> VanillaJungleTiles = new ReadOnlySet<int>( new HashSet<int> { 60, 61, 62, 74, 226 } );
		public readonly static ISet<int> VanillaShroomTiles = new ReadOnlySet<int>( new HashSet<int> { 70, 71, 72 } );
		public readonly static ISet<int> VanillaMeteorTiles = new ReadOnlySet<int>( new HashSet<int> { 37 } );
		public readonly static ISet<int> VanillaDungeonTiles = new ReadOnlySet<int>( new HashSet<int> { 41, 43, 44 } );
		public readonly static ISet<int> VanillaDesertTiles = new ReadOnlySet<int>( new HashSet<int> { 53, 112, 116, 234, 397, 398, 402, 399, 396, 400, 403, 401 } );
		public readonly static ISet<int> VanillaLihzahrdTiles = new ReadOnlySet<int>( new HashSet<int> { TileID.LihzahrdBrick } );

		public readonly static int VanillaHolyMinTiles = 100;
		public readonly static int VanillaCorruptionMinTiles = 200;
		public readonly static int VanillaCrimsonMinTiles = 200;
		public readonly static int VanillaMeteorMinTiles = 50;
		public readonly static int VanillaJungleMinTiles = 80;
		public readonly static int VanillaSnowMinTiles = 300;
		public readonly static int VanillaDesertMinTiles = 1000;
		public readonly static int VanillaShroomMinTiles = 100;
		public readonly static int VanillaDungeonMinTiles = 250;
		public readonly static int VanillaLihzahrdMinTiles = 250;



		////////////////

		public static IDictionary<string, float> GetVanillaBiomesNear( int tileX, int tileY ) {
			IDictionary<int, int> tiles = TileFinderHelpers.GetPlayerRangeTilesAt( tileX, tileY );
			var biomes = new Dictionary<string, float>();

			int holyTiles = 0;
			foreach( int holyTileType in TileBiomeHelpers.VanillaHolyTiles ) {
				if( tiles.ContainsKey( holyTileType ) ) {
					holyTiles += tiles[holyTileType];
				}
			}

			int corrTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaCorruptionTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					corrTiles += tiles[tileType];
				}
			}

			int crimTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaCrimsonTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					crimTiles += tiles[tileType];
				}
			}

			int snowTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaSnowTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					snowTiles += tiles[tileType];
				}
			}

			int jungTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaJungleTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					jungTiles += tiles[tileType];
				}
			}

			int mushTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaShroomTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					mushTiles += tiles[tileType];
				}
			}

			int meteTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaMeteorTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					meteTiles += tiles[tileType];
				}
			}

			int deseTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaDesertTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					deseTiles += tiles[tileType];
				}
			}

			int dungTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaDungeonTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					dungTiles += tiles[tileType];
				}
			}
			
			int lihzTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaLihzahrdTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					lihzTiles += tiles[tileType];
				}
			}

			biomes["Holy"] = (float)holyTiles / (float)TileBiomeHelpers.VanillaHolyMinTiles;
			biomes["Corruption"] = (float)corrTiles / (float)TileBiomeHelpers.VanillaCorruptionMinTiles;
			biomes["Crimson"] = (float)corrTiles / (float)TileBiomeHelpers.VanillaCrimsonMinTiles;
			biomes["Meteor"] = (float)meteTiles / (float)TileBiomeHelpers.VanillaMeteorMinTiles;
			biomes["Jungle"] = (float)jungTiles / (float)TileBiomeHelpers.VanillaJungleMinTiles;
			biomes["Snow"] = (float)snowTiles / (float)TileBiomeHelpers.VanillaSnowMinTiles;
			biomes["Desert"] = (float)deseTiles / (float)TileBiomeHelpers.VanillaDesertMinTiles;
			biomes["Mushroom"] = (float)mushTiles / (float)TileBiomeHelpers.VanillaShroomMinTiles;
			biomes["Dungeon"] = (float)dungTiles / (float)TileBiomeHelpers.VanillaDungeonMinTiles;
			biomes["Lihzahrd"] = (float)lihzTiles / (float)TileBiomeHelpers.VanillaLihzahrdMinTiles;

			return biomes;
		}
	}
}
