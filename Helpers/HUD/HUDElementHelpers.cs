using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to in-game HUD elements. 
	/// </summary>
	public class HUDElementHelpers {
		/// <summary>
		/// Top left screen position of player's health.
		/// </summary>
		/// <returns></returns>
		public static Vector2 GetVanillaHealthDisplayScreenPosition() =>
			new Vector2( Main.screenWidth - 300, 86 );


		/// <summary>
		/// Top left screen position of player's accessories.
		/// </summary>
		/// <returns></returns>
		public static Vector2 GetVanillaAccessorySlotScreenPosition( int slot ) {
			var pos = new Vector2( Main.screenWidth - 92, 318 );
			pos.Y += ( 48 * slot );

			if( Main.mapStyle == 1 ) {
				pos.Y += Main.screenHeight - 600;
				//pos.Y += 104;
			}

			return pos;
		}

		////

		/// <summary>
		/// Gets all buff icon rectangles by buff index.
		/// </summary>
		/// <param name="applyGameZoom">Factors game zoom into position calculations.</param>
		/// <returns></returns>
		public static IDictionary<int, Rectangle> GetVanillaBuffIconRectanglesByPosition( bool applyGameZoom ) {
			var rects = new Dictionary<int, Rectangle>();
			var player = Main.LocalPlayer;
			int dim = 32;
			Vector2 screenOffset = Vector2.Zero;

			if( applyGameZoom ) {
				var worldFrame = UIHelpers.GetWorldFrameOfScreen();
				screenOffset.X = worldFrame.X - Main.screenPosition.X;
				screenOffset.Y = worldFrame.Y - Main.screenPosition.Y;
			}

			//if( scaleType == InterfaceScaleType.UI ) {
			//if( scaleType == InterfaceScaleType.Game ) {
			if( applyGameZoom ) {
				dim = (int)( ((float)dim * Main.UIScale) / Main.GameZoomTarget );
			}

			for( int i = 0; i < player.buffType.Length; i++ ) {
				if( player.buffType[i] <= 0 ) { continue; }

				int x = 32 + ((i % 11) * 38);
				int y = 76 + (50 * (i / 11));

				//if( scaleType == InterfaceScaleType.UI ) {
				//if( scaleType == InterfaceScaleType.Game ) {
				if( applyGameZoom ) {
					x = (int)((((float)x * Main.UIScale) / Main.GameZoomTarget) + screenOffset.X);
					y = (int)((((float)y * Main.UIScale) / Main.GameZoomTarget) + screenOffset.Y);
				}

				rects[i] = new Rectangle( x, y, dim, dim );
			}

			return rects;
		}
	}
}
