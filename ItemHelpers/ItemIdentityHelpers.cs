using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static class ItemIdentityHelpers {
		private static IDictionary<int, int> ProjPene = new Dictionary<int, int>();

		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= 0 ) { return false; }

			if( !ItemIdentityHelpers.ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );
				ItemIdentityHelpers.ProjPene[item.shoot] = proj.penetrate;
			}

			return ItemIdentityHelpers.ProjPene[item.shoot] == -1 || ItemIdentityHelpers.ProjPene[item.shoot] >= 3;   // 3 seems fair?
		}


		public static bool IsTool( Item item ) {
			return (item.useStyle > 0 ||
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
					item.axe > 0) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity &&
				item.type != 849;   // Actuators are not consumable, apparently
		}


		public static bool IsArmor( Item item ) {
			return (item.headSlot != -1 ||
					item.bodySlot != -1 ||
					item.legSlot != -1) &&
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
	}
}
