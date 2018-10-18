using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemEquipmentGroups1( Action<string, string[], Matcher> add_def ) {
			add_def( "Any Weapon", null,
				( item, grps ) => {
					return item.damage > 0;
				} );
			add_def( "Any Tool", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsTool( item );
				} );
			add_def( "Any Vanilla Explosive", null,
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

			add_def( "Any Accessory", null,
				( item, grps ) => {
					return item.accessory && !item.vanity;
				} );
			add_def( "Any Armor", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsArmor( item );
				} );
			add_def( "Any Vanity", null,
				( item, grps ) => {
					return item.vanity;
				} );
			add_def( "Any Potion", null,
				( item, grps ) => {
					return item.potion;
				} );
			
			// Vanity Classes

			add_def( "Any Vanity Accessory", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} );
			add_def( "Any Vanity Garment", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return !item.accessory;
				} );
		}


		private void DefineItemEquipmentGroups2( Action<string, string[], Matcher> add_def ) {
			// Equipment Tiers

			add_def( "Any Tiki Equipment", null,
				( item, grps ) => {
					string name = ItemIdentityHelpers.GetQualifiedName( item );
					if( !name.Contains( "Tiki" ) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Plain Wood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Wood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Boreal Wood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BorealWood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Palm Wood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalmWood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Rich Mahogany Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.RichMahogany }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Ebonwood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Ebonwood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Shadewood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Shadewood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Pearlwood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Pearlwood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Dynasty Wood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DynastyWood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Spooky Wood Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpookyWood }, 2 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Tin Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TinBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Copper Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CopperBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Iron Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.IronBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Lead Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LeadBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Silver Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SilverBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Tungsten Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TungstenBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Gold Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.GoldBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Platinum Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PlatinumBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Meteor Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MeteoriteBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Demonite Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Crimtane Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Jungle Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.JungleSpores }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Bee Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeeWax }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Bone Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Bone }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Hellstone Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HellstoneBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Cobalt Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CobaltBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Palladium Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalladiumBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Mythril Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MythrilBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Orichalcum Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.OrichalcumBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Adamantite Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AdamantiteBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Titanium Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TitaniumBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Frost Core Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FrostCore }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Forbidden Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AncientBattleArmorMaterial }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Hallow Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HallowedBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Chlorophyte Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ChlorophyteBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Shroomite Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ShroomiteBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Spectre Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpectreBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Shell Equipment", null,
				( item, grps ) => {
					var has1 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeetleShell }, 1 );
					var has2 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TurtleShell }, 1 );
					if( !has1 && !has2 ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			add_def( "Any Nebula Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentNebula }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Vortex Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentVortex }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Solar Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentSolar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Stardust Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentStardust }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			add_def( "Any Luminite Ore Equipment", null,
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LunarBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
		}


		private void DefineItemEquipmentGroups3( Action<string, string[], Matcher> add_def ) {
			add_def( "Any Equipment",
				new string[] { "Any Weapon", "Any Tool", "Any Accessory", "Any Armor" },
				( item, grps ) => {
					return grps["Any Weapon"].Contains( item.type ) ||
						grps["Any Tool"].Contains( item.type ) ||
						grps["Any Accessory"].Contains( item.type ) ||
						grps["Any Armor"].Contains( item.type );
				}
			);

			add_def( "Any Wood Equipment",
				new string[] { "Any Wood" },
				( item, grps ) => {
					var has = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Wood"], 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				}
			);
			add_def( "Any Copper Or Tin Equipment",
				new string[] { "Any Copper Equipment", "Any Tin Equipment" },
				( item, grps ) => {
					return grps["Any Copper Equipment"].Contains( item.type ) ||
						grps["Any Tin Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Iron Or Lead Equipment",
				new string[] { "Any Iron Equipment", "Any Lead Equipment" },
				( item, grps ) => {
					return grps["Any Iron Equipment"].Contains( item.type ) ||
						grps["Any Lead Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Silver Or Tungsten Equipment",
				new string[] { "Any Silver Equipment", "Any Tungsten Equipment" },
				( item, grps ) => {
					return grps["Any Silver Equipment"].Contains( item.type ) ||
						grps["Any Tungsten Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Gold Or Platinum Equipment",
				new string[] { "Any Gold Equipment", "Any Platinum Equipment" },
				( item, grps ) => {
					return grps["Any Gold Equipment"].Contains( item.type ) ||
						grps["Any Platinum Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Demonite Or Crimtane Equipment",
				new string[] { "Any Demonite Equipment", "Any Crimtane Equipment" },
				( item, grps ) => {
					return grps["Any Demonite Equipment"].Contains( item.type ) ||
						grps["Any Crimtane Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Meteor Or Jungle Or Bone Or Bee Equipment",
				new string[] { "Any Meteor Equipment", "Any Jungle Equipment", "Any Bone Equipment", "Any Bee Equipment" },
				( item, grps ) => {
					return grps["Any Meteor Equipment"].Contains( item.type ) ||
						grps["Any Jungle Equipment"].Contains( item.type ) ||
						grps["Any Bone Equipment"].Contains( item.type ) ||
						grps["Any Bee Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Cobalt Or Palladium Equipment",
				new string[] { "Any Cobalt Equipment", "Any Palladium Equipment" },
				( item, grps ) => {
					return grps["Any Cobalt Equipment"].Contains( item.type ) ||
						grps["Any Palladium Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Mythril Or Orichalcum Equipment",
				new string[] { "Any Mythril Equipment", "Any Orichalcum Equipment" },
				( item, grps ) => {
					return grps["Any Mythril Equipment"].Contains( item.type ) ||
						grps["Any Orichalcum Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Adamantite Or Titanium Equipment",
				new string[] { "Any Adamantite Equipment", "Any Titanium Equipment" },
				( item, grps ) => {
					return grps["Any Adamantite Equipment"].Contains( item.type ) ||
						grps["Any Titanium Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Frost Core Or Forbidden Equipment",
				new string[] { "Any Frost Core Equipment", "Any Forbidden Equipment" },
				( item, grps ) => {
					return grps["Any Frost Core Equipment"].Contains( item.type ) ||
						grps["Any Forbidden Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Chlorophyte Or Shroomite Or Spectre Equipment",
				new string[] { "Any Chlorophyte Equipment", "Any Shroomite Equipment", "Any Spectre Equipment" },
				( item, grps ) => {
					return grps["Any Chlorophyte Equipment"].Contains( item.type ) ||
						grps["Any Shroomite Equipment"].Contains( item.type ) ||
						grps["Any Spectre Equipment"].Contains( item.type );
				}
			);
			add_def( "Any Celestial Equipment",
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
