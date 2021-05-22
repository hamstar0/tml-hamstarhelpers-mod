using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;


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
		[Obsolete( "use `SetFullScreenMapMarker(string label, int tileX, int tileY, Texture2D, float)`", true )]
		public static bool AddFullScreenMapMarker( int tileX, int tileY, string label, Texture2D icon ) {
			bool hasMarker = MapMarkers.TryGetFullScreenMapMarker( label, out _ );
			MapMarkers.SetFullScreenMapMarker( label, tileX, tileY, icon, 0.25f );
			return hasMarker;
		}

		/// @private
		[Obsolete( "use `SetFullScreenMapMarker(string label, int tileX, int tileY, Texture2D, float)`", true )]
		public static bool AddFullScreenMapMarker( int tileX, int tileY, string id, Texture2D icon, float scale ) {
			bool hasMarker = MapMarkers.TryGetFullScreenMapMarker( id, out _ );
			MapMarkers.SetFullScreenMapMarker( id, tileX, tileY, icon, scale );
			return hasMarker;
		}
	}
}
