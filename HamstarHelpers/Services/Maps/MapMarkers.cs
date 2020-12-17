using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Services.Maps {
	/// <summary>
	/// Provides functions for adding markers to the map.
	/// </summary>
	public partial class MapMarkers : ILoadable {
		/// <summary></summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="label">Must be unique.</param>
		/// <param name="icon"></param>
		/// <param name="scale"></param>
		/// <returns>`false` if a marker of the given label already exists.</returns>
		public static bool AddFullScreenMapMarker( int tileX, int tileY, string label, Texture2D icon, float scale ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			var marker = new MapMarker( label, icon, scale );
			
			if( markers.MarkersPerLabel.ContainsKey(label) ) {
				return false;
			}
			if( markers.Markers.Get2DOrDefault(tileX, tileY)?.ContainsKey(label) ?? false ) {
				return false;
			}

			if( !markers.Markers.ContainsKey(tileX) ) {
				markers.Markers[ tileX ] = new Dictionary<int, IDictionary<string, MapMarker>>();
			}
			if( !markers.Markers[tileX].ContainsKey(tileY) ) {
				markers.Markers[ tileX ][ tileY ] = new Dictionary<string, MapMarker>();
			}

			markers.Markers[tileX][tileY][label] = marker;
			markers.MarkersPerLabel[label] = ( tileX, tileY, marker );

			return true;
		}


		/// <summary></summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static bool RemoveFullScreenMapMarker( string label ) {
			var markers = ModContent.GetInstance<MapMarkers>();

			markers.MarkersPerLabel.Remove( label );

			(int x, int y, MapMarker marker) marker;
			if( !MapMarkers.TryGetFullScreenMapMarker( label, out marker ) ) {
				return false;
			}

			IDictionary<string, MapMarker> markersAt = markers.Markers.Get2DOrDefault( marker.x, marker.y );

			return markersAt?.Remove(label) ?? false;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public static IList<string> GetFullScreenMapMarkerLabels() {
			var markers = ModContent.GetInstance<MapMarkers>();
			return markers.MarkersPerLabel.Keys.ToList();
		}


		/// <summary></summary>
		/// <param name="label"></param>
		/// <param name="markerAt"></param>
		/// <returns></returns>
		public static bool TryGetFullScreenMapMarker( string label, out (int tileX, int tileY, MapMarker marker) markerAt ) {
			var markers = ModContent.GetInstance<MapMarkers>();

			return markers.MarkersPerLabel.TryGetValue( label, out markerAt );
		}


		/// <summary></summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static IDictionary<string, MapMarker> GetFullScreenMapMakersAt( int tileX, int tileY ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			var markersAt = markers.Markers.Get2DOrDefault( tileX, tileY );
			if( markersAt == null ) {
				return null;
			}

			return new Dictionary<string, MapMarker>( markersAt );
		}



		////////////////

		private IDictionary<int, IDictionary<int, IDictionary<string, MapMarker>>> Markers
			= new ConcurrentDictionary<int, IDictionary<int, IDictionary<string, MapMarker>>>();

		private IDictionary<string, (int, int, MapMarker)> MarkersPerLabel
			= new ConcurrentDictionary<string, (int, int, MapMarker)>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddWorldUnloadEachHook( () => {
				this.Markers.Clear();
				this.MarkersPerLabel.Clear();
			} );
		}
	}
}
