using System;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.TmlHelpers {
	[Obsolete("use Services.Tml.BuildPropertiesEditor", true)]
	public class BuildPropertiesEditor {
		public static Services.Tml.BuildPropertiesEditor GetBuildPropertiesForModFile( TmodFile mod_file ) {
			return Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod_file );
		}
	}
}
