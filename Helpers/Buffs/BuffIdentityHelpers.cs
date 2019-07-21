using HamstarHelpers.Components.DataStructures;
using ReLogic.Reflection;
using System;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Buffs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to buff identification.
	/// </summary>
	public partial class BuffIdentityHelpers {
		private static readonly IdDictionary Search = IdDictionary.Create<BuffID, int>();



		////////////////

		/// <summary>
		/// Gets a (human readable) unique key from a given buff type.
		/// </summary>
		/// <param name="buffType"></param>
		/// <returns></returns>
		public static string GetUniqueKey( int buffType ) {
			if( buffType < 1 || buffType >= BuffLoader.BuffCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + buffType );
			}
			if( buffType < BuffID.Count ) {
				return "Terraria " + BuffIdentityHelpers.Search.GetName( buffType );
			}

			var modBuff = BuffLoader.GetBuff( buffType );
			return $"{modBuff.mod.Name} {modBuff.Name}";
		}


		/// <summary>
		/// Gets a buff type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !BuffIdentityHelpers.Search.ContainsName( parts[1] ) ) {
					return 0;
				}
				return BuffIdentityHelpers.Search.GetId( parts[1] );
			}
			return ModLoader.GetMod( parts[0] )?.BuffType( parts[1] ) ?? 0;
		}



		////////////////

		/// <summary>
		/// A map of buff names to their Terraria IDs.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds {
			get { return ModHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}
	}
}
