using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs.Attributes;


namespace HamstarHelpers.Services.GameData {
	/// <summary></summary>
	public class BossDataEntry {
		/// <summary></summary>
#pragma warning disable CS0618 // Type or member is obsolete
		public int Order => BossData.BossMap[ this.Name ];


		////

		/// <summary></summary>
		public string Name;

		/// <summary></summary>
		public bool IsHardmode => this.Order > BossData.WallOfFlesh.Order;
		/// <summary></summary>
		public bool IsPostMechBoss => this.Order > BossData.SkeletronPrime.Order;
		/// <summary></summary>
		public bool IsPostPlantera => this.Order > BossData.Plantera.Order;
		/// <summary></summary>
		public bool IsPostGolem => this.Order > BossData.Golem.Order;
		/// <summary></summary>
		public bool IsPostMoonlord => this.Order > BossData.MoonLord.Order;
#pragma warning restore CS0618 // Type or member is obsolete



		////////////////

		/// <summary>
		/// Gets the NPC id of a given boss.
		/// </summary>
		/// <param name="npcId"></param>
		/// <returns></returns>
		public bool GetNpcId( out int npcId ) {
			return NPCAttributeHelpers.DisplayNamesToIds.TryGetValue( this.Name, out npcId );
		}
	}




	/// <summary>
	/// Supplies a table of all available bosses across vanilla Terraria and most major mods.
	/// 
	/// IMPORTANT: Not finished; untested.
	/// </summary>
	[Obsolete("Incomplete")]
	public class BossData {
		private static IList<BossDataEntry> _BossOrder;
		private static IDictionary<string, int> _BossMap;

		/// <summary>
		/// Orders bosses by guesstimations of sequence.
		/// </summary>
		public static IReadOnlyList<BossDataEntry> BossOrder { get; }
		/// <summary>
		/// Maps bosses (by name) to their (loosely) expected progression order.
		/// </summary>
		public static IReadOnlyDictionary<string, int> BossMap { get; }

		/// <summary></summary>
		public static BossDataEntry KingSlime;
		/// <summary></summary>
		public static BossDataEntry EyeOfCthulhu;
		/// <summary></summary>
		public static BossDataEntry EaterOfWorlds;
		/// <summary></summary>
		public static BossDataEntry BrainOfCthulhu;
		/// <summary></summary>
		public static BossDataEntry QueenBee;
		/// <summary></summary>
		public static BossDataEntry Skeletron;
		/// <summary></summary>
		public static BossDataEntry WallOfFlesh;
		/// <summary></summary>
		public static BossDataEntry TheDestroyer;
		/// <summary></summary>
		public static BossDataEntry Retinazer;
		/// <summary></summary>
		public static BossDataEntry Spazmatism;
		/// <summary></summary>
		public static BossDataEntry SkeletronPrime;
		/// <summary></summary>
		public static BossDataEntry Plantera;
		/// <summary></summary>
		public static BossDataEntry Golem;
		/// <summary></summary>
		public static BossDataEntry DukeFishron;
		/// <summary></summary>
		public static BossDataEntry Betsy;
		/// <summary></summary>
		public static BossDataEntry MoonLord;


		////

