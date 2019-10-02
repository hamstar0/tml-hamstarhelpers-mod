using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Combines 2 patterns into a new pattern. Positive filters are favored over negatives.
		/// </summary>
		/// <param name="pattern1"></param>
		/// <param name="pattern2"></param>
		/// <param name="blendLight"></param>
		/// <returns></returns>
		public static TilePattern CombinePositive( TilePattern pattern1, TilePattern pattern2, bool blendLight = false ) {
			var builder = new TilePatternBuilder();

			if( pattern1.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern1.IsAnyOfType );
				if( pattern2.IsAnyOfType != null ) {
					builder.IsAnyOfType.UnionWith( pattern2.IsAnyOfType );
				}
			} else if( pattern2.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern2.IsAnyOfType );
			}

			if( (pattern1.HasWire1.HasValue && pattern1.HasWire1.Value) ||
					(pattern2.HasWire1.HasValue && pattern2.HasWire1.Value) ) {
				builder.HasWire1 = true;
			} else if( pattern1.HasWire1.HasValue || pattern2.HasWire1.HasValue ) {
				builder.HasWire1 = false;
			}
			if( (pattern1.HasWire2.HasValue && pattern1.HasWire2.Value) ||
					(pattern2.HasWire2.HasValue && pattern2.HasWire2.Value) ) {
				builder.HasWire2 = true;
			} else if( pattern1.HasWire2.HasValue || pattern2.HasWire2.HasValue ) {
				builder.HasWire2 = false;
			}
			if( (pattern1.HasWire3.HasValue && pattern1.HasWire3.Value) ||
					(pattern2.HasWire3.HasValue && pattern2.HasWire3.Value) ) {
				builder.HasWire3 = true;
			} else if( pattern1.HasWire3.HasValue || pattern2.HasWire3.HasValue ) {
				builder.HasWire3 = false;
			}
			if( (pattern1.HasWire4.HasValue && pattern1.HasWire4.Value) ||
					(pattern2.HasWire4.HasValue && pattern2.HasWire4.Value) ) {
				builder.HasWire4 = true;
			} else if( pattern1.HasWire4.HasValue || pattern2.HasWire4.HasValue ) {
				builder.HasWire4 = false;
			}

			if( (pattern1.IsSolid.HasValue && pattern1.IsSolid.Value) ||
					(pattern2.IsSolid.HasValue && pattern2.IsSolid.Value) ) {
				builder.IsSolid = true;
			} else if( pattern1.IsSolid.HasValue || pattern2.IsSolid.HasValue ) {
				builder.IsSolid = false;
			}
			if( (pattern1.IsPlatform.HasValue && pattern1.IsPlatform.Value) ||
					(pattern2.IsPlatform.HasValue && pattern2.IsPlatform.Value) ) {
				builder.IsPlatform = true;
			} else if( pattern1.IsPlatform.HasValue || pattern2.IsPlatform.HasValue ) {
				builder.IsPlatform = false;
			}
			if( (pattern1.IsActuated.HasValue && pattern1.IsActuated.Value) ||
					(pattern2.IsActuated.HasValue && pattern2.IsActuated.Value) ) {
				builder.IsActuated = true;
			} else if( pattern1.IsActuated.HasValue || pattern2.IsActuated.HasValue ) {
				builder.IsActuated = false;
			}
			if( (pattern1.IsVanillaBombable.HasValue && pattern1.IsVanillaBombable.Value) ||
					(pattern2.IsVanillaBombable.HasValue && pattern2.IsVanillaBombable.Value) ) {
				builder.IsVanillaBombable = true;
			} else if( pattern1.IsVanillaBombable.HasValue || pattern2.IsVanillaBombable.HasValue ) {
				builder.IsVanillaBombable = false;
			}

			if( (pattern1.HasWall.HasValue && pattern1.HasWall.Value) ||
					(pattern2.HasWall.HasValue && pattern2.HasWall.Value) ) {
				builder.HasWall = true;
			} else if( pattern1.HasWall.HasValue || pattern2.HasWall.HasValue ) {
				builder.HasWall = false;
			}

			if( (pattern1.HasWater.HasValue && pattern1.HasWater.Value) ||
					(pattern2.HasWater.HasValue && pattern2.HasWater.Value) ) {
				builder.HasWater = true;
			} else if( pattern1.HasWater.HasValue || pattern2.HasWater.HasValue ) {
				builder.HasWater = false;
			}
			if( (pattern1.HasHoney.HasValue && pattern1.HasHoney.Value) ||
					(pattern2.HasHoney.HasValue && pattern2.HasHoney.Value) ) {
				builder.HasHoney = true;
			} else if( pattern1.HasHoney.HasValue || pattern2.HasHoney.HasValue ) {
				builder.HasHoney = false;
			}
			if( (pattern1.HasLava.HasValue && pattern1.HasLava.Value) ||
					(pattern2.HasLava.HasValue && pattern2.HasLava.Value) ) {
				builder.HasLava = true;
			} else if( pattern1.HasLava.HasValue || pattern2.HasLava.HasValue ) {
				builder.HasLava = false;
			}

			if( pattern1.Slope.HasValue && !pattern2.Slope.HasValue ) {
				builder.Slope = pattern1.Slope;
			} else if( !pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				builder.Slope = pattern2.Slope;
			} else if( pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				if( pattern1.Slope.Value == TileSlopeType.Top ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					} else {
						builder.Slope = pattern2.Slope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Bottom ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
					} else {
						builder.Slope = pattern2.Slope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Left ) {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					} else {
						builder.Slope = pattern2.Slope;
					}
				} else {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
					} else {
						builder.Slope = pattern2.Slope;
					}
				}
			}

			if( pattern1.MinimumBrightness.HasValue && !pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern1.MinimumBrightness;
			} else if( !pattern1.MinimumBrightness.HasValue && pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern2.MinimumBrightness;
			} else {
				if( blendLight ) {
					builder.MinimumBrightness = ( pattern1.MinimumBrightness.Value + pattern2.MinimumBrightness.Value )
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MinimumBrightness.Value < pattern2.MinimumBrightness.Value ?
						pattern1.MinimumBrightness.Value :
						pattern2.MinimumBrightness.Value;
				}
			}
			if( pattern1.MaximumBrightness.HasValue && !pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern1.MaximumBrightness;
			} else if( !pattern1.MaximumBrightness.HasValue && pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern2.MaximumBrightness;
			} else {
				if( blendLight ) {
					builder.MaximumBrightness = ( pattern1.MaximumBrightness.Value + pattern2.MaximumBrightness.Value )
						* 0.5f;
				} else {
					builder.MaximumBrightness = pattern1.MaximumBrightness.Value < pattern2.MaximumBrightness.Value ?
						pattern1.MaximumBrightness.Value :
						pattern2.MaximumBrightness.Value;
				}
			}

			if( (pattern1.IsModded.HasValue && pattern1.IsModded.Value) ||
					(pattern2.IsModded.HasValue && pattern2.IsModded.Value) ) {
				builder.HasLava = true;
			}
			
			return new TilePattern( builder );
		}


		/// <summary>
		/// Combines 2 patterns into a new pattern. Negative filters are favored over positives.
		/// </summary>
		/// <param name="pattern1"></param>
		/// <param name="pattern2"></param>
		/// <param name="blendLight"></param>
		/// <returns></returns>
		public static TilePattern CombineNegative( TilePattern pattern1, TilePattern pattern2, bool blendLight = false ) {
			var builder = new TilePatternBuilder();

			if( (pattern1.HasWire1.HasValue && !pattern1.HasWire1.Value) ||
					(pattern2.HasWire1.HasValue && !pattern2.HasWire1.Value) ) {
				builder.HasWire1 = false;
			} else if( pattern1.HasWire1.HasValue || pattern2.HasWire1.HasValue ) {
				builder.HasWire1 = true;
			}
			if( (pattern1.HasWire2.HasValue && !pattern1.HasWire2.Value) ||
					(pattern2.HasWire2.HasValue && !pattern2.HasWire2.Value) ) {
				builder.HasWire2 = false;
			} else if( pattern1.HasWire2.HasValue || pattern2.HasWire2.HasValue ) {
				builder.HasWire2 = true;
			}
			if( (pattern1.HasWire3.HasValue && !pattern1.HasWire3.Value) ||
					(pattern2.HasWire3.HasValue && !pattern2.HasWire3.Value) ) {
				builder.HasWire3 = false;
			} else if( pattern1.HasWire3.HasValue || pattern2.HasWire3.HasValue ) {
				builder.HasWire3 = true;
			}
			if( (pattern1.HasWire4.HasValue && !pattern1.HasWire4.Value) ||
					(pattern2.HasWire4.HasValue && !pattern2.HasWire4.Value) ) {
				builder.HasWire4 = false;
			} else if( pattern1.HasWire4.HasValue || pattern2.HasWire4.HasValue ) {
				builder.HasWire4 = true;
			}

			if( (pattern1.IsSolid.HasValue && pattern1.IsSolid.Value) ||
					(pattern2.IsSolid.HasValue && pattern2.IsSolid.Value) ) {
				builder.IsSolid = false;
			} else if( pattern1.IsSolid.HasValue || pattern2.IsSolid.HasValue ) {
				builder.IsSolid = true;
			}
			if( (pattern1.IsPlatform.HasValue && pattern1.IsPlatform.Value) ||
					(pattern2.IsPlatform.HasValue && pattern2.IsPlatform.Value) ) {
				builder.IsPlatform = false;
			} else if( pattern1.IsPlatform.HasValue || pattern2.IsPlatform.HasValue ) {
				builder.IsPlatform = true;
			}
			if( (pattern1.IsActuated.HasValue && pattern1.IsActuated.Value) ||
					(pattern2.IsActuated.HasValue && pattern2.IsActuated.Value) ) {
				builder.IsActuated = false;
			} else if( pattern1.IsActuated.HasValue || pattern2.IsActuated.HasValue ) {
				builder.IsActuated = true;
			}
			if( (pattern1.IsVanillaBombable.HasValue && pattern1.IsVanillaBombable.Value) ||
					(pattern2.IsVanillaBombable.HasValue && pattern2.IsVanillaBombable.Value) ) {
				builder.IsVanillaBombable = false;
			} else if( pattern1.IsVanillaBombable.HasValue || pattern2.IsVanillaBombable.HasValue ) {
				builder.IsVanillaBombable = true;
			}

			if( (pattern1.HasWall.HasValue && pattern1.HasWall.Value) ||
					(pattern2.HasWall.HasValue && pattern2.HasWall.Value) ) {
				builder.HasWall = false;
			} else if( pattern1.HasWall.HasValue || pattern2.HasWall.HasValue ) {
				builder.HasWall = true;
			}

			if( (pattern1.HasWater.HasValue && pattern1.HasWater.Value) ||
					(pattern2.HasWater.HasValue && pattern2.HasWater.Value) ) {
				builder.HasWater = false;
			} else if( pattern1.HasWater.HasValue || pattern2.HasWater.HasValue ) {
				builder.HasWater = true;
			}
			if( (pattern1.HasHoney.HasValue && pattern1.HasHoney.Value) ||
					(pattern2.HasHoney.HasValue && pattern2.HasHoney.Value) ) {
				builder.HasHoney = false;
			} else if( pattern1.HasHoney.HasValue || pattern2.HasHoney.HasValue ) {
				builder.HasHoney = true;
			}
			if( (pattern1.HasLava.HasValue && pattern1.HasLava.Value) ||
					(pattern2.HasLava.HasValue && pattern2.HasLava.Value) ) {
				builder.HasLava = false;
			} else if( pattern1.HasLava.HasValue || pattern2.HasLava.HasValue ) {
				builder.HasLava = true;
			}

			if( pattern1.Slope.HasValue && !pattern2.Slope.HasValue ) {
				builder.Slope = pattern1.Slope;
			} else if( !pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				builder.Slope = pattern2.Slope;
			} else if( pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				if( pattern1.Slope.Value == TileSlopeType.Top ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					} else {
						builder.Slope = pattern1.Slope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Bottom ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
					} else {
						builder.Slope = pattern1.Slope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Left ) {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					} else {
						builder.Slope = pattern1.Slope;
					}
				} else {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
					} else {
						builder.Slope = pattern1.Slope;
					}
				}
			}

			if( pattern1.MinimumBrightness.HasValue && !pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern1.MinimumBrightness;
			} else if( !pattern1.MinimumBrightness.HasValue && pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern2.MinimumBrightness;
			} else {
				if( blendLight ) {
					builder.MinimumBrightness = (pattern1.MinimumBrightness.Value + pattern2.MinimumBrightness.Value)
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MinimumBrightness.Value < pattern2.MinimumBrightness.Value ?
						pattern2.MinimumBrightness.Value :
						pattern1.MinimumBrightness.Value;
				}
			}
			if( pattern1.MaximumBrightness.HasValue && !pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern1.MaximumBrightness;
			} else if( !pattern1.MaximumBrightness.HasValue && pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern2.MaximumBrightness;
			} else {
				if( blendLight ) {
					builder.MaximumBrightness = (pattern1.MaximumBrightness.Value + pattern2.MaximumBrightness.Value)
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MaximumBrightness.Value < pattern2.MaximumBrightness.Value ?
						pattern2.MaximumBrightness.Value :
						pattern1.MaximumBrightness.Value;
				}
			}

			if( (pattern1.IsModded.HasValue && pattern1.IsModded.Value) ||
					(pattern2.IsModded.HasValue && pattern2.IsModded.Value) ) {
				builder.HasLava = false;
			} else if( pattern1.IsModded.HasValue || pattern2.IsModded.HasValue ) {
				builder.IsModded = true;
			}

			return new TilePattern( builder );
		}
	}
}
