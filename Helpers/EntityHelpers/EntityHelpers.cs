using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.EntityHelpers {
	public static class EntityHelpers {
		public static int GetVanillaSnapshotHash( Entity ent, bool no_context=false ) {
			int hash = ent.active.GetHashCode();

			if( !no_context ) {
				hash ^= ent.position.GetHashCode();
				hash ^= ent.velocity.GetHashCode();
				hash ^= ent.oldPosition.GetHashCode();
				hash ^= ent.oldVelocity.GetHashCode();
				hash ^= ent.oldDirection.GetHashCode();
				hash ^= ent.direction.GetHashCode();
				hash ^= ent.whoAmI.GetHashCode();
				hash ^= ent.wet.GetHashCode();
				hash ^= ent.honeyWet.GetHashCode();
				hash ^= ent.wetCount.GetHashCode();
				hash ^= ent.lavaWet.GetHashCode();
			}
			hash ^= ent.width.GetHashCode();
			hash ^= ent.height.GetHashCode();
			
			return hash;
		}


		public static void ApplyForce( Entity ent, Vector2 world_pos_from, float force ) {
			Vector2 offset = world_pos_from - ent.position;
			Vector2 force_vector = Vector2.Normalize( offset ) * force;
			ent.velocity += force_vector;
		}


		public static bool SimpleLineOfSight( Vector2 position, Entity to ) {
			var trace = new Utils.PerLinePoint( delegate ( int tile_x, int tile_y ) {
				return !TileHelpers.TileHelpers.IsSolid( Framing.GetTileSafely( tile_x, tile_y ) );
			} );
			return Utils.PlotTileLine( position, to.position, 1, trace );
		}
	}
}
