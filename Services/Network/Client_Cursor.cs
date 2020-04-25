using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Internals.NetProtocols;


namespace HamstarHelpers.Services.Network {
	/// <summary>
	/// Supplies assorted server informations and tools.
	/// </summary>
	public partial class Client : ILoadable {
		/// <summary>
		/// Shows last known positions of each player's mouse cursor. Must be activated via. StartBroadcastingMyCursorPosition(), first.
		/// </summary>
		public static IReadOnlyDictionary<int, (short X, short Y)> LastKnownCursorPositions;

		internal static IDictionary<int, (short X, short Y)> _LastKnownCursorPositions = new Dictionary<int, (short X, short Y)>();



		////////////////

		static Client() {
			Client.LastKnownCursorPositions = new ReadOnlyDictionary<int, (short X, short Y)>( Client._LastKnownCursorPositions );
		}


		////////////////

		/// <summary>
		/// Begins a broadcast loop (via. Timers) every 1/4 second to tell everyone where the current player's cursor is located.
		/// </summary>
		/// <returns>`true` if loop not already running.</returns>
		public static bool StartBroadcastingMyCursorPosition() {
			if( Main.netMode != 1 ) { throw new ModHelpersException( "Not a client." ); }

			string timerName = "cursor_broadcast_" + Main.myPlayer;
			if( Timers.Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			Timers.Timers.SetTimer( timerName, 15, false, () => {
				return Main.player[ Main.myPlayer ].active
					&& LoadHelpers.IsWorldBeingPlayed()
					&& CursorPositionProtocol.BroadcastCursor();
			} );
			return true;
		}


		/// <summary>
		/// Ends the current cursor position broadcast loop.
		/// </summary>
		public static void StopBroadcastingMyCursorPosition() {
			if( Main.netMode != 1 ) { throw new ModHelpersException( "Not a client." ); }

			string timerName = "cursor_broadcast_" + Main.myPlayer;

			Timers.Timers.UnsetTimer( timerName );
		}
	}
}
