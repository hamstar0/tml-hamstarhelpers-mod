using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Internals.NetProtocols;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players.
	/// </summary>
	public partial class PlayerHelpers {
		/// <summary></summary>
		public const int InventorySize = 58;
		/// <summary></summary>
		public const int InventoryHotbarSize = 10;
		/// <summary></summary>
		public const int InventoryMainSize = 40;



		////////////////

		/// <summary>
		/// Predicts impending fall damage amount for a given player at the current time (if any).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Loosely assesses player's relative level of power. Factors include appraisals of inventory items, player's defense,
		/// and player's life.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static float LooselyAssessPower( Player player ) {
			float armorCount=0, miscCount=0;
			float hotbarTech=0, armorTech=0, miscTech=0;

			for( int i=0; i<PlayerHelpers.InventoryHotbarSize; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant(item) ) { continue; }

				float tech = ItemAttributeHelpers.LooselyAppraise( item );
				hotbarTech = hotbarTech > tech ? hotbarTech : tech;
			}

			int maxArmorSlot = 8 + player.extraAccessorySlots;
			for( int i=0; i<maxArmorSlot; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				armorTech += ItemAttributeHelpers.LooselyAppraise( item );
				armorCount += 1;
			}

			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				Item item = player.miscEquips[i];
				if( item == null || item.IsAir || !ItemAttributeHelpers.IsGameplayRelevant( item ) ) { continue; }

				miscTech += ItemAttributeHelpers.LooselyAppraise( item );
				miscCount += 1;
			}

			float techFactor = armorTech / (armorCount * ItemRarityAttributeHelpers.HighestVanillaRarity);
			techFactor += miscTech / (miscCount * ItemRarityAttributeHelpers.HighestVanillaRarity);
			techFactor += hotbarTech + hotbarTech;
			techFactor /= 4;

			float defenseFactor = 1f + ((float)player.statDefense * 0.01f);
			float addedVitality = (float)player.statLifeMax / 20f;
			float vitalityFactor = addedVitality * defenseFactor;

			return (techFactor + techFactor + vitalityFactor) / 3f;
		}

		////

		/// <summary>
		/// Indicates if player counts as 'incapacitated'.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="freedomNeeded">Pulley, grappling, or minecarting count as incapacitation. Default `false`.</param>
		/// <param name="armsNeeded">Use of arms (e.g. no Cursed debuff) count as incapacitation. Default `false`.</param>
		/// <param name="sightNeeded">Visual occlusion (e.g. Blackout debuff) counts as incapacitation. Default `false`.</param>
		/// <param name="sanityNeeded">Control complications (e.g. Confused debuff) counts as incapacition. Default `false`.</param>
		/// <returns></returns>
		public static bool IsIncapacitated(
					Player player,
					bool freedomNeeded=false,
					bool armsNeeded=false,
					bool sightNeeded=false,
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

		/// <summary>
		/// Apply armor-bypassing damage to player, killing if needed (syncs via. Player.Hurt(...)).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="deathReason"></param>
		/// <param name="damage"></param>
		/// <param name="direction"></param>
		/// <param name="pvp"></param>
		/// <param name="quiet"></param>
		/// <param name="crit"></param>
		public static void RawHurt(
					Player player,
					PlayerDeathReason deathReason,
					int damage,
					int direction,
					bool pvp=false,
					bool quiet=false,
					bool crit=false ) {
			int def = player.statDefense;

			player.statDefense = 0;
			player.Hurt( deathReason, damage, direction, pvp, quiet, crit );
			player.statDefense = def;
		}

		////

		/// <summary>
		/// Kills a player permanently (as if hardcore mode).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="deathMsg"></param>
		public static void KillWithPermadeath( Player player, string deathMsg ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				PlayerPermaDeathProtocol.BroadcastFromClient( player.whoAmI, deathMsg );
			} else if( Main.netMode == NetmodeID.Server ) {
				PlayerPermaDeathProtocol.BroadcastFromServer( player.whoAmI, deathMsg );
			} else {
				PlayerHelpers.ApplyPermaDeath( player, deathMsg );
			}
		}

		internal static void ApplyPermaDeath( Player player, string deathMsg ) {
			player.difficulty = 2;
			player.KillMe( PlayerDeathReason.ByCustomReason( deathMsg ), 9999, 0 );
		}


		////

		/// <summary>
		/// Resets a player back to (vanilla) factory defaults.
		/// </summary>
		/// <param name="player"></param>
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

		/// <summary>
		/// For just the current game tick, the player is under complete lockdown: No movement, actions, or damage taken.
		/// </summary>
		/// <param name="player"></param>
		public static void LockdownPlayerPerTick( Player player ) {
			player.noItems = true;
			player.noBuilding = true;
			player.stoned = true;
			player.immune = true;
			player.immuneTime = 2;
		}
	}
}
