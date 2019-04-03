using HamstarHelpers.Components.Errors;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModCompatibilities.ExtensibleInventory {
	public partial class ExtensibleInventoryCompatibilities {
		public static void ApplyCompats() {
			Mod eiMod = ModLoader.GetMod( "ExtensibleInventory" );
			if( eiMod == null ) {
				throw new HamstarException( "Missing Extensible Inventory mod." );
			}
			
			if( ModLoader.GetMod( "kRPG" ) != null ) {
				ExtensibleInventoryCompatibilities.kRPGCompat();
			}
		}
	}
}
