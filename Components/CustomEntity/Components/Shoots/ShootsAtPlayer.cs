using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class ShootsAtPlayerEntityComponent : CustomEntityComponent {
		public int ProjectileType;
		public int MinRange;
		public int MaxRange;
		public bool IgnoresSolidCover;
		public int Cooldown;
		public int MaxCooldown;



		////////////////

		protected ShootsAtPlayerEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		protected override void OnInitialize() { }


		////////////////

		public override void UpdateSingle( CustomEntity ent ) {
			this.Update( ent );
		}

		public override void UpdateClient( CustomEntity ent ) {
			this.Update( ent );
		}

		public override void UpdateServer( CustomEntity ent ) {
			this.Update( ent );
		}

		////

		private void Update( CustomEntity ent ) {
			int max = Main.netMode == 0 ? 1 : Main.player.Length;

			for( int i = 0; i < max; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active || plr.dead ) { continue; }

				int dist = (int)Vector2.Distance( plr.Center, ent.Core.Center );
				if( dist >= this.MinRange && dist < this.MaxRange ) {
					this.AttemptShoot( ent, plr );
				}
			}
		}


		////////////////

		private void AttemptShoot( CustomEntity ent, Player targetPlayer ) {
			if( !this.IgnoresSolidCover ) {
				bool isBlocked = false;

				Utils.PlotTileLine( ent.Core.Center, targetPlayer.Center, 1f, ( x, y ) => {
					if( TileHelpers.IsSolid( Main.tile[x, y] ) ) {
						isBlocked = true;
						return false;
					}
					return true;
				} );

				if( isBlocked ) {
					return;
				}
			}

			if( this.Cooldown <= 0 ) {
				this.Cooldown = this.MaxCooldown;

				this.Shoot( ent, targetPlayer );
			}
		}

		////

		public void Shoot( CustomEntity ent, Player targetPlayer ) {
			var aim = targetPlayer.Center - ent.Core.Center;
			aim.Normalize();
			aim = this.GetShotVelocity( aim );

			Projectile.NewProjectile( ent.Core.Center, aim, this.ProjectileType, this.GetShotDamage(), this.GetShotKnockback() );
		}


		////////////////

		public virtual Vector2 GetShotVelocity( Vector2 direction ) {
			return direction * 5f;
		}


		public virtual int GetShotDamage() {
			return 16;
		}


		public virtual float GetShotKnockback() {
			return 1f;
		}
	}
}
