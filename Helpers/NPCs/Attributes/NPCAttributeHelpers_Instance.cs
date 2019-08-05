using HamstarHelpers.Classes.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.NPCs.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class NPCAttributeHelpers {
		private ReadOnlyDictionaryOfSets<string, int> _DisplayNamesToIds = null;


		////////////////

		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds =>
			ModHelpersMod.Instance.NPCAttributeHelpers._DisplayNamesToIds;



		////////////////

		internal NPCAttributeHelpers() { }


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				string name = NPCAttributeHelpers.GetQualifiedName( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._DisplayNamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
