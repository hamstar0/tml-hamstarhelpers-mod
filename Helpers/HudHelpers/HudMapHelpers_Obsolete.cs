using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HudHelpers {
	public static partial class HudMapHelpers {
		[Obsolete("use GetFullMapPosition(Rectangle, out Vector2)", true)]
		public static Vector2 GetFullMapPosition( Rectangle origin ) {    //Main.mapFullscreen
			var data = HudMapHelpers.GetFullMapScreenPosition( origin );
			return data.Item1;
		}


		[Obsolete( "use GetOverlayMapPosition(Rectangle, out Vector2)", true )]
		public static Vector2 GetOverlayMapPosition( Rectangle origin ) {    //Main.mapStyle == 2
			var data = HudMapHelpers.GetOverlayMapScreenPosition( origin );
			return data.Item1;
		}


		[Obsolete( "use GetMiniMapPosition(Rectangle, out Vector2)", true )]
		public static Vector2? GetMiniMapPosition( Rectangle origin ) { //Main.mapStyle == 1
			var data = HudMapHelpers.GetMiniMapScreenPosition( origin );
			if( data.Item2 ) {
				return data.Item1;
			}
			return null;
		}
	}
}
