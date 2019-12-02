using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ObjectData;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile placement (convenience bindings for WorldGen).
	/// </summary>
	public partial class TilePlacementHelpers {
		/// <summary>
		/// Convenience binding to place the given tile intuitively positioned. Places from the bottom.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="bottomY"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static bool Place( int leftX, int bottomY, ushort type, sbyte direction = -1, int style = 0 ) {
			if( TilePlacementHelpers.TryPrecisePlace(leftX, bottomY, type, direction, style) ) {
				return true;
			}

			var tileObjData = TileObjectData.GetTileData( type, style );
			int x = leftX + tileObjData.Origin.X;
			int y = bottomY + tileObjData.Origin.X + tileObjData.Height;

			Main.player[255].direction = direction;

			return WorldGen.PlaceTile( x, y, type, false, false, 255, style );
		}


		/// <summary>
		/// Convenience binding to attempt to place the given tile intuitively positioned. Places from the bottom.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="bottomY"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		/// <param name="style"></param>
		/// <returns>`false` if no suitable Place binding found.</returns>
		public static bool TryPrecisePlace( int leftX, int bottomY, ushort type, sbyte direction = -1, int style = 0 ) {
			var tileObjData = TileObjectData.GetTileData( type, style );
			int width = tileObjData.Width;
			int height = tileObjData.Height;

			switch( width ) {
			case 1:
				switch( height ) {
				case 1:
					return WorldGen.PlaceTile( leftX, bottomY - 1, type, false, false, -1, style );
				case 2:
					TilePlacementHelpers.Place1x2( leftX, bottomY - 1, type, style );
					return true;
				default:
					TilePlacementHelpers.Place1xX( leftX, bottomY - (height - 1), type, style );
					return true;
				}
			case 2:
				switch( height ) {
				case 2:
					TilePlacementHelpers.Place2x2( leftX, bottomY - 1, type, style );
					return true;
				default:
					TilePlacementHelpers.Place2xX( leftX, bottomY - (height - 1), type, style );
					return true;
				}
			case 3:
				switch( height ) {
				case 1:
					TilePlacementHelpers.Place3x1( leftX, bottomY, type, style );
					return true;
				case 2:
					TilePlacementHelpers.Place3x2( leftX, bottomY - 1, type, style );
					return true;
				case 3:
					TilePlacementHelpers.Place3x3( leftX, bottomY - 2, type, style );
					return true;
				case 4:
					TilePlacementHelpers.Place3x4( leftX, bottomY - 3, type, style );
					return true;
				}
				break;
			case 4:
				switch( height ) {
				case 2:
					TilePlacementHelpers.Place4x2( leftX, bottomY - 1, type, direction, style );
					return true;
				}
				break;
			case 5:
				switch( height ) {
				case 4:
					TilePlacementHelpers.Place5x4( leftX, bottomY - 3, type, style );
					return true;
				}
				break;
			case 6:
				switch( height ) {
				case 3:
					TilePlacementHelpers.Place6x3( leftX, bottomY - 2, type, direction, style );
					return true;
				}
				break;
			}

			return false;
		}


		////////////////

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place1xX( int leftX, int topY, ushort type, int style = 0 ) {
			int yOffset = type == 92 ? 6 : 3;

			WorldGen.Place1xX( leftX, topY - yOffset + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2xX( int leftX, int topY, ushort type, int style = 0 ) {
			int yOffset = 3;
			if( type == 104 ) {
				yOffset = 5;
			} else if( type == 207 ) {
				yOffset = 4;
			}

			WorldGen.Place2xX( leftX, topY - yOffset + 1, type, style );
		}


		///////////////

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place1x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place1x2( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2x1( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x1( leftX, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x1( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x1( leftX + 1, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x2( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x3( int leftX, int topY, ushort type, int style = 0 ) {
			if( type == 106 || type == 212 || type == 219 || type == 220 || type == 228 || type == 231 || type == 243
					|| type == 247 || type == 283 || (type >= 300 && type <= 308) || type == 354 || type == 355 ) {
				topY += 2;
			}
			WorldGen.Place3x3( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x4( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x4( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		/// <param name="style"></param>
		public static void Place4x2( int leftX, int topY, ushort type, sbyte direction = -1, int style = 0 ) {
			WorldGen.Place4x2( leftX + 1, topY + 1, type, direction, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place5x4( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place5x4( leftX + 2, topY + 3, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		/// <param name="style"></param>
		public static void Place6x3( int leftX, int topY, ushort type, sbyte direction = -1, int style = 0 ) {
			WorldGen.Place6x3( leftX + 3, topY + 2, type, direction, style );
		}


		////////////////

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place1x2Top( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place1x2Top( leftX, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2x2Horizontal( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2Horizontal( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2x2Style( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2Style( leftX + 1, topY + 1, type, style );
		}


		////////////////

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place2x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x3Wall( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x2Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x2Wall( leftX + 1, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place3x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x3Wall( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place4x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place4x3Wall( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void Place6x4Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place6x4Wall( leftX + 2, topY + 2, type, style );
		}
	}
}
