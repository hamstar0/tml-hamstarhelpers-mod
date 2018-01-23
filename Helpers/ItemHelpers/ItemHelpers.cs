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

			string id = ItemHelpers.GetUniqueId( item );

			hash ^= ("id"+id).GetHashCode();
			hash ^= ("mana"+item.mana).GetHashCode();
			//hash ^= ("buyOnce"+item.buyOnce).GetHashCode();	//?
			hash ^= ("manaIncrease"+item.manaIncrease).GetHashCode();
			hash ^= ("lifeRegen"+item.lifeRegen).GetHashCode();
			hash ^= ("notAmmo"+item.notAmmo).GetHashCode();
			hash ^= ("ammo"+item.ammo).GetHashCode();
			hash ^= ("shootSpeed"+item.shootSpeed).GetHashCode();
			hash ^= ("rare"+item.rare).GetHashCode();
			if( !no_context ) { hash ^= ("owner"+item.owner).GetHashCode(); }
			hash ^= ("noUseGraphic"+item.noUseGraphic).GetHashCode();
			hash ^= ("useAmmo"+item.useAmmo).GetHashCode();
			hash ^= ("shieldSlot"+item.shieldSlot).GetHashCode();
			hash ^= ("stringColor"+item.stringColor).GetHashCode();
			hash ^= ("balloonSlot"+item.balloonSlot).GetHashCode();
			hash ^= ("faceSlot"+item.faceSlot).GetHashCode();
			hash ^= ("neckSlot"+item.neckSlot).GetHashCode();
			hash ^= ("noMelee"+item.noMelee).GetHashCode();
			hash ^= ("wingSlot"+item.wingSlot).GetHashCode();
			hash ^= ("waistSlot"+item.waistSlot).GetHashCode();
			hash ^= ("shoeSlot"+item.shoeSlot).GetHashCode();
			hash ^= ("frontSlot"+item.frontSlot).GetHashCode();
			hash ^= ("backSlot"+item.backSlot).GetHashCode();
			hash ^= ("handOffSlot"+item.handOffSlot).GetHashCode();
			//hash ^= ("ToolTip"+item.ToolTip).GetHashCode();
			//hash ^= ("release"+item.release).GetHashCode();
			hash ^= ("buffTime"+item.buffTime).GetHashCode();
			//hash ^= ("buy"+item.buy).GetHashCode();	//?
			//hash ^= ("newAndShiny"+item.newAndShiny).GetHashCode();	//?
			hash ^= ("reuseDelay"+item.reuseDelay).GetHashCode();
			hash ^= ("sentry"+item.sentry).GetHashCode();
			hash ^= ("summon"+item.summon).GetHashCode();
			hash ^= ("thrown"+item.thrown).GetHashCode();
			hash ^= ("ranged"+item.ranged).GetHashCode();
			hash ^= ("magic"+item.magic).GetHashCode();
			hash ^= ("melee"+item.melee).GetHashCode();
			hash ^= ("prefix"+item.prefix).GetHashCode();
			hash ^= ("crit"+item.crit).GetHashCode();
			if( !no_context ) { hash ^= ("netID"+item.netID).GetHashCode(); }
			hash ^= ("DD2Summon"+item.DD2Summon).GetHashCode();
			hash ^= ("shopCustomPrice"+(int)item.shopCustomPrice).GetHashCode();
			hash ^= ("shopSpecialCurrency"+item.shopSpecialCurrency).GetHashCode();
			hash ^= ("uniqueStack"+item.uniqueStack).GetHashCode();
			hash ^= ("cartTrack"+item.cartTrack).GetHashCode();
			hash ^= ("mountType"+item.mountType).GetHashCode();
			hash ^= ("handOnSlot"+item.handOnSlot).GetHashCode();
			hash ^= ("buffType"+item.buffType).GetHashCode();
			hash ^= ("noWet"+item.noWet).GetHashCode();
			hash ^= ("material"+item.material).GetHashCode();
			hash ^= ("vanity"+item.vanity).GetHashCode();
			hash ^= ("social"+item.social).GetHashCode();
			hash ^= ("value"+item.value).GetHashCode();
			hash ^= ("legSlot"+item.legSlot).GetHashCode();
			hash ^= ("shoot"+item.shoot).GetHashCode();
			hash ^= ("headSlot"+item.headSlot).GetHashCode();
			hash ^= ("holdStyle"+item.holdStyle).GetHashCode();
			if( !no_context ) { hash ^= ("favorited"+item.favorited).GetHashCode(); }
			//////hash ^= ("type"+item.type).GetHashCode();
			//hash ^= ("keepTime"+item.keepTime).GetHashCode();
			//hash ^= ("ownTime"+item.ownTime).GetHashCode();
			//hash ^= ("ownIgnore"+item.ownIgnore).GetHashCode();
			hash ^= ("instanced"+item.instanced).GetHashCode();
			hash ^= ("paint"+item.paint).GetHashCode();
			hash ^= ("hairDye"+item.hairDye).GetHashCode();
			hash ^= ("expert"+item.expert).GetHashCode();
			hash ^= ("expertOnly"+item.expertOnly).GetHashCode();
			hash ^= ("useStyle"+item.useStyle).GetHashCode();
			hash ^= ("makeNPC"+item.makeNPC).GetHashCode();
			hash ^= ("fishingPole"+item.fishingPole).GetHashCode();
			hash ^= ("dye"+item.dye).GetHashCode();
			hash ^= ("wornArmor"+item.wornArmor).GetHashCode();
			hash ^= ("tileWand"+item.tileWand).GetHashCode();
			hash ^= ("spawnTime"+item.spawnTime).GetHashCode();
			//hash ^= ("isBeingGrabbed"+item.isBeingGrabbed).GetHashCode();
			//hash ^= ("beingGrabbed"+item.beingGrabbed).GetHashCode();
			//hash ^= ("noGrabDelay"+item.noGrabDelay).GetHashCode();
			hash ^= ("bodySlot"+item.bodySlot).GetHashCode();
			hash ^= ("flame"+item.flame).GetHashCode();
			hash ^= ("questItem"+item.questItem).GetHashCode();
			hash ^= ("bait"+item.bait).GetHashCode();
			hash ^= ("channel"+item.channel).GetHashCode();
			hash ^= ("mech"+item.mech).GetHashCode();
			hash ^= ("useAnimation"+item.useAnimation).GetHashCode();
			hash ^= ("defense"+item.defense).GetHashCode();
			//hash ^= ("UseSound"+item.UseSound).GetHashCode();
			hash ^= ("scale"+item.scale).GetHashCode();
			hash ^= ("glowMask"+item.glowMask).GetHashCode();
			hash ^= ("alpha"+item.alpha).GetHashCode();
			hash ^= ("color"+item.color).GetHashCode();
			hash ^= ("useTurn"+item.useTurn).GetHashCode();
			hash ^= ("autoReuse"+item.autoReuse).GetHashCode();
			hash ^= ("consumable"+item.consumable).GetHashCode();
			hash ^= ("potion"+item.potion).GetHashCode();
			hash ^= ("healMana"+item.healMana).GetHashCode();
			hash ^= ("knockBack"+item.knockBack).GetHashCode();
			hash ^= ("healLife"+item.healLife).GetHashCode();
			hash ^= ("placeStyle"+item.placeStyle).GetHashCode();
			hash ^= ("createWall"+item.createWall).GetHashCode();
			hash ^= ("createTile"+item.createTile).GetHashCode();
			hash ^= ("tileBoost"+item.tileBoost).GetHashCode();
			hash ^= ("hammer"+item.hammer).GetHashCode();
			hash ^= ("axe"+item.axe).GetHashCode();
			hash ^= ("pick"+item.pick).GetHashCode();
			hash ^= ("maxStack"+item.maxStack).GetHashCode();
			hash ^= ("stack"+item.stack).GetHashCode();
			hash ^= ("useTime"+item.useTime).GetHashCode();
			hash ^= ("damage"+item.damage).GetHashCode();
			hash ^= ("accessory"+item.accessory).GetHashCode();

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
