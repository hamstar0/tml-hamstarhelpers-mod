using System;
using System.IO;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Extension methods for BinaryReader.
	/// </summary>
	public static class BinaryReaderExtensions {
		/// <summary>
		/// Implement a safe way to read all bytes in a stream as a correctly-sized byte array.
		/// </summary>
		/// <param name="reader">Source stream.</param>
		/// <returns>All bytes in the stream.</returns>
		public static byte[] ReadAllBytes( this BinaryReader reader ) {
			const int bufferSize = 4096;

			using( var ms = new MemoryStream() ) {
				byte[] buffer = new byte[bufferSize];
				int count;

				while( (count = reader.Read(buffer, 0, buffer.Length)) != 0 ) {
					ms.Write( buffer, 0, count );
				}

				return ms.ToArray();
			}
		}
	}
}
