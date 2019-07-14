using HamstarHelpers.Components.Errors;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModCompatibilities.ExtensibleInventoryCompat {
	/// <summary>
	/// Defines functions for applying any needed inter-mod compatibility adjustments for the Extensible Inventory mod (if active).
	/// </summary>
	public partial class ExtensibleInventoryCompatibilities {
		/// <summary></summary>
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
