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
		public int IsActive { get; set; } = 0;

		/// <summary></summary>
		public Ref<Rectangle> AreaFromCenter { get; set; } = null;

		/// <summary></summary>
		public ISet<int> IsAnyOfType { get; set; } = new HashSet<int>();
		/// <summary></summary>
		public ISet<int> IsAnyOfWallType { get; set; } = new HashSet<int>();

		/// <summary></summary>
		public int HasWire1 { get; set; } = 0;
		/// <summary></summary>
		public int HasWire2 { get; set; } = 0;
		/// <summary></summary>
		public int HasWire3 { get; set; } = 0;
		/// <summary></summary>
		public int HasWire4 { get; set; } = 0;

		/// <summary></summary>
		public int HasSolidProperties { get; set; } = 0;
		/// <summary></summary>
		public int IsPlatform { get; set; } = 0;
		/// <summary></summary>
		public int IsActuated { get; set; } = 0;
		/// <summary></summary>
		public int IsVanillaBombable { get; set; } = 0;

		/// <summary></summary>
		public int HasWall { get; set; } = 0;

		/// <summary></summary>
		public int HasWater { get; set; } = 0;
		/// <summary></summary>
		public int HasHoney { get; set; } = 0;
		/// <summary></summary>
		public int HasLava { get; set; } = 0;

		/// <summary></summary>
		public Ref<int> Shape { get; set; }

		/// <summary></summary>
		public FloatRef MaximumBrightness { get; set; }

		/// <summary></summary>
		public FloatRef MinimumBrightness { get; set; }

		/// <summary></summary>
		public int IsModded { get; set; } = 0;

		/// <summary></summary>
		public ISet<TilePattern> AnyPattern { get; set; } = new HashSet<TilePattern>();



		////////////////

		/// <summary>
		/// Converts this class to a valid `TilePattern`.
		/// </summary>
		/// <returns></returns>
		public TilePattern ToTilePattern() {
			return new TilePattern( new TilePatternBuilder {
				IsActive = this.IsActive != 0 ? this.IsActive == 1 : (bool?)null,
				AreaFromCenter = this.AreaFromCenter?.Value,
				IsAnyOfType = this.IsAnyOfType,
				IsAnyOfWallType = this.IsAnyOfWallType,
				HasWire1 = this.HasWire1 != 0 ? this.HasWire1 == 1 : (bool?)null,
				HasWire2 = this.HasWire2 != 0 ? this.HasWire2 == 1 : (bool?)null,
				HasWire3 = this.HasWire3 != 0 ? this.HasWire3 == 1 : (bool?)null,
				HasWire4 = this.HasWire4 != 0 ? this.HasWire4 == 1 : (bool?)null,
				HasSolidProperties = this.HasSolidProperties != 0 ? this.HasSolidProperties == 1 : (bool?)null,
				IsPlatform = this.IsPlatform != 0 ? this.IsPlatform == 1 : (bool?)null,
				IsActuated = this.IsActuated != 0 ? this.IsActuated == 1 : (bool?)null,
				IsVanillaBombable = this.IsVanillaBombable != 0 ? this.IsVanillaBombable == 1 : (bool?)null,
				HasWall = this.HasWall != 0 ? this.HasWall == 1 : (bool?)null,
				HasWater = this.HasWater != 0 ? this.HasWater == 1 : (bool?)null,
				HasHoney = this.HasHoney != 0 ? this.HasHoney == 1 : (bool?)null,
				HasLava = this.HasLava != 0 ? this.HasLava == 1 : (bool?)null,
				Shape = (TileShapeType?)this.Shape?.Value,
				MaximumBrightness = this.MaximumBrightness?.Value,
				MinimumBrightness = this.MinimumBrightness?.Value,
				IsModded = this.IsModded != 0 ? this.IsModded == 1 : (bool?)null,
				AnyPattern = this.AnyPattern,
			} );
		}
	}
}
