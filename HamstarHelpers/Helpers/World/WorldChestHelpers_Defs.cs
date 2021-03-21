using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Definition for how to add filling to a given chest.
	/// </summary>
	public partial struct ChestFillDefinition {
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
		public ChestFillDefinition(
					(float Weight, ChestFillItemDefinition ItemDef)[] any,
					ChestFillItemDefinition[] all,
					float percentChance=1f ) {
			this.Any = any;
			this.All = all;
			this.PercentChance = percentChance;
		}
		
		/// <summary></summary>
		/// <param name="any"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition( (float Weight, ChestFillItemDefinition ItemDef)[] any, float percentChance=1f ) {
			this.Any = any;
			this.All = new ChestFillItemDefinition[ 0 ];
			this.PercentChance = percentChance;
		}

		/// <summary></summary>
		/// <param name="all"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition( ChestFillItemDefinition[] all, float percentChance=1f ) {
			this.Any = new (float, ChestFillItemDefinition)[ 0 ];
			this.All = all;
			this.PercentChance = percentChance;
		}

		/// <summary></summary>
		/// <param name="single"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition( ChestFillItemDefinition single, float percentChance=1f ) {
			this.Any = new (float, ChestFillItemDefinition)[ 0 ];
			this.All = new ChestFillItemDefinition[] { single };
			this.PercentChance = percentChance;
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

		/// <summary></summary>
		public ChestFillItemDefinition( int itemType ) {
			this.ItemType = itemType;
			this.MinQuantity = 1;
			this.MaxQuantity = 1;
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
		/// <summary>See `TileFrameHelpers.VanillaChestTypeNamesByFrame` (value is `chestTile.frameX / 36`).</summary>
		public (int? TileType, int? TileFrame)[] Tiles;



		////////////////

		/// <summary></summary>
		public ChestTypeDefinition(
					(int? tileType, int? tileFrame)[] tiles,
					bool alsoUndergroundChests=false,
					bool alsoDungeonAndTempleChests=false ) {
			this.Tiles = tiles;

			if( alsoUndergroundChests ) {
				return;
			}

			var addTiles = new List<(int?, int?)>( tiles );

			foreach( (string name, int frame) in TileFrameHelpers.VanillaChestFramesByTypeName ) {
				switch( name ) {
				case "Chest":
					break;
				//case "Locked Gold Chest":
				case "Locked Shadow Chest":
				case "Lihzahrd Chest":
				case "Locked Jungle Chest":
				case "Locked Corruption Chest":
				case "Locked Crimson Chest":
				case "Locked Hallowed Chest":
				case "Locked Frozen Chest":
				case "Locked Green Dungeon Chest":
				case "Locked Pink Dungeon Chest":
				case "Locked Blue Dungeon Chest":
					if( alsoDungeonAndTempleChests ) {
						addTiles.Add( (null, frame) );
					}
					break;
				default:
					addTiles.Add( (null, frame) );
					break;
				}
			}

			this.Tiles = addTiles.ToArray();
		}

		/// <summary></summary>
		public ChestTypeDefinition( int? tileType, int? tileFrame ) {
			this.Tiles = new (int?, int?)[] { (tileType, tileFrame) };
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

			foreach( (int? tileType, int? frame) in this.Tiles ) {
				if( tileType.HasValue ) {
					if( tile.type != tileType.Value ) {
						return false;
					}
				}
				if( frame.HasValue ) {
					if( (tile.frameX / 36) == frame.Value ) {
						return false;
					}
				}
			}
			return true;
		}
	}
}
