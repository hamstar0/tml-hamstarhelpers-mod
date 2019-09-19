using System;
using Terraria;

namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world.
	/// </summary>
	public partial class WorldHelpers {
		[Obsolete( "use SurfaceLayerTopTileY", true )]
		public static int SurfaceLayerTop => WorldHelpers.SurfaceLayerTopTileY;

		[Obsolete( "use SurfaceLayerBottomTileY", true )]
		public static int SurfaceLayerBottom => WorldHelpers.SurfaceLayerBottomTileY;

		[Obsolete( "use DirtLayerTopTileY", true )]
		public static int DirtLayerTop => WorldHelpers.DirtLayerTopTileY;

		[Obsolete( "use DirtLayerBottomTileY", true )]
		public static int DirtLayerBottom => WorldHelpers.DirtLayerBottomTileY;


		[Obsolete( "use RockLayerTopTileY", true )]
		public static int RockLayerTop => WorldHelpers.RockLayerTopTileY;

		[Obsolete( "use RockLayerBottomTileY", true )]
		public static int RockLayerBottom => WorldHelpers.RockLayerBottomTileY;

		[Obsolete( "use SkyLayerTopTileY", true )]
		public static int SkyLayerTop => WorldHelpers.SkyLayerTopTileY;

		[Obsolete( "use SkyLayerBottomTileY", true )]
		public static int SkyLayerBottom => WorldHelpers.SkyLayerBottomTileY;

		[Obsolete( "use UnderworldLayerTopTileY", true )]
		public static int UnderworldLayerTop => WorldHelpers.UnderworldLayerTopTileY;

		[Obsolete( "use UnderworldLayerBottomTileY", true )]
		public static int UnderworldLayerBottom => WorldHelpers.UnderworldLayerBottomTileY;
		[Obsolete("use BeachWest", true)]
		public static int IsBeachWest => WorldHelpers.BeachWestTileX;

		[Obsolete( "use BeachWest", true )]
		public static int IsBeachEast => WorldHelpers.BeachEastTileX;
	}
}
