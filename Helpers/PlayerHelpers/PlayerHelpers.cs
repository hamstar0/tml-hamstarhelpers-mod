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
			int immuneTime = player.immuneTime;

			player.Spawn();
			player.immune = immune;
			player.immuneTime = immuneTime;
		}


		public static void Teleport( Player player, Vector2 pos, int style = -1 ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			bool isImmune = player.immune;
			int immuneTime = player.immuneTime;
			player.Spawn();
			player.immune = isImmune;
			player.immuneTime = immuneTime;

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


		public static void SetSpawnPoint( Player player, int tileX, int tileY ) {
			IDictionary<string, IDictionary<int, int>> spawnMap;
			bool success = DataStore.Get( PlayerHelpers.SpawnPointKey, out spawnMap );

			player.SpawnX = tileX;
			player.SpawnY = tileY;

			if( !success ) {
				spawnMap = new Dictionary<string, IDictionary<int, int>>();

				for( int i = 0; i < 200; i++ ) {
					string key1 = player.spN[i];
					int key2 = player.spI[i];

					if( key1 == null ) {
						break;
					}

					if( !spawnMap.ContainsKey( key1 ) ) {
						spawnMap[ key1 ] = new Dictionary<int, int>();
					}
					spawnMap[key1][key2] = i;
				}

				DataStore.Set( PlayerHelpers.SpawnPointKey, spawnMap );
			}

			if( spawnMap.ContainsKey( Main.worldName ) && spawnMap[ Main.worldName ].ContainsKey( Main.worldID ) ) {
				int idx = spawnMap[Main.worldName][Main.worldID];

				player.spX[idx] = tileX;
				player.spY[idx] = tileY;
			} else {
				player.ChangeSpawn( tileX, tileY );

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

			int safetyMin = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safetyMin) || (player.gravDir == -1f && damage < -safetyMin) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int finalDamage = (int)((float)damage * player.gravDir - (float)safetyMin) * 10;
				if( player.mount.Active ) {
					finalDamage = (int)((float)finalDamage * player.mount.FallDamage);
				}
				return finalDamage;
			}

			return 0;
		}


		public static float LooselyAssessPower( Player player ) {
			float itemCount = 0;
			float tally = 0;

			for( int i=0; i<PlayerHelpers.InventoryHotbarSize; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant(item) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				itemCount += 1;
			}

			for( int i=0; i<player.armor.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				itemCount += 1;
			}

			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				Item item = player.miscEquips[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				tally += ItemAttributeHelpers.LooselyAppraise( item );
				itemCount += 1;
			}

			float techFactor = tally / (itemCount * ItemAttributeHelpers.HighestVanillaRarity);
			float defenseFactor = 1f + ((float)player.statDefense * 0.01f);
			float vitality = (float)player.statLifeMax / 20f;
			float vitalityFactor = (vitality / (4f * ItemAttributeHelpers.HighestVanillaRarity)) * defenseFactor;

			return (techFactor + vitalityFactor) / 2f;
		}


		public static bool IsIncapacitated( Player player, bool freedomNeeded=false, bool armsNeeded=false, bool sightNeeded=false,
				bool sanityNeeded=false ) {
			if( player == null || !player.active || player.dead || player.stoned || player.frozen || player.ghost ||
				player.gross || player.webbed || player.mapFullScreen ) { return true; }
			if( freedomNeeded && (player.pulley || player.grappling[0] >= 0 || player.mount.Cart) ) { return true; }
			if( armsNeeded && player.noItems ) { return true; }
			if( sightNeeded && player.blackout ) { return true; }
			if( sanityNeeded && player.confused ) { return true; }
			return false;
		}


		public static void KillWithPermadeath( Player player, string deathMsg ) {
			if( Main.netMode != 0 ) {
				PlayerPermaDeathProtocol.SendToAll( player.whoAmI, deathMsg );
			} else {
				PlayerHelpers.ApplyPermaDeath( player, deathMsg );
			}
		}

		internal static void ApplyPermaDeath( Player player, string deathMsg ) {
			player.difficulty = 2;
			player.KillMe( PlayerDeathReason.ByCustomReason( deathMsg ), 9999, 0 );
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
				PlayerHelpers.WingModReset( wingmod, player );
			}
		}


		private static void WingModReset( Mod wingmod, Player player ) {
			object _;
			ModPlayer mywingplayer = player.GetModPlayer( wingmod, "WingSlotPlayer" );
			Item wingItem;

			object wingEquipSlot;
			if( ReflectionHelpers.GetField(mywingplayer, "EquipSlot", out wingEquipSlot) && wingEquipSlot != null ) {
				if( ReflectionHelpers.GetProperty( wingEquipSlot, "Item", out wingItem ) ) {
					if( wingItem != null && !wingItem.IsAir ) {
						ReflectionHelpers.SetProperty( wingEquipSlot, "Item", new Item() );
						ReflectionHelpers.SetField( mywingplayer, "EquipSlot", wingEquipSlot );
					}
				}
			}

			object wingVanitySlot;
			if( ReflectionHelpers.GetField(mywingplayer, "VanitySlot", out wingVanitySlot) && wingVanitySlot != null ) {
				if( ReflectionHelpers.GetProperty( wingVanitySlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.SetProperty( wingVanitySlot, "Item", new Item() );
					ReflectionHelpers.SetField( mywingplayer, "VanitySlot", wingVanitySlot );
				}
			}

			object wingDyeSlot;

			if( ReflectionHelpers.GetField(mywingplayer, "DyeSlot", out wingDyeSlot) && wingDyeSlot != null ) {
				if( ReflectionHelpers.GetProperty( wingDyeSlot, "Item", out wingItem ) && wingItem != null && !wingItem.IsAir ) {
					ReflectionHelpers.SetProperty( wingDyeSlot, "Item", new Item() );
					ReflectionHelpers.SetField( mywingplayer, "DyeSlot", wingDyeSlot );
				}
			}
		}
	}
}
