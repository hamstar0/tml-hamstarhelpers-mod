using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Tiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/** <summary>Assorted static "helper" functions pertaining to the current world.</summary> */
	public partial class WorldHelpers {
		public static Point WorldSizeSmall => new Point( 4200, 1200 );
		public static Point WorldSizeMedium => new Point( 6400, 1800 );	//6300?
		public static Point WorldSizeLarge => new Point( 8400, 2400 );



		////////////////

		public static string GetUniqueId( bool asFileName ) {
			if( asFileName ) {
				return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileHelpers.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
		}


		////////////////

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

		public static bool IsRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= Main.maxTilesY - 200 && (double)tilePos.Y > Main.rockLayer;
		}

		public static bool IsDirtLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.rockLayer && (double)tilePos.Y > Main.worldSurface;
		}

		public static bool IsAboveWorldSurface( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y < Main.worldSurface;
		}

		public static bool IsOverworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.worldSurface && (double)tilePos.Y > Main.worldSurface * 0.35;
		}

		public static bool IsSky( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= (Main.worldSurface * 0.35);	//0.34999999403953552?
		}

		public static bool IsWithinUnderworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y > (Main.maxTilesY - 200);
		}

		public static bool IsBeach( Vector2 worldPos ) {
			if( !WorldHelpers.IsOverworld( worldPos ) ) {
				return false;
			}
			return IsBeachRegion( worldPos );
		}

		public static bool IsBeachRegion( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.X < 380 || tilePos.X > (Main.maxTilesX - 380);
		}


		////////////////

		public static Vector2 DropToGround( Vector2 worldPos ) {
			int x = (int)worldPos.X / 16;
			int y = (int)worldPos.Y / 16;

			do {
				y++;
			} while( y <= ( Main.maxTilesY - 42 ) && !TileHelpers.IsSolid( Framing.GetTileSafely( x, y ) ) );
			y--;

			return new Vector2( worldPos.X, y * 16 );
		}
	}
}
