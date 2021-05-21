using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world.
	/// </summary>
	public partial class WorldHelpers {
		/// <summary></summary>
		public static Point WorldSizeSmall => new Point( 4200, 1200 );
		/// <summary></summary>
		public static Point WorldSizeMedium => new Point( 6400, 1800 );	//6300?
		/// <summary></summary>
		public static Point WorldSizeLarge => new Point( 8400, 2400 );


		////////////////

		/// <summary></summary>
		public static int SurfaceLayerTopTileY => WorldHelpers.SkyLayerBottomTileY;

		/// <summary></summary>
		public static int SurfaceLayerBottomTileY => (int)Main.worldSurface;


		/// <summary></summary>
		public static int DirtLayerTopTileY => (int)Main.worldSurface;

		/// <summary></summary>
		public static int DirtLayerBottomTileY => (int)Main.rockLayer;


		/// <summary></summary>
		public static int RockLayerTopTileY => (int)Main.rockLayer;

		/// <summary></summary>
		public static int RockLayerBottomTileY => WorldHelpers.UnderworldLayerTopTileY;


		/// <summary></summary>
		public static int SkyLayerTopTileY => 0;

		/// <summary></summary>
		public static int SkyLayerBottomTileY => (int)(Main.worldSurface * 0.35d);


		/// <summary></summary>
		public static int UnderworldLayerTopTileY => Main.maxTilesY - 200;

		/// <summary></summary>
		public static int UnderworldLayerBottomTileY => Main.maxTilesY;


		/// <summary></summary>
		public static int BeachWestTileX => 380;

		/// <summary></summary>
		public static int BeachEastTileX => Main.maxTilesX - 380;



		////////////////

		/// <summary>
		/// Gets a unique identifier for the current loaded world.
		/// </summary>
		/// <param name="asFileName"></param>
		/// <returns></returns>
		public static string GetUniqueIdForCurrentWorld( bool asFileName ) {
			if( asFileName ) {
				return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileHelpers.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
		}


		////////////////

		/// <summary>
		/// Gets the size (range) of the current world.
		/// </summary>
		/// <returns></returns>
		public static WorldSize GetSize() {
			int size = Main.maxTilesX * Main.maxTilesY;

			if( size <= (4200 * 1200) / 2 ) {
				return WorldSize.SubSmall;
			} else if( size <= 4200 * 1200 + 1000 ) {
				return WorldSize.Small;
			} else if( size <= 6400 * 1800 + 1000 ) {   //6300?
				return WorldSize.Medium;
			} else if( size <= 8400 * 2400 + 1000 ) {
				return WorldSize.Large;
			} else {
				return WorldSize.SuperLarge;
			}
		}


		////////////////

		/// <summary>
		/// Gets the identifiable region of a given point in the world.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static WorldRegionFlags GetRegion( Vector2 worldPos ) {
			WorldRegionFlags where = 0;

			if( WorldHelpers.IsSky(worldPos) ) {
				where |= WorldRegionFlags.Sky;
			} else if( WorldHelpers.IsWithinUnderworld(worldPos) ) {
				where |= WorldRegionFlags.Hell;
			} else if( WorldHelpers.IsAboveWorldSurface(worldPos) ) {
				where |= WorldRegionFlags.Overworld;

				if( WorldHelpers.BeachEastTileX < (worldPos.X/16) ) {
					where |= WorldRegionFlags.OceanEast;
				} else if( WorldHelpers.BeachWestTileX > (worldPos.X/16) ) {
					where |= WorldRegionFlags.OceanWest;
				}
			} else {
				if( WorldHelpers.IsDirtLayer( worldPos ) ) {
					where |= WorldRegionFlags.CaveDirt;
				} else {
					if( WorldHelpers.IsPreRockLayer( worldPos ) ) {
						where |= WorldRegionFlags.CavePreRock;
					}
					if( WorldHelpers.IsRockLayer( worldPos ) ) {
						where |= WorldRegionFlags.CaveRock;

						if( WorldHelpers.IsLavaLayer( worldPos ) ) {
							where |= WorldRegionFlags.CaveLava;
						}
					}
				}
			}

			return where;
		}


		////////////////

		/// <summary>
		/// Indicates if the given position is in the sky/space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsSky( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= ( Main.worldSurface * 0.35 );   //0.34999999403953552?
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface, but not in the sky.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsOverworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.worldSurface && (double)tilePos.Y > Main.worldSurface * 0.35;
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsAboveWorldSurface( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y < Main.worldSurface;
		}

		/// <summary>
		/// Indicates if the given position is within the underground dirt layer (beneath elevation 0, above the rock layer).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsDirtLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y > Main.worldSurface && (double)tilePos.Y <= Main.rockLayer;
		}

		/// <summary>
		/// Indicates if the given position is within the underground pre-rock layer (background appears like dirt,
		/// but the game recognizes the 'rockLayer' depth).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsPreRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;    //between 33 and 37
			return (double)tilePos.Y > Main.rockLayer && (double)tilePos.Y <= Main.rockLayer + 34;
		}

		/// <summary>
		/// Indicates if the given position is within the underground rock layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y > Main.rockLayer && tilePos.Y <= Main.maxTilesY - 200;
		}

		/// <summary>
		/// Indicates if the given position is within the underground lava layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsLavaLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= Main.maxTilesY - 200 && (double)tilePos.Y > (Main.rockLayer + 601);
		}

		/// <summary>
		/// Indicates if the given position is within the underworld (hell).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsWithinUnderworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y > (Main.maxTilesY - 200);
		}

		////

		/// <summary>
		/// Indicates if the given position is at a beach/ocean area.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsBeach( Vector2 worldPos ) {
			if( !WorldHelpers.IsOverworld( worldPos ) ) {
				return false;
			}
			return IsBeachRegion( worldPos );
		}

		/// <summary>
		/// Indicates if the given position is within the region of a beach (regardless of elevation).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsBeachRegion( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.X < 380 || tilePos.X > (Main.maxTilesX - 380);
		}


		////////////////

		/// <summary>
		/// Drops from a given point to the ground.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="invertGravity"></param>
		/// <param name="ground">Tile pattern checker to define what "ground" is.</param>
		/// <param name="groundPos"></param>
		/// <returns>`true` if a ground point was found within world boundaries.</returns>
		public static bool DropToGround( Vector2 worldPos,
				bool invertGravity,
				TilePattern ground,
				out Vector2 groundPos ) {
			int furthestTileY = invertGravity ? 42 : Main.maxTilesY - 42;
			return WorldHelpers.DropToGround( worldPos, invertGravity, ground, furthestTileY, out groundPos );
		}


		/// <summary>
		/// Drops from a given point to the ground.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="invertGravity"></param>
		/// <param name="ground">Tile pattern checker to define what "ground" is.</param>
		/// <param name="furthestTileY">Limit to check tiles down (or up) to before giving up.</param>
		/// <param name="groundPos"></param>
		/// <returns>`true` if a ground point was found within world boundaries.</returns>
		public static bool DropToGround( Vector2 worldPos,
				bool invertGravity,
				TilePattern ground,
				int furthestTileY,
				out Vector2 groundPos ) {
			bool hitGround = true;
			int tileX = (int)worldPos.X >> 4;
			int tileY = (int)worldPos.Y >> 4;

			if( invertGravity ) {
				do {
					tileY--;
					if( tileY >= furthestTileY ) {
						hitGround = false;
						break;
					}
				} while( !ground.Check( tileX, tileY ) );
			} else {
				do {
					tileY++;
					if( tileY >= furthestTileY ) {
						hitGround = false;
						break;
					}
				} while( !ground.Check( tileX, tileY ) );
			}

			groundPos = new Vector2( worldPos.X, tileY * 16 );
			return hitGround;
		}
	}
}
