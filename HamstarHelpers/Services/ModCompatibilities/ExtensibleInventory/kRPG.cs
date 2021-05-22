using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModCompatibilities.ExtensibleInventoryCompat {
	/// <summary>
	/// Defines functions for applying any needed inter-mod compatibility adjustments for the Extensible Inventory mod (if active).
	/// </summary>
	public partial class ExtensibleInventoryCompatibilities {
		private static void kRPGCompat() {  //TODO use weak reference somehow
			Mod eiMod = ModLoader.GetMod( "ExtensibleInventory" );
			object eiConfig = eiMod.Call( "GetModSettings" );
			object newEIConifg = Activator.CreateInstance(
				eiMod.GetType().AssemblyQualifiedName,
				"ExtensibleInventoryConfigData"
			);

			float bookPosY, pagePosY, pageTickPosY,
				newBookPosY, newPagePosY, newPageTickPosY;

			ReflectionLibraries.Get( eiConfig, "BookPositionY", out bookPosY );
			ReflectionLibraries.Get( eiConfig, "PagePositionY", out pagePosY );
			ReflectionLibraries.Get( eiConfig, "PageTicksPositionY", out pageTickPosY );
			ReflectionLibraries.Get( newEIConifg, "BookPositionY", out newBookPosY );
			ReflectionLibraries.Get( newEIConifg, "PagePositionY", out newPagePosY );
			ReflectionLibraries.Get( newEIConifg, "PageTicksPositionY", out newPageTickPosY );
			
			if( bookPosY == newBookPosY ) {
				if( !ReflectionLibraries.Set( eiConfig, "BookPositionY", bookPosY + 112 ) ) {
					LogLibraries.Alert( "Could not set BookPositionY for ExtensibleInventory" );
				}
			}
			if( pagePosY == newPagePosY ) {
				if( !ReflectionLibraries.Set( eiConfig, "PagePositionY", pagePosY + 112 ) ) {
					LogLibraries.Alert( "Could not set PagePositionY for ExtensibleInventory" );
				}
			}
			if( pageTickPosY == newPageTickPosY ) {
				if( !ReflectionLibraries.Set( eiConfig, "PageTicksPositionY", pageTickPosY + 112 ) ) {
					LogLibraries.Alert( "Could not set PageTicksPositionY for ExtensibleInventory" );
				}
			}
		}
	}
}
