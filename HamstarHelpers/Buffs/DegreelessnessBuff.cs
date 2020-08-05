using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Cheats;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Buffs {
	/// <summary>
	/// Invulnerability buff.
	/// </summary>
	public class DegreelessnessBuff : ModBuff {
		/// <summary>
		/// Shows the typical animation effect of invulnerable entities.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <param name="particles"></param>
		public static void AnimateAttackBurstFX( Vector2 position, float radius, int particles ) {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			for( int i = 0; i < particles; i++ ) {
				Vector2 dir = new Vector2( rand.NextFloat() - 0.5f, rand.NextFloat() - 0.5f );
				dir.Normalize();
				Vector2 dustPos = position + ( dir * rand.NextFloat() * radius );

				int dustIdx = Dust.NewDust(
					Position: dustPos,
					Width: 1,
					Height: 1,
					Type: 269,
					SpeedX: 0f,
					SpeedY: 0f,
					Alpha: 0,
					newColor: Color.White,
					Scale: 1f
				);
				Dust dust = Main.dust[dustIdx];
				dust.noGravity = true;
			}
		}



		////////////////

		/// @private
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Degreelessness Mode" );
			this.Description.SetDefault( "Power overwhelming!" );
			//Main.buffNoTimeDisplay[this.Type] = true;
			//Main.buffNoSave[this.Type] = true;
		}


		////////////////

		/// @private
		public override void Update( Player player, ref int buffIndex ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			if( (myplayer.Logic.ActiveCheats & CheatModeType.BilboMode) == 0 ) {
				this.ApplyFx( player );
			}

			player.immune = true;

			Timers.SetTimer( "ModHelpersGodMode_P_" + player.whoAmI, 1, false, () => {
				player.immune = false;
				return true;
			} );
		}


		/// @private
		public override void Update( NPC npc, ref int buffIndex ) {
			this.ApplyFx( npc );

			//npc.immortal = true;
			npc.dontTakeDamage = true;

			Timers.SetTimer( "ModHelpersGodMode_N_" + npc.whoAmI, 1, false, () => {
				//npc.immortal = false;
				npc.dontTakeDamage = false;
				return true;
			} );
		}

		////////////////

		private void ApplyFx( Entity entity ) {
			int radius = ( entity.width + entity.height ) / 2;
			DegreelessnessBuff.AnimateAttackBurstFX( entity.Center, radius, radius / 10 );
		}
	}
}
