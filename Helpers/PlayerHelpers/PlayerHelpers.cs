using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.DataStore;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;

namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static partial class PlayerHelpers {
		public const int InventorySize = 58;
		public const int InventoryHotbarSize = 10;
		public const int InventoryMainSize = 40;


		////////////////

		private static object SpawnPointKey = new object();



		////////////////

		public static void Evac( Player player ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			for( int i = 0; i < 1000; i++ ) {
				if( Main.projectile[i].active && Main.projectile[i].owner == i && Main.projectile[i].aiStyle == 7 ) {
					Main.projectile[i].Kill();
				}
			}
			bool immune = player.immune;
			int immune_time = player.immuneTime;

			player.Spawn();
			player.immune = immune;
			player.immuneTime = immune_time;
		}


		public static void Teleport( Player player, Vector2 pos, int style = -1 ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			bool is_immune = player.immune;
			int immune_time = player.immuneTime;
			player.Spawn();
			player.immune = is_immune;
			player.immuneTime = immune_time;

			if( Main.netMode <= 1 ) {
				player.Teleport( pos, style );
			} else {
				style = style == -1 ? 1 : style;
				NetMessage.SendData( MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, style, 0, 0 );
			}
		}


		public static Vector2 GetSpawnPoint( Player player ) {
			var pos = new Vector2();

			if( player.SpawnX >= 0 && player.SpawnY >= 0 ) {
				pos.X = (float)( ( player.SpawnX * 16 ) + 8 - ( player.width / 2 ) );
				pos.Y = (float)( ( player.SpawnY * 16 ) - player.height );
			} else {
				pos.X = (float)( ( Main.spawnTileX * 16 ) + 8 - ( player.width / 2 ) );
				pos.Y = (float)( ( Main.spawnTileY * 16 ) - player.height );
			}

			return pos;
		}


		public static void SetSpawnPoint( Player player, int tile_x, int tile_y ) {
			bool success;
			var spawn_map = (IDictionary<string, IDictionary<int, int>>)DataStore.Get( PlayerHelpers.SpawnPointKey, out success );

			player.SpawnX = tile_x;
			player.SpawnY = tile_y;

			if( !success ) {
				spawn_map = new Dictionary<string, IDictionary<int, int>>();

				for( int i = 0; i < 200; i++ ) {
					string key1 = player.spN[i];
					int key2 = player.spI[i];

					if( key1 == null ) {
						break;
					}

					if( !spawn_map.ContainsKey( key1 ) ) {
						spawn_map[ key1 ] = new Dictionary<int, int>();
					}
					spawn_map[key1][key2] = i;
				}

				DataStore.Set( PlayerHelpers.SpawnPointKey, spawn_map );
			}

			if( spawn_map.ContainsKey( Main.worldName ) && spawn_map[ Main.worldName ].ContainsKey( Main.worldID ) ) {
				int idx = spawn_map[Main.worldName][Main.worldID];

				player.spX[idx] = tile_x;
				player.spY[idx] = tile_y;
			} else {
				player.ChangeSpawn( tile_x, tile_y );

				DataStore.Remove( PlayerHelpers.SpawnPointKey );	// <- Force rebuild
			}
		}


		public static int ComputeImpendingFallDamage( Player player ) {
			if( player.mount.CanFly ) {
				return 0;
			}
			if( player.mount.Cart && Minecart.OnTrack( player.position, player.width, player.height ) ) {
				return 0;
			}
			if( player.mount.Type == 1 ) {
				return 0;
			}

			int safety_min = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safety_min) || (player.gravDir == -1f && damage < -safety_min) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int final_damage = (int)((float)damage * player.gravDir - (float)safety_min) * 10;
				if( player.mount.Active ) {
					final_damage = (int)((float)final_damage * player.mount.FallDamage);
				}
				return final_damage;
			}

			return 0;
		}


		public static float LooselyAssessPower( Player player ) {
			float item_count = 0;
			float tally = 0;

			for( int i=0; i<PlayerHelpers.InventoryHotbarSize; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant(item) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				item_count += 1;
			}

			for( int i=0; i<player.armor.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				item_count += 1;
			}

			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				Item item = player.miscEquips[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				item_count += 1;
			}

			float tech_factor = tally / (item_count * ItemAttributeHelpers.HighestVanillaRarity);
			float defense_factor = 1f + ((float)player.statDefense * 0.01f);
			float vitality = (float)player.statLifeMax / 20f;
			float vitality_factor = (vitality / (4f * ItemAttributeHelpers.HighestVanillaRarity)) * defense_factor;

			return (tech_factor + vitality_factor) / 2f;
		}


		public static bool IsIncapacitated( Player player, bool freedom_needed=false, bool arms_needed=false, bool sight_needed=false,
				bool sanity_needed=false ) {
			if( player == null || !player.active || player.dead || player.stoned || player.frozen || player.ghost ||
				player.gross || player.webbed || player.mapFullScreen ) { return true; }
			if( freedom_needed && (player.pulley || player.grappling[0] >= 0 || player.mount.Cart) ) { return true; }
			if( arms_needed && player.noItems ) { return true; }
			if( sight_needed && player.blackout ) { return true; }
			if( sanity_needed && player.confused ) { return true; }
			return false;
		}


		public static void KillWithPermadeath( Player player, string death_msg ) {
			if( Main.netMode != 0 ) {
				PlayerPermaDeathProtocol.SendToAll( player.whoAmI, death_msg );
			} else {
				PlayerHelpers.ApplyPermaDeath( player, death_msg );
			}
		}

		internal static void ApplyPermaDeath( Player player, string death_msg ) {
			player.difficulty = 2;
			player.KillMe( PlayerDeathReason.ByCustomReason( death_msg ), 9999, 0 );
		}

		
		public static void FullVanillaReset( Player player ) {
			for( int i = 0; i < player.inventory.Length; i++ ) {
				player.inventory[i] = new Item();
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				player.armor[i] = new Item();
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				player.bank.item[i] = new Item();
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				player.bank2.item[i] = new Item();
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				player.bank3.item[i] = new Item();
			}
			for( int i = 0; i < player.dye.Length; i++ ) {
				player.dye[i] = new Item();
			}
			for( int i = 0; i < player.miscDyes.Length; i++ ) {
				player.miscDyes[i] = new Item();
			}
			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				player.miscEquips[i] = new Item();
			}

			for( int i = 0; i < player.buffType.Length; i++ ) {
				player.buffType[i] = 0;
				player.buffTime[i] = 0;
			}

			player.trashItem = new Item();
			if( player.whoAmI == Main.myPlayer ) {
				Main.mouseItem = new Item();
			}

			player.statLifeMax = 100;
			player.statManaMax = 20;

			player.extraAccessory = false;
			player.anglerQuestsFinished = 0;
			player.bartenderQuestLog = 0;
			player.downedDD2EventAnyDifficulty = false;
			player.taxMoney = 0;

			PlayerHooks.SetStartInventory( player );
		}


		public static void LockdownPlayerPerTick( Player player ) {
			player.noItems = true;
			player.noBuilding = true;
			player.stoned = true;
			player.immune = true;
			player.immuneTime = 2;
		}


		public static void ModdedExtensionsReset( Player player ) {
			var wingmod = ModLoader.GetMod( "Wing Slot" );

			if( wingmod != null ) {
				bool success;
				ModPlayer mywingplayer = player.GetModPlayer( wingmod, "WingSlotPlayer" );

				object wing_equip_slot = ReflectionHelpers.GetField( mywingplayer, "EquipSlot", out success );

				if( success && wing_equip_slot != null ) {
					Item wing_item = (Item)ReflectionHelpers.GetProperty( wing_equip_slot, "Item", out success );

					if( success && wing_item != null && !wing_item.IsAir ) {
						ReflectionHelpers.SetProperty( wing_equip_slot, "Item", new Item(), out success );
						ReflectionHelpers.SetField( mywingplayer, "EquipSlot", wing_equip_slot, out success );
					}
				}
				
				object wing_vanity_slot = ReflectionHelpers.GetField( mywingplayer, "VanitySlot", out success );

				if( success && wing_vanity_slot != null ) {
					Item wing_item = (Item)ReflectionHelpers.GetProperty( wing_vanity_slot, "Item", out success );

					if( success && wing_item != null && !wing_item.IsAir ) {
						ReflectionHelpers.SetProperty( wing_vanity_slot, "Item", new Item(), out success );
						ReflectionHelpers.SetField( mywingplayer, "VanitySlot", wing_vanity_slot, out success );
					}
				}

				object wing_dye_slot = ReflectionHelpers.GetField( mywingplayer, "DyeSlot", out success );

				if( success && wing_dye_slot != null ) {
					Item wing_item = (Item)ReflectionHelpers.GetProperty( wing_dye_slot, "Item", out success );

					if( success && wing_item != null && !wing_item.IsAir ) {
						ReflectionHelpers.SetProperty( wing_dye_slot, "Item", new Item(), out success );
						ReflectionHelpers.SetField( mywingplayer, "DyeSlot", wing_dye_slot, out success );
					}
				}
			}
		}
	}
}
