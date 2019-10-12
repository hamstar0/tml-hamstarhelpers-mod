using System;
using Terraria;

namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world.
	/// </summary>
	public partial class WorldHelpers {
		/// @private
		[Obsolete( "use SurfaceLayerTopTileY", true )]
		public static int SurfaceLayerTop => WorldHelpers.SurfaceLayerTopTileY;

		/// @private
		[Obsolete( "use SurfaceLayerBottomTileY", true )]
		public static int SurfaceLayerBottom => WorldHelpers.SurfaceLayerBottomTileY;

		/// @private
		[Obsolete( "use DirtLayerTopTileY", true )]
		public static int DirtLayerTop => WorldHelpers.DirtLayerTopTileY;

		/// @private
		[Obsolete( "use DirtLayerBottomTileY", true )]
		public static int DirtLayerBottom => WorldHelpers.DirtLayerBottomTileY;


		/// @private
		[Obsolete( "use RockLayerTopTileY", true )]
		public static int RockLayerTop => WorldHelpers.RockLayerTopTileY;

		/// @private
		[Obsolete( "use RockLayerBottomTileY", true )]
		public static int RockLayerBottom => WorldHelpers.RockLayerBottomTileY;

		/// @private
		[Obsolete( "use SkyLayerTopTileY", true )]
		public static int SkyLayerTop => WorldHelpers.SkyLayerTopTileY;

		/// @private
		[Obsolete( "use SkyLayerBottomTileY", true )]
		public static int SkyLayerBottom => WorldHelpers.SkyLayerBottomTileY;

		/// @private
		[Obsolete( "use UnderworldLayerTopTileY", true )]
		public static int UnderworldLayerTop => WorldHelpers.UnderworldLayerTopTileY;

		/// @private
		[Obsolete( "use UnderworldLayerBottomTileY", true )]
		public static int UnderworldLayerBottom => WorldHelpers.UnderworldLayerBottomTileY;
		/// @private
		[Obsolete("use BeachWest", true)]
		public static int IsBeachWest => WorldHelpers.BeachWestTileX;

		/// @private
		[Obsolete( "use BeachWest", true )]
		public static int IsBeachEast => WorldHelpers.BeachEastTileX;
	}
}
