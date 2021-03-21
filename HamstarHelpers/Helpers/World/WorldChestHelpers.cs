using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world's chests.
	/// </summary>
	public partial class WorldChestHelpers {
		/// <summary>
		/// Implants the current item(s) from a given chest type, in a given area (if specified).
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		/// <param name="within"></param>
		public static void AddToWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef,
					Rectangle? within=null ) {
			foreach( Chest chest in Main.chest ) {
				if( within.HasValue ) {
					if( !within.Value.Contains(chest.x, chest.y) ) {
						continue;
					}
				} else if( chest.x < 0 || chest.y < 0 ) {
					continue;
				}

				if( !chestDef.Validate(chest.x, chest.y) ) {
					continue;
				}

				fillDef.Fill( chest);
			}
		}


		/// <summary>
		/// Removes the current item(s) from a given chest type, in a given area (if specified).
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		/// <param name="within"></param>
		public static void RemoveFromWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef,
					Rectangle? within = null ) {
			foreach( Chest chest in Main.chest ) {
				if( within.HasValue ) {
					if( !within.Value.Contains( chest.x, chest.y ) ) {
						continue;
					}
				} else if( chest.x < 0 || chest.y < 0 ) {
					continue;
				}

				if( !chestDef.Validate( chest.x, chest.y ) ) {
					continue;
				}

				fillDef.Unfill( chest );
			}
		}
	}
}
