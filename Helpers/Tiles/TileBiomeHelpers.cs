using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles as relevant to biomes.
	/// </summary>
	public class TileBiomeHelpers {
		/// <summary></summary>
		public readonly static ISet<int> VanillaHolyTiles = new ReadOnlySet<int>( new HashSet<int> { 109, 110, 113, 117, 116, 164, 403, 402 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaCorruptionTiles = new ReadOnlySet<int>( new HashSet<int> { 23, 24, 25, 32, 112, 163, 400, 398 } ); //-5 * screenTileCounts[27];
		/// <summary></summary>
		public readonly static ISet<int> VanillaCrimsonTiles = new ReadOnlySet<int>( new HashSet<int> { 199, 203, 200, 401, 399, 234, 352 } ); //-5 * screenTileCounts[27];
		/// <summary></summary>
		public readonly static ISet<int> VanillaSnowTiles = new ReadOnlySet<int>( new HashSet<int> { 147, 148, 161, 162, 164, 163, 200 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaJungleTiles = new ReadOnlySet<int>( new HashSet<int> { 60, 61, 62, 74, 226 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaShroomTiles = new ReadOnlySet<int>( new HashSet<int> { 70, 71, 72 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaMeteorTiles = new ReadOnlySet<int>( new HashSet<int> { 37 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaDungeonTiles = new ReadOnlySet<int>( new HashSet<int> { 41, 43, 44 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaDesertTiles = new ReadOnlySet<int>( new HashSet<int> { 53, 112, 116, 234, 397, 398, 402, 399, 396, 400, 403, 401 } );
		/// <summary></summary>
		public readonly static ISet<int> VanillaLihzahrdTiles = new ReadOnlySet<int>( new HashSet<int> { TileID.LihzahrdBrick } );

		/// <summary></summary>
		public readonly static int VanillaHolyMinTiles = 100;
		/// <summary></summary>
		public readonly static int VanillaCorruptionMinTiles = 200;
		/// <summary></summary>
		public readonly static int VanillaCrimsonMinTiles = 200;
		/// <summary></summary>
		public readonly static int VanillaMeteorMinTiles = 50;
		/// <summary></summary>
		public readonly static int VanillaJungleMinTiles = 80;
		/// <summary></summary>
		public readonly static int VanillaSnowMinTiles = 300;
		/// <summary></summary>
		public readonly static int VanillaDesertMinTiles = 1000;
		/// <summary></summary>
		public readonly static int VanillaShroomMinTiles = 100;
		/// <summary></summary>
		public readonly static int VanillaDungeonMinTiles = 250;
		/// <summary></summary>
		public readonly static int VanillaLihzahrdMinTiles = 250;



		////////////////

		/// <summary>
		/// Gets percent values indicating how much of each vanilla biome type is near a given tile position. See
		/// `GetPlayerRangeTilesAt(...)` for the specification of the tile checking range. Percent values indicate
		/// how much of the *minimum* percent of tiles exist nearby to count as being within the given biome.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="totalTiles">Returns a count of non-air tiles in total.</param>
		/// <param name="unidenfiedTiles">Returns all non-air tiles not identified with a specific biome.</param>
		/// <returns></returns>
		[Obsolete( "Needs TileFinderHelpers.GetPlayerRangeTilesAt fixed" )]
		public static IDictionary<string, float> GetVanillaBiomePercentsNear( int tileX, int tileY,
					out int totalTiles,
					out int unidenfiedTiles ) {
			IDictionary<int, int> tiles = TileFinderHelpers.GetPlayerRangeTilesAt( tileX, tileY );

			return TileBiomeHelpers.GetVanillaBiomePercentsOf( ref tiles, out totalTiles, out unidenfiedTiles )
				.ToDictionary( kv=>Enum.GetName(typeof(VanillaBiome), kv), kv=>kv.Value );
		}


		/// <summary>
		/// Gets percent values indicating how much of each vanilla biome type of a set of tiles. See
		/// `GetPlayerRangeTilesAt(...)` for the specification of the tile checking range. Percent values indicate
		/// how much of the *minimum* percent of tiles exist nearby to count as being within the given biome.
		/// </summary>
		/// <param name="tiles">Counts of tiles by type.</param>
		/// <param name="totalTiles">Returns a count of non-air tiles in total.</param>
		/// <param name="unidenfiedTiles">Returns all non-air tiles not identified with a specific biome.</param>
		/// <returns></returns>
		public static IDictionary<VanillaBiome, float> GetVanillaBiomePercentsOf( ref IDictionary<int, int> tiles,
					out int totalTiles,
					out int unidenfiedTiles ) {
			int holyTiles = 0;
			int corrTiles = 0;
			int crimTiles = 0;
			int snowTiles = 0;
			int jungTiles = 0;
			int mushTiles = 0;
			int meteTiles = 0;
			int deseTiles = 0;
			int dungTiles = 0;
			int lihzTiles = 0;

			TileBiomeHelpers.GetVanillaBiomeAmountsOf( ref tiles,
				out holyTiles,
				out corrTiles,
				out crimTiles,
				out snowTiles,
				out jungTiles,
				out mushTiles,
				out meteTiles,
				out deseTiles,
				out dungTiles,
				out lihzTiles,
				out unidenfiedTiles,
				out totalTiles
			);

			var biomes = new Dictionary<VanillaBiome, float>();
			biomes[VanillaBiome.Hallow] = (float)holyTiles / (float)TileBiomeHelpers.VanillaHolyMinTiles;
			biomes[VanillaBiome.Corruption] = (float)corrTiles / (float)TileBiomeHelpers.VanillaCorruptionMinTiles;
			biomes[VanillaBiome.Crimson] = (float)crimTiles / (float)TileBiomeHelpers.VanillaCrimsonMinTiles;
			biomes[VanillaBiome.Meteor] = (float)meteTiles / (float)TileBiomeHelpers.VanillaMeteorMinTiles;
			biomes[VanillaBiome.Jungle] = (float)jungTiles / (float)TileBiomeHelpers.VanillaJungleMinTiles;
			biomes[VanillaBiome.Cold] = (float)snowTiles / (float)TileBiomeHelpers.VanillaSnowMinTiles;
			biomes[VanillaBiome.Desert] = (float)deseTiles / (float)TileBiomeHelpers.VanillaDesertMinTiles;
			biomes[VanillaBiome.Mushroom] = (float)mushTiles / (float)TileBiomeHelpers.VanillaShroomMinTiles;
			biomes[VanillaBiome.Dungeon] = (float)dungTiles / (float)TileBiomeHelpers.VanillaDungeonMinTiles;
			biomes[VanillaBiome.Temple] = (float)lihzTiles / (float)TileBiomeHelpers.VanillaLihzahrdMinTiles;

			return biomes;
		}


		////

		private static void GetVanillaBiomeAmountsOf( ref IDictionary<int, int> tiles,
					out int holyTiles,
					out int corrTiles,
					out int crimTiles,
					out int snowTiles,
					out int jungTiles,
					out int mushTiles,
					out int meteTiles,
					out int deseTiles,
					out int dungTiles,
					out int lihzTiles,
					out int totalTiles,
					out int unidenfiedTiles ) {
			holyTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaHolyTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					holyTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			corrTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaCorruptionTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					corrTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			crimTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaCrimsonTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					crimTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			snowTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaSnowTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					snowTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			jungTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaJungleTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					jungTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			mushTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaShroomTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					mushTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			meteTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaMeteorTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					meteTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			deseTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaDesertTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					deseTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			dungTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaDungeonTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					dungTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}
			
			lihzTiles = 0;
			foreach( int tileType in TileBiomeHelpers.VanillaLihzahrdTiles ) {
				if( tiles.ContainsKey( tileType ) ) {
					lihzTiles += tiles[tileType];
					tiles.Remove( tileType );
				}
			}

			unidenfiedTiles = tiles.Values.Sum();	// Unclaimed remainder
			totalTiles = unidenfiedTiles + holyTiles + corrTiles + crimTiles + meteTiles + jungTiles + snowTiles
				+ deseTiles + mushTiles + dungTiles + lihzTiles;
		}
	}
}
