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
		/// <returns></returns>
		public static int AddToWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef,
					Rectangle? within=null ) {
			int chestsModified = 0;

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

				(bool isModified, bool completed) status = fillDef.Fill( chest);
				if( status.isModified ) {
					chestsModified++;
				}
			}

			return chestsModified;
		}


		/// <summary>
		/// Removes the current item(s) from a given chest type, in a given area (if specified).
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		/// <param name="within"></param>
		/// <returns></returns>
		public static int RemoveFromWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef,
					Rectangle? within = null ) {
			int chestsModified = 0;

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

				(bool isModified, bool completed) status = fillDef.Unfill( chest );
				if( status.isModified ) {
					chestsModified++;
				}
			}

			return chestsModified;
		}
	}
}
