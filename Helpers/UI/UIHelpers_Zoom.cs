using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;


namespace HamstarHelpers.Helpers.UI {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).
	/// </summary>
	public partial class UIHelpers {
		/// <summary>
		/// Gets the current screen size according to the given scales.
		/// </summary>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <returns></returns>
		public static (float Width, float Height) GetScreenSize( bool? uiZoomState, bool? gameZoomState ) {
			float width = UIHelpers.ApplyZoom( Main.screenWidth, uiZoomState, gameZoomState );
			float height = UIHelpers.ApplyZoom( Main.screenHeight, uiZoomState, gameZoomState );

			return (width, height);
		}

		/// <summary>
		/// Gets the world position and range of the screen, according to the given zoom parameters.
		/// </summary>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <returns></returns>
		public static Rectangle GetWorldFrameOfScreen( bool? uiZoomState, bool? gameZoomState ) {
			float width = UIHelpers.ApplyZoom( Main.screenWidth, uiZoomState, gameZoomState );
			float height = UIHelpers.ApplyZoom( Main.screenHeight, uiZoomState, gameZoomState );

			int scrX = (int)Main.screenPosition.X;
			scrX += (int)( ((float)Main.screenWidth - width) * 0.5f );
			int scrY = (int)Main.screenPosition.Y;
			scrY += (int)( ((float)Main.screenHeight - height) * 0.5f );

			return new Rectangle( scrX, scrY, (int)width, (int)height );
		}


		////////////////

		/// <summary>
		/// Applies the given zoom parameters to the given float.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <returns></returns>
		public static float ApplyZoom( float value, bool? uiZoomState, bool? gameZoomState ) {
			if( uiZoomState.HasValue ) {
				if( uiZoomState.Value ) {
					value /= Main.UIScale;
				} else {
					value *= Main.UIScale;
				}
			}
			if( gameZoomState.HasValue ) {
				if( gameZoomState.Value ) {
					value /= Main.GameZoomTarget;
				} else {
					value *= Main.GameZoomTarget;
				}
			}
			return value;
		}

		/// <summary>
		/// Applies the given zoom parameters to the given Vector2.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it. `false` assumes it has been
		/// removed, and applies it.</param>
		/// <returns></returns>
		public static Vector2 ApplyZoom( Vector2 value, bool? uiZoomState, bool? gameZoomState ) {
			if( uiZoomState.HasValue ) {
				if( uiZoomState.Value ) {
					value /= Main.UIScale;
				} else {
					value *= Main.UIScale;
				}
			}
			if( gameZoomState.HasValue ) {
				if( gameZoomState.Value ) {
					value /= Main.GameZoomTarget;
				} else {
					value *= Main.GameZoomTarget;
				}
			}
			return value;
		}
	}
}
