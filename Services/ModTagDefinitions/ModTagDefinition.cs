using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace HamstarHelpers.Services.ModTagDefinitions {
	/// <summary>
	/// Describes a basic attribute of a given mod. Meant to be combined in sets to create a comprehensive
	/// categorical description of a mod.
	/// </summary>
	public partial class ModTagDefinition {
		/// <summary>Descriptions of each tag category.</summary>
		public readonly static IReadOnlyDictionary<string, string> CategoryDescriptions;
		/// <summary>A map of tag definitions to their tags.</summary>
		public readonly static IReadOnlyDictionary<string, ModTagDefinition> TagMap;
		/// <summary>An ordered list of the tag definitions.</summary>
		public readonly static IReadOnlyList<ModTagDefinition> Tags;


		////

		static ModTagDefinition() {
			ModTagDefinition m( string tag,
					string cat,
					string desc,
					TagFlavor flavor = TagFlavor.Specification,
					ISet<string> forces = null,
					ISet<string> excludesOnAdd = null ) {
				var def = new ModTagDefinition( tag, cat, desc, flavor, forces, excludesOnAdd );
				return def;
			}

			////

			var catDescs = new Dictionary<string, string> {
				{ "Specifications", "General descriptions of a mod." },
				{ "Mechanics",      "Describes what game mechanics are associated with a mod." },
				{ "Gameplay",       "Describes how a mod affects gameplay (more than specific mechanics)." },
				{ "State",          "Describes the existential state of a mod." },
				{ "Multiplayer",    "Describes what manner of relevance a mod has to multiplayer." },
				{ "Privilege",      "Indicates what manner of user or system priviledges a mod needs to function." },
				{ "Content",        "Describes what types of content a mod features." },
				{ "Theme",          "Describes the apparent thematic elements of a mod." },
				{ "When",           "Indicates what part of the game's (vanilla) progression a mod most pertains to." },
				{ "Where",          "Indicates where in a world the mod specifically emphasizes relevance to." },
				//{ "Judgmental",		"Wholly-subjective tags. Must be enabled in settings." }
			};
			ModTagDefinition.CategoryDescriptions = new ReadOnlyDictionary<string, string>( catDescs );

			////

			var list = new List<ModTagDefinition> {
				m( tag: "Core Game",			cat: "Mechanics",
					desc: "Fundamental; adds a \"SHMUP\" mode, adds a stamina bar, removes mining, etc.",
					flavor: TagFlavor.Important ),
				m( tag: "Combat",				"Mechanics",    "Adds weapon reloading, dual-wielding, monster AI changes, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Crafting",              "Mechanics",    "Adds bulk crafting, UI-based crafting, item drag-and-drop crafting, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Movement",              "Mechanics",    "Gives super speed, adds dodging mechanics, adds unlimited flight, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Mining",                "Mechanics",    "Adds fast tunneling, area mining, shape cutting, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Building",              "Mechanics",    "Add fast building options, measuring tools, world editing, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Traveling",             "Mechanics",    "Adds fast travel options, new types of minecarts/travel-mounts, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Misc. Interactions",    "Mechanics",    "Adds new block interactions, fishing mechanics, NPC dialogues, etc.",
					flavor: TagFlavor.Emphasis ),	// new HashSet<string> { "Core Game" } ),
				m( tag: "Item Storage",          "Mechanics",    "Adds or changes chest behavior, adds item sharing, global organization, etc." ),
				m( tag: "Item Equiping",         "Mechanics",    "Dual wielding, additional accessory slots, equipment management, etc."),
				m( tag: "Item Stats",            "Mechanics",    "Adjusts item damage, defense, price, etc." ),
				m( tag: "Item Behavior",         "Mechanics",    "Changes item projectile type or quantity, item class, equipability, etc." ),
				m( tag: "Player State",          "Mechanics",    "Applies (de)buff-like effects; resistances/weaknesses, terrain access/hindrance, etc." ),
				m( tag: "Player Class(es)",      "Mechanics",    "Adds or edits player 'classes'; custom damage types or abilities/strengths."),
				m( tag: "Player Stats",          "Mechanics",    "Modifies player attack, defense, and other intrinsic elements (minion slots, etc.)."),
				m( tag: "Inventory",             "Mechanics",    "Adds behavior to player inventories; additional slots, pages, organization, etc." ),
				m( tag: "NPC Stats",             "Mechanics",    "Modifies NPC attack, defense, and other intrinsic elements."),
				m( tag: "NPC Behavior",          "Mechanics",    "Modifies NPC AIs or state for new or altered behaviors."),

				m( tag: "Informational",         "Specifications",   "Adds game state reports (time, weather, progress, scores, etc.)." ),
				m( tag: "Specialized",           "Specifications",   "Focuses on one main specific, well-defined function." ),
				m( tag: "Technical",             "Specifications",   "May require a brain." ),
				//{ tag: "Multi-faceted",         "Specifications",	"Does more than one thing, whether focusing mainly on one thing or not." ),
				m( tag: "Replacements",          "Specifications",   "Primarily meant as an alternative to something the game already provides." ),
				m( tag: "Restrictions",          "Specifications",   "Limits or removes elements of the game; may make things easier or harder." ),
				//{ tag: "Esoteric",              "Specifications",	"Does something uncommon or unexpected. Likely one-of-a-kind."),
				//{ tag: "Visuals",               "Specifications",	"Implements new or improved sprites, adds new background details, etc."),
				//m( tag: "Spoilers",              "Specifications",	"Reveals information in advance about game or story elements, especially prematurely."),
				m( tag: "Needs New World",       "Specifications",   "Playing an existing world is difficult, problematic, or just impossible.",
					TagFlavor.TechnicallyImportant ),
				m( tag: "Needs New Player",      "Specifications",   "Character must begin as a blank slate, similarly.",
					TagFlavor.TechnicallyImportant ),
				m( tag: "Affects World",         "Specifications",   "Adds set pieces, alters biome shapes, adds new types of growth, etc."),
				m( tag: "Affects Game State",    "Specifications",   "Alters shop prices, activates invasion events, changes weather, etc."),
				m( tag: "Mod Interacting",       "Specifications",   "Supplies data, alters behavior, provides APIs, or manages other mods."),
				m( tag: "Mod Collab",            "Specifications",   "May be specifically paired with (an)other mod(s) to create a more-than-sum-of-parts result."),
				m( tag: "Adds UI",               "Specifications",   "Adds user interface components for mod functions."),
				m( tag: "Configurable",          "Specifications",   "Provides options for configuring game settings (menu, config file, commands, etc.)."),
				
				m( tag: "MP Compatible",         "Multiplayer",  "Built for multiplayer.",
					TagFlavor.Important ),
				m( tag: "MP Emphasis",           "Multiplayer",  "Mod is meant primarily (if not exclusively) for multiplayer use.",
					TagFlavor.Emphasis, new HashSet<string> { "MP Compatible" } ),
				m( tag: "PvP",                   "Multiplayer",  "Player vs player (multiplayer).",
					TagFlavor.Specification, new HashSet<string> { "MP Compatible" } ),
				m( tag: "Coop",                  "Multiplayer",  "Requires or involves direct player-to-player cooperation (multiplayer).",
					TagFlavor.Specification, new HashSet<string> { "MP Compatible" } ),
				m( tag: "Teams",                 "Multiplayer",  "Requires or involves teams of players (multiplayer).",
					TagFlavor.Specification, new HashSet<string> { "MP Compatible" } ),
				m( tag: "Client Only",           "Multiplayer",  "Intended to run only on a local client. Does not pertain to servers.",
					TagFlavor.TechnicallyImportant,
					new HashSet<string> { "MP Compatible" },
					new HashSet<string> { "Server Use" } ),
				m( tag: "Server Use",            "Multiplayer",  "Player management tools, permissions, game rule changes, scheduled events, etc.",
					TagFlavor.TechnicallyImportant,
					new HashSet<string> { "MP Compatible" },
					new HashSet<string> { "Client Only" } ),

				////
				
				m( tag: "Open Source",           "State",        "Freely available source code." ),
				m( tag: "Has Documentation",     "State",        "Has an associated wiki or other comprehensive information source."),
				m( tag: "Made By Team",			"State",		"Features significant contributions from multiple people." ),
				m( tag: "Unmaintained",          "State",        "No longer receives version updates.",
					TagFlavor.Deficient ),
				m( tag: "Unfinished",            "State",        "Has missing or partially-working features.",
					TagFlavor.Deficient ),
				m( tag: "May Lag",               "State",        "May use system resources or network bandwidth heavily. Good computer recommended.",
					TagFlavor.Alert ),
				m( tag: "Buggy",                 "State",        "Does unexpected or erroneous things.",
					TagFlavor.Warning ),
				m( tag: "Non-functional",        "State",        "Does not work (for its main use).",
					TagFlavor.Broken ),
				m( tag: "Misleading Info",		"State",		"Missing or unhelpful mod information (description, tooltips, etc.).",
					TagFlavor.Warning ),
				m( tag: "Poor Homepage",         "State",        "Missing or unhelpful homepage.",
					TagFlavor.Warning, new HashSet<string> { "Misleading Info" } ),
				//{ tag: "Rated R",               "State",		"Guess." },
				//{ tag: "Simplistic",			"" },
				//{ tag: "Minimalistic",			"" },
				//{ tag: "Polished",				"" },
				
				////
				
				m( tag: "Game Mode(s)",          "Gameplay",     "New game rules; added end goals, progression, setting, session, etc.",
					TagFlavor.TechnicallyImportant ),
				//m( tag: "Changes Genre",         "Gameplay",		"Adds linear progression, includes a SHMUP sequence, adds cooking minigames, etc."),
				m( tag: "Quests",                "Gameplay",     "Adds goals for player to progress the game or gain profit from."),
				m( tag: "Creativity",            "Gameplay",     "Emphasizes building or artistic expression (as opposed to fighting and adventuring)."),
				//m( tag: "Cheat-like",            "General",		"Significantly reduces or removes some game challenges; may be 'unfair'."),
				m( tag: "Adds Convenience",      "Gameplay",     "Reduces annoyances; auto-trashes junk items, reduces grinding, etc."),
				m( tag: "Challenge",             "Gameplay",     "Increases difficulty of specific elements: Time limits, harder boss AI, etc."),
				m( tag: "Challenge Emphasis",    "Gameplay",     "Focuses significantly on adding challenge to the game.",
					TagFlavor.Emphasis, new HashSet<string> { "Adds Challenge" } ),
				//m( tag: "Easings",               "General",		"Decreases difficulty of specific elements: Stronger weapons, added player defense, etc."),
				//m( tag: "Vanilla Balanced",      "General",		"Balanced around plain Terraria; progress will not happen faster than usual."),
				m( tag: "Loosely Balanced",      "Gameplay",     "Inconsistent or vague attempt to maintain consistent balance, vanilla or otherwise.",
					TagFlavor.Alert ),
				m( tag: "Plus Balanced",         "Gameplay",     "Balanced in excess of vanilla; expect sequence breaks (e.g. killing powerful bosses early).",
					TagFlavor.Alert ),

				m( tag: "Needs Credentials",     "Privilege",    "Requires input of user information for features to work.",
					TagFlavor.TechnicallyImportant ),
				m( tag: "Accesses System",       "Privilege",    "Accesses files, opens programs, uses system functions, etc.",
					TagFlavor.TechnicallyImportant ),
				m( tag: "Accesses Web",          "Privilege",    "Makes web requests to send or receive data.",
					TagFlavor.TechnicallyImportant ),
				//{ tag: "Injects Code",           make("Priviledge",	"Uses Reflection, swaps methods, or invokes libraries that do these."),
				
				////

				//{ tag: "Item Sets",				"Content",	"Adds or edits discrete (often themed) sets or types of items."),
				m( tag: "Weapons",					"Content",  "Adds or edits weapon items."),
				m( tag: "Tools",					"Content",  "Adds or edits tool items."),
				m( tag: "Armors",					"Content",  "Adds or edits armor items in particular."),
				m( tag: "Accessories",				"Content",  "Adds or edits player accessory items (includes wings)."),
				m( tag: "Mounts & Familiars",		"Content",  "Adds or edits player mounts."),
				m( tag: "Pets",						"Content",  "Adds or edits player pets (including light pets)."),
				m( tag: "Vanity",					"Content",  "Adds or edits player vanity items, dyes, or non-gameplay pets."),
				m( tag: "Ores",						"Content",  "Adds mineable ores (and probably matching equipment tiers)."),
				m( tag: "Recipes",					"Content",  "Adds or edits recipes beyond the expected minimum, or provides recipe information."),
				m( tag: "Hostile NPCs",				"Content",  "Adds or edits hostile NPCs (monsters)."),
				m( tag: "Friendly NPCs",			"Content",  "Adds or edits town or actively helpful NPCs."),
				m( tag: "Critters",					"Content",  "Adds or edits (passive) biome fauna."),
				m( tag: "Bosses",					"Content",  "Adds or edits boss monsters."),
				m( tag: "Invasions & Events",		"Content",  "Adds or edits invasions or game events."),
				m( tag: "Fishing",					"Content",  "Adds or edits tools for fishing or types of fish."),
				m( tag: "Blocks",					"Content",  "Adds or edits new block types."),
				m( tag: "Biomes",					"Content",  "Adds or edits world biomes."),
				m( tag: "Decorative",				"Content",  "Adds or edits decorative objects (e.g. furniture for houses)."),
				m( tag: "Wiring",					"Content",  "Adds or edits tools and toys for use for wiring."),
				m( tag: "Music",					"Content",  "Adds or edits music."),
				m( tag: "Rich Art",					"Content",  "Adds extensive or detailed art for new or existing content."),
				m( tag: "Sounds",					"Content",  "Adds or edits sound effects or ambience."),
				m( tag: "Story or Lore",			"Content",  "Implements elements of story telling or universe lore."),
				m( tag: "Special FX",				"Content",  "Adds gore effects, adds motion blurs, improves particle effects, etc."),
				m( tag: "Buffs & Pots.",			"Content",  "Adds or edits buffs and potion items."),
				m( tag: "Weapon Emphasis",			"Content",  "Emphasizes added or modified weapon items.",
					TagFlavor.Emphasis, new HashSet<string> { "Weapons" } ),
				m( tag: "Tools Emphasis",			"Content",  "Emphasizes added or modified tool items.",
					TagFlavor.Emphasis, new HashSet<string> { "Tools" } ),
				m( tag: "Armors Emphasis",			"Content",  "Emphasizes added or modified armor items.",
					TagFlavor.Emphasis, new HashSet<string> { "Armors" } ),
				m( tag: "Accessory Emphasis",		"Content",  "Emphasizes added or modified player accessory items (includes wings).",
					TagFlavor.Emphasis, new HashSet<string> { "Accessories" } ),
				m( tag: "Mounts Emphasis",			"Content",  "Emphasizes added or modified player mounts.",
					TagFlavor.Emphasis, new HashSet<string> { "Mounts & Familiars" } ),
				m( tag: "Vanity Emphasis",			"Content",  "Emphasizes added or modified player vanity items, dyes, or non-gameplay pets.",
					TagFlavor.Emphasis, new HashSet<string> { "Vanity" } ),
				m( tag: "Ores Emphasis",			"Content",  "Emphasizes added mineable ores (and probably matching equipment tiers).",
					TagFlavor.Emphasis, new HashSet<string> { "Ores" } ),
				m( tag: "Recipes Emphasis",			"Content",  "Emphasizes added or modified recipes beyond the expected minimum, or provides recipe information.",
					TagFlavor.Emphasis, new HashSet<string> { "Recipes" } ),
				m( tag: "Foes Emphasis",			"Content",  "Emphasizes added or modified hostile NPCs (enemies).",
					TagFlavor.Emphasis, new HashSet<string> { "Hostile NPCs" } ),
				m( tag: "Friends Emphasis",			"Content",  "Emphasizes added or modified town or actively helpful NPCs.",
					TagFlavor.Emphasis, new HashSet<string> { "Town NPCs" } ),
				m( tag: "Critters Emphasis",		"Content",  "Emphasizes added or modified (passive) biome fauna.",
					TagFlavor.Emphasis, new HashSet<string> { "Critters" } ),
				m( tag: "Bosses Emphasis",			"Content",  "Emphasizes added or modified boss monsters.",
					TagFlavor.Emphasis, new HashSet<string> { "Bosses" } ),
				m( tag: "Invasions Emphasis",		"Content",  "Emphasizes added or modified invasions or game events.",
					TagFlavor.Emphasis, new HashSet<string> { "Invasions & Events" } ),
				m( tag: "Fishing Emphasis",			"Content",  "Emphasizes added or modified tools for fishing or types of fish.",
					TagFlavor.Emphasis, new HashSet<string> { "Fishing" } ),
				m( tag: "Blocks Emphasis",			"Content",  "Emphasizes added or modified new block types.",
					TagFlavor.Emphasis, new HashSet<string> { "Blocks" } ),
				m( tag: "Biomes Emphasis",			"Content",  "Emphasizes added or modified world biomes.",
					TagFlavor.Emphasis, new HashSet<string> { "Biomes" } ),
				m( tag: "Decorative Emph.",			"Content",  "Emphasizes added or modified decorative objects (e.g. furniture for houses).",
					TagFlavor.Emphasis, new HashSet<string> { "Decorative" } ),
				m( tag: "Wiring Emphasis",			"Content",  "Emphasizes added or modified tools and toys for use for wiring.",
					TagFlavor.Emphasis, new HashSet<string> { "Wiring" } ),
				m( tag: "Music Emphasis",			"Content",  "Emphasizes added or modified music.",
					TagFlavor.Emphasis, new HashSet<string> { "Music" } ),
				m( tag: "Rich Art Emphasis",		"Content",  "Emphasizes added extensive or detailed art for new or existing content.",
					TagFlavor.Emphasis, new HashSet<string> { "Rich Art" } ),
				m( tag: "Sounds Emphasis",			"Content",  "Emphasizes added or modified sound effects or ambience.",
					TagFlavor.Emphasis, new HashSet<string> { "Sounds" } ),
				m( tag: "Story Emphasis",			"Content",  "Emphasizes added elements of story telling or universe lore.",
					TagFlavor.Emphasis, new HashSet<string> { "Story" } ),
				m( tag: "SFX Emphasis",				"Content",  "Emphasizes added or modified special FX.",
					TagFlavor.Emphasis, new HashSet<string> { "Special FX" } ),
				m( tag: "Projectile Emphasis",		"Content",	"Emphasizes added or modified projectiles.",
					TagFlavor.Emphasis ),

				m( tag: "Dark",                 cat: "Theme",	desc: "Gloomy, edgy, or just plain poor visibility." ),
				m( tag: "Silly",                cat: "Theme",   desc: "Light-hearted, immersion-breaking, or just plain absurd." ),
				m( tag: "Fantasy",              cat: "Theme",	desc: "Elements of mythologies, swords & sorcery, and maybe a hobbit or 2." ),
				m( tag: "Medieval",             cat: "Theme",   desc: "Chivalry, castles, primitive technology, melee fighting, archery, etc." ),
				m( tag: "Magical",              cat: "Theme",   desc: "Specifically meta-physical, spiritual, or heavily involving things of magic." ),
				m( tag: "Military",             cat: "Theme",   desc: "Guns and stuff." ),
				m( tag: "Nature",               cat: "Theme",   desc: "Birds, bees, rocks, trees, etc." ),
				m( tag: "Sci-Fi",               cat: "Theme",   desc: "Robots, lasers, flying machines, etc." ),
				//m( tag: "Futuristic",         cat: "Theme",	desc: "" ),
				m( tag: "Civilized",            cat: "Theme",   desc: "NPC interactions, town workings, player living spaces, etc." ),
				//{ tag: "Mixed",				cat: "Theme",	desc: "Mashup of genres, but not purely in a silly way." ),
				m( tag: "Joke",					cat: "Theme",   desc: "Does not take itself seriously, or tries focuses exclusively on meta." ),

				m( tag: "Surface",               "Where",    "Emphasizes involvement with a world's surface-level region." ),
				m( tag: "Sky",                   "Where",    "Emphasizes involvement with a world's skies." ),
				m( tag: "Underground",           "Where",    "Emphasizes involvement with a world's underground regions (except underworld)." ),
				m( tag: "Ocean",                 "Where",    "Emphasizes involvement with a world's oceans." ),
				m( tag: "Underworld",            "Where",    "Emphasizes involvement with a world's underworld region." ),
				m( tag: "Dungeon",               "Where",    "Emphasizes involvement with a world's dungeon." ),
				m( tag: "Snow Biomes",           "Where",    "Emphasizes involvement with a world's snow biomes." ),
				m( tag: "Desert Biomes",         "Where",    "Emphasizes involvement with a world's desert biomes." ),
				m( tag: "Jungle Biome",          "Where",    "Emphasizes involvement with a world's jungle biomes." ),
				m( tag: "Evil Biome",            "Where",    "Emphasizes involvement with a world's evil biomes (crimson or corruption)." ),
				m( tag: "Hallow Biome",          "Where",    "Emphasizes involvement with a world's hallowed biomes." ),

				m( tag: "The Beginning",         "When", "Emphasizes time before any boss kills or invasion events."),
				m( tag: "Bosses Begun",          "When", "Emphasizes time after player has begun killing bosses (world now mostly accessible)."),
				m( tag: "Post-BoC & EoW",        "When", "Emphasizes time after corruption/crimson conquered. Underworld accessible."),
				m( tag: "Hard Mode",             "When", "Emphasizes time after Wall of Flesh conquered (hallowed+evil biomes, harder monsters)."),
				m( tag: "Post-Mech bosses",      "When", "Emphasizes time after Mech bosses conquered (restless jungle)."),
				m( tag: "Post-Plantera",         "When", "Emphasizes time after Plantera conquered. Temple accessible."),
				m( tag: "Post-Moonlord",         "When", "Emphasizes time after Moon Lord killed."),
				m( tag: "Contextual",            "When", "Concerns with a specific, discrete event or (non-boss) game context.")
			};

			if( !ModHelpersConfig.Instance.DisableJudgmentalTags ) {
				list.Add( m( tag: "Unimaginative", "Judgmental", "Nothing special; exceedingly common, generic, or flavorless.",
					TagFlavor.IllFavored
				) );
				list.Add( m( tag: "Low Effort", "Judgmental", "Evident lack of effort involved.",
					TagFlavor.IllFavored, null, new HashSet<string> { "Shows Effort" }
				) );
				list.Add( m( tag: "Shows Effort", "Judgmental", "Evident investment of effort involved.",
					TagFlavor.Specification, null, new HashSet<string> { "Low Effort" }
				) );
				list.Add( m( tag: "Unoriginal Content", "Judgmental", "Contains stolen or extensively-derived content.",
					TagFlavor.IllFavored
				) );
			}

			ModTagDefinition.TagMap = new ReadOnlyDictionary<string, ModTagDefinition>(
				list.ToDictionary( tagDef => tagDef.Tag, tagDef => tagDef )
			);
			ModTagDefinition.Tags = list.ToList().AsReadOnly();
		}
	}
}
