using System;
using Terraria.ModLoader;


namespace HamstarHelpers.MiscHelpers {
	public partial class DataFileHelpers {
		[Obsolete("use LoadBinary<T>(Mod, string, bool)", true)]
		public static T LoadBinary<T>( Mod mod, string file_name_has_ext, out bool success ) where T : class {
			T file = DataFileHelpers.LoadBinary<T>( mod, file_name_has_ext, false );
			success = file != null;

			return file;
		}


		[Obsolete( "use SaveAsBinary<T>(Mod, string, bool, T)", true )]
		public static void SaveAsBinary<T>( Mod mod, string file_name_has_ext, T data ) where T : class {
			DataFileHelpers.SaveAsBinary<T>( mod, file_name_has_ext, false, data );
		}
	}
}
