using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile1x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place1x2( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile1x2Top( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place1x2Top( leftX, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile2x1( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x1( leftX, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile2x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile2x2Horizontal( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2Horizontal( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile2x2Style( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x2Style( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile2x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place2x3Wall( leftX, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile3x1( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x1( leftX + 1, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile3x2( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x2( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile3x2Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x2Wall( leftX + 1, topY, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile3x3( int leftX, int topY, ushort type, int style = 0 ) {
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
		public static void PlaceTile3x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place3x3Wall( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile3x4( int leftX, int topY, ushort type, int style = 0 ) {
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
		public static void PlaceTile4x2( int leftX, int topY, ushort type, int direction = -1, int style = 0 ) {
			WorldGen.Place4x2( leftX + 1, topY + 1, type, direction, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile4x3Wall( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place4x3Wall( leftX + 1, topY + 1, type, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile5x4( int leftX, int topY, ushort type, int style = 0 ) {
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
		public static void PlaceTile6x3( int leftX, int topY, ushort type, int direction = -1, int style = 0 ) {
			WorldGen.Place6x3( leftX + 3, topY + 2, type, direction, style );
		}

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile6x4( int leftX, int topY, ushort type, int style = 0 ) {
			WorldGen.Place6x4Wall( leftX + 2, topY + 2, type, style );
		}


		///

		/// <summary>
		/// Convenience binding for placing the given tile intuitively positioned.
		/// </summary>
		/// <param name="leftX"></param>
		/// <param name="topY"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		public static void PlaceTile1xX( int leftX, int topY, ushort type, int style = 0 ) {
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
	}
}
