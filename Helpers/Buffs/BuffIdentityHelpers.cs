using HamstarHelpers.Components.DataStructures;


namespace HamstarHelpers.Helpers.Buffs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to buff identification.
	/// </summary>
	public partial class BuffIdentityHelpers {
		/// <summary>
		/// A map of buff names to their Terraria IDs.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}
	}
}
