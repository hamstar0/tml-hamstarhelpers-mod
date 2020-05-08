using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using HamstarHelpers.Tiles;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersPlayer : ModPlayer {
		private void CheckForBrambles() {
			bool enbrambled = false;
			int brambleType = ModContent.TileType<CursedBrambleTile>();
			int begX = (int)this.player.position.X;
			int endX = begX + this.player.width;

			for( int i = begX; i < endX; i += 16 ) {
				int begY = (int)this.player.position.Y;
				int endY = begY + this.player.height;

				for( int j = begY; j < endY; j += 16 ) {
					if( Framing.GetTileSafely( i >> 4, j >> 4 ).type == brambleType ) {
						enbrambled = true;
						break;
					}
				}
			}

			if( enbrambled ) {
				this.ApplyBrambleEffects();
			}
		}


		////////////////

		private void ApplyBrambleEffects() {
			string timerName = "AmbushesCursedBrambleHurt_" + this.player.whoAmI;
			float brambleFriction = ModHelpersConfig.Instance.BrambleFriction;
			int brambleDmgTickRate = ModHelpersConfig.Instance.BrambleDamageTickRate;
			int brambleDmg = ModHelpersConfig.Instance.BrambleDamage;

			if( this.player.velocity.LengthSquared() > 0.1f ) {
				this.player.velocity *= 1f - brambleFriction;
			}

			if( Timers.GetTimerTickDuration( timerName ) <= 0 ) {
				Timers.SetTimer( timerName, brambleDmgTickRate, false, () => {
					PlayerHelpers.RawHurt(
						player: this.player,
						deathReason: PlayerDeathReason.ByCustomReason( " was devoured by cursed brambles" ),
						damage: brambleDmg,
						direction: 0,
						pvp: false,
						quiet: true,
						crit: false
					);
					return false;
				} );

				this.player.AddBuff( BuffID.Venom, 60 );
			}
		}
	}
}
