using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Components.CustomEntity.Properties {
	public class RespectsGravityEntityProperty : CustomEntityProperty {
		public override void Update( CustomEntity ent ) {
			float gravity = 0.1f;
			float max_fall_speed = 7f;

			// Halts movement into unload parts of the map
			if( Main.netMode == 1 ) {
				int x = (int)( ent.position.X + (float)( ent.width / 2 ) ) / 16;
				int y = (int)( ent.position.Y + (float)( ent.height / 2 ) ) / 16;

				if( x >= 0 && y >= 0 && x < Main.maxTilesX && y < Main.maxTilesY && Main.tile[x, y] == null ) {
					gravity = 0f;
					ent.velocity.X = 0f;
					ent.velocity.Y = 0f;
				}
			}

			if( ent.honeyWet ) {
				gravity = 0.05f;
				max_fall_speed = 3f;
			} else if( ent.wet ) {
				max_fall_speed = 5f;
				gravity = 0.08f;
			}
			
			ent.velocity.Y = ent.velocity.Y + gravity;
			if( ent.velocity.Y > max_fall_speed ) {
				ent.velocity.Y = max_fall_speed;
			}

			ent.velocity.X = ent.velocity.X * 0.95f;

			if( (double)ent.velocity.X < 0.1 && (double)ent.velocity.X > -0.1 ) {
				ent.velocity.X = 0f;
			}
		}
	}
}
