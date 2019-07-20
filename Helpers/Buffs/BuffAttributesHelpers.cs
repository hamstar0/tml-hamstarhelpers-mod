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

			return Lang.GetBuffName( buffType );
		}
	}
}
