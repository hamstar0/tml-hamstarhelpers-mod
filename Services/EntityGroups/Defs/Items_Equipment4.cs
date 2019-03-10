using HamstarHelpers.Helpers.RecipeHelpers;
using System;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups4( Action<string, string[], Matcher> addDef ) {
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
