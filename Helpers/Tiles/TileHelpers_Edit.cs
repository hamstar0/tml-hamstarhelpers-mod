using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// <summary>
		/// Places a given tile of a given type. Synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="tileType"></param>
		/// <param name="placeStyle"></param>
		/// <param name="muted"></param>
		/// <param name="forced"></param>
		/// <param name="plrWho"></param>
		/// <returns>`true` if tile placement succeeded.</returns>
		public static bool PlaceTileSynced( int tileX, int tileY, int tileType, int placeStyle = 0, bool muted = false, bool forced = false, int plrWho = -1 ) {
			if( tileType == TileID.Containers || tileType == TileID.Containers2 ) {
				int chestIdx = WorldGen.PlaceChest( tileX, tileY, (ushort)tileType, false, placeStyle );
				if( chestIdx == -1 ) { return false; }

				int? chestType = Attributes.TileAttributeHelpers.GetChestTypeCode( tileType );
				if( !chestType.HasValue ) { return false; }

				NetMessage.SendData(
					msgType: MessageID.ChestUpdates,
					remoteClient: -1,
					ignoreClient: -1,
					text: null,
					number: chestType.Value,
					number2: (float)tileX,
					number3: (float)tileY,
					number4: 0f,
					number5: chestIdx,
					number6: tileType,
					number7: 0 );
				int itemSpawn = Chest.chestItemSpawn[placeStyle];//b8 < 100 ?  : TileLoader.GetTile( tileType ).chestDrop;
				if( itemSpawn > 0 ) {
					Item.NewItem( tileX<<4, tileY<<4, 32, 32, itemSpawn, 1, true, 0, false, false );
				}
			} else if( !WorldGen.PlaceTile( tileX, tileY, tileType, muted, forced, plrWho, placeStyle ) ) {
				return false;
			}

			if( Main.netMode != 0 ) {
				NetMessage.SendData( MessageID.TileChange, plrWho, -1, null, 1, (float)tileX, (float)tileY, (float)tileType, placeStyle, 0, 0 );
			}

			if( Main.netMode == 1 ) {
				if( tileType == TileID.Chairs ) {
					NetMessage.SendTileSquare( -1, tileX - 1, tileY - 1, 3, TileChangeType.None );
				} else if( tileType == TileID.Beds || tileType == TileID.Bathtubs ) {
					NetMessage.SendTileSquare( -1, tileX, tileY, 5, TileChangeType.None );
				}
			}

			return true;
		}


		////////////////

		/// <summary>
		/// Kills a given tile. Results are synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="effectOnly">Only a visual effect; tile is not actually killed (nothing to sync).</param>
		/// <param name="dropsItem"></param>
		public static void KillTileSynced( int tileX, int tileY, bool effectOnly, bool dropsItem ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			if( !tile.active() ) {
				if( Main.netMode != 0 ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
				}
				return;
			}

			if( tile.type == TileID.Containers || tile.type == TileID.Containers2 ) {
				int chestIdx = Chest.FindChest( tileX, tileY );
				int chestType = 1;
				if( tile.type == TileID.Containers2 ) {
					chestType = 5;
				}

				if( chestIdx != -1 && Chest.DestroyChest( tileX, tileY ) ) {
					//if( Main.tile[x, y].type >= TileID.Count ) {
					//	number2 = 101;
					//}

					if( Main.netMode != 0 ) {
						NetMessage.SendData(
							msgType: MessageID.ChestUpdates,
							remoteClient: -1,
							ignoreClient: -1,
							text: null,
							number: chestType,
							number2: (float)tileX,
							number3: (float)tileY,
							number4: 0f,
							number5: chestIdx,
							number6: tile.type,
							number7: 0
						);
						NetMessage.SendTileSquare( -1, tileX, tileY, 3, TileChangeType.None );
					}
				}
			}

			WorldGen.KillTile( tileX, tileY, false, effectOnly, !dropsItem );
			Main.tile[ tileX, tileY ]?.active( false );

			if( !effectOnly && Main.netMode != 0 ) {
				int itemDropMode = dropsItem ? 0 : 4;
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, itemDropMode, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
			}
		}


		/// <summary>
		/// Swaps 1x1 tiles. Destructive.
		/// </summary>
		/// <param name="fromTileX"></param>
		/// <param name="fromTileY"></param>
		/// <param name="toTileX"></param>
		/// <param name="toTileY"></param>
		/// <param name="preserveWall"></param>
		/// <param name="preserveWire"></param>
		/// <param name="preserveLiquid"></param>
		public static void Swap1x1Synced(
				int fromTileX,
				int fromTileY,
				int toTileX,
				int toTileY,
				bool preserveWall = false,
				bool preserveWire = false,
				bool preserveLiquid = false ) {
			Tile fromTile = Framing.GetTileSafely( fromTileX, fromTileY );
			Tile fromTileCopy = (Tile)fromTile.Clone();
			Tile toTile = Framing.GetTileSafely( toTileX, toTileY );

			fromTile.CopyFrom( toTile );
			toTile.CopyFrom( fromTileCopy );

			if( preserveWall ) {
				ushort oldToWall = fromTile.wall;
				fromTile.wall = fromTileCopy.wall;
				toTile.wall = oldToWall;
			}

			if( preserveWire ) {
				bool oldToWire = fromTile.wire();
				fromTile.wire( fromTileCopy.wire() );
				toTile.wire( oldToWire );

				bool oldToWire2 = fromTile.wire2();
				fromTile.wire2( fromTileCopy.wire2() );
				toTile.wire2( oldToWire2 );

				bool oldToWire3 = fromTile.wire3();
				fromTile.wire3( fromTileCopy.wire3() );
				toTile.wire3( oldToWire3 );

				bool oldToWire4 = fromTile.wire4();
				fromTile.wire4( fromTileCopy.wire4() );
				toTile.wire4( oldToWire4 );
			}

			if( preserveLiquid ) {
				byte oldToLiquid = fromTile.liquid;
				fromTile.liquid = fromTileCopy.liquid;
				toTile.liquid = oldToLiquid;

				bool oldToHoney = fromTile.honey();
				fromTile.honey( fromTileCopy.honey() );
				toTile.honey( oldToHoney );

				bool oldToLava = fromTile.lava();
				fromTile.lava( fromTileCopy.lava() );
				toTile.lava( oldToLava );
			}

			if( Main.netMode != 0 ) {
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)fromTileX, (float)fromTileY, 0f, 0, 0, 0 );
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)toTileX, (float)toTileY, 0f, 0, 0, 0 );
			}
		}
	}
}
