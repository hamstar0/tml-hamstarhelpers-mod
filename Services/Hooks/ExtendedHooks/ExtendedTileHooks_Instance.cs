using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ObjectData;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for tile-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedTileHooks : ILoadable {
		private static object MyLock = new object();



		////////////////

		/// <summary>
		/// Represents a GlobalTile.KillTile hook binding.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="fail"></param>
		/// <param name="effectOnly"></param>
		/// <param name="noItem"></param>
		public delegate void KillTileDelegate( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem );

		/// <summary>
		/// Represents a GlobalWall.KillWall hook binding.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="fail"></param>
		public delegate void KillWallDelegate( int i, int j, int type, ref bool fail );

		/// <summary>
		/// Represents a GlobalTile.KillTile hook binding specifically for multi-tiles.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		public delegate void KillMultiTileDelegate( int i, int j, int type );



		////////////////

		private Func<bool> OnTick;

		private ISet<KillTileDelegate> OnKillTileHooks = new HashSet<KillTileDelegate>();
		private ISet<KillWallDelegate> OnKillWallHooks = new HashSet<KillWallDelegate>();
		private ISet<KillMultiTileDelegate> OnKillMultiTileHooks = new HashSet<KillMultiTileDelegate>();

		private ISet<int> CheckedTiles = new HashSet<int>();
		private ISet<int> CheckedWalls = new HashSet<int>();



		////////////////

		private ExtendedTileHooks() { }

		////

		/// @private
		void ILoadable.OnModsLoad() {
			this.OnTick = Timers.Timers.MainOnTickGet();
			Main.OnTick += ExtendedTileHooks._Update;
		}

		/// @private
		void ILoadable.OnModsUnload() {
			Main.OnTick -= ExtendedTileHooks._Update;
		}

		/// @private
		void ILoadable.OnPostModsLoad() {
		}


		////////////////

		private static void _Update() {	// <- seems to help avoid Mystery Bugs(TM)
			var eth = TmlHelpers.SafelyGetInstance<ExtendedTileHooks>();
			eth.Update();
		}

		private void Update() {
			if( !this.OnTick() ) {
				return;
			}

			this.CheckedTiles.Clear();
			this.CheckedWalls.Clear();
		}


		////////////////

		internal void CallKillTileHooks( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			int tileToCheck = ( i << 16 ) + j;

			// Important stack overflow failsafe:
			if( this.CheckedTiles.Contains( tileToCheck ) ) {
				return;
			}

			IEnumerable<KillTileDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillTileHooks.ToArray();
			}

			foreach( KillTileDelegate deleg in hooks ) {
				deleg.Invoke( i, j, type, ref fail, ref effectOnly, ref noItem );
			}

			this.CheckedTiles.Add( tileToCheck );
		}

		internal void CallKillWallHooks( int i, int j, int type, ref bool fail ) {
			int wallToCheck = ( i << 16 ) + j;

			// Important stack overflow failsafe:
			if( this.CheckedWalls.Contains( wallToCheck ) ) {
				return;
			}

			IEnumerable<KillWallDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillWallHooks.ToArray();
			}

			foreach( KillWallDelegate deleg in this.OnKillWallHooks ) {
				deleg.Invoke( i, j, type, ref fail );
			}

			this.CheckedWalls.Add( wallToCheck );
		}

		////

		internal bool CanCallKillMultiTileHooks( int i, int j, int type ) {
			Tile tile = Main.tile[i, j];
			if( tile?.active() != true ) {
				return false;
			}

			TileObjectData data = TileObjectData.GetTileData( tile );
			if( data == null || (data.Width == 0 || data.Height == 0) ) {
				return false;
			}

			int frameXOffset = tile.frameX;
			int frameYOffset = tile.frameY;

			if( data.StyleHorizontal ) {
				int frameWidth = data.CoordinateWidth + data.CoordinatePadding;
				frameXOffset = tile.frameX % ( data.Width * frameWidth );
			} else {
				int frameHeight = data.CoordinateHeights[0] + data.CoordinatePadding;
				frameYOffset = tile.frameY % ( data.Height * frameHeight );
			}

			if( frameXOffset != 0 || frameYOffset != 0 ) {
				return false;
			}

			return true;
		}

		internal void CallKillMultiTileHooks( int i, int j, int type ) {
			if( !this.CanCallKillMultiTileHooks(i, j, type) ) {
				return;
			}

			IEnumerable<KillMultiTileDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillMultiTileHooks.ToArray();
			}

			foreach( KillMultiTileDelegate deleg in hooks ) {
				deleg.Invoke( i, j, type );
			}
		}
	}
}
