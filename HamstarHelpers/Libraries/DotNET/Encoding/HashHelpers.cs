using System;
using System.Security.Cryptography;


namespace HamstarHelpers.Libraries.DotNET.Encoding {
	/// <summary>
	/// Assorted static "helper" functions pertaining to hash codes.
	/// </summary>
	public class HashLibraries {
		/// <summary>
		/// Produces a SHA256 hash from a given input string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ComputeSHA256Hash( string str ) {
			var crypt = new SHA256Managed();
			byte[] hashBytes = crypt.ComputeHash( System.Text.Encoding.UTF8.GetBytes( str ) );
			string hash = Convert.ToBase64String( hashBytes );
			
			return hash;
		}
	}
}
