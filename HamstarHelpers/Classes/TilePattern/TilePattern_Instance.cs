using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HamstarHelpers.Helpers.Tiles;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary>
	/// Used to in TilePattern's constructor to simplify specifying non-optional values. Keeps TilePattern immutable.
	/// </summary>
	public class TilePatternBuilder {
		/// <summary></summary>
		/// <param name="slopeType"></param>
		/// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
		public static TileShapeType ConvertToShape( TileSlopeType slopeType ) {
			switch( slopeType ) {
			case TileSlopeType.None:
				return TileShapeType.None;
			case TileSlopeType.Any:
				return TileShapeType.Any;
			case TileSlopeType.Top:
				return TileShapeType.TopSlope;
			case TileSlopeType.Bottom:
				return TileShapeType.BottomSlope;
			case TileSlopeType.Left:
				return TileShapeType.LeftSlope;
			case TileSlopeType.Right:
				return TileShapeType.RightSlope;
			case TileSlopeType.HalfBrick:
				return TileShapeType.HalfBrick;
			case TileSlopeType.TopRightSlope:
				return TileShapeType.TopRightSlope;
			case TileSlopeType.TopLeftSlope:
				return TileShapeType.TopLeftSlope;
			case TileSlopeType.BottomRightSlope:
				return TileShapeType.BottomRightSlope;
			case TileSlopeType.BottomLeftSlope:
				return TileShapeType.BottomLeftSlope;
			default:
				return TileShapeType.None;
			}
		}
#pragma warning restore CS0618 // Type or member is obsolete



		////////////////

		/// <summary>Inverts results.</summary>
		public bool Invert = false;

		/// <summary>If tile is active (not wall-only).</summary>
		public bool? IsActive = null;

		/// <summary>Distance to also check adjacent tiles from a given center point.</summary>
		public Rectangle? AreaFromCenter = null;

		/// <summary></summary>
		public ISet<int> IsAnyOfType = null;
		/// <summary></summary>
		public ISet<int> IsAnyOfWallType = null;

		/// <summary></summary>
		public ISet<int> IsNotAnyOfType = null;
		/// <summary></summary>
		public ISet<int> IsNotAnyOfWallType = null;

		/// <summary></summary>
		public bool? HasWire1 = null;
		/// <summary></summary>
		public bool? HasWire2 = null;
		/// <summary></summary>
		public bool? HasWire3 = null;
		/// <summary></summary>
		public bool? HasWire4 = null;

		/// <summary>Tile is wholly solid.</summary>
		[Obsolete("use HasSolidProperties and IsPlatform accordingly, instead")]
		public bool? IsSolid = null;
		/// <summary>Corresponds to `Main.tileSolid`.</summary>
		public bool? HasSolidProperties = null;
		/// <summary>Corresponds to `Main.tileSolidTop`</summary>
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

		/// @private
		[Obsolete( "use Shape" )]
		public TileSlopeType? Slope = null;

		/// <summary></summary>
		public TileShapeType? Shape = null;

		/// <summary></summary>
		public float? MinimumBrightness = null;
		/// <summary></summary>
		public float? MaximumBrightness = null;

		/// <summary></summary>
		public bool? IsModded = null;

		/// <summary></summary>
		public Func<int, int, bool> CustomCheck = null;

		/// <summary></summary>
		public ISet<TilePattern> AnyPattern = null;
	}




	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>Inverts results.</summary>
		public bool Invert = false;

		/// <summary>If tile is active (not wall-only).</summary>
		public bool? IsActive = null;

		/// <summary>Distance to also check adjacent tiles from a given center point.</summary>
		public Rectangle? AreaFromCenter { get; private set; } = null;

		/// <summary></summary>
		public ISet<int> IsAnyOfType = null;
		/// <summary></summary>
		public ISet<int> IsAnyOfWallType = null;

		/// <summary></summary>
		public ISet<int> IsNotAnyOfType = null;
		/// <summary></summary>
		public ISet<int> IsNotAnyOfWallType = null;

		/// <summary></summary>
		public bool? HasWire1 { get; private set; }
		/// <summary></summary>
		public bool? HasWire2 { get; private set; }
		/// <summary></summary>
		public bool? HasWire3 { get; private set; }
		/// <summary></summary>
		public bool? HasWire4 { get; private set; }

		/// <summary>Tile is wholly solid.</summary>
		[Obsolete( "use HasSolidProperties and IsPlatform accordingly" )]
		public bool? IsSolid {
			get {
				return this.HasSolidProperties.HasValue
					&& this.HasSolidProperties.Value
					&& (!this.IsPlatform.HasValue || !this.IsPlatform.Value);
			}
			private set {
				this.HasSolidProperties = value;
				if( value.HasValue && value.Value ) {
					this.HasSolidProperties = value;
					this.IsPlatform = false;
				}
			}
		}
		/// <summary>Corresponds to `Main.tileSolid`.</summary>
		public bool? HasSolidProperties { get; private set; }
		/// <summary>Corresponds to `Main.tileSolidTop`</summary>
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

		/// @private
		[Obsolete( "use Shape" )]
		public TileSlopeType? Slope = null;

		/// <summary></summary>
		public TileShapeType? Shape = null;

		/// <summary></summary>
		public float? MaximumBrightness { get; private set; }

		/// <summary></summary>
		public float? MinimumBrightness { get; private set; }

		/// <summary></summary>
		public bool? IsModded { get; private set; }

		/// <summary></summary>
		public Func<int, int, bool> CustomCheck = null;

		/// <summary></summary>
		public ISet<TilePattern> AnyPattern = null;



		////////////////

		/// <summary>
		/// </summary>
		public TilePattern( TilePatternBuilder builder ) {
			this.Invert = builder.Invert;
			this.IsActive = builder.IsActive;
			this.AreaFromCenter = builder.AreaFromCenter;

			this.IsAnyOfType = builder.IsAnyOfType;
			this.IsAnyOfWallType = builder.IsAnyOfWallType;
			this.IsNotAnyOfType = builder.IsNotAnyOfType;
			this.IsNotAnyOfWallType = builder.IsNotAnyOfWallType;

			this.HasWire1 = builder.HasWire1;
			this.HasWire2 = builder.HasWire2;
			this.HasWire3 = builder.HasWire3;
			this.HasWire4 = builder.HasWire4;

#pragma warning disable CS0618 // Type or member is obsolete
			this.IsSolid = builder.IsSolid;
#pragma warning restore CS0618 // Type or member is obsolete
			this.HasSolidProperties = builder.HasSolidProperties;
			this.IsPlatform = builder.IsPlatform;
			this.IsActuated = builder.IsActuated;
			this.IsVanillaBombable = builder.IsVanillaBombable;

			this.HasWall = builder.HasWall;

			this.HasWater = builder.HasWater;
			this.HasHoney = builder.HasHoney;
			this.HasLava = builder.HasLava;

#pragma warning disable CS0618 // Type or member is obsolete
			this.Slope = builder.Slope;
			this.Shape = builder.Shape;

			if( !builder.Shape.HasValue && builder.Slope.HasValue ) {
				this.Shape = TilePatternBuilder.ConvertToShape( builder.Slope.Value );
			}
#pragma warning restore CS0618 // Type or member is obsolete

			this.MinimumBrightness = builder.MinimumBrightness;
			this.MaximumBrightness = builder.MaximumBrightness;

			this.IsModded = builder.IsModded;

			this.CustomCheck = builder.CustomCheck;

			this.AnyPattern = builder.AnyPattern;
		}
	}
}
