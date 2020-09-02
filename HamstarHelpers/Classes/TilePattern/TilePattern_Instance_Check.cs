using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary></summary>
	public enum TileCollideType {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		Active,
		/// <summary></summary>
		TileType,
		/// <summary></summary>
		WallType,
		/// <summary></summary>
		TileNotType,
		/// <summary></summary>
		WallNotType,
		/// <summary></summary>
		Solid,
		/// <summary></summary>
		Wall,
		/// <summary></summary>
		Platform,
		/// <summary></summary>
		Wire1,
		/// <summary></summary>
		Wire2,
		/// <summary></summary>
		Wire3,
		/// <summary></summary>
		Wire4,
		/// <summary></summary>
		Actuated,
		/// <summary></summary>
		Water,
		/// <summary></summary>
		Honey,
		/// <summary></summary>
		Lava,
		/// <summary></summary>
		SlopeAny,
		/// <summary></summary>
		SlopeHalfBrick,
		/// <summary></summary>
		SlopeTopRight,
		/// <summary></summary>
		SlopeTopLeft,
		/// <summary></summary>
		SlopeBottomRight,
		/// <summary></summary>
		SlopeBottomLeft,
		/// <summary></summary>
		SlopeTop,
		/// <summary></summary>
		SlopeBottom,
		/// <summary></summary>
		SlopeLeft,
		/// <summary></summary>
		SlopeRight,
		/// <summary></summary>
		BrightnessLow,
		/// <summary></summary>
		BrightnessHigh,
		/// <summary></summary>
		Custom,
		/// <summary></summary>
		WorldEdge,
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY ) {
			TileCollideType _;
			Point __;
			return this.Check( tileX, tileY, out _, out __ );
		}

		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY, out TileCollideType collideType ) {
			Point __;
			return this.Check( tileX, tileY, out collideType, out __ );
		}

		/// <summary>
		/// Tests a given tile against the current settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="collideType"></param>
		/// <param name="collisionAt"></param>
		/// <returns>`true` if all settings pass the test, and identify the tile as the current type.</returns>
		public bool Check( int tileX, int tileY, out TileCollideType collideType, out Point collisionAt ) {
			if( !this.AreaFromCenter.HasValue ) {
				collisionAt = new Point( tileX, tileY );
				return this.CheckPoint( tileX, tileY, out collideType );
			}

			return this.CheckArea(
				tileX + this.AreaFromCenter.Value.X,
				tileY + this.AreaFromCenter.Value.Y,
				this.AreaFromCenter.Value.Width,
				this.AreaFromCenter.Value.Height,
				out collideType,
				out collisionAt
			);
		}


		////////////////

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileArea"></param>
		/// <returns></returns>
		public bool CheckArea( Rectangle tileArea ) {
			return this.CheckArea( tileArea.X, tileArea.Y, tileArea.Width, tileArea.Height );
		}

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public bool CheckArea( int tileX, int tileY, int width, int height ) {
			TileCollideType _;
			Point __;
			return this.CheckArea( tileX, tileY, width, height, out _, out __ );
		}

		/// <summary>
		/// Checks all tiles in an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="collideType"></param>
		/// <param name="collisionAt"></param>
		/// <returns></returns>
		public bool CheckArea( int tileX, int tileY, int width, int height,
					out TileCollideType collideType,
					out Point collisionAt ) {
			if( tileX < 0 || tileY < 0 ) {
				collideType = TileCollideType.WorldEdge;
				collisionAt = new Point( tileX, tileY );
				return false;
			}
			if( (tileX + width) >= Main.maxTilesX ) {
				collideType = TileCollideType.WorldEdge;
				collisionAt = new Point(tileX + width, tileY );
				return false;
			}
			if( (tileY + height) >= Main.maxTilesY ) {
				collideType = TileCollideType.WorldEdge;
				collisionAt = new Point(tileX, tileY + height );
				return false;
			}

			int maxX = tileX + width;
			int maxY = tileY + height;

			for( int i = tileX; i < maxX; i++ ) {
				for( int j = tileY; j < maxY; j++ ) {
					if( !this.CheckPoint( i, j, out collideType ) ) {
						collisionAt = new Point( i, j );
						return false;
					}
				}
			}

			collisionAt = new Point(-1, -1);
			collideType = TileCollideType.None;
			return true;
		}
	}
}
