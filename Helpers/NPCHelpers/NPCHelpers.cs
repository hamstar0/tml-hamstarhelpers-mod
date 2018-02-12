using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.NPCHelpers {
	public static partial class NPCHelpers {
		public static IList<NPC> GetActive() {
			var list = new List<NPC>();

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active && npc.type != 0 ) {
					list.Add( npc );
				}
			}
			return list;
		}

		////////////////

		public static bool IsNPCDead( NPC check_npc ) {
			return check_npc.life <= 0 || !check_npc.active;
		}


		public static void Kill( NPC npc ) {
			npc.life = 0;
			npc.checkDead();
			npc.active = false;
			NetMessage.SendData( 28, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0 );
		}

		////////////////

		public static Vector2 GetKnockbackVector( NPC npc, int dir, float knockback, bool crit, bool is_killing_blow ) {
			float force = knockback * npc.knockBackResist;
			Vector2 force_vector = npc.velocity;

			if( (double)force > 8.0 ) { force = 8f + (force - 8f) * 0.9f; }
			if( (double)force > 10.0 ) { force = 10f + (force - 10f) * 0.8f; }
			if( (double)force > 12.0 ) { force = 12f + (force - 12f) * 0.7f; }
			if( (double)force > 14.0 ) { force = 14f + (force - 14f) * 0.6f; }
			if( (double)force > 16.0 ) { force = 16f; }
			if( crit ) { force *= 1.4f; }

			if( is_killing_blow ) {
				if( dir < 0 && (double)force_vector.X > -(double)force ) {
					if( (double)force_vector.X > 0.0 ) {
						force_vector.X -= force;
					}
					force_vector.X -= force;
					if( (double)force_vector.X < -(double)force ) {
						force_vector.X = -force;
					}
				} else if( dir > 0 && (double)force_vector.X < (double)force ) {
					if( (double)force_vector.X < 0.0 ) {
						force_vector.X += force;
					}
					force_vector.X += force;
					if( (double)force_vector.X > (double)force ) {
						force_vector.X = force;
					}
				}
				if( npc.type == NPCID.SnowFlinx ) {
					force *= 1.5f;
				}

				float launch_force = npc.noGravity ? force * -0.5f : force * -0.75f;
				if( (double)force_vector.Y > (double)launch_force ) {
					force_vector.Y += launch_force;
					if( (double)force_vector.Y < (double)launch_force ) {
						force_vector.Y = launch_force;
					}
				}
			} else {
				if( !npc.noGravity ) {
					force_vector.Y = (float)(-(double)force * 0.75) * npc.knockBackResist;
				} else {
					force_vector.Y = (float)(-(double)force * 0.5) * npc.knockBackResist;
				}
				force_vector.X = force * (float)dir * npc.knockBackResist;
			}
			
			return force_vector - npc.velocity;
		}

		////////////////

		public static float LooselyAssessThreat( NPC npc ) {
			float damage_factor = (npc.damage / 100f) * (npc.coldDamage ? 1.2f : 1f);
			float defense_factor = 1f + (npc.defense * 0.01f);

			float vitality = ((float)npc.lifeMax / 80000f) * defense_factor;

			//float versatility = 0f;
			//for( int i=0; i<npc.buffImmune.Length; i++ ) {
			//	if( npc.buffImmune[i] ) { versatility++; }
			//}
			//float versatility_factor = 1f + (versatility * 0.01f);
			float mobility_factor = npc.noTileCollide ? 1.2f : 1f;
			float knockback_factor = ((1f - npc.knockBackResist) * 0.1f) + 1f;

			float vitality_factor = vitality * mobility_factor * knockback_factor;  //* versatility_factor

			if( npc.value > 0 ) {
				float value_factor = (float)npc.value / 150000f;
				return (vitality_factor + damage_factor + value_factor) / 3f;
			}

			return (vitality_factor + damage_factor) / 2f;
		}

		////////////////

		public static int CurrentPlayerKillsOfBannerNpc( int npc_type ) {
			int npc_banner_type = Item.NPCtoBanner( npc_type );
			if( npc_banner_type == 0 ) { return -1; }

			if( npc_banner_type >= NPC.killCount.Length ) { return -1; }
			return NPC.killCount[npc_banner_type];
		}
	}
}
