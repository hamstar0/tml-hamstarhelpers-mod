using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	/// <summary>
	/// Provides a way to delay the onset of a given action by a set amount of ticks. As a secondary function,
	/// MainOnTickGet() provides a way to use Main.OnTick for running background tasks at 60FPS.
	/// </summary>
	public partial class Timers {
		/// @private
		[Obsolete( "use SetTimer(string, int, bool, Func<bool>)", true )]
		public static void SetTimer( string name, int tickDuration, Func<bool> action ) {
			Timers.SetTimer( name, tickDuration, true, action );
		}
	}
}
