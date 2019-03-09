using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemEquipmentGroups1( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Weapon", null,
				( item, grps ) => {
					return item.damage > 0;
				} );
			addDef( "Any Tool", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsTool( item );
				} );
			addDef( "Any Vanilla Explosive", null,
				( item, grps ) => {
					switch( item.type ) {
					case ItemID.Bomb:
					case ItemID.StickyBomb:
					case ItemID.BouncyBomb:
					case ItemID.Dynamite:
					case ItemID.StickyDynamite:
					case ItemID.BouncyDynamite:
					case ItemID.Grenade:
					case ItemID.StickyGrenade:
					case ItemID.BouncyGrenade:
					case ItemID.BombFish:
					case ItemID.PartyGirlGrenade:
					case ItemID.Explosives:	//?
					case ItemID.LandMine:   //?
					case ItemID.RocketI:
					case ItemID.RocketII:
					case ItemID.RocketIII:
					case ItemID.RocketIV:
					case ItemID.StyngerBolt:
					case ItemID.HellfireArrow:
					case ItemID.ExplosiveJackOLantern:
					case ItemID.ExplosiveBunny:
					case ItemID.Cannonball:
					case ItemID.Beenade:	//?
						return true;
					}
					return false;
				} );

			addDef( "Any Accessory", null,
				( item, grps ) => {
					return item.accessory && !item.vanity;
				} );
			addDef( "Any Armor", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsArmor( item );
				} );
			addDef( "Any Vanity", null,
				( item, grps ) => {
					return item.vanity;
				} );
			addDef( "Any Potion", null,
				( item, grps ) => {
					return item.potion;
				} );
			
			// Vanity Classes

			addDef( "Any Vanity Accessory", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} );
			addDef( "Any Vanity Garment", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return !item.accessory;
				} );
		}


		private void DefineItemEquipmentGroups2( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Equipment",
				new string[] { "Any Weapon", "Any Tool", "Any Accessory", "Any Armor" },
				( item, grps ) => {
					return grps["Any Weapon"].Contains( item.type ) ||
						grps["Any Tool"].Contains( item.type ) ||
						grps["Any Accessory"].Contains( item.type ) ||
						grps["Any Armor"].Contains( item.type );
				}
			);
		}


		private void DefineItemEquipmentGroups3( Action<string, string[], Matcher> addDef ) {
			// Equipment Tiers

			addDef( "Any Tiki Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					string name = ItemIdentityHelpers.GetQualifiedName( item );
					if( !hasEquip || !name.Contains( "Tiki" ) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Cactus Equipment",
				new string[] { "Any Equipment", },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Cactus }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				}
			);

			addDef( "Any Plain Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Wood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Boreal Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BorealWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Palm Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalmWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Rich Mahogany Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.RichMahogany }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Ebonwood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Ebonwood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shadewood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Shadewood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Pearlwood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Pearlwood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Dynasty Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DynastyWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Spooky Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpookyWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Tin Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TinBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Copper Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CopperBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Iron Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.IronBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Lead Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LeadBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Silver Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SilverBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Tungsten Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TungstenBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Gold Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.GoldBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Platinum Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PlatinumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Meteor Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MeteoriteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Demonite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Crimtane Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Jungle Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.JungleSpores }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Bee Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeeWax }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Bone Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Bone }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Hellstone Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HellstoneBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Cobalt Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CobaltBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Palladium Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalladiumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Mythril Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MythrilBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Orichalcum Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.OrichalcumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Adamantite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AdamantiteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Titanium Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TitaniumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Frost Core Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FrostCore }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Forbidden Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AncientBattleArmorMaterial }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Hallow Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HallowedBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Chlorophyte Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ChlorophyteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shroomite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ShroomiteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Spectre Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpectreBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shell Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has1 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeetleShell }, 1 );
					var has2 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TurtleShell }, 1 );
					if( !hasEquip || (!has1 && !has2) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Nebula Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentNebula }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Vortex Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentVortex }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Solar Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentSolar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Stardust Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentStardust }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Luminite Ore Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LunarBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
		}


		private void DefineItemEquipmentGroups4( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Wood Equipment",
				new string[] { "Any Equipment", "Any Wood" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					bool hasWood = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Wood"], 1 );
					if( !hasEquip || !hasWood ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				}
			);
			addDef( "Any Copper Or Tin Equipment",
				new string[] { "Any Copper Equipment", "Any Tin Equipment" },
				( item, grps ) => {
					return grps["Any Copper Equipment"].Contains( item.type ) ||
						grps["Any Tin Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Iron Or Lead Equipment",
				new string[] { "Any Iron Equipment", "Any Lead Equipment" },
				( item, grps ) => {
					return grps["Any Iron Equipment"].Contains( item.type ) ||
						grps["Any Lead Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Silver Or Tungsten Equipment",
				new string[] { "Any Silver Equipment", "Any Tungsten Equipment" },
				( item, grps ) => {
					return grps["Any Silver Equipment"].Contains( item.type ) ||
						grps["Any Tungsten Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Gold Or Platinum Equipment",
				new string[] { "Any Gold Equipment", "Any Platinum Equipment" },
				( item, grps ) => {
					return grps["Any Gold Equipment"].Contains( item.type ) ||
						grps["Any Platinum Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Demonite Or Crimtane Equipment",
				new string[] { "Any Demonite Equipment", "Any Crimtane Equipment" },
				( item, grps ) => {
					return grps["Any Demonite Equipment"].Contains( item.type ) ||
						grps["Any Crimtane Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Meteor Or Jungle Or Bone Or Bee Equipment",
				new string[] { "Any Meteor Equipment", "Any Jungle Equipment", "Any Bone Equipment", "Any Bee Equipment" },
				( item, grps ) => {
					return grps["Any Meteor Equipment"].Contains( item.type ) ||
						grps["Any Jungle Equipment"].Contains( item.type ) ||
						grps["Any Bone Equipment"].Contains( item.type ) ||
						grps["Any Bee Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Cobalt Or Palladium Equipment",
				new string[] { "Any Cobalt Equipment", "Any Palladium Equipment" },
				( item, grps ) => {
					return grps["Any Cobalt Equipment"].Contains( item.type ) ||
						grps["Any Palladium Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Mythril Or Orichalcum Equipment",
				new string[] { "Any Mythril Equipment", "Any Orichalcum Equipment" },
				( item, grps ) => {
					return grps["Any Mythril Equipment"].Contains( item.type ) ||
						grps["Any Orichalcum Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Adamantite Or Titanium Equipment",
				new string[] { "Any Adamantite Equipment", "Any Titanium Equipment" },
				( item, grps ) => {
					return grps["Any Adamantite Equipment"].Contains( item.type ) ||
						grps["Any Titanium Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Frost Core Or Forbidden Equipment",
				new string[] { "Any Frost Core Equipment", "Any Forbidden Equipment" },
				( item, grps ) => {
					return grps["Any Frost Core Equipment"].Contains( item.type ) ||
						grps["Any Forbidden Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Chlorophyte Or Shroomite Or Spectre Equipment",
				new string[] { "Any Chlorophyte Equipment", "Any Shroomite Equipment", "Any Spectre Equipment" },
				( item, grps ) => {
					return grps["Any Chlorophyte Equipment"].Contains( item.type ) ||
						grps["Any Shroomite Equipment"].Contains( item.type ) ||
						grps["Any Spectre Equipment"].Contains( item.type );
				}
			);
			addDef( "Any Celestial Equipment",
				new string[] { "Any Nebula Equipment", "Any Vortex Equipment", "Any Solar Equipment", "Any Stardust Equipment" },
				( item, grps ) => {
					return grps["Any Nebula Equipment"].Contains( item.type ) ||
						grps["Any Vortex Equipment"].Contains( item.type ) ||
						grps["Any Solar Equipment"].Contains( item.type ) ||
						grps["Any Stardust Equipment"].Contains( item.type );
				}
			);
		}
	}
}
