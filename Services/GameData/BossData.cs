using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace HamstarHelpers.Services.GameData {
	public class BossDataEntry {
		[Obsolete("use `GetNpcId()`", true)]
		public int NpcId {
			get {
				int npcId;
				bool found = this.GetNpcId( out npcId );
				return npcId;
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



		////////////////

		public bool GetNpcId( out int npcId ) {
			return NPCIdentityHelpers.NamesToIds.TryGetValue( this.Name, out npcId );
		}
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
			 BossData.AppendBoss( "ThoriumMod", "The Grand Thunderbird" );
			 BossData.AppendBoss( "SpiritMod", "Scarabeus" );
			 BossData.AppendBoss( "Exodus", "Abomination" );
			BossData.KingSlime = BossData.AppendBoss( "Vanilla", "King Slime" );
			 BossData.AppendBoss( "Exodus", "Evil Blob" );
			 BossData.AppendBoss( "W1KModRedux", "Yian Kut Ku" );
			 BossData.AppendBoss( "CalamityMod", "Desert Scourge" );
			BossData.EyeOfCthulhu = BossData.AppendBoss( "Vanilla", "Eye of Cthulhu" );
			 BossData.AppendBoss( "Split", "Spirit" );
			 BossData.AppendBoss( "Exodus", "Master of Possession" );
			 BossData.AppendBoss( "Exodus", "Sludge Heart" );
			 BossData.AppendBoss( "Exodus", "Colossus" );
			 BossData.AppendBoss( "Exodus", "Mindflayer" );
			 BossData.AppendBoss( "Laugicality", "Dune Sharkron" );
			 BossData.AppendBoss( "Laugicality", "Hypothema" );
			 BossData.AppendBoss( "Laugicality", "Ragnar" );
			 BossData.AppendBoss( "Laugicality", "Andesia" );
			 BossData.AppendBoss( "Laugicality", "Dioritus" );
			 BossData.AppendBoss( "Laugicality", "AnDio" );
			 BossData.AppendBoss( "Tremor", "The Rukh" );
			 BossData.AppendBoss( "GabeHasWonsMod", "Empress Fly" );
			 BossData.AppendBoss( "TrueEater", "Antlion Queen" );
			 BossData.AppendBoss( "CalamityMod", "Crabulon" );
			BossData.EaterOfWorlds = BossData.AppendBoss( "Vanilla", "Eater of Worlds" );
			BossData.BrainOfCthulhu = BossData.AppendBoss( "Vanilla", "Brain of Cthulhu" );
			 BossData.AppendBoss( "Split", "One-Shot" );
			 BossData.AppendBoss( "SpiritMod", "Vinewrath Bane" );
			 BossData.AppendBoss( "GabeHasWonsMod", "The Murk" );
			 BossData.AppendBoss( "Tremor", "Tiki Totem" );
			 BossData.AppendBoss( "Tremor", "Evil Corn" );
			 BossData.AppendBoss( "Tremor", "Storm Jellyfish" );
			 BossData.AppendBoss( "Grealm", "Folivine" );
			 BossData.AppendBoss( "Shrooms", "Shroomus" );
			 BossData.AppendBoss( "EpicnessMod", "Red Goblin King" );
			 BossData.AppendBoss( "EpicnessMod", "Meteorite Guardian" );
			 BossData.AppendBoss( "CalamityMod", "The Perforators" );
			 BossData.AppendBoss( "CalamityMod", "The Hive Mind" );
			 BossData.AppendBoss( "Exodus", "Desert Emperor" );
			 BossData.AppendBoss( "ThoriumMod", "The Queen Jellyfish" );
			 BossData.AppendBoss( "ThoriumMod", "Viscount" );
			BossData.QueenBee = BossData.AppendBoss( "Vanilla", "Queen Bee" );
			 BossData.AppendBoss( "SacredTools", "The Flaming Pumpkin" );
			 BossData.AppendBoss( "Antiaris", "Antlion Queen" );
			 BossData.AppendBoss( "TrueEater", "Clampula" );
			 BossData.AppendBoss( "SpiritMod", "Ancient Flyer" );
			BossData.Skeletron = BossData.AppendBoss( "Vanilla", "Skeletron" );
			 BossData.AppendBoss( "W1KModRedux", "Aquatix" );
			 BossData.AppendBoss( "W1KModRedux", "Ardorix" );
			 BossData.AppendBoss( "W1KModRedux", "Arborix" );
			 BossData.AppendBoss( "Elerium", "Cursed Pharaoh" );
			 BossData.AppendBoss( "ThoriumMod", "Granite Energy Storm" );
			 BossData.AppendBoss( "TrueEater", "Mother Spitter" );
			 BossData.AppendBoss( "ThoriumMod", "The Star Scouter" );
			 BossData.AppendBoss( "SacredTools", "Jensen, the Grand Harpy" );
			 BossData.AppendBoss( "ThoriumMod", "The Buried Champion" );
			 BossData.AppendBoss( "SpiritMod", "Starplate Raider" );
			 BossData.AppendBoss( "CalamityMod", "The Slime God" );
			 BossData.AppendBoss( "Tremor", "Heater of Worlds" );
			 BossData.AppendBoss( "Tremor", "Fungus Beetle" );
			BossData.WallOfFlesh = BossData.AppendBoss( "Vanilla", "Wall of Flesh" );	// HM
			 BossData.AppendBoss( "SacredTools", "Harpy Queen Raynare" );
			 BossData.AppendBoss( "Antiaris", "Tower Keeper" );
			 BossData.AppendBoss( "W1KModRedux", "Ivy Plant" );
			 BossData.AppendBoss( "ThoriumMod", "Borean Strider" );
			 BossData.AppendBoss( "ThoriumMod", "Coznix, The Fallen Beholder" );
			 BossData.AppendBoss( "Tremor", "Alchemaster" );
			 BossData.AppendBoss( "CalamityMod", "Cryogen" );
			 BossData.AppendBoss( "SpiritMod", "Infernon" );
			 BossData.AppendBoss( "GabeHasWonsMod", "Drone" );
			 BossData.AppendBoss( "Ersion", "Gold Slime" );
			 BossData.AppendBoss( "W1KModRedux", "Rathalos" );
			 BossData.AppendBoss( "W1KModRedux", "Ridley" );
			 BossData.AppendBoss( "W1KModRedux", "Death" );
			 BossData.AppendBoss( "EpicnessMod", "Pixie King" );
			BossData.Retinazer = BossData.AppendBoss( "Vanilla", "Retinazer" );
			BossData.Spazmatism = BossData.AppendBoss( "Vanilla", "Spazmatism" );
			 BossData.AppendBoss( "SpiritMod", "Dusking" );
			 BossData.AppendBoss( "SpiritMod", "Ethereal Umbra" );
			 BossData.AppendBoss( "CalamityMod", "Brimstone Elemental" );
			 BossData.AppendBoss( "MinimgMod", "Infected Eye" );
			BossData.TheDestroyer = BossData.AppendBoss( "Vanilla", "The Destroyer" );
			 BossData.AppendBoss( "CalamityMod", "Aquatic Scourge" );
			 BossData.AppendBoss( "Tremor", "Motherboard" );
			 BossData.AppendBoss( "Elerium", "The Controller" );
			BossData.SkeletronPrime = BossData.AppendBoss( "Vanilla", "Skeletron Prime" );
			 BossData.AppendBoss( "Laugicality", "The Annihilator" );
			 BossData.AppendBoss( "Laugicality", "Slybertron" );
			 BossData.AppendBoss( "Laugicality", "Steam Train" );
			 BossData.AppendBoss( "ThoriumMod", "The Lich" );
			 BossData.AppendBoss( "Tremor", "Pixie Queen" );
			 BossData.AppendBoss( "CalamityMod", "Calamitas" );
			 BossData.AppendBoss( "W1KModRedux", "Dark Shogun Mask/Okiku" );
			 BossData.AppendBoss( "SacredTools", "Harpy Queen Raynare" );
			 BossData.AppendBoss( "Elerium", "Antlion Broodmother" );
			 BossData.AppendBoss( "Ersion", "Bionic Brain" );
			 BossData.AppendBoss( "TrueEater", "Master Drone" );
			 BossData.AppendBoss( "NecronaquensMod", "Probe Queen" );
			 BossData.AppendBoss( "EpicnessMod", "Argoth, The Demon Lord" );
			BossData.Plantera = BossData.AppendBoss( "Vanilla", "Plantera" );
			 BossData.AppendBoss( "Laugicality", "Etheria" );
			 BossData.AppendBoss( "SpiritMod", "Illuminant Master" );
			 BossData.AppendBoss( "Elerium", "The War King" );
			 BossData.AppendBoss( "CalamityMod", "The Siren" );
			 BossData.AppendBoss( "CalamityMod", "The Leviathan" );
			 BossData.AppendBoss( "CalamityMod", "Astrum Aureus" );
			 BossData.AppendBoss( "CalamityMod", "Astrum Deus" );
			 BossData.AppendBoss( "Grealm", "The Horde (Barbarian)" );
			 BossData.AppendBoss( "Tremor", "Frost King" );
			 BossData.AppendBoss( "Tremor", "Wall of Shadows" );
			BossData.Golem = BossData.AppendBoss( "Vanilla", "Golem" );
			 BossData.AppendBoss( "Tremor", "Cog Lord" );
			 BossData.AppendBoss( "Tremor", "Mothership" );
			 BossData.AppendBoss( "Tremor", "Cyber King" );
			 BossData.AppendBoss( "ThoriumMod", "Abyssion, The Forgotten One" );
			 BossData.AppendBoss( "CalamityMod", "The Plaguebringer Goliath" );
			BossData.Betsy = BossData.AppendBoss( "Vanilla", "Betsy" );
			 BossData.AppendBoss( "TrueEater", "Behemoth" );
			 BossData.AppendBoss( "Crystillium", "The Crystal King" );
			 BossData.AppendBoss( "TrueEater", "Gimimmick" );
			 BossData.AppendBoss( "MinimgMod", "Vlitch Cleaver" );
			BossData.DukeFishron = BossData.AppendBoss( "Vanilla", "Duke Fishron" );
			 BossData.AppendBoss( "SpiritMod", "Atlas" );
			 BossData.AppendBoss( "EpicnessMod", "Derpatron" );
			 BossData.AppendBoss( "Pumpking", "The Pumpking’s Horseman" );
			 BossData.AppendBoss( "BlueMagic", "The Abomination" );
			 BossData.AppendBoss( "CalamityMod", "Ravagers" );
			BossData.DukeFishron = BossData.AppendBoss( "Vanilla", "Lunatic Cultist" );
			BossData.MoonLord = BossData.AppendBoss( "Vanilla", "Moon Lord's Core" );  //MoonLordHead?
			 BossData.AppendBoss( "SacredTools", "Abaddon, the Emissary of Nightmares" );
			 BossData.AppendBoss( "TrueEater", "The Void Marshall" );
			 BossData.AppendBoss( "EpicnessMod", "Dark Nebula" );
			 BossData.AppendBoss( "SacredTools", "Araghur, the Flare Serpent" );
			 BossData.AppendBoss( "ThoriumMod", "The Ragnarök" );
			 BossData.AppendBoss( "SpiritMod", "Overseer" );
			 BossData.AppendBoss( "Pumpking", "Terra Lord" );
			 BossData.AppendBoss( "SacredTools", "The Lunarians" );
			 BossData.AppendBoss( "SacredTools", "The Challenger" );
			 BossData.AppendBoss( "MinimgMod", "Vlitch Gigipede" );
			 BossData.AppendBoss( "ZoaklenMod", "Magical Cube" );
			 BossData.AppendBoss( "Tremor", "The Dark Emperor" );
			 BossData.AppendBoss( "Epicness", "Corruptor of Worlds" );
			 BossData.AppendBoss( "CalamityMod", "Profaned Guardians" );
			 BossData.AppendBoss( "BlueMagic", "The Abomination (Rematch)" );
			 BossData.AppendBoss( "Tremor", "Brutallisk" );
			 BossData.AppendBoss( "Tremor", "Space Whale" );
			 BossData.AppendBoss( "Tremor", "The Trinity" );
			 BossData.AppendBoss( "Tremor", "Andas" );
			 BossData.AppendBoss( "JoostMod", "Jumbo Cactaur" );
			 BossData.AppendBoss( "JoostMod", "SA-X" );
			 BossData.AppendBoss( "CalamityMod", "Providence, The Profaned Goddess" );
			 BossData.AppendBoss( "CalamityMod", "Ceaseless Void" );
			 BossData.AppendBoss( "CalamityMod", "Storm Weaver" );
			 BossData.AppendBoss( "CalamityMod", "Signus, Envoy of the Devourer" );
			 BossData.AppendBoss( "CalamityMod", "Polterghast" );
			 BossData.AppendBoss( "BlueMagic", "Spirit of Purity" );
			 BossData.AppendBoss( "CalamityMod", "The Devourer of Gods" );
			 BossData.AppendBoss( "CalamityMod", "Bumblebirb" );
			 BossData.AppendBoss( "CalamityMod", "Yharon The Jungle Dragon" );
			 BossData.AppendBoss( "JoostMod", "Gilgamesh" );
			 BossData.AppendBoss( "CalamityMod", "Supreme Calamitas" );
			 BossData.AppendBoss( "BlueMagic", "Spirit of Chaos" );

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
