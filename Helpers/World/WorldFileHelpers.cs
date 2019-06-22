using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Social;


namespace HamstarHelpers.Helpers.World {
	/** <summary>Assorted static "helper" functions pertaining to world files (.wld or .twld).</summary> */
	public static class WorldFileHelpers {
		/*public static void EraseWorld_WIN( int which ) {
			WorldFileData data = Main.WorldList[which];

			try {
				if( !data.IsCloudSave ) {
					FileOperationAPIWrapper.MoveToRecycleBin( data.Path );
					FileOperationAPIWrapper.MoveToRecycleBin( data + ".bak");
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( data.Path );
				}

				string tpath = Path.ChangeExtension( data.Path, ".twld" );

				if( !data.IsCloudSave ) {
					FileOperationAPIWrapper.MoveToRecycleBin( tPath );
					FileOperationAPIWrapper.MoveToRecycleBin( tPath + ".bak" );
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( tPath );
				}
			} catch { }

			Main.LoadWorlds();
		}*/

		
		public static void EraseWorld( WorldFileData data, bool alsoBak=true ) {
			try {
				if( !data.IsCloudSave ) {
					File.Delete( data.Path );
					if( alsoBak ) {
						File.Delete( data.Path + ".bak" );
					}
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( data.Path );
				}

				string tPath = Path.ChangeExtension( data.Path, ".twld" );

				if( !data.IsCloudSave ) {
					File.Delete( tPath );
					if( alsoBak ) {
						File.Delete( tPath + ".bak" );
					}
				} else if( SocialAPI.Cloud != null ) {
					SocialAPI.Cloud.Delete( tPath );
				}

				LogHelpers.Log( "WorldFileHelpers.EraseWorld - World \"" + data.Name + "\" deleted. ("+data.Path+")" );
			} catch( Exception e ) {
				LogHelpers.Log( "WorldFileHelpers.EraseWorld - Error (path: " + data.Path + ") - " + e.ToString() );
			}

			Main.LoadWorlds();
		}
	}
}
