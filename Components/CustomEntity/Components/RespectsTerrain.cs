using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class RespectsTerrainEntityComponent : CustomEntityComponent {
		public static RespectsTerrainEntityComponent CreateRespectsTerrainEntityComponent() {
			return (RespectsTerrainEntityComponent)PacketProtocolData.CreateRawUninitialized( typeof(RespectsTerrainEntityComponent) );
		}



		////////////////

		protected RespectsTerrainEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		public override void OnInitialize() { }


		////////////////

		private void UpdateMe( CustomEntity ent ) {
			var core = ent.Core;
			bool respectsGravity = ent.GetComponentByType<RespectsGravityEntityComponent>() != null;
			Vector2 wetVelocity = core.velocity * 0.5f;
			bool lavaWet, honeyWet;

			if( !respectsGravity ) {
				this.ApplyZeroGravityMovement( ent );
			}
			this.RefreshLiquidContactStates( ent, out lavaWet, out honeyWet );
			this.ApplyLiquidMovement( ent, lavaWet, honeyWet );
			this.ApplyCollisionMovement( ent, ref wetVelocity );
			this.ApplySlopeDodgeAndConveyorMovement( ent );

			if( core.wet ) {
				core.position += wetVelocity;
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

		public void RefreshLiquidContactStates( CustomEntity ent, out bool lavaWet, out bool honeyWet ) {
			var core = ent.Core;

			lavaWet = Collision.LavaCollision( core.position, core.width, core.height );
			if( lavaWet ) {
				core.lavaWet = true;
			}

			honeyWet = Collision.WetCollision( core.position, core.width, core.height );
			if( Collision.honey ) {
				core.honeyWet = true;
			}
		}


		public void ApplyLiquidMovement( CustomEntity ent, bool lavaWet, bool honeyWet ) {
			var core = ent.Core;

			if( honeyWet ) {
				if( !core.wet ) {
					if( core.wetCount == 0 ) {
						core.wetCount = 20;

						var dustPos = new Vector2( core.position.X - 6f, core.position.Y + (float)( core.height / 2 ) - 8f );

						if( !lavaWet ) {
							if( core.honeyWet ) {
								for( int i = 0; i < 5; i++ ) {
									int idx = Dust.NewDust( dustPos, core.width + 12, 24, 152, 0f, 0f, 0, default( Color ), 1f );
									Dust dust = Main.dust[idx];

									dust.velocity.Y = dust.velocity.Y - 1f;
									dust.velocity.X = dust.velocity.X * 2.5f;
									dust.scale = 1.3f;
									dust.alpha = 100;
									dust.noGravity = true;
								}
							} else {
								for( int i = 0; i < 10; i++ ) {
									int idx = Dust.NewDust( dustPos, core.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default( Color ), 1f );
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
								int idx = Dust.NewDust( dustPos, core.width + 12, 24, 35, 0f, 0f, 0, default( Color ), 1f );
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


		public void ApplyCollisionMovement( CustomEntity ent, ref Vector2 wetVelocity ) {
			var core = ent.Core;

			if( core.wet ) {
				Vector2 oldVel = core.velocity;
				core.velocity = Collision.TileCollision( core.position, core.velocity, core.width, core.height, false, false, 1 );

				if( core.velocity.X != oldVel.X ) {
					wetVelocity.X = core.velocity.X;
				}
				if( core.velocity.Y != oldVel.Y ) {
					wetVelocity.Y = core.velocity.Y;
				}
			} else {
				core.velocity = Collision.TileCollision( core.position, core.velocity, core.width, core.height, false, false, 1 );
			}
		}


		public void ApplySlopeDodgeAndConveyorMovement( CustomEntity ent ) {
			var core = ent.Core;
			Vector4 slopeDodge = Collision.SlopeCollision( core.position, core.velocity, core.width, core.height );

			core.position.X = slopeDodge.X;
			core.position.Y = slopeDodge.Y;
			core.velocity.X = slopeDodge.Z;
			core.velocity.Y = slopeDodge.W;

			try {
				Collision.StepConveyorBelt( core, 1f );
			} catch( Exception ) { }
		}
	}
}
