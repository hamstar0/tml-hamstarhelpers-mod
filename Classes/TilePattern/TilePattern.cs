using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary></summary>
	public enum TileSlopeType {
		/// <summary></summary>
		None,
		/// <summary></summary>
		Any,
		/// <summary></summary>
		Top,
		/// <summary></summary>
		Bottom,
		/// <summary></summary>
		Left,
		/// <summary></summary>
		Right,
		/// <summary></summary>
		HalfBrick,
		/// <summary></summary>
		TopRightSlope,
		/// <summary></summary>
		TopLeftSlope,
		/// <summary></summary>
		BottomRightSlope,
		/// <summary></summary>
		BottomLeftSlope
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
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
				HasWire1 = false,
				HasWire2 = false,
				HasWire3 = false,
				HasWire4 = false,
				IsSolid = false,
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
				IsSolid = false,
				HasWall = true
			}
		);

		/// <summary>
		/// Preset for any non-solid tiles.
		/// </summary>
		public readonly static TilePattern NonSolid = new TilePattern(
			new TilePatternBuilder {
				IsSolid = false,
				IsActuated = false
			}
		);

		/// <summary>
		/// Preset for any non-"filled" space (no solids, no liquids).
		/// </summary>
		public readonly static TilePattern NonFilled = new TilePattern(
			new TilePatternBuilder {
				IsSolid = false,
				HasWater = false,
				HasHoney = false,
				HasLava = false
			}
		);

		/// <summary>
		/// Preset for common solid tiles.
		/// </summary>
		public readonly static TilePattern CommonSolid = new TilePattern(
			new TilePatternBuilder {
				IsSolid = true,
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
