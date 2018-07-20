using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RailBoundEntityComponent : CustomEntityComponent {
		[JsonIgnore]
		public bool IsOnRail;


		////////////////

		public override void Update( CustomEntity ent ) {
			bool is_on_rail = false;
			int x_beg = (int)ent.position.X / 16;
			int y_beg = ( (int)ent.position.Y + ent.height ) / 16;

			int x_end = Math.Max( ( (int)ent.position.X + ent.width ) / 16, x_beg + 1 );

			int x_mid = ( x_beg + x_end ) / 2;

			for( int i_off = 0; i_off < 2; i_off++ ) {
				int flip = 0;

				do {
					int i = x_mid + i_off;

					for( int j = y_beg; j < y_beg + 2; j++ ) {
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
		}

		////////////////

		public void SnapToTrack( CustomEntity ent, int tile_x, int tile_y ) {
			ent.position.Y = (tile_y * 16) - ent.height;
			ent.velocity.X = 0;
			ent.velocity.Y = 0;
		}
	}
}
