using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Internals.NetProtocols;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Players {
	/** <summary>Assorted static "helper" functions pertaining to players.</summary> */
	public static partial class PlayerHelpers {
		public const int InventorySize = 58;
		public const int InventoryHotbarSize = 10;
		public const int InventoryMainSize = 40;



		////////////////

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

			float techFactor = tally / (itemCount * ItemRarityAttributeHelpers.HighestVanillaRarity);
			float defenseFactor = 1f + ((float)player.statDefense * 0.01f);
			float vitality = (float)player.statLifeMax / 20f;
			float vitalityFactor = (vitality / (4f * ItemRarityAttributeHelpers.HighestVanillaRarity)) * defenseFactor;

			return (techFactor + vitalityFactor) / 2f;
		}

		////

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


		////

		public static void RawHurt( Player player, PlayerDeathReason deathReason, int damage, int direction, bool pvp=false, bool quiet=false, bool crit=false ) {
			int def = player.statDefense;

			player.statDefense = 0;
			player.Hurt( deathReason, damage, direction, pvp, quiet, crit );
			player.statDefense = def;
		}

		////

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


		////

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

		////

		public static void LockdownPlayerPerTick( Player player ) {
			player.noItems = true;
			player.noBuilding = true;
			player.stoned = true;
			player.immune = true;
			player.immuneTime = 2;
		}
	}
}
