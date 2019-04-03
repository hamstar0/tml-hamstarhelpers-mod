using ExtensibleInventory;
using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Services.ModCompatibilities.ExtensibleInventory {
	public partial class ExtensibleInventoryCompatibilities {
		private static string kRPGCompat() {
			var eiConfig = ExtensibleInventoryAPI.GetModSettings();
			var newConfig = new ExtensibleInventoryConfigData();
			
			if( eiConfig.BookPositionY == newConfig.BookPositionY ) {
				eiConfig.BookPositionY += 112;
			}
			if( eiConfig.PagePositionY == newConfig.PagePositionY ) {
				eiConfig.PagePositionY += 112;
			}
			if( eiConfig.PageTicksPositionY == newConfig.PageTicksPositionY ) {
				eiConfig.PageTicksPositionY += 112;
			}

			return null;
		}
	}
}
