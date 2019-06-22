using System;
using System.Security.Cryptography;


namespace HamstarHelpers.Helpers.DotNET.Encoding {
	/** <summary>Assorted static "helper" functions pertaining to hash codes.</summary> */
	public class HashHelpers {
		public static string ComputeSHA256Hash( string str ) {
			var crypt = new SHA256Managed();
			byte[] hashBytes = crypt.ComputeHash( System.Text.Encoding.UTF8.GetBytes( str ) );
			string hash = Convert.ToBase64String( hashBytes );

			return hash;
		}
	}
}
