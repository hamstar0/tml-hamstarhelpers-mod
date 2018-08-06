using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RespectsTerrainEntityComponent : CustomEntityComponent {
		private RespectsTerrainEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) { }

		public RespectsTerrainEntityComponent() {
			this.ConfirmLoad();
		}


		////////////////

		private void UpdateMe( CustomEntity ent ) {
			var core = ent.Core;
			bool respects_gravity = ent.GetComponentByType<RespectsGravityEntityComponent>() != null;
			Vector2 wet_velocity = core.velocity * 0.5f;
			bool lava_wet, honey_wet;

			if( !respects_gravity ) {
				this.ApplyZeroGravityMovement( ent );
			}
			this.RefreshLiquidContactStates( ent, out lava_wet, out honey_wet );
			this.ApplyLiquidMovement( ent, lava_wet, honey_wet );
			this.ApplyCollisionMovement( ent, ref wet_velocity );
			this.ApplySlopeDodgeAndConveyorMovement( ent );

			if( core.wet ) {
				core.position += wet_velocity;
			} else {
				core.position += core.velocity;
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


		////////////////

		public void RefreshLiquidContactStates( CustomEntity ent, out bool lava_wet, out bool honey_wet ) {
			var core = ent.Core;

			lava_wet = Collision.LavaCollision( core.position, core.width, core.height );
			if( lava_wet ) {
				core.lavaWet = true;
			}

			honey_wet = Collision.WetCollision( core.position, core.width, core.height );
			if( Collision.honey ) {
				core.honeyWet = true;
			}
		}


		public void ApplyLiquidMovement( CustomEntity ent, bool lava_wet, bool honey_wet ) {
			var core = ent.Core;

			if( honey_wet ) {
				if( !core.wet ) {
					if( core.wetCount == 0 ) {
						core.wetCount = 20;

						var dust_pos = new Vector2( core.position.X - 6f, core.position.Y + (float)( core.height / 2 ) - 8f );

						if( !lava_wet ) {
							if( core.honeyWet ) {
								for( int i = 0; i < 5; i++ ) {
									int idx = Dust.NewDust( dust_pos, core.width + 12, 24, 152, 0f, 0f, 0, default( Color ), 1f );
									Dust dust = Main.dust[idx];

									dust.velocity.Y = dust.velocity.Y - 1f;
									dust.velocity.X = dust.velocity.X * 2.5f;
									dust.scale = 1.3f;
									dust.alpha = 100;
									dust.noGravity = true;
								}
							} else {
								for( int i = 0; i < 10; i++ ) {
									int idx = Dust.NewDust( dust_pos, core.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default( Color ), 1f );
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
								int idx = Dust.NewDust( dust_pos, core.width + 12, 24, 35, 0f, 0f, 0, default( Color ), 1f );
								Dust dust = Main.dust[idx];

								dust.velocity.Y = dust.velocity.Y - 1.5f;
								dust.velocity.X = dust.velocity.X * 2.5f;
								dust.scale = 1.3f;
								dust.alpha = 100;
								dust.noGravity = true;
							}
						}

						Main.PlaySound( 19, (int)core.position.X, (int)core.position.Y, 1, 1f, 0f );
					}

					core.wet = true;
				}
			} else if( core.wet ) {
				core.wet = false;
			}

			// Update wet state
			if( !core.wet ) {
				core.lavaWet = false;
				core.honeyWet = false;
			}
			if( core.wetCount > 0 ) {
				core.wetCount -= 1;
			}
		}


		public void ApplyZeroGravityMovement( CustomEntity ent ) {
			var core = ent.Core;

			core.velocity.X = core.velocity.X* 0.95f;

				if((double)core.velocity.X< 0.01 && (double)core.velocity.X > -0.01 ) {
				core.velocity.X = 0f;
			}
			core.velocity.Y *= 0.95f;

			if( (double)core.velocity.Y < 0.01 && (double)core.velocity.Y > -0.01 ) {
				core.velocity.Y = 0f;
			}
		}


		public void ApplyCollisionMovement( CustomEntity ent, ref Vector2 wet_velocity ) {
			var core = ent.Core;

			if( core.wet ) {
				Vector2 old_vel = core.velocity;
				core.velocity = Collision.TileCollision( core.position, core.velocity, core.width, core.height, false, false, 1 );

				if( core.velocity.X != old_vel.X ) {
					wet_velocity.X = core.velocity.X;
				}
				if( core.velocity.Y != old_vel.Y ) {
					wet_velocity.Y = core.velocity.Y;
				}
			} else {
				core.velocity = Collision.TileCollision( core.position, core.velocity, core.width, core.height, false, false, 1 );
			}
		}


		public void ApplySlopeDodgeAndConveyorMovement( CustomEntity ent ) {
			var core = ent.Core;
			Vector4 slope_dodge = Collision.SlopeCollision( core.position, core.velocity, core.width, core.height );

			core.position.X = slope_dodge.X;
			core.position.Y = slope_dodge.Y;
			core.velocity.X = slope_dodge.Z;
			core.velocity.Y = slope_dodge.W;

			try {
				Collision.StepConveyorBelt( core, 1f );
			} catch( Exception ) { }
		}
	}
}
