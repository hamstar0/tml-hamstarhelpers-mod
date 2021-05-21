using System;
using Microsoft.Xna.Framework;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the HUD map.
	/// </summary>
	public partial class HUDMapHelpers {
		/// @private
		[Obsolete("use value tuple variant", true)]
		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Vector2 worldPosition ) {
			var tuple = HUDMapHelpers.GetFullMapPositionAsScreenPosition( worldPosition );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}

		/// @private
		[Obsolete( "use value tuple variant", true )]
		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Rectangle worldArea ) {
			var tuple = HUDMapHelpers.GetFullMapPositionAsScreenPosition( worldArea );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}

		/// @private
		[Obsolete( "use value tuple variant", true )]
		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Vector2 worldPosition ) {
			var tuple = HUDMapHelpers.GetOverlayMapPositionAsScreenPosition( worldPosition );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}

		/// @private
		[Obsolete( "use value tuple variant", true )]
		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Rectangle worldArea ) {
			var tuple = HUDMapHelpers.GetOverlayMapPositionAsScreenPosition( worldArea );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}

		/// @private
		[Obsolete( "use value tuple variant", true )]
		public static Tuple<Vector2, bool> GetMiniMapScreenPosition( Vector2 worldPosition ) {
			var tuple = HUDMapHelpers.GetMiniMapPositionAsScreenPosition( worldPosition );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}

		/// @private
		[Obsolete( "use value tuple variant", true )]
		public static Tuple<Vector2, bool> GetMiniMapScreenPosition( Rectangle worldArea ) {
			var tuple = HUDMapHelpers.GetMiniMapPositionAsScreenPosition( worldArea );
			return Tuple.Create( tuple.ScreenPosition, tuple.IsOnScreen );
		}
	}
}
