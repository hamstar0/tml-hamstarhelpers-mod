using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public static class ItemAttributeHelpers {
		public const int HighestVanillaRarity = 11;
		public const int JunkRarity = -1;
		public const int QuestItemRarity = -11;


		////////////////

		public static Color GetRarityColor( int rarity ) {
			switch( rarity ) {
			case -1:
				return new Color( 130, 130, 130 );
			case 1:
				return new Color( 150, 150, 255 );
			case 2:
				return new Color( 150, 255, 150 );
			case 3:
				return new Color( 255, 200, 150 );
			case 4:
				return new Color( 255, 150, 150 );
			case 5:
				return new Color( 255, 150, 255 );
			case 6:
				return new Color( 210, 160, 255 );
			case 7:
				return new Color( 150, 255, 10 );
			case 8:
				return new Color( 255, 255, 10 );
			case 9:
				return new Color( 5, 200, 255 );
			case 10:
				return new Color( 255, 40, 100 );
			case 11:
				return new Color( 180, 40, 255 );
			case -11:
				return new Color( 255, 175, 0 );
			}
			return Color.White;
		}


		////////////////

		 private static IDictionary<int, int> ProjPene = new Dictionary<int, int>();

		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= 0 ) { return false; }

			if( !ItemAttributeHelpers.ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				ItemAttributeHelpers.ProjPene[ item.shoot ] = proj.penetrate;
			}

			return  ItemAttributeHelpers.ProjPene[ item.shoot ] == -1 ||
					ItemAttributeHelpers.ProjPene[ item.shoot ] >= 3;   // 3 seems fair?
		}


		public static bool IsTool( Item item ) {
			return ( item.useStyle > 0 ||
					item.damage > 0 ||
					item.crit > 0 ||
					item.knockBack > 0 ||
					item.melee ||
					item.magic ||
					item.ranged ||
					item.thrown ||
					item.summon ||
					item.pick > 0 ||
					item.hammer > 0 ||
					item.axe > 0 ) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity &&
				item.type != 849;   // Actuators are not consumable, apparently
		}


		public static bool IsArmor( Item item ) {
			return ( item.headSlot != -1 ||
					item.bodySlot != -1 ||
					item.legSlot != -1 ) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity;
		}


		public static bool IsGrapple( Item item ) {
			return Main.projHook[item.shoot];
		}


		public static bool IsYoyo( Item item ) {
			if( item.shoot > 0 && item.useStyle == 5 && item.melee && item.channel ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				return proj.aiStyle == 99;
			}
			return false;
		}


		public static bool IsGameplayRelevant( Item item, bool toys_relevant = false, bool junk_relevant = false ) {
			if( !toys_relevant ) {
				switch( item.type ) {
				case ItemID.WaterGun:
				case ItemID.SlimeGun:
				case ItemID.BeachBall:
					return false;
				}
			}
			if( junk_relevant && item.rare < 0 ) { return false; }
			return !item.vanity && item.dye <= 0 && item.hairDye <= 0 && item.paint == 0 && !Main.vanityPet[item.buffType];
		}


		public static float LooselyAppraise( Item item ) {
			float appraisal = item.rare;
			if( item.value > 0 ) {
				float value = (float)item.value / 8000f;
				appraisal = ( ( appraisal * 4f ) + value ) / 5f;
			}
			return appraisal;
		}
	}
}
