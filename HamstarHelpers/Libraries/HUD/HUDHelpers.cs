using System;
using Terraria;


namespace HamstarHelpers.Libraries.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to general HUD. 
	/// </summary>
	public partial class HUDHelpers {
		/// <summary>
		/// Indicates if `Main.LocalPlayer.mouseInterface` was true at the end of the previous tick.
		/// </summary>
		public static bool IsMouseInterfacingWithUI => ModHelpersMod.Instance.MouseInterface;
	}
}
