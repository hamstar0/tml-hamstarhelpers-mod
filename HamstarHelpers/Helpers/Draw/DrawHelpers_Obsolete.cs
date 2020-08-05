using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.Draw;


namespace HamstarHelpers.Helpers.Draw {
	/// <summary>
	/// Assorted static "helper" functions pertaining to drawing to the screen. 
	/// </summary>
	public partial class DrawHelpers {
		/// @private
		[Obsolete("use Services.Draw.AddPostDrawTilesAction", true)]
		public static void AddPostDrawTilesAction( Func<bool> func ) {
			DrawHooks.AddPostDrawTilesHook( func );
		}
	}
}
