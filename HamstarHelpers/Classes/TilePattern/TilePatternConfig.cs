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
		public Ref<bool> IsModded { get; set; }

		/// <summary></summary>
		public HashSet<TilePattern> AnyPattern { get; set; }    //= new HashSet<TilePattern>();



		////////////////

		/// <summary></summary>
		public TilePatternConfig() { }

		/// <summary></summary>
		/// <param name="builder"></param>
		public TilePatternConfig( TilePatternBuilder builder ) {
			this.IsActive = builder.IsActive.HasValue ? new Ref<bool>(builder.IsActive.Value) : null;
			this.AreaFromCenter = builder.AreaFromCenter.HasValue ? new Ref<Rectangle>(builder.AreaFromCenter.Value) : null;
			this.IsAnyOfType = builder.IsAnyOfType != null ? new HashSet<int>( builder.IsAnyOfType ) : null;
			this.IsAnyOfWallType = builder.IsAnyOfWallType != null ? new HashSet<int>( builder.IsAnyOfWallType ) : null;
			this.HasWire1 = builder.HasWire1.HasValue ? new Ref<bool>(builder.HasWire1.Value) : null;
			this.HasWire2 = builder.HasWire2.HasValue ? new Ref<bool>(builder.HasWire2.Value) : null;
			this.HasWire3 = builder.HasWire3.HasValue ? new Ref<bool>(builder.HasWire3.Value) : null;
			this.HasWire4 = builder.HasWire4.HasValue ? new Ref<bool>(builder.HasWire4.Value) : null;
			this.HasSolidProperties = builder.HasSolidProperties.HasValue ? new Ref<bool>(builder.HasSolidProperties.Value) : null;
			this.IsPlatform = builder.IsPlatform.HasValue ? new Ref<bool>(builder.IsPlatform.Value) : null;
			this.IsActuated = builder.IsActuated.HasValue ? new Ref<bool>(builder.IsActuated.Value) : null;
			this.IsVanillaBombable = builder.IsVanillaBombable.HasValue ? new Ref<bool>(builder.IsVanillaBombable.Value) : null;
			this.HasWall = builder.HasWall.HasValue ? new Ref<bool>(builder.HasWall.Value) : null;
			this.HasWater = builder.HasWater.HasValue ? new Ref<bool>(builder.HasWater.Value) : null;
			this.HasHoney = builder.HasHoney.HasValue ? new Ref<bool>(builder.HasHoney.Value) : null;
			this.HasLava = builder.HasLava.HasValue ? new Ref<bool>(builder.HasLava.Value) : null;
			this.Shape = builder.Shape.HasValue ? new Ref<int>((int)builder.Shape.Value) : null;
			this.MaximumBrightness = builder.MaximumBrightness.HasValue ? new FloatRef(builder.MaximumBrightness.Value) : null;
			this.MinimumBrightness = builder.MinimumBrightness.HasValue ? new FloatRef(builder.MinimumBrightness.Value) : null;
			this.IsModded = builder.IsModded.HasValue ? new Ref<bool>(builder.IsModded.Value) : null;
			this.AnyPattern = builder.AnyPattern != null ? new HashSet<TilePattern>( builder.AnyPattern ) : null;
		}


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
				IsModded = this.IsModded != null ? this.IsModded.Value : (bool?)null,
				AnyPattern = this.AnyPattern,
			} );
		}
	}
}
