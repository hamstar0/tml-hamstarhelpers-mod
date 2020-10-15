using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Services.Maps {
	/// <summary></summary>
	public class MapMarker {
		/// <summary></summary>
		public string Label { get; private set; }

		/// <summary></summary>
		public Texture2D Icon { get; private set; }



		////////////////

		/// <summary></summary>
		public MapMarker( string label, Texture2D icon ) {
			this.Label = label;
			this.Icon = icon;
		}

		/// <summary></summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return this.Label.GetHashCode() + this.Icon.GetHashCode();
		}

		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj ) {
			if( obj == null ) {
				return false;
			}
			if( obj.GetType() != typeof(MapMarker) ) {
				return false;
			}
			return obj.GetHashCode() == this.GetHashCode();
		}
	}




	/// <summary>
	/// Provides functions for adding markers to the map.
	/// </summary>
	public class MapMarkers : ILoadable {
		/// <summary></summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="label">Must be unique.</param>
		/// <param name="icon"></param>
		/// <returns>`false` if a matching marker already exists at the given location.</returns>
		public static bool AddFullScreenMapMarker( int tileX, int tileY, string label, Texture2D icon ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			var marker = new MapMarker( label, icon );
			
			if( markers.MarkersPerLabel.ContainsKey(label) ) {
				return false;
			}
			if( markers.Markers.Get2DOrDefault(tileX, tileY).ContainsKey(label) ) {
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
			(int x, int y, MapMarker marker) marker = MapMarkers.GetFullScreenMapMarker( label );

			markers.MarkersPerLabel.Remove( label );
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
		/// <returns></returns>
		public static (int tileX, int tileY, MapMarker marker) GetFullScreenMapMarker( string label ) {
			var markers = ModContent.GetInstance<MapMarkers>();
			return markers.MarkersPerLabel.GetOrDefault( label );
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

		void ILoadable.OnPostModsLoad() { }
	}
}
