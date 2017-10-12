using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/*class MyGlobalNpc : GlobalNPC {
		public override bool PreAI( NPC npc ) {
		}

		public override void AI( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.AI();
			}
		}
		public override void BossHeadRotation( NPC npc, ref float rotation ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.BossHeadRotation( ref rotation );
			}
		}
		public override void BossHeadSlot( NPC npc, ref int index ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.BossHeadSlot( ref index );
			}
		}
		public override void BossHeadSpriteEffects( NPC npc, ref SpriteEffects sprite_effects ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.BossHeadSpriteEffects( ref sprite_effects );
			}
		}
		public override void BuffTownNPC( ref float damageMult, ref int defense ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.BuffTownNPC( ref damageMult, ref defense );
				}
			}
		}
		public override bool? CanBeHitByItem( NPC npc, Player player, Item item ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				bool? can_hit = info.CanBeHitByItem( player, item );
				if( can_hit != null ) {
					return can_hit;
				}
			}
			return base.CanBeHitByItem( npc, player, item );
		}
		public override bool? CanBeHitByProjectile( NPC npc, Projectile projectile ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				bool? can_hit = info.CanBeHitByProjectile( projectile );
				if( can_hit != null ) {
					return can_hit;
				}
			}
			return base.CanBeHitByProjectile( npc, projectile );
		}
		public override bool? CanHitNPC( NPC npc, NPC target ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				bool? can_hit = info.CanHitNPC( target );
				if( can_hit != null ) {
					return can_hit;
				}
			}
			return base.CanHitNPC( npc, target );
		}
		public override bool CanHitPlayer( NPC npc, Player target, ref int cooldown_slot ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.CanHitPlayer( target, ref cooldown_slot ) ) {
					return true;
				}
			}
			return base.CanHitPlayer( npc, target, ref cooldown_slot );
		}
		public override bool CheckActive( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.CheckActive() ) {
					return true;
				}
			}
			return base.CheckActive( npc );
		}
		public override bool CheckDead( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.CheckDead() ) {
					return true;
				}
			}
			return base.CheckDead( npc );
		}
		public override void DrawEffects( NPC npc, ref Color drawColor ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.DrawEffects( ref drawColor );
			}
		}
		public override bool? DrawHealthBar( NPC npc, byte hb_position, ref float scale, ref Vector2 position ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				bool? can_hit = info.DrawHealthBar( hb_position, ref scale, ref position );
				if( can_hit != null ) {
					return can_hit;
				}
			}
			return base.DrawHealthBar( npc, hb_position, ref scale, ref position );
		}
		public override void DrawTownAttackGun( NPC npc, ref float scale, ref int item, ref int closeness ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.DrawTownAttackGun( ref scale, ref item, ref closeness );
			}
		}
		public override void DrawTownAttackSwing( NPC npc, ref Texture2D item, ref int item_size, ref float scale, ref Vector2 offset ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.DrawTownAttackSwing( ref item, ref item_size, ref scale, ref offset );
			}
		}
		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawn_info ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.EditSpawnPool( pool, spawn_info );
				}
			}
		}
		public override void EditSpawnRange( Player player, ref int spawn_range_x, ref int spawn_range_y, ref int safe_range_x, ref int safe_range_y ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.EditSpawnRange( player, ref spawn_range_x, ref spawn_range_y, ref safe_range_x, ref safe_range_y );
				}
			}
		}
		public override void EditSpawnRate( Player player, ref int spawn_rate, ref int max_spawns ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.EditSpawnRate( player, ref spawn_rate, ref max_spawns );
				}
			}
		}
		public override void FindFrame( NPC npc, int frame_height ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.FindFrame( frame_height );
			}
		}
		public override Color? GetAlpha( NPC npc, Color draw_color ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				Color? color = info.GetAlpha( draw_color );
				if( color != null ) {
					return color;
				}
			}
			return base.GetAlpha( npc, draw_color );
		}
		public override void GetChat( NPC npc, ref string chat ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.GetChat( ref chat );
			}
		}
		public override void HitEffect( NPC npc, int hit_direction, double damage ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.HitEffect( hit_direction, damage );
			}
		}
		public override void ModifyHitByItem( NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ModifyHitByItem( player, item, ref damage, ref knockback, ref crit );
			}
		}
		public override void ModifyHitByProjectile( NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hit_direction ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ModifyHitByProjectile( projectile, ref damage, ref knockback, ref crit, ref hit_direction );
			}
		}
		public override void ModifyHitNPC( NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ModifyHitNPC( target, ref damage, ref knockback, ref crit );
			}
		}
		public override void ModifyHitPlayer( NPC npc, Player target, ref int damage, ref bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ModifyHitPlayer( target, ref damage, ref crit );
			}
		}
		public override void NPCLoot( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.NPCLoot();
			}
		}
		public override void OnHitByItem( NPC npc, Player player, Item item, int damage, float knockback, bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.OnHitByItem( player, item, damage, knockback, crit );
			}
		}
		public override void OnHitByProjectile( NPC npc, Projectile projectile, int damage, float knockback, bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.OnHitByProjectile( projectile, damage, knockback, crit );
			}
		}
		public override void OnHitNPC( NPC npc, NPC target, int damage, float knockback, bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.OnHitNPC( target, damage, knockback, crit );
			}
		}
		public override void OnHitPlayer( NPC npc, Player target, int damage, bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.OnHitPlayer( target, damage, crit );
			}
		}
		public override void PostAI( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.PostAI();
			}
		}
		public override void PostDraw( NPC npc, SpriteBatch sprite_batch, Color draw_color ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.PostDraw( sprite_batch, draw_color );
			}
		}
		public override bool PreAI( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.PreAI( ) ) {
					return true;
				}
			}
			return base.CheckActive( npc );
		}
		public override bool PreDraw( NPC npc, SpriteBatch sprite_batch, Color draw_color ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.PreDraw( sprite_batch, draw_color ) ) {
					return true;
				}
			}
			return base.CheckActive( npc );
		}
		public override bool PreNPCLoot( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.PreNPCLoot() ) {
					return true;
				}
			}
			return base.CheckActive( npc );
		}
		public override void ResetEffects( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ResetEffects();
			}
		}
		public override void ScaleExpertStats( NPC npc, int num_players, float boss_life_scale ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.ScaleExpertStats( num_players, boss_life_scale );
			}
		}
		public override void SetDefaults( NPC npc ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.SetDefaults();
			}
		}
		public override void SetupShop( int type, Chest shop, ref int next_slot ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.SetupShop( type, shop, ref next_slot );
				}
			}
		}
		public override void SetupTravelShop( int[] shop, ref int next_slot ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.SetupTravelShop( shop, ref next_slot );
				}
			}
		}
		public override void SpawnNPC( int npc_type, int tile_x, int tile_y ) {
			foreach( var name_v_info in AltNPCInfo.NpcInfos.Values ) {
				foreach( var info in name_v_info.Values ) {
					info.SpawnNPC( npc_type, tile_x, tile_y );
				}
			}
		}
		public override bool StrikeNPC( NPC npc, ref double damage, int defense, ref float knockback, int hit_direction, ref bool crit ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				if( info.StrikeNPC( ref damage, defense, ref knockback, hit_direction, ref crit ) ) {
					return true;
				}
			}
			return base.CheckActive( npc );
		}
		public override void TownNPCAttackCooldown( NPC npc, ref int cooldown, ref int rand_extra_cooldown ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackCooldown( ref cooldown, ref rand_extra_cooldown );
			}
		}
		public override void TownNPCAttackMagic( NPC npc, ref float aura_light_multiplier ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackMagic( ref aura_light_multiplier );
			}
		}
		public override void TownNPCAttackProj( NPC npc, ref int proj_type, ref int attack_delay ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackProj( ref proj_type, ref attack_delay );
			}
		}
		public override void TownNPCAttackProjSpeed( NPC npc, ref float multiplier, ref float gravity_correction, ref float random_offset ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackProjSpeed( ref multiplier, ref gravity_correction, ref random_offset );
			}
		}
		public override void TownNPCAttackShoot( NPC npc, ref bool inBetweenShots ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackShoot( ref inBetweenShots );
			}
		}
		public override void TownNPCAttackStrength( NPC npc, ref int damage, ref float knockback ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackStrength( ref damage, ref knockback );
			}
		}
		public override void TownNPCAttackSwing( NPC npc, ref int item_width, ref int item_height ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.TownNPCAttackSwing( ref item_width, ref item_height );
			}
		}
		public override void UpdateLifeRegen( NPC npc, ref int damage ) {
			int who = npc.whoAmI;
			foreach( var info in AltNPCInfo.NpcInfos[who].Values ) {
				info.UpdateLifeRegen( ref damage );
			}
		}


		/*public override void SetDefaults( NPC npc ) {
			if( Main.netMode == 0 ) {   // Single
				if( NPCSpawnInfoHelpers.IsSimulatingSpawns ) {
					if( Main.npc[npc.whoAmI] != null && Main.npc[npc.whoAmI].active ) {
						NPCSpawnInfoHelpers.AddSpawn( npc.type );
					}
				}
			}
		}

		public override void EditSpawnRate( Player player, ref int spawn_rate, ref int max_spawns ) {
			if( NPCSpawnInfoHelpers.IsSimulatingSpawns ) {
				spawn_rate = 1;
				max_spawns = 100;
			}
		}
	}*/
}
