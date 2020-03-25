using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Internals.NetProtocols;


namespace HamstarHelpers.Services.Network {
	public class Client {
		public static IReadOnlyDictionary<int, (short X, short Y)> LastKnownCursorPositions;
		internal static IDictionary<int, (short X, short Y)> _LastKnownCursorPositions = new Dictionary<int, (short X, short Y)>();



		////////////////

		static Client() {
			Client.LastKnownCursorPositions = new ReadOnlyDictionary<int, (short X, short Y)>( Client._LastKnownCursorPositions );
		}


		////////////////

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
	}
}