		static BossData() {	// WIP!
			BossData._BossOrder = new List<BossDataEntry>();
			BossData._BossMap = new Dictionary<string, int>();
			BossData.BossOrder = ( (List<BossDataEntry>)BossData._BossOrder ).AsReadOnly();
			BossData.BossMap = new ReadOnlyDictionary<string, int>( BossData._BossMap );
			
			 BossData.AppendBoss( "Ersion", "Giant Slime" );
			 BossData.AppendBoss( "ThoriumMod", "The Grand Thunderbird" );
			 BossData.AppendBoss( "SpiritMod", "Scarabeus" );
			 BossData.AppendBoss( "Exodus", "Abomination" );
			BossData.KingSlime = BossData.AppendBoss( "Terraria", "Terraria KingSlime" );
			 BossData.AppendBoss( "Exodus", "Evil Blob" );
			 BossData.AppendBoss( "W1KModRedux", "Yian Kut Ku" );
			 BossData.AppendBoss( "CalamityMod", "Desert Scourge" );
			BossData.EyeOfCthulhu = BossData.AppendBoss( "Terraria", "Terraria EyeofCthulhu" );
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
			BossData.EaterOfWorlds = BossData.AppendBoss( "Terraria", "Terraria EaterofWorldsHead" );
			BossData.BrainOfCthulhu = BossData.AppendBoss( "Terraria", "Terraria BrainofCthulhu" );
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
			BossData.QueenBee = BossData.AppendBoss( "Terraria", "Terraria QueenBee" );
			 BossData.AppendBoss( "SacredTools", "The Flaming Pumpkin" );
			 BossData.AppendBoss( "Antiaris", "Antlion Queen" );
			 BossData.AppendBoss( "TrueEater", "Clampula" );
			 BossData.AppendBoss( "SpiritMod", "Ancient Flyer" );
			BossData.Skeletron = BossData.AppendBoss( "Terraria", "Terraria SkeletronHead" );
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
			BossData.WallOfFlesh = BossData.AppendBoss( "Terraria", "Terraria WallofFlesh" );	// HM
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
			BossData.Retinazer = BossData.AppendBoss( "Terraria", "Terraria Retinazer" );
			BossData.Spazmatism = BossData.AppendBoss( "Terraria", "Terraria Spazmatism" );
			 BossData.AppendBoss( "SpiritMod", "Dusking" );
			 BossData.AppendBoss( "SpiritMod", "Ethereal Umbra" );
			 BossData.AppendBoss( "CalamityMod", "Brimstone Elemental" );
			 BossData.AppendBoss( "MinimgMod", "Infected Eye" );
			BossData.TheDestroyer = BossData.AppendBoss( "Terraria", "Terraria TheDestroyer" );
			 BossData.AppendBoss( "CalamityMod", "Aquatic Scourge" );
			 BossData.AppendBoss( "Tremor", "Motherboard" );
			 BossData.AppendBoss( "Elerium", "The Controller" );
			BossData.SkeletronPrime = BossData.AppendBoss( "Terraria", "Terraria SkeletronPrime" );
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
			BossData.Plantera = BossData.AppendBoss( "Terraria", "Terraria Plantera" );
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
			BossData.Golem = BossData.AppendBoss( "Terraria", "Terraria Golem" );
			 BossData.AppendBoss( "Tremor", "Cog Lord" );
			 BossData.AppendBoss( "Tremor", "Mothership" );
			 BossData.AppendBoss( "Tremor", "Cyber King" );
			 BossData.AppendBoss( "ThoriumMod", "Abyssion, The Forgotten One" );
			 BossData.AppendBoss( "CalamityMod", "The Plaguebringer Goliath" );
			BossData.Betsy = BossData.AppendBoss( "Terraria", "Terraria DD2Betsy" );
			 BossData.AppendBoss( "TrueEater", "Behemoth" );
			 BossData.AppendBoss( "Crystillium", "The Crystal King" );
			 BossData.AppendBoss( "TrueEater", "Gimimmick" );
			 BossData.AppendBoss( "MinimgMod", "Vlitch Cleaver" );
			BossData.DukeFishron = BossData.AppendBoss( "Terraria", "Terraria DukeFishron" );
			 BossData.AppendBoss( "SpiritMod", "Atlas" );
			 BossData.AppendBoss( "EpicnessMod", "Derpatron" );
			 BossData.AppendBoss( "Pumpking", "The Pumpking’s Horseman" );
			 BossData.AppendBoss( "BlueMagic", "The Abomination" );
			 BossData.AppendBoss( "CalamityMod", "Ravagers" );
			BossData.DukeFishron = BossData.AppendBoss( "Terraria", "Terraria CultistBoss" );
			BossData.MoonLord = BossData.AppendBoss( "Terraria", "Terraria MoonLordCore" );  //MoonLordHead?
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


		////

		/// <summary>
		/// Adds a boss definition relative to another boss in progression order.
		/// </summary>
		/// <param name="sourceMod"></param>
		/// <param name="bossName"></param>
		/// <param name="betterThanBossOfName"></param>
		/// <returns></returns>
		public static BossDataEntry AddBoss( string sourceMod, string bossName, string betterThanBossOfName ) {
			var entry = new BossDataEntry { Name = bossName };
			int order = BossData.BossMap[ betterThanBossOfName ];

			BossData._BossOrder.Insert( order, entry );

			BossData.UpdateOrder();

			return entry;
		}

		/// <summary>
		/// Refreshes boss progression order.
		/// </summary>
		public static void UpdateOrder() {
			int count = BossData.BossOrder.Count;

			for( int i=0; i<count; i++ ) {
				string name = BossData.BossOrder[i].Name;
				BossData._BossMap[ name ] = i;
			}
		}
	}
}
