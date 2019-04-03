using HamstarHelpers.Components.Errors;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModCompatibilities {
	public partial class ModCompatibilities {
		public static void WithExtensibleInventory() {
			Mod krpgMod = ModLoader.GetMod( "kRPG" );
			Mod eiMod = ModLoader.GetMod( "ExtensibleInventory" );
			if( krpgMod == null || eiMod == null ) {
				throw new HamstarException("Missing required mods.");
			}

			//eiMod.
		}
	}
}
