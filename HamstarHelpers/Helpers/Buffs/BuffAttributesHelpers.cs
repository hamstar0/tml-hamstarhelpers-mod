using HamstarHelpers.Classes.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Buffs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to buff attributes.
	/// </summary>
	public partial class BuffAttributesHelpers {
		/// <summary>
		/// A map of buff names to their Terraria IDs.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds {
			get { return ModHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}



		////////////////

		/// <summary>
		/// Alias for `Lang.GetBuffName(int)`.
		/// 
		/// Credit: Jofairden @ Even More Modifiers
		/// </summary>
		/// <param name="buffType"></param>
		/// <returns></returns>
		public static string GetBuffDisplayName( int buffType ) {
			if( buffType >= BuffID.Count ) {
				return BuffLoader.GetBuff( buffType )?
					.DisplayName
					.GetTranslation( LanguageManager.Instance.ActiveCulture )
					?? "null";
			}

			try {
				return Lang.GetBuffName( buffType );
			} catch {
				return "";
			}
		}
	}
}
