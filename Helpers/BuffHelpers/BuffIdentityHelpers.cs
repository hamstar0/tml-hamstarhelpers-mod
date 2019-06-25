using HamstarHelpers.Components.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.BuffHelpers {
	public partial class BuffIdentityHelpers {
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}



		////////////////

		public static string GetBuffName( int buffType ) {	// Credit to Jofairden
			if( buffType >= BuffID.Count ) {
				return BuffLoader.GetBuff( buffType )?.DisplayName.GetTranslation( LanguageManager.Instance.ActiveCulture ) ?? "null";
			}

			return Lang.GetBuffName( buffType );
		}
	}
}
