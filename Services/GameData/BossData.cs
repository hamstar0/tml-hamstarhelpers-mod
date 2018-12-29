using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace HamstarHelpers.Services.GameData {
	public class BossDataEntry {
		public int NpcId {
			get {
				if( !NPCIdentityHelpers.NamesToIds.ContainsKey( this.Name ) ) {
					LogHelpers.Log( "ModHelpers.BossDataEntry.NpcId - Could not find id of " + this.Name );
					return 0;
				}
				return NPCIdentityHelpers.NamesToIds[this.Name];
			}
		}

		public int Order => BossData.BossMap[ this.Name ];

		////

		public string Name;

		public bool IsHardmode => this.Order > BossData.WallOfFlesh.Order;
		public bool IsPostMechBoss => this.Order > BossData.SkeletronPrime.Order;
		public bool IsPostPlantera => this.Order > BossData.Plantera.Order;
		public bool IsPostGolem => this.Order > BossData.Golem.Order;
		public bool IsPostMoonlord => this.Order > BossData.MoonLord.Order;
	}




	public class BossData {
		private static IList<BossDataEntry> _BossOrder;
		private static IDictionary<string, int> _BossMap;

		public static IReadOnlyList<BossDataEntry> BossOrder { get; }
		public static IReadOnlyDictionary<string, int> BossMap { get; }

		public static BossDataEntry KingSlime;
		public static BossDataEntry EyeOfCthulhu;
		public static BossDataEntry EaterOfWorlds;
		public static BossDataEntry BrainOfCthulhu;
		public static BossDataEntry QueenBee;
		public static BossDataEntry Skeletron;
		public static BossDataEntry WallOfFlesh;
		public static BossDataEntry TheDestroyer;
		public static BossDataEntry Retinazer;
		public static BossDataEntry Spazmatism;
		public static BossDataEntry SkeletronPrime;
		public static BossDataEntry Plantera;
		public static BossDataEntry Golem;
		public static BossDataEntry DukeFishron;
		public static BossDataEntry Betsy;
		public static BossDataEntry MoonLord;


		////

		static BossData() {
			BossData._BossOrder = new List<BossDataEntry>();
			BossData._BossMap = new Dictionary<string, int>();
			BossData.BossOrder = ( (List<BossDataEntry>)BossData._BossOrder ).AsReadOnly();
			BossData.BossMap = new ReadOnlyDictionary<string, int>( BossData._BossMap );

			 BossData.AppendBoss( "Ersion", "Giant Slime" );
			 BossData.AppendBoss( "Thorium", "Grand Thunderbird" );
			 BossData.AppendBoss( "Spirit", "Scarabeus" );
			BossData.KingSlime = BossData.AppendBoss( "Vanilla", "King Slime" );
			 BossData.AppendBoss( "W1K", "Yian Kut Ku" );
			 BossData.AppendBoss( "Calamity", "Desert Scourge" );
			BossData.EyeOfCthulhu = BossData.AppendBoss( "Vanilla", "Eye of Cthulhu" );
			 BossData.AppendBoss( "Tremor", "Rukh" );
			 BossData.AppendBoss( "Gabehaswon", "Empress Fly" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Antlion Queen" );
			BossData.EaterOfWorlds = BossData.AppendBoss( "Vanilla", "Eater of Worlds" );
			BossData.BrainOfCthulhu = BossData.AppendBoss( "Vanilla", "Brain of Cthulhu" );
			 BossData.AppendBoss( "Gabehaswon", "The Murk" );
			 BossData.AppendBoss( "Tremor", "Tiki Totem" );
			 BossData.AppendBoss( "Tremor", "Evil Corn" );
			 BossData.AppendBoss( "Tremor", "Storm Jellyfish" );
			 BossData.AppendBoss( "Grealm", "Folivine" );
			 BossData.AppendBoss( "Shrooms", "Shroomus" );
			 BossData.AppendBoss( "Epicness Remastered", "Red Goblin King" );
			 BossData.AppendBoss( "Epicness Remastered", "Meteorite Guardian" );
			 BossData.AppendBoss( "Calamity", "The Perforators" );
			 BossData.AppendBoss( "Calamity", "Hive Mind" );
			 BossData.AppendBoss( "Exodus", "Desert Emperor" );
			BossData.QueenBee = BossData.AppendBoss( "Vanilla", "Queen Bee" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Clampula" );
			 BossData.AppendBoss( "Spirit", "Ancient Flyer" );
			 BossData.AppendBoss( "Thorium", "Queen Jellyfish" );
			BossData.Skeletron = BossData.AppendBoss( "Vanilla", "Skeletron" );
			 BossData.AppendBoss( "W1K", "Aquatix" );
			 BossData.AppendBoss( "W1K", "Ardorix" );
			 BossData.AppendBoss( "W1K", "Arborix" );
			 BossData.AppendBoss( "Elerium", "Cursed Pharaoh" );
			 BossData.AppendBoss( "Thorium", "Granite Energy Storm" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Mother Spitter" );
			 BossData.AppendBoss( "Thorium", "Star Scouter" );
			 BossData.AppendBoss( "Sacred Tools", "Jensen (Grand Harpy)" );
			 BossData.AppendBoss( "Thorium", "Buried Champion" );
			 BossData.AppendBoss( "Spirit", "Starplate Raider" );
			 BossData.AppendBoss( "Calamity", "Slime God" );
			 BossData.AppendBoss( "Tremor", "Heater of Worlds" );
			BossData.WallOfFlesh = BossData.AppendBoss( "Vanilla", "Wall of Flesh" );	// HM
			 BossData.AppendBoss( "W1K", "Ivy Plant" );
			 BossData.AppendBoss( "Thorium", "Borean Strider" );
			 BossData.AppendBoss( "Thorium", "Coznix" );
			 BossData.AppendBoss( "Tremor", "Alchemaster" );
			 BossData.AppendBoss( "Calamity", "Cryogen" );
			 BossData.AppendBoss( "Spirit", "Infernon" );
			 BossData.AppendBoss( "Gabehaswon", "Drone" );
			 BossData.AppendBoss( "Ersion", "Gold Slime" );
			 BossData.AppendBoss( "W1K", "Rathalos" );
			 BossData.AppendBoss( "W1K", "Ridley" );
			 BossData.AppendBoss( "W1K", "Death" );
			 BossData.AppendBoss( "Epicness Remastered", "Pixie King" );
			BossData.Retinazer = BossData.AppendBoss( "Vanilla", "Retinazer" );
			BossData.Spazmatism = BossData.AppendBoss( "Vanilla", "Spazmatism" );
			 BossData.AppendBoss( "Spirit", "Dusking" );
			 BossData.AppendBoss( "Calamity", "Brimstone Elemental" );
			 BossData.AppendBoss( "Mod of Randomness", "Infected Eye" );
			BossData.TheDestroyer = BossData.AppendBoss( "Vanilla", "The Destroyer" );
			 BossData.AppendBoss( "Tremor", "Motherboard" );
			 BossData.AppendBoss( "Elerium", "The Controller" );
			BossData.SkeletronPrime = BossData.AppendBoss( "Vanilla", "Skeletron Prime" );
			 BossData.AppendBoss( "Thorium", "Lich" );
			 BossData.AppendBoss( "Tremor", "Cog Lord" );
			 BossData.AppendBoss( "Tremor", "Pixie Queen" );
			 BossData.AppendBoss( "Calamity", "Calamitas" );
			 BossData.AppendBoss( "W1K", "Dark Shogun Mask/Okiku" );
			 BossData.AppendBoss( "Sacred Tools", "Harpy Queen Raynare" );
			 BossData.AppendBoss( "Elerium", "Antlion Broodmother" );
			 BossData.AppendBoss( "Ersion", "Bionic Brain" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Master Drone" );
			 BossData.AppendBoss( "Necronaquen's Mod", "Probe Queen" );
			 BossData.AppendBoss( "Epicness Remastered", "Argoth, The Demon Lord" );
			BossData.Plantera = BossData.AppendBoss( "Vanilla", "Plantera" );
			 BossData.AppendBoss( "Spirit", "Illuminant Master" );
			 BossData.AppendBoss( "Elerium", "The War King" );
			 BossData.AppendBoss( "Thorium", "Abyssion" );
			 BossData.AppendBoss( "Calamity", "Leviathan" );
			 BossData.AppendBoss( "Grealm", "The Horde (Barbarian)" );
			 BossData.AppendBoss( "Tremor", "Mothership/Cyber King" );
			 BossData.AppendBoss( "Tremor", "Frost King" );
			BossData.Golem = BossData.AppendBoss( "Vanilla", "Golem" );
			 BossData.AppendBoss( "Calamity", "Plaguebringer Goliath" );
			BossData.Betsy = BossData.AppendBoss( "Vanilla", "Betsy" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Behemoth" );
			 BossData.AppendBoss( "Crystillium", "The Crystal King" );
			 BossData.AppendBoss( "Nightmares Unleashed", "Gimimmick" );
			 BossData.AppendBoss( "Mod of Randomness", "Vlitch Cleaver" );
			 BossData.AppendBoss( "Spirit", "Atlas" );
			BossData.DukeFishron = BossData.AppendBoss( "Vanilla", "Duke Fishron" );
			 BossData.AppendBoss( "Epicness Remastered", "Derpatron" );
			 BossData.AppendBoss( "Pumpking", "The Pumpking’s Horseman" );
			 BossData.AppendBoss( "BlueMagic's Mod", "The Abomination" );
			BossData.DukeFishron = BossData.AppendBoss( "Vanilla", "Lunatic Cultist" );
			BossData.MoonLord = BossData.AppendBoss( "Vanilla", "Moon Lord's Core" );  //MoonLordHead?
			 BossData.AppendBoss( "Sacred Tools", "Abaddon, Shade of Nightmares" );
			 BossData.AppendBoss( "Nightmares Unleashed", "The Void Marshall" );
			 BossData.AppendBoss( "Epicness Remastered", "Dark Nebula" );
			 BossData.AppendBoss( "Sacred Tools", "Araghur (Flare Serpent)" );
			 BossData.AppendBoss( "Thorium", "The Ragnarök" );
			 BossData.AppendBoss( "Spirit", "Overseer" );
			 BossData.AppendBoss( "Pumpking", "Terra Lord" );
			 BossData.AppendBoss( "Sacred Tools", "Lunarians" );
			 BossData.AppendBoss( "Mod of Randomness", "Vlitch Gigipede" );
			 BossData.AppendBoss( "Zoaklen", "Magical Cube" );
			 BossData.AppendBoss( "Tremor", "The Dark Emperor" );
			 BossData.AppendBoss( "Epicness", "Corruptor of Worlds" );
			 BossData.AppendBoss( "Calamity", "Profaned Guardians" );
			 BossData.AppendBoss( "BlueMagic's Mod", "The Abomination (Rematch)" );
			 BossData.AppendBoss( "Tremor", "Bruttalisk" );
			 BossData.AppendBoss( "Tremor", "Space Whale" );
			 BossData.AppendBoss( "Tremor", "The Trinity" );
			 BossData.AppendBoss( "Joost", "Jumbo Cactaur" );
			 BossData.AppendBoss( "Joost", "SA-X" );
			 BossData.AppendBoss( "Calamity", "Providence, The Profaned God" );
			 BossData.AppendBoss( "Calamity", "Ceaseless Void" );
			 BossData.AppendBoss( "Calamity", "Storm Weaver" );
			 BossData.AppendBoss( "Calamity", "Signus" );
			 BossData.AppendBoss( "BlueMagic's Mod", "Spirit of Purity" );
			 BossData.AppendBoss( "Calamity", "The Devourer of Gods" );
			 BossData.AppendBoss( "Calamity", "Bumblebirb" );
			 BossData.AppendBoss( "Calamity", "Yharon The Jungle Dragon" );
			 BossData.AppendBoss( "Joost", "Gilgamesh" );
			 BossData.AppendBoss( "Calamity", "Supreme Calamitas" );
			 BossData.AppendBoss( "BlueMagic's Mod", "Spirit of Chaos" );

			BossData.UpdateOrder();
		}


		////

		private static BossDataEntry AppendBoss( string sourceMod, string name ) {
			var entry = new BossDataEntry { Name = name };
			int order = BossData._BossOrder.Count;

			BossData._BossOrder.Add( entry );
			BossData._BossMap[ name ] = order;

			return entry;
		}

		public static BossDataEntry AddBoss( string sourceMod, string name, string betterThan ) {
			var entry = new BossDataEntry { Name = name };
			int order = BossData.BossMap[ betterThan ];

			BossData._BossOrder.Insert( order, entry );

			BossData.UpdateOrder();

			return entry;
		}

		////

		public static void UpdateOrder() {
			int count = BossData.BossOrder.Count;

			for( int i=0; i<count; i++ ) {
				string name = BossData.BossOrder[i].Name;
				BossData._BossMap[ name ] = i;
			}
		}
	}
}
