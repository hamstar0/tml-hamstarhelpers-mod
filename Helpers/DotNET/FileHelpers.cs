using System;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to basic file IO.
	/// </summary>
	public partial class FileHelpers {
		/// <summary>
		/// Sanitizes a string to work correctly as a file path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string SanitizePath( string path ) {
			char[] invChars = Path.GetInvalidFileNameChars();
			string[] splits = path.Split( invChars );
			if( splits.Length == 1 ) { return path; }

			return String.Concat( splits );
		}
	}
}
