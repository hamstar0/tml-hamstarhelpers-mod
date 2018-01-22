using HamstarHelpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static class ItemHelpers {
		public static IList<Item> GetActive() {
			var list = new List<Item>();

			for( int i = 0; i < Main.item.Length; i++ ) {
				Item item = Main.item[i];
				if( item != null && item.active && item.type != 0 ) {
					list.Add( item );
				}
			}
			return list;
		}

		////////////////

		public static string GetUniqueId( Item item ) {
			if( item.modItem != null ) {
				return item.modItem.mod.Name + " " + item.Name;
			}
			return ""+ item.netID;
		}


		public static int GetVanillaSnapshotHash( Item item, bool no_context=false ) {
			int hash = EntityHelpers.EntityHelpers.GetVanillaSnapshotHash( item, no_context );
			
			hash ^= item.mana.GetHashCode();
			//hash ^= item.buyOnce.GetHashCode();	//?
			hash ^= item.manaIncrease.GetHashCode();
			hash ^= item.lifeRegen.GetHashCode();
			hash ^= item.notAmmo.GetHashCode();
			hash ^= item.ammo.GetHashCode();
			hash ^= item.shootSpeed.GetHashCode();
			hash ^= item.rare.GetHashCode();
			if( !no_context ) { hash ^= item.owner.GetHashCode(); }
			hash ^= item.noUseGraphic.GetHashCode();
			hash ^= item.useAmmo.GetHashCode();
			hash ^= item.shieldSlot.GetHashCode();
			hash ^= item.stringColor.GetHashCode();
			hash ^= item.balloonSlot.GetHashCode();
			hash ^= item.faceSlot.GetHashCode();
			hash ^= item.neckSlot.GetHashCode();
			hash ^= item.noMelee.GetHashCode();
			hash ^= item.wingSlot.GetHashCode();
			hash ^= item.waistSlot.GetHashCode();
			hash ^= item.shoeSlot.GetHashCode();
			hash ^= item.frontSlot.GetHashCode();
			hash ^= item.backSlot.GetHashCode();
			hash ^= item.handOffSlot.GetHashCode();
			//hash ^= item.ToolTip.GetHashCode();
			//hash ^= item.release.GetHashCode();
			hash ^= item.buffTime.GetHashCode();
			//hash ^= item.buy.GetHashCode();	//?
			//hash ^= item.newAndShiny.GetHashCode();	//?
			hash ^= item.reuseDelay.GetHashCode();
			hash ^= item.sentry.GetHashCode();
			hash ^= item.summon.GetHashCode();
			hash ^= item.thrown.GetHashCode();
			hash ^= item.ranged.GetHashCode();
			hash ^= item.magic.GetHashCode();
			hash ^= item.melee.GetHashCode();
			hash ^= item.prefix.GetHashCode();
			hash ^= item.crit.GetHashCode();
			if( !no_context ) { hash ^= item.netID.GetHashCode(); }
			hash ^= item.DD2Summon.GetHashCode();
			hash ^= ( (int)item.shopCustomPrice).GetHashCode();
			hash ^= item.shopSpecialCurrency.GetHashCode();
			hash ^= item.uniqueStack.GetHashCode();
			hash ^= item.cartTrack.GetHashCode();
			hash ^= item.mountType.GetHashCode();
			hash ^= item.handOnSlot.GetHashCode();
			hash ^= item.buffType.GetHashCode();
			hash ^= item.noWet.GetHashCode();
			hash ^= item.material.GetHashCode();
			hash ^= item.vanity.GetHashCode();
			hash ^= item.social.GetHashCode();
			hash ^= item.value.GetHashCode();
			hash ^= item.legSlot.GetHashCode();
			hash ^= item.shoot.GetHashCode();
			hash ^= item.headSlot.GetHashCode();
			hash ^= item.holdStyle.GetHashCode();
			hash ^= item.favorited.GetHashCode();	//?
			hash ^= item.type.GetHashCode();
			//hash ^= item.keepTime.GetHashCode();
			//hash ^= item.ownTime.GetHashCode();
			//hash ^= item.ownIgnore.GetHashCode();
			hash ^= item.instanced.GetHashCode();
			hash ^= item.paint.GetHashCode();
			hash ^= item.hairDye.GetHashCode();
			hash ^= item.expert.GetHashCode();
			hash ^= item.expertOnly.GetHashCode();
			hash ^= item.useStyle.GetHashCode();
			hash ^= item.makeNPC.GetHashCode();
			hash ^= item.fishingPole.GetHashCode();
			hash ^= item.dye.GetHashCode();
			hash ^= item.wornArmor.GetHashCode();
			hash ^= item.tileWand.GetHashCode();
			hash ^= item.spawnTime.GetHashCode();
			hash ^= item.isBeingGrabbed.GetHashCode();
			hash ^= item.beingGrabbed.GetHashCode();
			hash ^= item.noGrabDelay.GetHashCode();
			hash ^= item.bodySlot.GetHashCode();
			hash ^= item.flame.GetHashCode();
			hash ^= item.questItem.GetHashCode();
			hash ^= item.bait.GetHashCode();
			hash ^= item.channel.GetHashCode();
			hash ^= item.mech.GetHashCode();
			hash ^= item.useAnimation.GetHashCode();
			hash ^= item.defense.GetHashCode();
			//hash ^= item.UseSound.GetHashCode();
			hash ^= item.scale.GetHashCode();
			hash ^= item.glowMask.GetHashCode();
			hash ^= item.alpha.GetHashCode();
			hash ^= item.color.GetHashCode();
			hash ^= item.useTurn.GetHashCode();
			hash ^= item.autoReuse.GetHashCode();
			hash ^= item.consumable.GetHashCode();
			hash ^= item.potion.GetHashCode();
			hash ^= item.healMana.GetHashCode();
			hash ^= item.knockBack.GetHashCode();
			hash ^= item.healLife.GetHashCode();
			hash ^= item.placeStyle.GetHashCode();
			hash ^= item.createWall.GetHashCode();
			hash ^= item.createTile.GetHashCode();
			hash ^= item.tileBoost.GetHashCode();
			hash ^= item.hammer.GetHashCode();
			hash ^= item.axe.GetHashCode();
			hash ^= item.pick.GetHashCode();
			hash ^= item.maxStack.GetHashCode();
			hash ^= item.stack.GetHashCode();
			hash ^= item.useTime.GetHashCode();
			hash ^= item.damage.GetHashCode();
			hash ^= item.accessory.GetHashCode();
			return hash;
		}


		////////////////

		public static int CreateItem( Vector2 pos, int type, int stack, int width, int height, int prefix = 0 ) {
			int idx = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode == 1 ) {	// Client
				NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
			}
			return idx;
		}

		////////////////

		public static void DestroyItem( Item item ) {
			item.active = false;
			item.type = 0;
			//item.name = "";
			item.stack = 0;
		}

		public static void DestroyWorldItem( int idx ) {
			Item item = Main.item[idx];
			ItemHelpers.DestroyItem( item );

			if( Main.netMode == 2 ) {	// Server
				NetMessage.SendData( 21, -1, -1, null, idx );
			}
		}


		public static void ReduceStack( Item item, int amt ) {
			item.stack -= amt;

			if( item.stack <= 0 ) {
				item.TurnToAir();
				item.active = false;
			}

			if( Main.netMode != 0 && item.owner == Main.myPlayer && item.whoAmI > 0 ) {
				NetMessage.SendData( 21, -1, -1, null, item.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}

		////////////////

		public static int CalculateStandardUseTime( Item item ) {
			int use_time;

			// No exact science for this one (Note: No accommodations made for other mods' non-standard use of useTime!)
			if( item.melee || item.useTime == 0 ) {
				use_time = item.useAnimation;
			} else {
				use_time = item.useTime;
				if( item.reuseDelay > 0 ) { use_time = (use_time + item.reuseDelay) / 2; }
			}

			if( item.useTime <= 0 || item.useTime == 100 ) {    // 100 = default amount
				if( item.useAnimation > 0 && item.useAnimation != 100 ) {   // 100 = default amount
					use_time = item.useAnimation;
				} else {
					use_time = 100;
				}
			}

			return use_time;
		}


		////////////////

		[System.Obsolete( "use PlayerItemHelpers.GetGrappleItem", true )]
		public static Item GetGrappleItem( Player player ) {
			return PlayerItemHelpers.GetGrappleItem( player );
		}
	}
}
