﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Services.Maps;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void PostDrawFullscreenMap( ref string mouseText ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			if( markers == null ) {
				return;
			}

			foreach( string label in MapMarkers.GetFullScreenMapMarkerLabels() ) {
				(int x, int y, MapMarker marker) info = MapMarkers.GetFullScreenMapMarker( label );

				this.DrawFullScreenMapMarker( info.x, info.y, info.marker );
			}
		}


		////

		private void DrawFullScreenMapMarker( int tileX, int tileY, MapMarker marker ) {
			Texture2D tex = marker.Icon;
			Vector2 origin = new Vector2( tex.Width / 2, tex.Height / 2 );

			var mapPos = HUDMapHelpers.GetFullMapPositionAsScreenPosition( new Vector2(tileX*16, tileY*16) );
			if( !mapPos.IsOnScreen ) {
				return;
			}

			Main.spriteBatch.Draw(
				texture: tex,
				position: mapPos.ScreenPosition,
				sourceRectangle: null,
				color: Color.White,
				rotation: 0f,
				origin: origin,
				scale: 0.25f,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}
	}
}
