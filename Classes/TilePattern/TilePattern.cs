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



		////////////////

		/// <summary></summary>
		public bool? HasWire1 { get; private set; }
		/// <summary></summary>
		public bool? HasWire2 { get; private set; }
		/// <summary></summary>
		public bool? HasWire3 { get; private set; }
		/// <summary></summary>
		public bool? HasWire4 { get; private set; }

		/// <summary></summary>
		public bool? IsSolid { get; private set; }
		/// <summary></summary>
		public bool? IsPlatform { get; private set; }
		/// <summary></summary>
		public bool? IsActuated { get; private set; }
		/// <summary></summary>
		public bool? IsVanillaBombable { get; private set; }
		
		/// <summary></summary>
		public bool? HasWall { get; private set; }

		/// <summary></summary>
		public bool? HasWater { get; private set; }
		/// <summary></summary>
		public bool? HasHoney { get; private set; }
		/// <summary></summary>
		public bool? HasLava { get; private set; }

		/// <summary></summary>
		public TileSlopeType? Slope { get; private set; }

		/// <summary></summary>
		public float? MaximumBrightness { get; private set; }

		/// <summary></summary>
		public float? MinimumBrightness { get; private set; }

		/// <summary></summary>
		public bool? IsModded { get; private set; }
	}
}
