using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Draw {
	/// <summary>
	/// Assorted static "helper" functions pertaining to drawing to the screen. 
	/// </summary>
	public partial class DrawHelpers {
		/// <summary>
		/// Adds a function to call when ModWorld.PostTileDraw() is called. Main.SpriteBatch is 'begun'.
		/// </summary>
		/// <param name="func">Returns `false` to stop being called.</param>
		public static void AddPostDrawTilesAction( Func<bool> func ) {
			var dh = ModContent.GetInstance<DrawHelpersInternal>();
			dh.PostDrawTilesActions.Add( func );
		}
	}
}
