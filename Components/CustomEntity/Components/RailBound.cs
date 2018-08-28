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

		private RailBoundEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) { }

		public RailBoundEntityComponent() {
			this.ConfirmLoad();
		}


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
			bool is_on_rail = false;
			int x_beg = (int)core.position.X / 16;
			int y_beg = ( (int)core.position.Y + core.height - 1 ) / 16;
			int i = 0, j = 0;

			int x_end = Math.Max( ( (int)core.position.X + core.width ) / 16, x_beg + 1 );

			int x_mid = ( x_beg + x_end ) / 2;

			for( int i_off = 0; i_off < 2; i_off++ ) {
				int flip = 0;

				do {
					i = x_mid + i_off;

					for( j = y_beg; j < y_beg + 2; j++ ) {
						if( Main.tile[i, j] != null && Main.tile[i, j].type == TileID.MinecartTrack ) {
							this.SnapToTrack( ent, i, j );
							is_on_rail = true;

							break;
						}
					}

					if( is_on_rail ) { break; }

					if( i_off == 0 ) { break; }
					i_off = -i_off;
				} while( flip++ == 0 );

				if( is_on_rail ) { break; }
			}

			if( !this.IsOnRail && is_on_rail ) {
				this.IsOnRail = true;
				Main.PlaySound( SoundID.Item53 );
			}

			this.IsOnRail = is_on_rail;

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

		public void SnapToTrack( CustomEntity ent, int tile_x, int tile_y ) {
			Entity core = ent.Core;

			core.position.Y = ((tile_y * 16) - core.height) + 8;
			core.velocity.X = 0;
			core.velocity.Y = 0;
		}
	}
}
