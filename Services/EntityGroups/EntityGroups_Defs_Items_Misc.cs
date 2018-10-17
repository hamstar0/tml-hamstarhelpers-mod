using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemMiscGroups4( Action<string, string[], Matcher> add_def ) {
			add_def( "Any Item", null,
				( item, grps ) => {
					return true;
				} );

			for( int i = -12; i <= ItemAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				add_def( "Any " + ItemAttributeHelpers.RarityColorText[i] + " Tier", null,
					( item, grps ) => {
						return item.rare == tier;
					} );
			}

			add_def( "Any Plain Material", new string[] { "Any Equipment" },
				( item, grps ) => {
					return item.material &&
						//!EntityGroups.ItemGroups["Any Placeable"].Contains( item.type ) &&
						!grps["Any Equipment"].Contains( item.type );
				} );

			add_def( "Any Vanilla Corruption Item", null,
				( item, grps ) => {
					switch( item.type ) {
					case ItemID.Ebonwood:
					case ItemID.EbonsandBlock:
					case ItemID.CorruptSandstone:
					case ItemID.CorruptSandstoneWall:
					case ItemID.CorruptHardenedSand:
					case ItemID.CorruptHardenedSandWall:
					case ItemID.EbonstoneBlock:
					case ItemID.DemoniteBrick:
					case ItemID.PurpleIceBlock:

					case ItemID.CorruptSeeds:
					case ItemID.VileMushroom:
					case ItemID.VilePowder:
					case ItemID.Deathweed:
					case ItemID.DeathweedSeeds:
					case ItemID.RottenChunk:
					case ItemID.WormTooth:
					case ItemID.SoulofNight:
					case ItemID.DarkShard:

					case ItemID.ShadowScale:
					case ItemID.EaterMask:
					case ItemID.EaterofWorldsTrophy:
					case ItemID.EatersBone:

					case ItemID.CorruptionKey:
					case ItemID.CorruptionKeyMold://?

					case ItemID.AncientShadowGreaves:
					case ItemID.AncientShadowHelmet:
					case ItemID.AncientShadowScalemail:
					case ItemID.BallOHurt:
					case ItemID.Musket:
					case ItemID.ShadowOrb:
					case ItemID.Vilethorn:
					case ItemID.BandofStarpower:

					case ItemID.ChainGuillotines:
					case ItemID.ClingerStaff:
					case ItemID.DartRifle:
					case ItemID.PutridScent:

					case ItemID.WormFood:
					case ItemID.NightsEdge:
					case ItemID.TrueNightsEdge:

					case ItemID.Ebonkoi:
					case ItemID.PurpleClubberfish:
					case ItemID.Toxikarp:
					case ItemID.CorruptFishingCrate:
						return true;
					}

					if( item.type <= ItemID.Count ) {
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.VilePowder }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteOre }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteBar }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ShadowScale }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CursedFlames }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Ebonwood }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PurpleIceBlock }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.EbonstoneBlock }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.EbonstoneBrick }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteBrick }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CorruptSeeds }, 1 ) ) {
							return true;
						}
					}

					return false;
				} );

			add_def( "Any Vanilla Crimson Item", null,
				( item, grps ) => {
					switch( item.type ) {
					case ItemID.Shadewood:
					case ItemID.CrimsandBlock:
					case ItemID.CrimsonSandstone:
					case ItemID.CrimsonSandstoneWall:
					case ItemID.CrimsonHardenedSand:
					case ItemID.CrimsonHardenedSandWall:
					case ItemID.CrimstoneBlock:
					case ItemID.CrimtaneBrick:
					case ItemID.RedIceBlock:
					case ItemID.FleshBlock:

					case ItemID.CrimsonSeeds:
					case ItemID.ViciousMushroom:
					case ItemID.ViciousPowder:
					case ItemID.Deathweed:
					case ItemID.DeathweedSeeds:
					case ItemID.Vertebrae:
					case ItemID.SoulofNight:
					case ItemID.DarkShard:
					case ItemID.MeatGrinder:

					case ItemID.TissueSample:
					case ItemID.BrainMask:
					case ItemID.BrainofCthulhuTrophy:
					case ItemID.BoneRattle:

					case ItemID.CrimsonKey:
					case ItemID.CrimsonKeyMold://?
				
					case ItemID.TheUndertaker:
					case ItemID.TheRottedFork:
					case ItemID.CrimsonRod:
					case ItemID.PanicNecklace:
					case ItemID.CrimsonHeart:

					case ItemID.SoulDrain:
					case ItemID.DartPistol:
					case ItemID.FetidBaghnakhs:
					case ItemID.FleshKnuckles:
					case ItemID.TendonHook:

					case ItemID.BloodySpine:
					case ItemID.NightsEdge:
					case ItemID.TrueNightsEdge:

					case ItemID.CrimsonTigerfish:
					case ItemID.Hemopiranha:
					case ItemID.CrimsonFishingCrate:
					case ItemID.Bladetongue:
						return true;
					}

					if( item.type <= ItemID.Count ) {
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ViciousPowder }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Vertebrae }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneOre }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneBar }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Ichor }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TissueSample }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Shadewood }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.RedIceBlock }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimstoneBlock }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneBrick }, 1 ) ) {
							return true;
						}
						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FleshBlock }, 1 ) ) {
							return true;
						}

						if( RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimsonSeeds }, 1 ) ) {
							return true;
						}
					}

					return false;
				} );
		}
	}
}
