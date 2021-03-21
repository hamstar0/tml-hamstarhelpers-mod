using System;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Definition for how to add filling to a given chest.
	/// </summary>
	public struct ChestFillDefinition {
		/// <summary>
		/// Any of these items are evaluated to decide on placement.
		/// </summary>
		public (float Weight, ChestFillItemDefinition ItemDef)[] Any;
		/// <summary>
		/// Each of the following are added until `PercentChance`.
		/// </summary>
		public ChestFillItemDefinition[] All;
		/// <summary>
		/// Chance any or all of this chest's fill definition are avoided.
		/// </summary>
		public float PercentChance;



		////////////////

		/// <summary></summary>
		/// <param name="any"></param>
		/// <param name="all"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition( (float Weight, ChestFillItemDefinition ItemDef)[] any,
					ChestFillItemDefinition[] all,
					float percentChance=1f ) {
			this.Any = any;
			this.All = all;
			this.PercentChance = percentChance;
		}


		////////////////

		/// <summary>
		/// Attempts to fill a given chest with the scenario to encounter.
		/// </summary>
		/// <param name="chest"></param>
		public void Fill( Chest chest ) {
			if( WorldGen.genRand.NextFloat() >= this.PercentChance ) {
				return;
			}

			for( int i = 0; i < chest.item.Length; i++ ) {
				Item item = chest.item[i];
				if( item?.active == true ) {
					continue;
				}

				this.FillAt( chest, i );
				break;
			}
		}

		private void FillAt( Chest chest, int idx ) {
			float maxWeight = this.Any
				.Select(kv=>kv.Weight)
				.Sum();
			float weightVal = maxWeight * WorldGen.genRand.NextFloat();

			float countedWeight = 0f;
			foreach( (float weight, ChestFillItemDefinition def) in this.Any ) {
				countedWeight += weight;
				if( countedWeight < weightVal ) {
					continue;
				}

				this.FillAtWith( chest, idx, def );
				break;
			}

			foreach( ChestFillItemDefinition def in this.All ) {
				this.FillAtWith( chest, idx, def );
				break;
			}
		}

		private void FillAtWith( Chest chest, int idx, ChestFillItemDefinition def ) {
			chest.item[idx] = def.CreateItem();
		}
	}




	/// <summary></summary>
	public struct ChestFillItemDefinition {
		/// <summary></summary>
		public int ItemType;
		/// <summary></summary>
		public int MinQuantity;
		/// <summary></summary>
		public int MaxQuantity;



		////////////////

		/// <summary></summary>
		public ChestFillItemDefinition( int itemType, int min, int max ) {
			this.ItemType = itemType;
			this.MinQuantity = min;
			this.MaxQuantity = max;
		}


		////////////////

		/// <summary></summary>
		public Item CreateItem() {
			var item = new Item();
			item.SetDefaults( this.ItemType, true );
			item.stack = WorldGen.genRand.Next( this.MinQuantity, this.MaxQuantity );
			return item;
		}
	}




	/// <summary></summary>
	public struct ChestTypeDefinition {
		/// <summary></summary>
		public int? TileType;
		/// <summary></summary>
		public int? TileFrame;  //chestTile.frameX / 36



		////////////////

		/// <summary></summary>
		public ChestTypeDefinition( int? tileType, int? tileFrame ) {
			this.TileType = tileType;
			this.TileFrame = tileFrame;
		}


		////////////////

		/// <summary>
		/// Validates if the given coordinates refactor 
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public bool Validate( int tileX, int tileY ) {
			Tile tile = Main.tile[tileX, tileY];
			if( tile?.active() != true ) {
				return false;
			}

			if( this.TileType.HasValue ) {
				if( tile.type != this.TileType.Value ) {
					return false;
				}
			}
			if( this.TileFrame.HasValue ) {
				if( (tile.frameX / 36) == this.TileFrame.Value ) {
					return false;
				}
			}
			return true;
		}
	}
}
