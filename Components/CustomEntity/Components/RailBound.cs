using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RailBoundEntityComponent : CustomEntityComponent {
		[PacketProtocolIgnore]
		[JsonIgnore]
		public bool IsOnRail;


		////////////////

		protected RailBoundEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		protected override void OnInitialize() { }


		////////////////

		public override void UpdateSingle( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateServer( CustomEntity ent ) {
			this.UpdateMe( ent );
		}


		private void UpdateMe( CustomEntity ent ) {
			Entity core = ent.Core;
			bool isOnRail = false;
			int xBeg = (int)core.position.X / 16;
			int yBeg = ( (int)core.position.Y + core.height - 1 ) / 16;
			int i = 0, j = 0;

			int xEnd = Math.Max( ( (int)core.position.X + core.width ) / 16, xBeg + 1 );

			int xMid = ( xBeg + xEnd ) / 2;

			for( int iOff = 0; iOff < 2; iOff++ ) {
				int flip = 0;

				do {
					i = xMid + iOff;

					for( j = yBeg; j < yBeg + 2; j++ ) {
						if( Main.tile[i, j] != null && Main.tile[i, j].type == TileID.MinecartTrack ) {
							this.SnapToTrack( ent, i, j );
							isOnRail = true;

							break;
						}
					}

					if( isOnRail ) { break; }

					if( iOff == 0 ) { break; }
					iOff = -iOff;
				} while( flip++ == 0 );

				if( isOnRail ) { break; }
			}

			if( !this.IsOnRail && isOnRail ) {
				this.IsOnRail = true;
				Main.PlaySound( SoundID.Item53 );
			}

			this.IsOnRail = isOnRail;

			if( this.IsOnRail ) {
				if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
					int idx = Dust.NewDust( new Vector2( i * 16 + 8, j * 16 + 8 ), 0, 0, 1,
						Main.rand.NextFloat() * 0.2f - 0.1f,
						Main.rand.NextFloat() * 0.2f - 0.1f );
					Main.dust[idx].noGravity = true;
					Main.dust[idx].shader = GameShaders.Armor.GetSecondaryShader( 97, Main.LocalPlayer );
				}
			}
		}


		////////////////

		public void SnapToTrack( CustomEntity ent, int tileX, int tileY ) {
			Entity core = ent.Core;

			core.position.Y = ((tileY * 16) - core.height) + 8;
			core.velocity.X = 0;
			core.velocity.Y = 0;
		}
	}
}
