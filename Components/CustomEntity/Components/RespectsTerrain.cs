using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RespectsTerrainEntityComponent : CustomEntityComponent {
		public override void Update( CustomEntity ent ) {
			bool respects_gravity = ent.GetComponentByType<RespectsGravityEntityComponent>() != null;
			Vector2 wet_velocity = ent.velocity * 0.5f;
			bool lava_wet, honey_wet;

			if( !respects_gravity ) {
				this.ApplyZeroGravityMovement( ent );
			}
			this.RefreshLiquidContactStates( ent, out lava_wet, out honey_wet );
			this.ApplyLiquidMovement( ent, lava_wet, honey_wet );
			this.ApplyCollisionMovement( ent, ref wet_velocity );
			this.ApplySlopeDodgeAndConveyorMovement( ent );

			if( ent.wet ) {
				ent.position += wet_velocity;
			} else {
				ent.position += ent.velocity;
			}
		}


		////////////////

		public void RefreshLiquidContactStates( CustomEntity ent, out bool lava_wet, out bool honey_wet ) {
			lava_wet = Collision.LavaCollision( ent.position, ent.width, ent.height );
			if( lava_wet ) {
				ent.lavaWet = true;
			}

			honey_wet = Collision.WetCollision( ent.position, ent.width, ent.height );
			if( Collision.honey ) {
				ent.honeyWet = true;
			}
		}


		public void ApplyLiquidMovement( CustomEntity ent, bool lava_wet, bool honey_wet ) {
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

			// Update wet state
			if( !ent.wet ) {
				ent.lavaWet = false;
				ent.honeyWet = false;
			}
			if( ent.wetCount > 0 ) {
				ent.wetCount -= 1;
			}
		}


		public void ApplyZeroGravityMovement( CustomEntity ent ) {
			ent.velocity.X = ent.velocity.X* 0.95f;

				if((double) ent.velocity.X< 0.01 && (double) ent.velocity.X > -0.01 ) {
				ent.velocity.X = 0f;
			}
			ent.velocity.Y *= 0.95f;

			if( (double)ent.velocity.Y < 0.01 && (double)ent.velocity.Y > -0.01 ) {
				ent.velocity.Y = 0f;
			}
		}


		public void ApplyCollisionMovement( CustomEntity ent, ref Vector2 wet_velocity ) {
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
		}


		public void ApplySlopeDodgeAndConveyorMovement( CustomEntity ent ) {
			Vector4 slope_dodge = Collision.SlopeCollision( ent.position, ent.velocity, ent.width, ent.height );

			ent.position.X = slope_dodge.X;
			ent.position.Y = slope_dodge.Y;
			ent.velocity.X = slope_dodge.Z;
			ent.velocity.Y = slope_dodge.W;

			Collision.StepConveyorBelt( ent, 1f );
		}
	}
}
