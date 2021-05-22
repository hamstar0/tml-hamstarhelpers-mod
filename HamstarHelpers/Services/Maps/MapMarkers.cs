using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Services.Maps {
	/// <summary>
	/// Provides functions for adding markers to the map.
	/// </summary>
	public partial class MapMarkers : ILoadable {
		/// <summary>Adds or updates a given map marker by id.</summary>
		/// <param name="id">Must be unique.</param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="icon"></param>
		/// <param name="scale"></param>
		public static void SetFullScreenMapMarker( string id, int tileX, int tileY, Texture2D icon, float scale ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			var marker = new MapMarker( id, icon, scale );
			
			if( markers.MarkersPerLabel.ContainsKey(id) ) {
				MapMarkers.RemoveFullScreenMapMarker( id );
			}

			if( !markers.Markers.ContainsKey(tileX) ) {
				markers.Markers[ tileX ] = new Dictionary<int, IDictionary<string, MapMarker>>();
			}
			if( !markers.Markers[tileX].ContainsKey(tileY) ) {
				markers.Markers[ tileX ][ tileY ] = new Dictionary<string, MapMarker>();
			}

			markers.Markers[ tileX ][ tileY ][ id ] = marker;
			markers.MarkersPerLabel[ id ] = ( tileX, tileY, marker );
		}


		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool RemoveFullScreenMapMarker( string id ) {
			var markers = ModContent.GetInstance<MapMarkers>();

			(int x, int y, MapMarker marker) marker;
			if( !markers.MarkersPerLabel.TryGetValue( id, out marker ) ) {
				return false;
			}

			IDictionary<string, MapMarker> markersByIds = markers.Markers.Get2DOrDefault( marker.x, marker.y );
			
			bool hadMarkerPerLabel = markers.MarkersPerLabel.Remove( id );
			bool hadMarkerById = markersByIds?.Remove(id) ?? false;
			return hadMarkerPerLabel && hadMarkerById;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public static IList<string> GetFullScreenMapMarkerLabels() {
			var markers = ModContent.GetInstance<MapMarkers>();
			return markers.MarkersPerLabel.Keys.ToList();
		}


		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="markerAt"></param>
		/// <returns></returns>
		public static bool TryGetFullScreenMapMarker( string id, out (int tileX, int tileY, MapMarker marker) markerAt ) {
			var markers = ModContent.GetInstance<MapMarkers>();

			return markers.MarkersPerLabel.TryGetValue( id, out markerAt );
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
