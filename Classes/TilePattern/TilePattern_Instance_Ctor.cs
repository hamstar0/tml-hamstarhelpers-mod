using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	public class TilePatternBuilder {
		/// <summary></summary>
		public bool? HasWire1 = null;
		/// <summary></summary>
		public bool? HasWire2 = null;
		/// <summary></summary>
		public bool? HasWire3 = null;
		/// <summary></summary>
		public bool? HasWire4 = null;

		/// <summary></summary>
		public bool? IsSolid = null;
		/// <summary></summary>
		public bool? IsPlatform = null;
		/// <summary></summary>
		public bool? IsActuated = null;
		/// <summary></summary>
		public bool? IsVanillaBombable = null;

		/// <summary></summary>
		public bool? HasWall = null;

		/// <summary></summary>
		public bool? HasWater = null;
		/// <summary></summary>
		public bool? HasHoney = null;
		/// <summary></summary>
		public bool? HasLava = null;

		/// <summary></summary>
		public TileSlopeType? Slope = null;

		/// <summary></summary>
		public float? MinimumBrightness = null;
		/// <summary></summary>
		public float? MaximumBrightness = null;

		/// <summary></summary>
		public bool? IsModded = null;
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// </summary>
		public TilePattern( TilePatternBuilder builder ) {
			this.HasWire1 = builder.HasWire1;
			this.HasWire2 = builder.HasWire2;
			this.HasWire3 = builder.HasWire3;
			this.HasWire4 = builder.HasWire4;

			this.IsSolid = builder.IsSolid;
			this.IsPlatform = builder.IsPlatform;
			this.IsActuated = builder.IsActuated;
			this.IsVanillaBombable = builder.IsVanillaBombable;

			this.HasWall = builder.HasWall;

			this.HasWater = builder.HasWater;
			this.HasHoney = builder.HasHoney;
			this.HasLava = builder.HasLava;

			this.Slope = builder.Slope;

			this.MinimumBrightness = builder.MinimumBrightness;
			this.MaximumBrightness = builder.MaximumBrightness;

			this.IsModded = builder.IsModded;
		}
	}
}
