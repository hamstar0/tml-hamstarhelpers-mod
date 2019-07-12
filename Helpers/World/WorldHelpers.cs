using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Tiles;
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

			if( size <= ( 4200 * 1200 ) / 2 ) {
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
		/// Indicates if the given position is within the underground rock layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= Main.maxTilesY - 200 && (double)tilePos.Y > Main.rockLayer;
		}

		/// <summary>
		/// Indicates if the given position is within the underground dirt layer (beneath elevation 0, above the rock layer).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsDirtLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.rockLayer && (double)tilePos.Y > Main.worldSurface;
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
		/// Indicates if the given position is above the world's surface, but not in the sky.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsOverworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.worldSurface && (double)tilePos.Y > Main.worldSurface * 0.35;
		}

		/// <summary>
		/// Indicates if the given position is in the sky/space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsSky( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= (Main.worldSurface * 0.35);	//0.34999999403953552?
		}

		/// <summary>
		/// Indicates if the given position is within the underworld.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsWithinUnderworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y > (Main.maxTilesY - 200);
		}

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
		/// <param name="groundPos"></param>
		/// <returns>`true` if a ground point was found within world boundaries.</returns>
		public static bool DropToGround( Vector2 worldPos, bool invertGravity, out Vector2 groundPos ) { f
			bool hitGround = true;
			int x = (int)worldPos.X / 16;
			int y = (int)worldPos.Y / 16;

			if( invertGravity ) {
				do {
					y--;
					if( y >= 42 ) {
						hitGround = false;
						break;
					}
				} while( !TileHelpers.IsSolid( Framing.GetTileSafely(x, y) ) );
			} else {
				do {
					y++;
					if( y >= Main.maxTilesY - 42 ) {
						hitGround = false;
						break;
					}
				} while( !TileHelpers.IsSolid( Framing.GetTileSafely(x, y) ) );
			}

			groundPos = new Vector2( worldPos.X, y * 16 );
			return hitGround;
		}
	}
}
