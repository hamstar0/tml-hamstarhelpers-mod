using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HudHelpers {
	public static partial class HudMapHelpers {
		[Obsolete("use GetFullMapPosition(Rectangle, out Vector2)", true)]
		public static Vector2 GetFullMapPosition( Rectangle origin ) {    //Main.mapFullscreen
			Vector2 position;
			HudMapHelpers.GetFullMapScreenPosition( origin, out position );
			return position;
		}


		[Obsolete( "use GetOverlayMapPosition(Rectangle, out Vector2)", true )]
		public static Vector2 GetOverlayMapPosition( Rectangle origin ) {    //Main.mapStyle == 2
			Vector2 position;
			HudMapHelpers.GetOverlayMapScreenPosition( origin, out position );
			return position;
		}


		[Obsolete( "use GetMiniMapPosition(Rectangle, out Vector2)", true )]
		public static Vector2? GetMiniMapPosition( Rectangle origin ) {	//Main.mapStyle == 1
			Vector2 position;
			if( HudMapHelpers.GetMiniMapScreenPosition( origin, out position ) ) {
				return position;
			}
			return null;
		}
	}
}
