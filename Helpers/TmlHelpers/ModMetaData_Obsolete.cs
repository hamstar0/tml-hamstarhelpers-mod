using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	public partial class ModMetaDataManager {
		[System.Obsolete( "use ModHelpers.GetAllMods", true )]
		public static IEnumerable<Mod> GetAllMods() {
			return ModHelpers.ModHelpers.GetAllMods();
		}
	}
}
