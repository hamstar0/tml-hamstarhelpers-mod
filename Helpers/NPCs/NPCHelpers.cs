using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPCs.
	/// </summary>
	public partial class NPCHelpers {
		/// <summary>
		/// Gets all active NPCs in the world.
		/// </summary>
		/// <returns></returns>
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
		
		/// <summary>
		/// Applies damage to an NPC, killing it if need be. Synced.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="damage"></param>
		public static void RawHurt( NPC npc, int damage ) {
			if( damage >= npc.life ) {
				NPCHelpers.Kill( npc );
			} else {
				npc.life -= damage;
				NetMessage.SendData( MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0 );
			}
		}


		/// <summary>
		/// Kills an NPC. Synced.
		/// </summary>
		/// <param name="npc"></param>
		public static void Kill( NPC npc ) {
			npc.life = 0;
			npc.checkDead();
			npc.active = false;
			NetMessage.SendData( MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0 );
		}

		/// <summary>
		/// Removes a given NPC. Synced.
		/// </summary>
		/// <param name="npc"></param>
		public static void Remove( NPC npc ) {
			npc.active = false;

			if( Main.netMode != 0 ) {   // == 2 only?
				npc.netSkip = -1;
				npc.life = 0;
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}

		////////////////

		/// <summary>
		/// Predicts the knockback travel vector (velocity) of a given knockback-inducing hit, according to vanilla Terraria's
		/// expected behavior.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="dir"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		/// <param name="isKillingBlow">Adds power.</param>
		/// <returns></returns>
		public static Vector2 GetKnockbackVector( NPC npc, int dir, float knockback, bool crit, bool isKillingBlow ) {
			float force = knockback * npc.knockBackResist;
			Vector2 forceVector = npc.velocity;

			if( (double)force > 8.0 ) { force = 8f + (force - 8f) * 0.9f; }
			if( (double)force > 10.0 ) { force = 10f + (force - 10f) * 0.8f; }
			if( (double)force > 12.0 ) { force = 12f + (force - 12f) * 0.7f; }
			if( (double)force > 14.0 ) { force = 14f + (force - 14f) * 0.6f; }
			if( (double)force > 16.0 ) { force = 16f; }
			if( crit ) { force *= 1.4f; }

			if( isKillingBlow ) {
				if( dir < 0 && (double)forceVector.X > -(double)force ) {
					if( (double)forceVector.X > 0.0 ) {
						forceVector.X -= force;
					}
					forceVector.X -= force;
					if( (double)forceVector.X < -(double)force ) {
						forceVector.X = -force;
					}
				} else if( dir > 0 && (double)forceVector.X < (double)force ) {
					if( (double)forceVector.X < 0.0 ) {
						forceVector.X += force;
					}
					forceVector.X += force;
					if( (double)forceVector.X > (double)force ) {
						forceVector.X = force;
					}
				}
				if( npc.type == NPCID.SnowFlinx ) {
					force *= 1.5f;
				}

				float launchForce = npc.noGravity ? force * -0.5f : force * -0.75f;
				if( (double)forceVector.Y > (double)launchForce ) {
					forceVector.Y += launchForce;
					if( (double)forceVector.Y < (double)launchForce ) {
						forceVector.Y = launchForce;
					}
				}
			} else {
				if( !npc.noGravity ) {
					forceVector.Y = (float)(-(double)force * 0.75) * npc.knockBackResist;
				} else {
					forceVector.Y = (float)(-(double)force * 0.5) * npc.knockBackResist;
				}
				forceVector.X = force * (float)dir * npc.knockBackResist;
			}
			
			return forceVector - npc.velocity;
		}

		////////////////

		/// <summary>
		/// Makes an (unsophisticated) attempt at assessing an NPC's threat level. Factors include damage, defense, life, tile
		/// collide, knockback resist, and 'value' (money dropped).
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static float LooselyAssessThreat( NPC npc ) {
			float damageFactor = (npc.damage / 100f) * (npc.coldDamage ? 1.2f : 1f);
			float defenseFactor = 1f + (npc.defense * 0.01f);

			float vitality = ((float)npc.lifeMax / 80000f) * defenseFactor;

			//float versatility = 0f;
			//for( int i=0; i<npc.buffImmune.Length; i++ ) {
			//	if( npc.buffImmune[i] ) { versatility++; }
			//}
			//float versatilityFactor = 1f + (versatility * 0.01f);
			float mobilityFactor = npc.noTileCollide ? 1.2f : 1f;
			float knockbackFactor = ((1f - npc.knockBackResist) * 0.1f) + 1f;

			float vitalityFactor = vitality * mobilityFactor * knockbackFactor;  //* versatilityFactor

			if( npc.value > 0 ) {
				float valueFactor = (float)npc.value / 150000f;
				return (vitalityFactor + damageFactor + valueFactor) / 3f;
			}

			return (vitalityFactor + damageFactor) / 2f;
		}

		////////////////

		/// <summary>
		/// Kills counted towards earning banners for a given NPC type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static int CurrentPlayerKillsOfBannerNpc( int npcType ) {
			int npcBannerType = Item.NPCtoBanner( npcType );
			if( npcBannerType == 0 ) { return -1; }

			if( npcBannerType >= NPC.killCount.Length ) { return -1; }
			return NPC.killCount[npcBannerType];
		}
	}
}
