using System;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// @private
	[Obsolete("use Helpers.Tile.TileShapeType")]
	public enum TileSlopeType {
		/// @private
		None,
		/// @private
		Any,
		/// @private
		Top,
		/// @private
		Bottom,
		/// @private
		Left,
		/// @private
		Right,
		/// @private
		HalfBrick,
		/// @private
		TopRightSlope,
		/// @private
		TopLeftSlope,
		/// @private
		BottomRightSlope,
		/// @private
		BottomLeftSlope
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	[Serializable]
	public partial class TilePattern {
		/// <summary>
		/// Preset for any tile, including empty space.
		/// </summary>
		public readonly static TilePattern Any = new TilePattern(
			new TilePatternBuilder { }
		);

		/// <summary>
		/// Preset for completely empty space.
		/// </summary>
		public readonly static TilePattern AbsoluteAir = new TilePattern(
			new TilePatternBuilder {
				IsActive = false,
				HasWire1 = false,
				HasWire2 = false,
				HasWire3 = false,
				HasWire4 = false,
				//HasSolidProperties = false,
				HasWall = false,
				HasWater = false,
				HasHoney = false,
				HasLava = false
			}
		);

		/// <summary>
		/// Preset for walls, no tiles.
		/// </summary>
		public readonly static TilePattern OpenWall = new TilePattern(
			new TilePatternBuilder {
				IsActive = false,
				HasWall = true
			}
		);

		/// <summary>
		/// Preset for any non-active tiles (at most only liquids, wire, or walls).
		/// </summary>
		public readonly static TilePattern NonActive = new TilePattern(
			new TilePatternBuilder {
				IsActive = false
			}
		);

		/// <summary>
		/// Preset for any non-solid tiles (includes actuated tiles and active-but-non-solid tiles).
		/// </summary>
		public readonly static TilePattern NonSolid = new TilePattern(
			new TilePatternBuilder {
				CustomCheck = (x, y) => {
					Tile tile = Main.tile[ x, y ];
					return !tile.active() || tile.inActive() || !Main.tileSolid[tile.type];
				}
			}
		);

		/// <summary>
		/// Preset for any non-"filled" space (no actives, no solids, no liquids, no actuated tiles; walls, furniture, etc.
		/// allowed).
		/// </summary>
		public readonly static TilePattern NonFilled = new TilePattern(
			new TilePatternBuilder {
				HasWater = false,
				HasHoney = false,
				HasLava = false,
				CustomCheck = ( x, y ) => {
					Tile tile = Main.tile[x, y];
					return !tile.active() /*|| tile.inActive()*/ || !Main.tileSolid[tile.type];
				}
			}
		);

		/// <summary>
		/// Preset for common solid tiles (non-actuated).
		/// </summary>
		public readonly static TilePattern CommonSolid = new TilePattern(
			new TilePatternBuilder {
				IsActive = true,
				HasSolidProperties = true,
				IsActuated = false,
				IsPlatform = false
			}
		);

		/// <summary>
		/// Preset for total blackness.
		/// </summary>
		public readonly static TilePattern PitchDark = new TilePattern(
			new TilePatternBuilder {
				MaximumBrightness = 0f
			}
		);
	}
}
