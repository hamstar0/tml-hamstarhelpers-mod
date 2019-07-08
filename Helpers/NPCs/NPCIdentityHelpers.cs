using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Components.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC identification.
	/// </summary>
	public partial class NPCIdentityHelpers {
		/// <summary>
		/// Gets the identifier of an NPC. For Terraria NPCs, this is `Terraria ZombieDoctor`, with the portion after "Terraria"
		/// being the NPC's field name in `NPCID`. For modded items, the format is NPCModName ModdedNPCInternalName; the mod
		/// name first, and the modded NPC's internal `Name` after.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static string GetUniqueId( int npcType ) {
			if( NPCID.Search.ContainsId( npcType ) ) {
				return "Terraria " + NPCID.Search.GetName( npcType );
			} else {
				var npc = new NPC();
				npc.SetDefaults( npcType );

				if( npc.modNPC != null ) {
					return npc.modNPC.mod.Name + " " + npc.modNPC.Name;
				}
			}

			return "" + npcType;
		}

		/// <summary>
		/// Gets the identifier of an NPC. For Terraria NPCs, this is `Terraria ZombieDoctor`, with the portion after "Terraria"
		/// being the NPC's field name in `NPCID`. For modded items, the format is NPCModName ModdedNPCInternalName; the mod
		/// name first, and the modded NPC's internal `Name` after.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetUniqueId( NPC npc ) {
			if( npc.modNPC == null ) {
				return "Terraria " + NPCID.Search.GetName( npc.type );
			} else {
				return npc.modNPC.mod.Name + " " + npc.modNPC.Name;
			}
		}

		////

		/*public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}*/


		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetQualifiedName( NPC npc ) {
			return Lang.GetNPCNameValue( npc.netID );
		}

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="netid"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int netid ) {    //npcType?
			return Lang.GetNPCNameValue( netid );
		}

		// TODO: GetVanillaSnapshotHash



		////////////////
		
		/// <summary>
		/// Gets an NPC type from its unique ID string.
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public static int GetNpcTypeByUniqueId( string uid ) {
			string[] npcNameSplit = uid.Split( ' ' );

			if( npcNameSplit[0] == "Terraria" ) {
				return NPCID.Search.GetId( npcNameSplit[1] );
			} else {
				Mod mod = ModLoader.GetMod( npcNameSplit[0] );
				if( mod == null ) {
					return 0;
				}

				npcNameSplit = npcNameSplit.Copy( 1, npcNameSplit.Length - 1 );
				string modNpcName = string.Join( " ", npcNameSplit );

				return mod.NPCType( modNpcName );
			}
		}


		////////////////

		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds =>
			ModHelpersMod.Instance.NPCIdentityHelpers._NamesToIds;
	}
}
