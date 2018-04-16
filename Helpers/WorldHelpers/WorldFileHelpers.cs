using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Social;


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
			} catch { }

			Main.LoadWorlds();
		}*/


		[System.Obsolete( "use WorldFileHelpers.EraseWorld( WorldFileData, bool )", false )]
		public static void EraseWorld( int which, bool also_bak = true ) {
			WorldFileData data = Main.WorldList[ which ];
			WorldFileHelpers.EraseWorld( data, also_bak );
		}


		public static void EraseWorld( WorldFileData data, bool also_bak=true ) {
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

				LogHelpers.Log( "World "+data.Name+" deleted." );
			} catch( Exception e ) {
				LogHelpers.Log( "WorldFileHelpers.EraseWorld - Path: " + data.Path + " - " + e.ToString() );
			}

			Main.LoadWorlds();
		}
	}
}
