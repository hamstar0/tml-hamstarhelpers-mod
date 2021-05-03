using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// <summary></summary>
		/// <param name="writer"></param>
		/// <param name="tile"></param>
		/// <param name="forceLiquids"></param>
		public static void ToStream( BinaryWriter writer, Tile tile, bool forceLiquids=false ) {
			BitsByte bits1 = 0;
			BitsByte bits2 = 0;
			byte fColor = 0;
			byte wColor = 0;

			bits1[0] = tile.active();
			bits1[2] = tile.wall > 0;
			bits1[3] = tile.liquid > 0 && Main.netMode == NetmodeID.Server;
			bits1[5] = tile.halfBrick();
			bits1[6] = tile.actuator();
			bits1[7] = tile.inActive();

			bits1[4] = tile.wire();
			bits2[0] = tile.wire2();
			bits2[1] = tile.wire3();
			bits2[7] = tile.wire4();

			if( tile.active() && tile.color() > 0 ) {
				bits2[2] = true;
				fColor = tile.color();
			}
			if( tile.wall > 0 && tile.wallColor() > 0 ) {
				bits2[3] = true;
				wColor = tile.wallColor();
			}
			bits2 += (byte)(tile.slope() << 4);

			writer.Write( bits1 );
			writer.Write( bits2 );

			if( fColor > 0 ) {
				writer.Write( fColor );
			}
			if( wColor > 0 ) {
				writer.Write( wColor );
			}

			if( tile.active() ) {
				writer.Write( tile.type );
				if( Main.tileFrameImportant[(int)tile.type] ) {
					writer.Write( tile.frameX );
					writer.Write( tile.frameY );
				}
			}

			if( tile.wall > 0 ) {
				if( ModNet.AllowVanillaClients ) {
					writer.Write( (byte)tile.wall );
				} else {
					writer.Write( tile.wall );
				}
			}

			if( forceLiquids || (tile.liquid > 0 && Main.netMode == NetmodeID.Server) ) {
				writer.Write( tile.liquid );
				writer.Write( tile.liquidType() );
			}
		}


		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="tile"></param>
		/// <param name="forceLiquids"></param>
		public static void FromStream( BinaryReader reader, ref Tile tile, bool forceLiquids=false ) {
			BitsByte bits1 = 0;
			BitsByte bits2 = 0;

			bool wasActive = tile.active();
			bits1 = reader.ReadByte();
			bits2 = reader.ReadByte();

			tile.active( bits1[0] );
			tile.wall = (byte)(bits1[2] ? 1 : 0);

			bool isLiquid = bits1[3];
			if( forceLiquids || Main.netMode != NetmodeID.Server ) {
				tile.liquid = (byte)(isLiquid ? 1 : 0);
			}

			tile.halfBrick( bits1[5] );
			tile.actuator( bits1[6] );
			tile.inActive( bits1[7] );

			tile.wire( bits1[4] );
			tile.wire2( bits2[0] );
			tile.wire3( bits2[1] );
			tile.wire4( bits2[7] );

			if( bits2[2] ) {
				tile.color( reader.ReadByte() );
			}
			if( bits2[3] ) {
				tile.wallColor( reader.ReadByte() );
			}

			if( tile.active() ) {
				int oldTileType = (int)tile.type;

				tile.type = reader.ReadUInt16();

				if( Main.tileFrameImportant[(int)tile.type] ) {
					tile.frameX = reader.ReadInt16();
					tile.frameY = reader.ReadInt16();
				} else if( !wasActive || (int)tile.type != oldTileType ) {
					tile.frameX = -1;
					tile.frameY = -1;
				}

				byte slope = 0;
				if( bits2[4] ) {
					slope += 1;
				}
				if( bits2[5] ) {
					slope += 2;
				}
				if( bits2[6] ) {
					slope += 4;
				}
				tile.slope( slope );
			}

			if( tile.wall > 0 ) {
				tile.wall = ModNet.AllowVanillaClients
					? reader.ReadByte()
					: reader.ReadUInt16();
			}

			if( isLiquid ) {
				tile.liquid = reader.ReadByte();
				tile.liquidType( (int)reader.ReadByte() );
			}
		}
	}
}
