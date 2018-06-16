using System;
using System.IO;


namespace HamstarHelpers.Helpers {
	[Obsolete("use DotNetHelpers.FileHelpers", true)]
	public static class FileHelpers {
		[Obsolete( "use DotNetHelpers.FileHelpers", true )]
		public static string SanitizePath( string path ) {
			return HamstarHelpers.DotNetHelpers.FileHelpers.SanitizePath( path );
		}
		[Obsolete( "use DotNetHelpers.FileHelpers", true )]
		public static bool SaveBinaryFile<T>( T obj, string full_path, bool is_cloud, bool backup_old )
				where T : class {
			return HamstarHelpers.DotNetHelpers.FileHelpers.SaveBinaryFile<T>( obj, full_path, is_cloud, backup_old );
		}
		[Obsolete( "use DotNetHelpers.FileHelpers", true )]
		public static T LoadBinaryFile<T>( string full_path, bool is_cloud )
				where T : class {
			return HamstarHelpers.DotNetHelpers.FileHelpers.LoadBinaryFile<T>( full_path, is_cloud );
		}
		[Obsolete( "use DotNetHelpers.FileHelpers", true )]
		public static void ToStream( string src, Stream dest_stream ) {
			HamstarHelpers.DotNetHelpers.FileHelpers.ToStream( src, dest_stream );
		}
		[Obsolete( "use DotNetHelpers.FileHelpers", true )]
		public static string FromStream( Stream src_stream ) {
			return HamstarHelpers.DotNetHelpers.FileHelpers.FromStream( src_stream );
		}
	}
}
