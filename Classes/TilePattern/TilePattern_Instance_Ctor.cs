using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	public class TilePatternBuilder {
		/// <summary></summary>
		public bool? HasWire1;
		/// <summary></summary>
		public bool? HasWire2;
		/// <summary></summary>
		public bool? HasWire3;
		/// <summary></summary>
		public bool? HasWire4;

		/// <summary></summary>
		public bool? IsSolid;
		/// <summary></summary>
		public bool? IsPlatform;
		/// <summary></summary>
		public bool? IsActuated;
		/// <summary></summary>
		public bool? IsVanillaBombable;

		/// <summary></summary>
		public bool? HasWall;

		/// <summary></summary>
		public bool? HasWater;
		/// <summary></summary>
		public bool? HasHoney;
		/// <summary></summary>
		public bool? HasLava;

		/// <summary></summary>
		public TileSlopeType? Slope;

		/// <summary></summary>
		public bool? IsModded;
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

			this.IsModded = builder.IsModded;
		}
	}
}
