using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Properties {
	public class RespectsTerrainEntityProperty : CustomEntityProperty {
		public override void Update( CustomEntity ent ) {
			bool respects_gravity = ent.GetPropertyByName( "RespectsGravityEntityProperty" ) != null;
			Vector2 wet_velocity = ent.velocity * 0.5f;

			if( !respects_gravity ) {
				ent.velocity.X = ent.velocity.X * 0.95f;

				if( (double)ent.velocity.X < 0.1 && (double)ent.velocity.X > -0.1 ) {
					ent.velocity.X = 0f;
				}
				ent.velocity.Y = ent.velocity.Y * 0.95f;

				if( (double)ent.velocity.Y < 0.1 && (double)ent.velocity.Y > -0.1 ) {
					ent.velocity.Y = 0f;
				}
			}

			bool lava_wet = Collision.LavaCollision( ent.position, ent.width, ent.height );
			if( lava_wet ) {
				ent.lavaWet = true;
			}

			bool honey_wet = Collision.WetCollision( ent.position, ent.width, ent.height );
			if( Collision.honey ) {
				ent.honeyWet = true;
			}

			if( honey_wet ) {
				if( !ent.wet ) {
					if( ent.wetCount == 0 ) {
						ent.wetCount = 20;

						var dust_pos = new Vector2( ent.position.X - 6f, ent.position.Y + (float)( ent.height / 2 ) - 8f );

						if( !lava_wet ) {
							if( ent.honeyWet ) {
								for( int i = 0; i < 5; i++ ) {
									int idx = Dust.NewDust( dust_pos, ent.width + 12, 24, 152, 0f, 0f, 0, default( Color ), 1f );
									Dust dust = Main.dust[idx];

									dust.velocity.Y = dust.velocity.Y - 1f;
									dust.velocity.X = dust.velocity.X * 2.5f;
									dust.scale = 1.3f;
									dust.alpha = 100;
									dust.noGravity = true;
								}
							} else {
								for( int i = 0; i < 10; i++ ) {
									int idx = Dust.NewDust( dust_pos, ent.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default( Color ), 1f );
									Dust dust = Main.dust[idx];

									dust.velocity.Y = dust.velocity.Y - 4f;
									dust.velocity.X = dust.velocity.X * 2.5f;
									dust.scale *= 0.8f;
									dust.alpha = 100;
									dust.noGravity = true;
								}
							}
						} else {
							for( int i = 0; i < 5; i++ ) {
								int idx = Dust.NewDust( dust_pos, ent.width + 12, 24, 35, 0f, 0f, 0, default( Color ), 1f );
								Dust dust = Main.dust[idx];

								dust.velocity.Y = dust.velocity.Y - 1.5f;
								dust.velocity.X = dust.velocity.X * 2.5f;
								dust.scale = 1.3f;
								dust.alpha = 100;
								dust.noGravity = true;
							}
						}

						Main.PlaySound( 19, (int)ent.position.X, (int)ent.position.Y, 1, 1f, 0f );
					}

					ent.wet = true;
				}
			} else if( ent.wet ) {
				ent.wet = false;
			}

			if( !ent.wet ) {
				ent.lavaWet = false;
				ent.honeyWet = false;
			}
			if( ent.wetCount > 0 ) {
				ent.wetCount -= 1;
			}

			if( ent.wet ) {
				Vector2 old_vel = ent.velocity;
				ent.velocity = Collision.TileCollision( ent.position, ent.velocity, ent.width, ent.height, false, false, 1 );

				if( ent.velocity.X != old_vel.X ) {
					wet_velocity.X = ent.velocity.X;
				}
				if( ent.velocity.Y != old_vel.Y ) {
					wet_velocity.Y = ent.velocity.Y;
				}
			} else {
				ent.velocity = Collision.TileCollision( ent.position, ent.velocity, ent.width, ent.height, false, false, 1 );
			}

			Vector4 slope_dodge = Collision.SlopeCollision( ent.position, ent.velocity, ent.width, ent.height, respects_gravity ? 1 : 0, false );

			ent.position.X = slope_dodge.X;
			ent.position.Y = slope_dodge.Y;
			ent.velocity.X = slope_dodge.Z;
			ent.velocity.Y = slope_dodge.W;

			Collision.StepConveyorBelt( ent, 1f );
			
			if( ent.wet ) {
				ent.position += wet_velocity;
			} else {
				ent.position += ent.velocity;
			}
		}
	}
}
