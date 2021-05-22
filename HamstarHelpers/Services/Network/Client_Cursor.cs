using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
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
		/// <returns>`true` if loop has begun (and wasn't already).</returns>
		public static bool StartBroadcastingMyCursorPosition() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not a client." );
			}

//LogHelpers.LogOnce( "UUU StartBroadcastingMyCursorPosition - "+string.Join("\n  ", DebugHelpers.GetContextSlice()) );
			string timerName = "cursor_broadcast_" + Main.myPlayer;
			if( Timers.Timers.GetTimerTickDuration(timerName) > 0 ) {
				return false;
			}

			Timers.Timers.SetTimer( timerName, 15, false, () => {
				bool canBroadcast = Main.player[Main.myPlayer].active
					&& LoadLibraries.IsWorldBeingPlayed();

				if( canBroadcast ) {
					CursorPositionProtocol.BroadcastCursorIf();
				}

				return canBroadcast;
			} );

			return true;
		}


		/// <summary>
		/// Ends the current cursor position broadcast loop.
		/// </summary>
		public static void StopBroadcastingMyCursorPosition() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not a client." );
			}

			string timerName = "cursor_broadcast_" + Main.myPlayer;

			Timers.Timers.UnsetTimer( timerName );
		}
	}
}
