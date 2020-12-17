using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Services.Maps {
	/// <summary>
	/// Provides functions for adding markers to the map.
	/// </summary>
	public partial class MapMarkers : ILoadable {
		/// @private
		[Obsolete("use `GetFullScreenMapMarker(string, out (int, int, MapMarker))`", true)]
		public static (int tileX, int tileY, MapMarker marker) GetFullScreenMapMarker( string label ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			return markers.MarkersPerLabel.GetOrDefault( label );
		}

		/// @private
		[Obsolete( "use `AddFullScreenMapMarker(int tileX, int tileY, string label, Texture2D, float)`", true )]
		public static bool AddFullScreenMapMarker( int tileX, int tileY, string label, Texture2D icon ) {
			return MapMarkers.AddFullScreenMapMarker( tileX, tileY, label, icon, 0.25f );
		}
	}
}
