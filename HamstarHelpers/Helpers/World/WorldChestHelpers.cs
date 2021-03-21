using System;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world's chests.
	/// </summary>
	public partial class WorldChestHelpers {
		/// <summary>
		/// Implants a given item or items into a given chest.
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		public static void AddToWorldChests( ChestFillDefinition fillDef, ChestTypeDefinition chestDef ) {
			foreach( Chest chest in Main.chest ) {
				if( chest.x < 0 || chest.y < 0 ) {
					continue;
				}

				if( !chestDef.Validate(chest.x, chest.y) ) {
					continue;
				}

				fillDef.Fill( chest);
			}
		}
	}
}
