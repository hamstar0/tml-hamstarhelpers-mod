using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModCompatibilities.ExtensibleInventoryCompat {
	public partial class ExtensibleInventoryCompatibilities {
		private static void kRPGCompat() {  //TODO use weak reference somehow
			Mod eiMod = ModLoader.GetMod( "ExtensibleInventory" );
			object eiConfig = eiMod.Call( "GetModSettings" );
			object newEIConifg = Activator.CreateInstance( eiMod.GetType().AssemblyQualifiedName, "ExtensibleInventoryConfigData" );

			float bookPosY, pagePosY, pageTickPosY,
				newBookPosY, newPagePosY, newPageTickPosY;

			ReflectionHelpers.Get( eiConfig, "BookPositionY", out bookPosY );
			ReflectionHelpers.Get( eiConfig, "PagePositionY", out pagePosY );
			ReflectionHelpers.Get( eiConfig, "PageTicksPositionY", out pageTickPosY );
			ReflectionHelpers.Get( newEIConifg, "BookPositionY", out newBookPosY );
			ReflectionHelpers.Get( newEIConifg, "PagePositionY", out newPagePosY );
			ReflectionHelpers.Get( newEIConifg, "PageTicksPositionY", out newPageTickPosY );
			
			if( bookPosY == newBookPosY ) {
				ReflectionHelpers.Set( eiConfig, "BookPositionY", bookPosY + 112 );
			}
			if( pagePosY == newPagePosY ) {
				ReflectionHelpers.Set( eiConfig, "PagePositionY", pagePosY + 112 );
			}
			if( pageTickPosY == newPageTickPosY ) {
				ReflectionHelpers.Set( eiConfig, "PageTicksPositionY", pageTickPosY + 112 );
			}
		}
	}
}
