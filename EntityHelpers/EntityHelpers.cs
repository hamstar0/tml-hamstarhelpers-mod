using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.EntityHelpers {
	public static class EntityHelpers {
		public static void ApplyForce( Entity ent, Vector2 world_pos_from, float force ) {
			Vector2 offset = world_pos_from - ent.position;
			Vector2 force_vector = Vector2.Normalize( offset ) * force;
			ent.velocity += force_vector;
		}
	}
}
