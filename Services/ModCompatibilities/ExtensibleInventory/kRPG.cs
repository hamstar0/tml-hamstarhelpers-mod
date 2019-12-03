using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
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

			ReflectionHelpers.Get( eiConfig, "BookPositionY", out bookPosY );
			ReflectionHelpers.Get( eiConfig, "PagePositionY", out pagePosY );
			ReflectionHelpers.Get( eiConfig, "PageTicksPositionY", out pageTickPosY );
			ReflectionHelpers.Get( newEIConifg, "BookPositionY", out newBookPosY );
			ReflectionHelpers.Get( newEIConifg, "PagePositionY", out newPagePosY );
			ReflectionHelpers.Get( newEIConifg, "PageTicksPositionY", out newPageTickPosY );
			
			if( bookPosY == newBookPosY ) {
				if( !ReflectionHelpers.Set( eiConfig, "BookPositionY", bookPosY + 112 ) ) {
					LogHelpers.Alert( "Could not set BookPositionY for ExtensibleInventory" );
				}
			}
			if( pagePosY == newPagePosY ) {
				if( !ReflectionHelpers.Set( eiConfig, "PagePositionY", pagePosY + 112 ) ) {
					LogHelpers.Alert( "Could not set PagePositionY for ExtensibleInventory" );
				}
			}
			if( pageTickPosY == newPageTickPosY ) {
				if( !ReflectionHelpers.Set( eiConfig, "PageTicksPositionY", pageTickPosY + 112 ) ) {
					LogHelpers.Alert( "Could not set PageTicksPositionY for ExtensibleInventory" );
				}
			}
		}
	}
}
