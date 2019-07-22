using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Preset for walls, no tiles.
		/// </summary>
		public readonly static TilePattern OpenWall = new TilePattern {
			IsSolid = false,
			HasWall = true
		};

		/// <summary>
		/// Preset for common solid tiles.
		/// </summary>
		public readonly static TilePattern CommonSolid = new TilePattern {
			IsSolid = true,
			IsActuated = false,
			IsPlatform = false
		};



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
	}
}
