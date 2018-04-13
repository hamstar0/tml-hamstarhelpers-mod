using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.WorldHelpers {
	public class WorldFileHelpers {
		/*public static void EraseWorld_WIN( int which ) {
			WorldFileData data = Main.WorldList[which];

			try {
				if( !data.IsCloudSave ) {
					FileOperationAPIWrapper.MoveToRecycleBin( data.Path );
					FileOperationAPIWrapper.MoveToRecycleBin( data + ".bak");
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( data.Path );
				}

				string t_path = Path.ChangeExtension( data.Path, ".twld" );

				if( !data.IsCloudSave ) {
					FileOperationAPIWrapper.MoveToRecycleBin( t_path );
					FileOperationAPIWrapper.MoveToRecycleBin( t_path + ".bak" );
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( t_path );
				}

				Main.LoadWorlds();
			} catch { }
		}*/

		public static void EraseWorld( int i, bool also_bak=true ) {
			WorldFileData data = Main.WorldList[i];

			try {
				if( !data.IsCloudSave ) {
					File.Delete( data.Path );
					if( also_bak ) {
						File.Delete( data.Path + ".bak" );
					}
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( data.Path );
				}

				string t_path = Path.ChangeExtension( data.Path, ".twld" );

				if( !data.IsCloudSave ) {
					File.Delete( t_path );
					if( also_bak ) {
						File.Delete( t_path + ".bak" );
					}
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( t_path );
				}

				Main.LoadWorlds();
			} catch { }
		}
	}
}
