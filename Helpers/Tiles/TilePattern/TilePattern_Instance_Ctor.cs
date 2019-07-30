using Terraria;


namespace HamstarHelpers.Helpers.Tiles.TilePattern {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		private TilePattern() { }


		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="hasWire1"></param>
		/// <param name="hasWire2"></param>
		/// <param name="hasWire3"></param>
		/// <param name="hasWire4"></param>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		/// <param name="hasWater"></param>
		/// <param name="hasHoney"></param>
		/// <param name="hasLava"></param>
		public TilePattern( bool? hasWire1, bool? hasWire2, bool? hasWire3, bool? hasWire4,
				bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope,
				bool? hasWater, bool? hasHoney, bool? hasLava ) {
			this.HasWire1 = hasWire1;
			this.HasWire2 = hasWire2;
			this.HasWire3 = hasWire3;
			this.HasWire4 = hasWire4;
			this.IsSolid = isSolid;
			this.IsPlatform = isPlatform;
			this.IsActuated = isActuated;
			this.IsVanillaBombable = isVanillaBombable;
			this.HasWall = hasWall;
			this.Slope = slope;
			this.HasWater = hasWater;
			this.HasHoney = hasHoney;
			this.HasLava = hasLava;
		}

		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		/// <param name="hasWater"></param>
		/// <param name="hasHoney"></param>
		/// <param name="hasLava"></param>
		public TilePattern( bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope,
				bool? hasWater, bool? hasHoney, bool? hasLava )
			: this( null, null, null, null,
				isSolid,
				isPlatform,
				isActuated,
				isVanillaBombable,
				hasWall,
				slope,
				hasWater, hasHoney, hasLava ) {
		}

		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		public TilePattern( bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope )
			: this( isSolid,
				isPlatform,
				isActuated,
				isVanillaBombable,
				hasWall,
				slope,
				null, null, null ) {
		}


		////

		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="baseModel">Base pattern to derive from. Use `null` for the following parameters to defer to base.</param>
		/// <param name="hasWire1"></param>
		/// <param name="hasWire2"></param>
		/// <param name="hasWire3"></param>
		/// <param name="hasWire4"></param>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		/// <param name="hasWater"></param>
		/// <param name="hasHoney"></param>
		/// <param name="hasLava"></param>
		public TilePattern( TilePattern baseModel,
				bool? hasWire1, bool? hasWire2, bool? hasWire3, bool? hasWire4,
				bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope,
				bool? hasWater, bool? hasHoney, bool? hasLava )
			: this( hasWire1, hasWire2, hasWire3, hasWire4,
				isSolid,
				isPlatform,
				isActuated,
				isVanillaBombable,
				hasWall,
				slope,
				hasWater, hasHoney, hasLava ) {
			this.HasWire1 = this.HasWire1 ?? baseModel.HasWire1;
			this.HasWire2 = this.HasWire2 ?? baseModel.HasWire2;
			this.HasWire3 = this.HasWire3 ?? baseModel.HasWire3;
			this.HasWire4 = this.HasWire4 ?? baseModel.HasWire4;
			this.IsSolid = this.IsSolid ?? baseModel.IsSolid;
			this.HasWall = this.HasWall ?? baseModel.HasWall;
			this.HasWater = this.HasWater ?? baseModel.HasWater;
			this.HasHoney = this.HasHoney ?? baseModel.HasHoney;
			this.HasLava = this.HasLava ?? baseModel.HasLava;
		}

		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="baseModel">Base pattern to derive from. Use `null` for the following parameters to defer to base.</param>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		/// <param name="hasWater"></param>
		/// <param name="hasHoney"></param>
		/// <param name="hasLava"></param>
		public TilePattern( TilePattern baseModel,
				bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope,
				bool? hasWater, bool? hasHoney, bool? hasLava )
			: this( baseModel,
				baseModel.HasWire1, baseModel.HasWire2, baseModel.HasWire3, baseModel.HasWire4,
				isSolid,
				isPlatform,
				isActuated,
				isVanillaBombable,
				hasWall,
				slope,
				hasWater, hasHoney, hasLava ) {
		}

		/// <summary>
		/// Note: `null` values for `bool?` parameters mean either type of that tile are matched.
		/// </summary>
		/// <param name="baseModel">Base pattern to derive from. Use `null` for the following parameters to defer to base.</param>
		/// <param name="isSolid"></param>
		/// <param name="isPlatform"></param>
		/// <param name="isActuated"></param>
		/// <param name="isVanillaBombable"></param>
		/// <param name="hasWall"></param>
		/// <param name="slope"></param>
		public TilePattern( TilePattern baseModel,
				bool? isSolid,
				bool? isPlatform,
				bool? isActuated,
				bool? isVanillaBombable,
				bool? hasWall,
				TileSlopeType? slope )
			: this( baseModel,
				baseModel.HasWire1, baseModel.HasWire2, baseModel.HasWire3, baseModel.HasWire4,
				isSolid,
				isPlatform,
				isActuated,
				isVanillaBombable,
				hasWall,
				slope,
				baseModel.HasWater, baseModel.HasHoney, baseModel.HasLava ) {
		}
	}
}
