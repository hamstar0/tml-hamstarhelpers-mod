using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RespectsGravityEntityComponent : CustomEntityComponent {
		public static RespectsGravityEntityComponent Create() {
			return new RespectsGravityEntityComponent( new PacketProtocolDataConstructorLock( typeof( RespectsGravityEntityComponent ) ) );
		}



		////////////////

		protected RespectsGravityEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		private void UpdateMe( CustomEntity ent ) {
			Entity core = ent.Core;
			float gravity = 0.1f;
			float max_fall_speed = 7f;

			// Halts movement into unload parts of the map
			if( Main.netMode == 1 ) {
				int x = (int)( core.position.X + (float)( core.width / 2 ) ) / 16;
				int y = (int)( core.position.Y + (float)( core.height / 2 ) ) / 16;

				if( x >= 0 && y >= 0 && x < Main.maxTilesX && y < Main.maxTilesY && Main.tile[x, y] == null ) {
					gravity = 0f;
					core.velocity.X = 0f;
					core.velocity.Y = 0f;
				}
			}

			if( core.honeyWet ) {
				gravity = 0.05f;
				max_fall_speed = 3f;
			} else if( core.wet ) {
				max_fall_speed = 5f;
				gravity = 0.08f;
			}

			core.velocity.Y = core.velocity.Y + gravity;
			if( core.velocity.Y > max_fall_speed ) {
				core.velocity.Y = max_fall_speed;
			}

			core.velocity.X = core.velocity.X * 0.95f;

			if( (double)core.velocity.X < 0.1 && (double)core.velocity.X > -0.1 ) {
				core.velocity.X = 0f;
			}
		}


		public override void UpdateSingle( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateServer( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
	}
}
