﻿using HamstarHelpers.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			if( item.type == ItemID.Coal ) {
				var mymod = ModHelpersMod.Instance;

				if( mymod.Config.CoalAsTile ) {
					item.maxStack = 999;
					item.rare = 2;
					item.value = 1000;

					item.useStyle = 1;
					item.useTurn = true;
					item.useAnimation = 15;
					item.useTime = 10;
					item.autoReuse = true;

					item.consumable = true;
					item.createTile = mymod.TileType<CoalTile>();
				}
			}

			base.SetDefaults( item );
		}
	}
}
