using System;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Libraries.World {
	/// <summary>
	/// Definition for how to add filling to a given chest.
	/// </summary>
	public partial struct ChestFillDefinition {
		/// <summary>
		/// Attempts to fill a given chest according to the current specifications.
		/// </summary>
		/// <param name="chest"></param>
		/// <returns></returns>
		public (bool IsModified, bool Completed) Fill( Chest chest ) {
			return this.FillWith( chest, false );
		}


		////////////////

		/// <summary>
		/// Attempts to remove from a given chest with the scenario to encounter.
		/// </summary>
		/// <param name="chest"></param>
		/// <returns></returns>
		public (bool IsModified, bool Completed) Unfill( Chest chest ) {
			return this.FillWith( chest, true );
		}


		////////////////

		private (bool IsModified, bool Completed) FillWith( Chest chest, bool removeOnly ) {
			if( WorldGen.genRand.NextFloat() >= this.PercentChance ) {
				return (false, false);
			}

			bool isModified = false;
			ChestFillDefinition self = this;

			float maxWeight = this.Any
				.Select( kv => kv.Weight )
				.Sum();
			float weightVal = maxWeight * WorldGen.genRand.NextFloat();

			float countedWeight = 0f;
			foreach( (float weight, ChestFillItemDefinition def) in this.Any ) {
				countedWeight += weight;
				if( countedWeight < weightVal ) {
					continue;
				}

				if( !this.EditNextItem(chest, def, removeOnly) ) {
					return (isModified, false);
				}
				isModified = true;
				break;
			}

			foreach( ChestFillItemDefinition def in this.All ) {
				if( !this.EditNextItem(chest, def, removeOnly) ) {
					return (isModified, false);
				}
				isModified = true;
			}

			return (isModified, true);
		}


		////////////////

		private bool EditNextItem( Chest chest, ChestFillItemDefinition def, bool removeOnly ) {
			int findItemType = removeOnly
				? -1
				: def.ItemType;
			int slot = this.GetSlotIdx( chest, findItemType );
			if( slot == -1 ) {
				return false;
			}

			Item editItem = removeOnly
				? new Item()
				: def.CreateItem();

			chest.item[slot] = editItem;
			return true;
		}

		private int GetSlotIdx( Chest chest, int itemType ) {
			bool isActive = itemType >= 0;

			for( int i = 0; i < chest.item.Length; i++ ) {
				Item item = chest.item[i];
				if( (item?.active ?? false) == isActive ) {
					if( isActive && item.type != itemType ) {
						continue;
					}
					return i;
				}
			}
			return -1;
		}
	}
}
