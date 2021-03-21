using System;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.World {
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

			//

			Item CreateItem( ChestFillItemDefinition def ) {
				return !removeOnly
					? def.CreateItem()
					: new Item();
			}

			//

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

				int idx = this.GetSlotIdx( chest, def.ItemType );
				if( idx == -1 ) {
					return (isModified, false);
				}

				chest.item[idx] = CreateItem( def );
				isModified = true;
				break;
			}

			foreach( ChestFillItemDefinition def in this.All ) {
				int idx = this.GetSlotIdx( chest, def.ItemType );
				if( idx == -1 ) {
					return (isModified, false);
				}

				chest.item[idx] = CreateItem( def );
				isModified = true;
			}

			return (isModified, true);
		}


		////////////////

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
