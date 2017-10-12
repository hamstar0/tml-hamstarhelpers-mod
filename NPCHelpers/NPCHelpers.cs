using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCHelpers {
		public static bool IsNPCDead( NPC check_npc ) {
			return check_npc.life <= 0 || !check_npc.active;
		}


		public static void Kill( NPC npc ) {
			npc.life = 0;
			npc.checkDead();
			npc.active = false;
			NetMessage.SendData( 28, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0 );
		}


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


		public static void DrawSimple( SpriteBatch sb, NPC npc, int frame, Vector2 position, float rotation, float scale, Color color ) {
			Texture2D tex = Main.npcTexture[ npc.type ];
			int frame_count = Main.npcFrameCount[ npc.type ];
			int tex_height = tex.Height / frame_count;

			Rectangle frame_rect = new Rectangle( 0, frame * tex_height, tex.Width, tex_height );

			float y_offset = 0.0f;
			float height_offset = Main.NPCAddHeight( npc.whoAmI );
			Vector2 origin = new Vector2( (float)(tex.Width / 2), (float)((tex.Height / frame_count) / 2) );

			if( npc.type == 108 || npc.type == 124 ) {
				y_offset = 2f;
			} else if( npc.type == 357 ) {
				y_offset = npc.localAI[0];
			} else if( npc.type == 467 ) {
				y_offset = 7f;
			} else if( npc.type == 537 ) {
				y_offset = 2f;
			} else if( npc.type == 509 ) {
				y_offset = -6f;
			} else if( npc.type == 490 ) {
				y_offset = 4f;
			} else if( npc.type == 484 ) {
				y_offset = 2f;
			} else if( npc.type == 483 ) {
				y_offset = 14f;
			} else if( npc.type == 477 ) {
				height_offset = 22f;
			} else if( npc.type == 478 ) {
				y_offset -= 2f;
			} else if( npc.type == 469 && (double)npc.ai[2] == 1.0 ) {
				y_offset = 14f;
			} else if( npc.type == 4 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 125 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 126 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 63 || npc.type == 64 || npc.type == 103 ) {
				origin.Y += 4f;
			} else if( npc.type == 69 ) {
				origin.Y += 8f;
			} else if( npc.type == 262 ) {
				origin.Y = 77f;
				height_offset += 26f;
			} else if( npc.type == 264 ) {
				origin.Y = 21f;
				height_offset += 2f;
			} else if( npc.type == 266 ) {
				height_offset += 50f;
			} else if( npc.type == 268 ) {
				height_offset += 16f;
			} else if( npc.type == 288 ) {
				height_offset += 6f;
			}
			
			//if( npc.aiStyle == 10 || npc.type == 72 )
			//	color1 = Color.White;

			SpriteEffects fx = SpriteEffects.None;
			if( npc.spriteDirection == 1 ) {
				fx = SpriteEffects.FlipHorizontally;
			}

			float y_off = height_offset + y_offset + npc.gfxOffY + 4f;
			float x = position.X + ((float)npc.width / 2f) - (((float)tex.Width * scale) / 2f) + (origin.X * scale);
			float y = position.Y + (float)npc.height - ((float)tex_height * scale) + (origin.Y * scale) + y_off;
			Vector2 pos = UIHelpers.UIHelpers.ConvertToScreenPosition( new Vector2( x, y ) );
			
			sb.Draw( tex, pos, frame_rect, color, rotation, origin, scale, fx, 1f );
		}


		////////////////

		[System.Obsolete( "use NPCIdentityHelpers.GetUniqueId", true )]
		public static string GetUniqueId( NPC npc ) {
			return NPCIdentityHelpers.GetUniqueId( npc );
		}


		[System.Obsolete( "use NPCTownHelpers.Leave", true )]
		public static void Leave( NPC npc, bool announce=true ) {
			NPCTownHelpers.Leave( npc, announce );
		}

		[System.Obsolete( "use NPCTownHelpers.GetShop", true )]
		public static Chest GetShop( int npc_type ) {
			return NPCTownHelpers.GetShop( npc_type );
		}


		[System.Obsolete( "use NPCTownHelpers.GetFemaleTownNpcTypes", true )]
		public static ISet<int> GetFemaleTownNpcTypes() {
			return NPCTownHelpers.GetFemaleTownNpcTypes();
		}

		[System.Obsolete( "use NPCTownHelpers.GetNonGenderedTownNpcTypes", true )]
		public static ISet<int> GetNonGenderedTownNpcTypes() {
			return NPCTownHelpers.GetNonGenderedTownNpcTypes();
		}



		/*#region Forced spawns
		private static int ClearBadForcedSpawns( Player player, ISet<int> orig_npc_whos, int find_of_npc_type, float orig_active_npcs ) {
			int npc_who = -1;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || orig_npc_whos.Contains( i ) ) { continue; }

				// Found THE spawn
				if( npc_who == -1 && npc.type == find_of_npc_type ) {
					npc_who = i;
				} else {
					// Otherwise get rid of it
					npc.active = false;
					Main.npc[i] = new NPC();
				}
			}
			player.activeNPCs = orig_active_npcs + (npc_who == -1 ? 0 : 1);

			return npc_who;
		}
		
		public static int ForceSpawnForPlayer( Player player, int find_of_npc_type, int determination ) {
			var other_players = NPCSpawnInfoHelpers.IsolatePlayer( player );
			var npc_whos_snapshot = NPCSpawnInfoHelpers.GetNpcSnapshot();
			float orig_active_npcs = player.activeNPCs;
			int npc_who = -1;

			NPCSpawnInfoHelpers.IsSimulatingSpawns = true;
			// Test spawns
			for( int i = 1; i <= determination; i++ ) {
				NPC.SpawnNPC();

				if( (i % 25) == 0 ) {
					// Force remove spawned npcs that aren't the given type
					npc_who = NPCHelpers.ClearBadForcedSpawns( player, npc_whos_snapshot, find_of_npc_type, orig_active_npcs );
					if( npc_who != -1 ) { break; }
				}
			}
			NPCSpawnInfoHelpers.IsSimulatingSpawns = false;

			// Restore players
			foreach( var kv in other_players ) {
				Main.player[kv.Key] = kv.Value;
			}

			return npc_who;
		}
		#endregion*/
	}
}
