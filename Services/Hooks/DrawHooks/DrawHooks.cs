using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.Hooks.Draw {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for conveniently plugging into existing drawing layers.
	/// </summary>
	public partial class DrawHooks {
		/// <summary>
		/// Adds a function to call when ModWorld.PostTileDraw() is called. Main.SpriteBatch is 'Begun'.
		/// </summary>
		/// <param name="func">Returns `false` to stop being called.</param>
		public static void AddPostDrawTilesHook( Func<bool> func ) {
			var dh = ModContent.GetInstance<DrawHooksInternal>();
			dh.PostDrawTilesHooks.Add( func );
		}
	}
}
