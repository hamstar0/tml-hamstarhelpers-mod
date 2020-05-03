using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.UI.Config;
using HamstarHelpers.Helpers.Tiles;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary>
	/// Defines a ModConfig-friendly class for defining a TilePattern.
	/// </summary>
	public class TilePatternConfig {
		/// <summary></summary>
		public Ref<bool> IsActive { get; set; }

		/// <summary></summary>
		public Ref<Rectangle> AreaFromCenter { get; set; } = null;

		/// <summary></summary>
		public HashSet<int> IsAnyOfType { get; set; }	// = new HashSet<int>();
		/// <summary></summary>
		public HashSet<int> IsAnyOfWallType { get; set; }	// = new HashSet<int>();

		/// <summary></summary>
		public Ref<bool> HasWire1 { get; set; }
		/// <summary></summary>
		public Ref<bool> HasWire2 { get; set; }
		/// <summary></summary>
		public Ref<bool> HasWire3 { get; set; }
		/// <summary></summary>
		public Ref<bool> HasWire4 { get; set; }

		/// <summary></summary>
		public Ref<bool> HasSolidProperties { get; set; }
		/// <summary></summary>
		public Ref<bool> IsPlatform { get; set; }
		/// <summary></summary>
		public Ref<bool> IsActuated { get; set; }
		/// <summary></summary>
		public Ref<bool> IsVanillaBombable { get; set; }

		/// <summary></summary>
		public Ref<bool> HasWall { get; set; }

		/// <summary></summary>
		public Ref<bool> HasWater { get; set; }
		/// <summary></summary>
		public Ref<bool> HasHoney { get; set; }
		/// <summary></summary>
		public Ref<bool> HasLava { get; set; }

		/// <summary></summary>
		public Ref<int> Shape { get; set; }

		/// <summary></summary>
		public FloatRef MaximumBrightness { get; set; }

		/// <summary></summary>
		public FloatRef MinimumBrightness { get; set; }

		/// <summary></summary>
		public int IsModded { get; set; } = 0;

		/// <summary></summary>
		public HashSet<TilePattern> AnyPattern { get; set; }	//= new HashSet<TilePattern>();



		////////////////

		/// <summary>
		/// Converts this class to a valid `TilePattern`.
		/// </summary>
		/// <returns></returns>
		public TilePattern ToTilePattern() {
			return new TilePattern( new TilePatternBuilder {
				IsActive = this.IsActive != null ? this.IsActive.Value : (bool?)null,
				AreaFromCenter = this.AreaFromCenter?.Value,
				IsAnyOfType = this.IsAnyOfType,
				IsAnyOfWallType = this.IsAnyOfWallType,
				HasWire1 = this.HasWire1 != null ? this.HasWire1.Value : (bool?)null,
				HasWire2 = this.HasWire2 != null ? this.HasWire2.Value : (bool?)null,
				HasWire3 = this.HasWire3 != null ? this.HasWire3.Value : (bool?)null,
				HasWire4 = this.HasWire4 != null ? this.HasWire4.Value : (bool?)null,
				HasSolidProperties = this.HasSolidProperties != null ? this.HasSolidProperties.Value : (bool?)null,
				IsPlatform = this.IsPlatform != null ? this.IsPlatform.Value : (bool?)null,
				IsActuated = this.IsActuated != null ? this.IsActuated.Value : (bool?)null,
				IsVanillaBombable = this.IsVanillaBombable != null ? this.IsVanillaBombable.Value : (bool?)null,
				HasWall = this.HasWall != null ? this.HasWall.Value : (bool?)null,
				HasWater = this.HasWater != null ? this.HasWater.Value : (bool?)null,
				HasHoney = this.HasHoney != null ? this.HasHoney.Value : (bool?)null,
				HasLava = this.HasLava != null ? this.HasLava.Value : (bool?)null,
				Shape = (TileShapeType?)this.Shape?.Value,
				MaximumBrightness = this.MaximumBrightness?.Value,
				MinimumBrightness = this.MinimumBrightness?.Value,
				IsModded = this.IsModded != 0 ? this.IsModded == 1 : (bool?)null,
				AnyPattern = this.AnyPattern,
			} );
		}
	}
}
