using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles {
	/*public struct TileData {
		public BitsByte Bits1;
		public BitsByte Bits2;
		public byte? FgColor;
		public byte? BgColor;
		public ushort? TileType;
		public ushort? WallType;
		public short? FrameX;
		public short? FrameY;
		public byte? LiquidAmt;
		public byte? LiquidType;
	}




	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// <summary></summary>
		/// <param name="tile"></param>
		/// <param name="forceLiquids"></param>
		/// <returns></returns>
		public static TileData Encode( Tile tile, bool forceLiquids=false ) {
			var data = new TileData();

			data.Bits1[0] = tile.active();
			data.Bits1[2] = tile.wall > 0;
			data.Bits1[3] = tile.liquid > 0 && Main.netMode == NetmodeID.Server;
			data.Bits1[5] = tile.halfBrick();
			data.Bits1[6] = tile.actuator();
			data.Bits1[7] = tile.inActive();

			data.Bits1[4] = tile.wire();
			data.Bits2[0] = tile.wire2();
			data.Bits2[1] = tile.wire3();
			data.Bits2[7] = tile.wire4();

			if( tile.active() && tile.color() > 0 ) {
				data.Bits2[2] = true;
				data.FgColor = tile.color();
			}
			if( tile.wall > 0 && tile.wallColor() > 0 ) {
				data.Bits2[3] = true;
				data.BgColor = tile.wallColor();
			}
			data.Bits2 += (byte)(tile.slope() << 4);

			if( tile.active() ) {
				data.TileType = tile.type;

				if( Main.tileFrameImportant[(int)tile.type] ) {
					data.FrameX = tile.frameX;
					data.FrameY = tile.frameY;
				}
			}

			if( tile.wall > 0 ) {
				data.WallType = tile.wall;
			}

			if( forceLiquids || (tile.liquid > 0 && Main.netMode == NetmodeID.Server) ) {
				data.LiquidAmt = tile.liquid;
				data.LiquidType = tile.liquidType();
			}

			return data;
		}


		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="tile"></param>
		/// <param name="forceLiquids"></param>
		public static void Decode( TileData data, ref Tile tile, bool forceLiquids=false ) {
			bool wasActive = tile.active();

			tile.active( data.Bits1[0] );
			tile.wall = (byte)(data.Bits1[2] ? 1 : 0);

			bool isLiquid = data.Bits1[3];
			if( forceLiquids || Main.netMode != NetmodeID.Server ) {
				tile.liquid = (byte)(isLiquid ? 1 : 0);
			}

			tile.halfBrick( data.Bits1[5] );
			tile.actuator( data.Bits1[6] );
			tile.inActive( data.Bits1[7] );

			tile.wire( data.Bits1[4] );
			tile.wire2( data.Bits2[0] );
			tile.wire3( data.Bits2[1] );
			tile.wire4( data.Bits2[7] );

			if( data.Bits2[2] ) {
				tile.color( data.FgColor.Value );
			}
			if( data.Bits2[3] ) {
				tile.wallColor( data.BgColor.Value );
			}

			if( tile.active() ) {
				int oldTileType = (int)tile.type;

				tile.type = data.TileType.Value;

				if( Main.tileFrameImportant[(int)tile.type] ) {
					tile.frameX = data.FrameX.Value;
					tile.frameY = data.FrameY.Value;
				} else if( !wasActive || (int)tile.type != oldTileType ) {
					tile.frameX = -1;
					tile.frameY = -1;
				}

				byte slope = 0;
				if( data.Bits2[4] ) {
					slope += 1;
				}
				if( data.Bits2[5] ) {
					slope += 2;
				}
				if( data.Bits2[6] ) {
					slope += 4;
				}
				tile.slope( slope );
			}

			if( tile.wall > 0 ) {
				tile.wall = data.WallType.Value;
			}

			if( isLiquid ) {
				tile.liquid = data.LiquidAmt.Value;
				tile.liquidType( data.LiquidType.Value );
			}
		}
	}*/
}
