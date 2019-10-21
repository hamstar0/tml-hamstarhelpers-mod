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

			if( pattern1.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern1.IsAnyOfWallType );
				if( pattern2.IsAnyOfWallType != null ) {
					builder.IsAnyOfWallType.UnionWith( pattern2.IsAnyOfWallType );
				}
			} else if( pattern2.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern2.IsAnyOfWallType );
			}

			builder.HasWire1 = TilePattern.CombinePositive( pattern1.HasWire1, pattern2.HasWire1 );
			builder.HasWire2 = TilePattern.CombinePositive( pattern1.HasWire2, pattern2.HasWire2 );
			builder.HasWire3 = TilePattern.CombinePositive( pattern1.HasWire3, pattern2.HasWire3 );
			builder.HasWire4 = TilePattern.CombinePositive( pattern1.HasWire4, pattern2.HasWire4 );

			builder.IsSolid = TilePattern.CombinePositive( pattern1.IsSolid, pattern2.IsSolid );
			builder.IsPlatform = TilePattern.CombinePositive( pattern1.IsPlatform, pattern2.IsPlatform );
			builder.IsActuated = TilePattern.CombinePositive( pattern1.IsActuated, pattern2.IsActuated );
			builder.IsVanillaBombable = TilePattern.CombinePositive( pattern1.IsVanillaBombable, pattern2.IsVanillaBombable );

			builder.HasWall = TilePattern.CombinePositive( pattern1.HasWall, pattern2.HasWall );

			builder.HasWater = TilePattern.CombinePositive( pattern1.HasWater, pattern2.HasWater );
			builder.HasHoney = TilePattern.CombinePositive( pattern1.HasHoney, pattern2.HasHoney );
			builder.HasLava = TilePattern.CombinePositive( pattern1.HasLava, pattern2.HasLava );

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

			builder.IsModded = TilePattern.CombinePositive( pattern1.IsModded, pattern2.IsModded );

			if( pattern1.CustomCheck != null && pattern2.CustomCheck != null ) {
				builder.CustomCheck = ( x, y ) => pattern1.CustomCheck(x, y) || pattern2.CustomCheck(x, y);
			} else if( pattern1.CustomCheck != null ) {
				builder.CustomCheck = pattern1.CustomCheck;
			} else if( pattern2.CustomCheck != null ) {
				builder.CustomCheck = pattern2.CustomCheck;
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

			if( pattern1.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern1.IsAnyOfType );
				if( pattern2.IsAnyOfType != null ) {
					builder.IsAnyOfType.UnionWith( pattern2.IsAnyOfType );
				}
			} else if( pattern2.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern2.IsAnyOfType );
			}

			if( pattern1.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern1.IsAnyOfWallType );
				if( pattern2.IsAnyOfWallType != null ) {
					builder.IsAnyOfWallType.UnionWith( pattern2.IsAnyOfWallType );
				}
			} else if( pattern2.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern2.IsAnyOfWallType );
			}

			builder.HasWire1 = TilePattern.CombineNegative( pattern1.HasWire1, pattern2.HasWire1 );
			builder.HasWire2 = TilePattern.CombineNegative( pattern1.HasWire2, pattern2.HasWire2 );
			builder.HasWire3 = TilePattern.CombineNegative( pattern1.HasWire3, pattern2.HasWire3 );
			builder.HasWire4 = TilePattern.CombineNegative( pattern1.HasWire4, pattern2.HasWire4 );

			builder.IsSolid = TilePattern.CombineNegative( pattern1.IsSolid, pattern2.IsSolid );
			builder.IsPlatform = TilePattern.CombineNegative( pattern1.IsPlatform, pattern2.IsPlatform );
			builder.IsActuated = TilePattern.CombineNegative( pattern1.IsActuated, pattern2.IsActuated );
			builder.IsVanillaBombable = TilePattern.CombineNegative( pattern1.IsVanillaBombable, pattern2.IsVanillaBombable );

			builder.HasWall = TilePattern.CombineNegative( pattern1.HasWall, pattern2.HasWall );

			builder.HasWater = TilePattern.CombineNegative( pattern1.HasWater, pattern2.HasWater );
			builder.HasHoney = TilePattern.CombineNegative( pattern1.HasHoney, pattern2.HasHoney );
			builder.HasLava = TilePattern.CombineNegative( pattern1.HasLava, pattern2.HasLava );

			if( pattern1.Slope.HasValue && !pattern2.Slope.HasValue ) {
				builder.Slope = pattern1.Slope;
			} else if( !pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				builder.Slope = pattern2.Slope;
			} else if( pattern1.Slope.HasValue && pattern2.Slope.HasValue ) {
				builder.Slope = pattern1.Slope;
				if( pattern1.Slope.Value == TileSlopeType.Top ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Bottom ) {
					if( pattern2.Slope.Value == TileSlopeType.Left ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Right ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
					}
				} else if( pattern1.Slope.Value == TileSlopeType.Left ) {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopLeftSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomLeftSlope;
					}
				} else {
					if( pattern2.Slope.Value == TileSlopeType.Top ) {
						builder.Slope = TileSlopeType.TopRightSlope;
					} else if( pattern2.Slope.Value == TileSlopeType.Bottom ) {
						builder.Slope = TileSlopeType.BottomRightSlope;
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

			builder.IsModded = TilePattern.CombineNegative( pattern1.IsModded, pattern2.IsModded );

			if( pattern1.CustomCheck != null && pattern2.CustomCheck != null ) {
				builder.CustomCheck = ( x, y ) => pattern1.CustomCheck(x, y) && pattern2.CustomCheck(x, y);
			} else if( pattern1.CustomCheck != null ) {
				builder.CustomCheck = pattern1.CustomCheck;
			} else if( pattern2.CustomCheck != null ) {
				builder.CustomCheck = pattern2.CustomCheck;
			}

			return new TilePattern( builder );
		}


		////

		private static bool? CombinePositive( bool? a, bool? b ) {
			if( a.HasValue && b.HasValue ) {
				return a.Value || b.Value;
			} else if( a.HasValue ) {
				return a.Value;
			} else if( b.HasValue ) {
				return b.Value;
			}
			return null;
		}

		private static bool? CombineNegative( bool? a, bool? b ) {
			if( a.HasValue && b.HasValue ) {
				return a.Value && b.Value;
			} else if( a.HasValue ) {
				return a.Value;
			} else if( b.HasValue ) {
				return b.Value;
			}
			return null;
		}
	}
}
