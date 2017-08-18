using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace HamstarHelpers.NPCHelpers {
	/*public static class NPCAIHelpers {
		private static IDictionary<int, float> _Appraisals = new Dictionary<int, float> {
			{ 0, 0.0f },	// Immobile
			{ 1, 1.0f },	// Slimes
			{ 2, 1.2f },	// Demon Eye
			{ 3, 1.2f },	// Fighter AI
			{ 4, 2.0f },	// Eye of Cthulhu AI 
			{ 5, 1.3f },	// Flying AI
			{ 6, 1.6f },	// Worm AI
			{ 7, 0.0f },	// Passive AI
			{ 8, 1.5f },	// Caster AI
			{ 9, 1.2f },	// Spell AI
			{ 10, 1.3f },	// Cursed Skull AI
			{ 11, 1.7f },	// Head AI (Skeletron or Dungeon Guardian)
			{ 12, 2.5f },	// Skeletron Hand AI
			{ 13, 1.6f },	// Plant AI
			{ 14, 2.0f },	// Bat AI
			{ 15, 1.8f },	// King Slime AI
			{ 16, 0.9f },	// Swimming AI
			{ 17, 1.3f },	// Vulture AI
			{ 18, 0.8f },	// Jellyfish AI
			{ 19, 1.0f },	// Antlion AI
			{ 20, 1.0f },	// Spike Ball AI
			{ 21, 1.0f },	// Blazing Wheel AI
			{ 22, 1.3f },	// Hovering AI
			{ 23, 1.6f },	// Flying Weapon AI
			{ 24, 0.0f },	// Bird AI
			{ 25, 1.8f },	// Mimic AI
			{ 26, 1.5f },	// Unicorn AI
			{ 27, 3.0f },	// Wall of Flesh Body AI
			{ 28, 2.5f },	// Wall of Flesh Eye AI
			{ 29, 1.8f },	// The Hungry AI
			{ 30, 3.0f },	// Retinazer AI
			{ 31, 3.0f },	// Spazmatism AI
			{ 32, 1.8f },	// Skeletron Prime Head AI
			{ 33, 2.2f },	// Prime Saw AI
			{ 34, 2.2f },	// Prime Vice AI
			{ 35, 2.0f },	// Prime Cannon AI
			{ 36, 2.2f },	// Prime Laser AI
			{ 37, 3.0f },	// The Destroyer AI
			{ 38, 1.5f },	// Snowman AI
			{ 39, 1.6f },	// Tortoise AI
			{ 40, 1.6f },	// Spider AI
			{ 41, 2.0f },	// Herpling AI
			{ 42, 1.6f },	// Lost Girl AI
			{ 43, 2.5f },	// Queen Bee AI
			{ 44, 1f },	// Flying Fish AI
			{ 45, 3.0f },	// Golem Body AI
			{ 46, 2.5f },	// Golem Head AI
			{ 47, 1.6f },	// Golem Fist AI
			{ 48, 2.5f },	// Flying Golem Head AI
			{ 49, 1.5f },	// Angry Nimbus AI
			{ 50, 1.0f },	// Spore AI
			{ 51, 3.2f },	// Plantera AI
			{ 52, 1.2f },	// Plantera's Hook AI
			{ 53, 1.8f },	// Plantera's Tentacle AI
			{ 54, 2.2f },	// Brain of Cthulhu AI
			{ 55, 2.5f },	// Creeper AI
			{ 56, 1.8f },	// Dungeon Spirit AI
			{ 57, 3.2f },	// Mourning Wood AI
			{ 58, 3.5f },	// Pumpking AI
			{ 59, 1.8f },	// Pumpking Scythe AI
			{ 60, 2.5f },	// Ice Queen AI
			{ 61, 2.2f },  // Santa - NK1 AI
			{ 62, 1.8f }, // Elf Copter AI
			{ 63, 2.0f }, // Flocko AI
			{ 64, 0f },  // Firefly AI
			{ 65, 0f }, // Butterfly AI
			{ 66, 0f }, // Passive Worm AI
			{ 67, 0f }, // Snail AI
			{ 68, 0f },  // Duck AI
			{ 69, 4.5f }, // Duke Fishron AI
			{ 70, 1f }, // Detonating Bubble AI
			{ 71, 1f }, // Sharkron AI
			{ 72, 1f }, // Bubble shield AI
			{ 73, 1f }, // Tesla Turret AI
			{ 74, 1f }, // Corite AI
			{ 75, 1f }, // Rider AI
			{ 76, 1f }, // Martian Saucer
			{ 77, 1f },  // Moon Lord Core
			{ 78, 1f }, // Moon Lord Hand
			{ 79, 1f }, // Moon Lord Head
			{ 80, 1f },  // Martian Probe
			{ 81, 1f }, // True Eye of Cthulhu
			{ 82, 1f }, // Moon Leech Clot
			{ 83, 1f }, // Lunatic Devote
			{ 84, 1f },  // Lunatic Cultist
			{ 85, 1f }, // Star Cell/ Brain Suckler
			{ 86, 1f },	// Ancient Vision
			{ 87, 1f },	// Biome Mimic
			{ 88, 1f },	// Mothron AI
			{ 89, 1f },	// Mothron Egg AI
			{ 90, 1f },	// Baby Mothron AI
			{ 91, 1f },	// Granite Elemental AI
			{ 92, 1f },	// Target Dummy AI
			{ 93, 1f },	// Flying Dutchman AI
			{ 94, 1f },	// Celestial Tower AI
			{ 95, 1f },	// Small Star Cell AI
			{ 96, 1f },	// Flow Invader AI
			{ 97, 1f },	// Nebula Floater AI
			{ 98, 1f },	// Unknown
			{ 99, 1f },	// Solar Fragment AI
			{ 100, 1f },	// Ancient Light AI
			{ 101, 1f },	// Ancient Doom AI
			{ 102, 1f },	// Sand Elemental AI
			{ 103, 1f },	// Sand Shark AI
			{ 104, 1f },	// Unknown
			{ 105, 1f },	// Eternia Crystal AI
			{ 106, 1f },	// Mysterious Portal AI
			{ 107, 1f },	// Attacker AI
			{ 108, 1f },	// Flying Attacker AI
			{ 109, 1f },	// Dark Mage AI
			{ 110, 1f },	// Betsy AI
			{ 111, 1f }	// Etherian Lightning
		};

		public static IReadOnlyDictionary<int, float> Appraisals = new ReadOnlyDictionary<int, float>( NPCAIHelpers._Appraisals );
	}*/
}
