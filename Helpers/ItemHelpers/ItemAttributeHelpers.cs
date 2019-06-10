using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Items {
	public static partial class ItemAttributeHelpers {
		private static IDictionary<int, int> _ProjPene = new Dictionary<int, int>();



		////////////////

		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= 0 ) { return false; }

			if( !ItemAttributeHelpers._ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				ItemAttributeHelpers._ProjPene[ item.shoot ] = proj.penetrate;
			}

			return  ItemAttributeHelpers._ProjPene[ item.shoot ] == -1 ||
					ItemAttributeHelpers._ProjPene[ item.shoot ] >= 3;   // 3 seems fair?
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


		public static bool IsGameplayRelevant( Item item, bool toysRelevant = false, bool junkRelevant = false ) {
			if( !toysRelevant ) {
				switch( item.type ) {
				case ItemID.WaterGun:
				case ItemID.SlimeGun:
				case ItemID.BeachBall:
					return false;
				}
			}
			if( junkRelevant && item.rare < 0 ) { return false; }
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


		public static string GetContainerContext( Item item ) {
			if( ( item.type >= 3318 && item.type <= 3332 )
					|| item.type == 3860 || item.type == 3862 || item.type == 3861
					|| ItemLoader.IsModBossBag( item ) ) {
				return "bossBag";
			}
			if( ( item.type >= 2334 && item.type <= 2336 )
					|| ( item.type >= 3203 && item.type <= 3208 ) ) {
				return "crate";
			}
			if( item.type == ItemID.HerbBag ) {
				return "herbBag";
			}
			if( item.type == ItemID.GoodieBag ) {
				return "goodieBag";
			}
			if( item.type == ItemID.LockBox ) {
				return "lockBox";
			}
			if( item.type == ItemID.Present ) {
				return "present";
			}
			return "";
		}

		public static int[] GetContainerItemTypes( string context ) {
			if( context == "bossBag" ) {
				return new int[] { 3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327, 3328, 3329, 3330, 3331, 3332,
					3860, 3862, 3861 };
			}
			if( context == "crate" ) {
				return new int[] { 2334, 2335, 2336,
					3203, 3204, 3205, 3206, 3207, 3208 };
			}
			if( context == "herbBag" ) {
				return new int[] { ItemID.HerbBag };
			}
			if( context == "goodieBag" ) {
				return new int[] { ItemID.GoodieBag };
			}
			if( context == "lockBox" ) {
				return new int[] { ItemID.LockBox };
			}
			if( context == "present" ) {
				return new int[] { ItemID.Present };
			}
			return new int[] { };
		}
	}
}
