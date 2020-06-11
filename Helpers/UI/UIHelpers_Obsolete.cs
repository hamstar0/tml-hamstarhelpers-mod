using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.UI {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).
	/// </summary>
	public partial class UIHelpers {
		/// @private
		[Obsolete( "use ApplyZoom method for screen width and height", true )]
		public static Tuple<int, int> GetScreenSize() {
			int screenWid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screenHei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );

			return Tuple.Create( screenWid, screenHei );
		}

		/// @private
		[Obsolete( "use GetWorldFrameOfScreen(bool?, bool?)", true )]
		public static Rectangle GetWorldFrameOfScreen() {
			float screenWid = (float)Main.screenWidth / Main.GameZoomTarget;
			float screenHei = (float)Main.screenHeight / Main.GameZoomTarget;
			int screenX = (int)Main.screenPosition.X + (int)(((float)Main.screenWidth - screenWid) * 0.5f);
			int screenY = (int)Main.screenPosition.Y + (int)(((float)Main.screenHeight - screenHei) * 0.5f);

			return new Rectangle( screenX, screenY, (int)screenWid, (int)screenHei );
		}

		/// @private
		[Obsolete( "use ApplyZoom(Vector2, bool?, bool?)", true )]
		public static Vector2 ConvertToScreenPosition( Vector2 worldPos ) {
			var frame = UIHelpers.GetWorldFrameOfScreen();
			var screenPos = new Vector2( frame.X, frame.Y );

			return (worldPos - screenPos) * Main.GameZoomTarget;
		}

		/// @private
		[Obsolete( "use ApplyZoom(Vector2, bool?, bool?)", true )]
		public static Vector2 GetWorldMousePosition() {
			Rectangle zoomedScreenFrame = UIHelpers.GetWorldFrameOfScreen();
			var zoomedScreenPos = new Vector2( zoomedScreenFrame.X, zoomedScreenFrame.Y );
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			Vector2 screenMousePos = UIHelpers.ConvertToScreenPosition( mousePos + Main.screenPosition );
			Vector2 worldMousePos = screenMousePos + zoomedScreenPos;

			return worldMousePos;
		}
	}
}
