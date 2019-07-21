using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Components.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC identification.
	/// </summary>
	public partial class NPCIdentityHelpers {
		/// <summary>
		/// Gets a (human readable) unique key from a given NPC type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetUniqueKey( int type ) {
			if( type < -65 || type >= NPCLoader.NPCCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < NPCID.Count ) {
				return "Terraria " + NPCID.Search.GetName( type );
			}

			var modNPC = NPCLoader.GetNPC( type );
			return $"{modNPC.mod.Name} {modNPC.Name}";
		}

		/// <summary>
		/// Gets a (human readable) unique key from a given NPC.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetUniqueKey( NPC npc ) => GetUniqueKey( npc.type );

		////

		/// <summary>
		/// Gets an NPC type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !NPCID.Search.ContainsName( parts[1] ) ) {
					return 0;
				}
				return NPCID.Search.GetId( parts[1] );
			}
			return ModLoader.GetMod( parts[0] )?.NPCType( parts[1] ) ?? 0;
		}


		////////////////

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
