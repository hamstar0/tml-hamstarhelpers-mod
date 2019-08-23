using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.ModTags.Base.Manager {
	class TagDefinition {
		public string Tag;
		public string Category;
		public string Description;
		public ISet<string> ForcesTags;
		public ISet<string> ExcludesOnAdd;

		public TagDefinition( string tag,
				string category,
				string description,
				ISet<string> forcesTag =null,
				ISet<string> excludesOnAdd = null ) {
			this.Tag = tag;
			this.Category = category;
			this.Description = description;
			this.ForcesTags = forcesTag ?? new HashSet<string>();
			this.ExcludesOnAdd = excludesOnAdd ?? new HashSet<string>();
		}
	}




	abstract partial class ModTagsManager {
		public readonly static IDictionary<string, string> CategoryDescriptions;
		public readonly static IDictionary<string, TagDefinition> TagMap;
		public readonly static TagDefinition[] Tags;


		static ModTagsManager() {
			TagDefinition m( string tag,
					string category,
					string desc,
					ISet<string> forces =null,
					ISet<string> excludesOnAdd=null ) {
				var def = new TagDefinition( tag, category, desc, forces, excludesOnAdd );
				return def;
			}

			////

			ModTagsManager.CategoryDescriptions = new Dictionary<string, string> {
				{ "Specifications",	"General descriptions of a mod." },
				{ "Mechanics",		"Describes what game mechanics are associated with a mod." },
				{ "Gameplay",		"Describes how a mod affects gameplay (more than specific mechanics)." },
				{ "State",			"Describes the existential state of a mod." },
				{ "Privilege",		"Indicates what manner of user or system priviledges a mod needs to function." },
				{ "Content",		"Describes what types of content a mod features." },
				{ "Theme",			"Describes the apparent thematic elements of a mod." },
				{ "When",			"Indicates what part of the game's (vanilla) progression a mod most pertains to." },
				{ "Where",          "Indicates where in a world the mod specifically emphasizes relevance to." },
				{ "Judgmental",		"Wholly-subjective tags. Must be enabled in settings." }
			};

			////

			var list = new List<TagDefinition> {
				m( "Core Game",				"Mechanics",	"Adds a \"bullet hell\" mode, adds a stamina bar, removes mining, etc." ),
				m( "Combat",				"Mechanics",    "Adds weapon reloading, dual-wielding, monster AI changes, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Crafting",				"Mechanics",    "Adds bulk crafting, UI-based crafting, item drag-and-drop crafting, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Movement",				"Mechanics",    "Gives super speed, adds dodging mechanics, adds unlimited flight, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Mining",				"Mechanics",    "Adds fast tunneling, area mining, shape cutting, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Building",				"Mechanics",    "Add fast building options, measuring tools, world editing, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Traveling",				"Mechanics",    "Adds fast travel options, new types of minecarts/travel-mounts, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Misc. Interactions",	"Mechanics",    "Adds new block interactions, fishing mechanics, NPC dialogues, etc.",
					new HashSet<string> { "Core Game" } ),
				m( "Item Storage",          "Mechanics",    "Adds or changes chest behavior, adds item sharing, global organization, etc." ),
				m( "Item Equiping",         "Mechanics",    "Dual wielding, additional accessory slots, equipment management, etc."),
				m( "Item Stats",            "Mechanics",    "Adjusts item damage, defense, price, etc." ),
				m( "Item Behavior",         "Mechanics",    "Changes item projectile type or quantity, item class, equipability, etc." ),
				m( "Player State",          "Mechanics",    "Applies (de)buff-like effects; resistances/weaknesses, terrain access/hindrance, etc." ),
				m( "Player Class(es)",      "Mechanics",    "Adds or edits player 'classes'; custom damage types or abilities/strengths."),
				m( "Player Stats",          "Mechanics",    "Modifies player attack, defense, and other intrinsic elements (minion slots, etc.)."),
				m( "Inventory",				"Mechanics",	"Adds behavior to player inventories; additional slots, pages, organization, etc." ),
				m( "NPC Stats",             "Mechanics",    "Modifies NPC attack, defense, and other intrinsic elements."),
				m( "NPC Behavior",          "Mechanics",    "Modifies NPC AIs or state for new or altered behaviors."),

				m( "Informational",         "Specifications",	"Adds game state reports (time, weather, progress, scores, etc.)." ),
				m( "Specialized",           "Specifications",	"Focuses on one main specific, well-defined function." ),
				m( "Technical",             "Specifications",	"May require a brain." ),
				//{ "Multi-faceted",         "Specifications",	"Does more than one thing, whether focusing mainly on one thing or not." ),
				m( "Replacements",          "Specifications",	"Primarily meant as an alternative to something the game already provides." ),
				m( "Restrictions",          "Specifications",	"Limits or removes elements of the game; may make things easier or harder." ),
				//{ "Esoteric",              "Specifications",	"Does something uncommon or unexpected. Likely one-of-a-kind."),
				//{ "Visuals",               "Specifications",	"Implements new or improved sprites, adds new background details, etc."),
				//m( "Spoilers",              "Specifications",	"Reveals information in advance about game or story elements, especially prematurely."),
				m( "Needs New World",       "Specifications",   "Playing an existing world is difficult, problematic, or just impossible."),
				m( "Needs New Player",      "Specifications",   "Character must begin as a blank slate, similarly."),
				m( "Affects World",         "Specifications",   "Adds set pieces, alters biome shapes, adds new types of growth, etc."),
				m( "Affects Game State",    "Specifications",   "Alters shop prices, activates invasion events, changes weather, etc."),
				m( "Mod Interacting",       "Specifications",   "Supplies data, alters behavior, provides APIs, or manages other mods."),
				m( "Mod Collab",            "Specifications",   "May be specifically paired with (an)other mod(s) to create a more-than-sum-of-parts result."),
				m( "Adds UI",               "Specifications",   "Adds user interface components for mod functions."),
				m( "Configurable",          "Specifications",   "Provides options for configuring game settings (menu, config file, commands, etc.)."),

				m( "MP Compatible",         "Multiplayer",  "Built for multiplayer." ),
				m( "MP Emphasis",			"Multiplayer",  "Mod is meant primarily (if not exclusively) for multiplayer use.",
					new HashSet<string> { "MP Compatible" } ),
				m( "PvP",                   "Multiplayer",  "Player vs player (multiplayer).",
					new HashSet<string> { "MP Compatible" } ),
				m( "Coop",                  "Multiplayer",  "Requires or involves direct player-to-player cooperation (multiplayer).",
					new HashSet<string> { "MP Compatible" } ),
				m( "Teams",                 "Multiplayer",  "Requires or involves teams of players (multiplayer).",
					new HashSet<string> { "MP Compatible" } ),
				m( "Client Only",			"Multiplayer",  "Intended to run only on a local client. Does not pertain to servers.",
					new HashSet<string> { "MP Compatible" }, new HashSet<string> { "Server Use" } ),
				m( "Server Use",            "Multiplayer",  "Player management tools, permissions, game rule changes, scheduled events, etc.",
					new HashSet<string> { "MP Compatible" }, new HashSet<string> { "Client Only" } ),

				////
				
				m( "Open Source",           "State",        "Freely available source code." ),
				m( "Has Documentation",     "State",        "Has an associated wiki or other comprehensive information source."),
				m( "Unmaintained",          "State",        "No longer receives version updates."),
				m( "Unfinished",            "State",        "Has missing or partially-working features."),
				m( "May Lag",               "State",		"May use system resources or network bandwidth heavily. Good computer recommended."),
				m( "Buggy",                 "State",        "Does unexpected or erroneous things."),
				m( "Non-functional",        "State",        "Does not work (for its main use)." ),
				m( "Poor Homepage",			"State",        "Missing or unhelpful homepage.",
					new HashSet<string> { "Misleading Info" } ),
				//{ "Rated R",               "State",		"Guess." },
				m( "Made By Team",			"State",		"Features significant contributions from multiple people." ),
				//{ "Simplistic",			"" },
				//{ "Minimalistic",			"" },
				//{ "Polished",				"" },
				
				////
				
				m( "Game Mode(s)",          "Gameplay",		"New game rules; added end goals, progression, setting, session, etc."),
				//m( "Changes Genre",         "Gameplay",		"Adds linear progression, includes a SHMUP sequence, adds cooking minigames, etc."),
				m( "Quests",                "Gameplay",		"Adds goals for player to progress the game or gain profit from."),
				m( "Creativity",            "Gameplay",		"Emphasizes building or artistic expression (as opposed to fighting and adventuring)."),
				//m( "Cheat-like",            "General",		"Significantly reduces or removes some game challenges; may be 'unfair'."),
				m( "Adds Convenience",      "Gameplay",		"Reduces annoyances; auto-trashes junk items, reduces grinding, etc."),
				m( "Challenge",				"Gameplay",		"Increases difficulty of specific elements: Time limits, harder boss AI, etc."),
				m( "Challenge Emphasis",	"Gameplay",		"Focuses significantly on adding challenge to the game.",
					new HashSet<string> { "Adds Challenge" } ),
				//m( "Easings",               "General",		"Decreases difficulty of specific elements: Stronger weapons, added player defense, etc."),
				//m( "Vanilla Balanced",      "General",		"Balanced around plain Terraria; progress will not happen faster than usual."),
				m( "Loosely Balanced",      "Gameplay",		"Inconsistent or vague attempt to maintain balance, vanilla or otherwise."),
				m( "Plus Balanced",         "Gameplay",		"Balanced in excess of vanilla; expect sequence breaks (e.g. killing powerful bosses early)."),

				m( "Needs Credentials",		"Privilege",	"Requires input of user information for features to work." ),
				m( "Accesses System",       "Privilege",	"Accesses files, opens programs, uses system functions, etc."),
				m( "Accesses Web",          "Privilege",	"Makes web requests to send or receive data."),
				//{ "Injects Code",           make("Priviledge",	"Uses Reflection, swaps methods, or invokes libraries that do these."),
				
				////

				//{ "Item Sets",				"Content",	"Adds or edits discrete (often themed) sets or types of items."),
				m( "Weapons",					"Content",  "Adds or edits weapon items."),
				m( "Tools",						"Content",  "Adds or edits tool items."),
				m( "Armors",				    "Content",  "Adds or edits armor items in particular."),
				m( "Buffs & Pots.",			    "Content",  "Adds or edits buffs and potion items."),
				m( "Accessories",			    "Content",  "Adds or edits player accessory items (includes wings)."),
				m( "Mounts & Familiars",	    "Content",  "Adds or edits player mounts or gameplay-affecting 'pets'."),
				m( "Pets",						"Content",  "Adds or edits player pets."),
				m( "Vanity",				    "Content",  "Adds or edits player vanity items, dyes, or non-gameplay pets."),
				m( "Ores",					    "Content",  "Adds mineable ores (and probably matching equipment tiers)."),
				m( "Recipes",				    "Content",  "Adds or edits recipes beyond the expected minimum, or provides recipe information."),
				m( "Hostile NPCs",			    "Content",  "Adds or edits hostile NPCs (monsters)."),
				m( "Town NPCs",				    "Content",  "Adds or edits town NPCs (villagers)."),
				m( "Critters",				    "Content",  "Adds or edits (passive) biome fauna."),
				m( "Bosses",				    "Content",  "Adds or edits boss monsters."),
				m( "Invasions & Events",	    "Content",  "Adds or edits invasions or game events."),
				m( "Fishing",				    "Content",  "Adds or edits tools for fishing or types of fish."),
				m( "Blocks",				    "Content",  "Adds or edits new block types."),
				m( "Biomes",				    "Content",  "Adds or edits world biomes."),
				m( "Decorative",			    "Content",  "Adds or edits decorative objects (e.g. furniture for houses)."),
				m( "Wiring",				    "Content",  "Adds or edits tools and toys for use for wiring."),
				m( "Music",					    "Content",  "Adds or edits music."),
				m( "Rich Art",					"Content",  "Adds extensive or detailed art for new or existing content."),
				m( "Sounds",					"Content",  "Adds or edits sound effects or ambience."),
				m( "Story or Lore",				"Content",  "Implements elements of story telling or universe lore."),
				m( "Special FX",				"Content",  "Adds gore effects, adds motion blurs, improves particle effects, etc."),
				m( "Weapon Emphasis",			"Content",  "Emphasizes added or modified weapon items.",
					new HashSet<string> { "Weapons" } ),
				m( "Tools Emphasis",		    "Content",  "Emphasizes added or modified tool items.",
					new HashSet<string> { "Tools" } ),
				m( "Armors Emphasis",		    "Content",  "Emphasizes added or modified armor items.",
					new HashSet<string> { "Armors" } ),
				m( "Accessory Emphasis",		"Content",  "Emphasizes added or modified player accessory items (includes wings).",
					new HashSet<string> { "Accessories" } ),
				m( "Mounts Emphasis",		    "Content",  "Emphasizes added or modified player mounts or gameplay-affecting 'pets'.",
					new HashSet<string> { "Mounts & Familiars" } ),
				m( "Vanity Emphasis",		    "Content",  "Emphasizes added or modified player vanity items, dyes, or non-gameplay pets.",
					new HashSet<string> { "Vanity" } ),
				m( "Ores Emphasis",				"Content",  "Emphasizes added mineable ores (and probably matching equipment tiers).",
					new HashSet<string> { "Ores" } ),
				m( "Recipes Emphasis",			"Content",  "Emphasizes added or modified recipes beyond the expected minimum, or provides recipe information.",
					new HashSet<string> { "Recipes" } ),
				m( "Foes Emphasis",				"Content",  "Emphasizes added or modified hostile NPCs (monsters).",
					new HashSet<string> { "Hostile NPCs" } ),
				m( "Friends Emphasis",			"Content",  "Emphasizes added or modified town NPCs (villagers).",
					new HashSet<string> { "Town NPCs" } ),
				m( "Critters Emphasis",			"Content",  "Emphasizes added or modified (passive) biome fauna.",
					new HashSet<string> { "Critters" } ),
				m( "Bosses Emphasis",			"Content",  "Emphasizes added or modified boss monsters.",
					new HashSet<string> { "Bosses" } ),
				m( "Invasions Emphasis",		"Content",  "Emphasizes added or modified invasions or game events.",
					new HashSet<string> { "Invasions & Events" } ),
				m( "Fishing Emphasis",			"Content",  "Emphasizes added or modified tools for fishing or types of fish.",
					new HashSet<string> { "Fishing" } ),
				m( "Blocks Emphasis",			"Content",  "Emphasizes added or modified new block types.",
					new HashSet<string> { "Blocks" } ),
				m( "Biomes Emphasis",			"Content",  "Emphasizes added or modified world biomes.",
					new HashSet<string> { "Biomes" } ),
				m( "Decorative Emphasis",		"Content",  "Emphasizes added or modified decorative objects (e.g. furniture for houses).",
					new HashSet<string> { "Decorative" } ),
				m( "Wiring Emphasis",			"Content",  "Emphasizes added or modified tools and toys for use for wiring.",
					new HashSet<string> { "Wiring" } ),
				m( "Music Emphasis",			"Content",  "Emphasizes added or modified music.",
					new HashSet<string> { "Music" } ),
				m( "Rich Art Emphasis",			"Content",  "Emphasizes added extensive or detailed art for new or existing content.",
					new HashSet<string> { "Rich Art" } ),
				m( "Sounds Emphasis",			"Content",  "Emphasizes added or modified sound effects or ambience.",
					new HashSet<string> { "Sounds" } ),
				m( "Story Emphasis",			"Content",  "Emphasizes added elements of story telling or universe lore.",
					new HashSet<string> { "Story" } ),
				m( "SFX Emphasis",				"Content",  "Emphasizes added special FX.",
					new HashSet<string> { "Special FX" } ),

				m( "Dark",					"Theme",    "Gloomy, edgy, or just plain poor visibility." ),
				m( "Silly",                 "Theme",    "Light-hearted, immersion-breaking, or just plain absurd." ),
				m( "Fantasy",               "Theme",    "Elements of mythologies, swords & sorcery, and maybe a hobbit or 2." ),
				m( "Medieval",              "Theme",    "Chivalry, castles, primitive technology, melee fighting, archery, etc." ),
				m( "Military",              "Theme",    "Guns and stuff." ),
				m( "Nature",                "Theme",    "Birds, bees, rocks, trees, etc." ),
				m( "Sci-Fi",                "Theme",    "Robots, lasers, flying machines, etc." ),
				//m( "Futuristic",            "Theme",    "" ),
				m( "Civilized",             "Theme",    "NPC interactions, town workings, player living spaces, etc." ),
				//{ "Mixed",				"Theme",    "Mashup of genres, but not purely in a silly way." ),
				
				m( "Surface",				"Where",	"Emphasizes involvement with a world's surface-level region." ),
				m( "Sky",					"Where",    "Emphasizes involvement with a world's skies." ),
				m( "Underground",           "Where",    "Emphasizes involvement with a world's underground regions (except underworld)." ),
				m( "Ocean",                 "Where",    "Emphasizes involvement with a world's oceans." ),
				m( "Underworld",			"Where",    "Emphasizes involvement with a world's underworld region." ),
				m( "Dungeon",               "Where",    "Emphasizes involvement with a world's dungeon." ),
				m( "Snow Biomes",			"Where",    "Emphasizes involvement with a world's snow biomes." ),
				m( "Desert Biomes",			"Where",    "Emphasizes involvement with a world's desert biomes." ),
				m( "Jungle Biome",			"Where",    "Emphasizes involvement with a world's jungle biomes." ),
				m( "Evil Biome",            "Where",    "Emphasizes involvement with a world's evil biomes (crimson or corruption)." ),
				m( "Hallow Biome",          "Where",    "Emphasizes involvement with a world's hallowed biomes." ),

				m( "The Beginning",         "When",	"Emphasizes time before any boss kills or invasion events."),
				m( "Bosses Begun",          "When", "Emphasizes time after player has begun killing bosses (world now mostly accessible)."),
				m( "Post-BoC & EoW",        "When", "Emphasizes time after corruption/crimson conquered. Underworld accessible."),
				m( "Hard Mode",             "When", "Emphasizes time after Wall of Flesh conquered (hallowed+evil biomes, harder monsters)."),
				m( "Post-Mech bosses",      "When", "Emphasizes time after Mech bosses conquered (restless jungle)."),
				m( "Post-Plantera",         "When", "Emphasizes time after Plantera conquered. Temple accessible."),
				m( "Post-Moonlord",         "When", "Emphasizes time after Moon Lord killed."),
				m( "Contextual",            "When", "Concerns with a specific, discrete event or (non-boss) game context.")
			};

			if( !ModHelpersMod.Instance.Config.DisableJudgmentalTags ) {
				list.Add( m( "Unimaginative",		"Judgmental", "Nothing special; exceedingly common, generic, or flavorless." ) );
				list.Add( m( "Low Effort",			"Judgmental", "Evident lack of effort involved.",
					null, new HashSet<string> { "Shows Effort" }
				) );
				list.Add( m( "Shows Effort",		"Judgmental", "Evident investment of effort involved.",
					null, new HashSet<string> { "Low Effort" }
				) );
				list.Add( m( "Unoriginal Content",	"Judgmental", "Contains stolen or extensively-derived content." ) );
			}

			ModTagsManager.TagMap = list.ToDictionary( tagDef=>tagDef.Tag, tagDef=>tagDef );
			ModTagsManager.Tags = list.ToArray();
		}
	}
}
