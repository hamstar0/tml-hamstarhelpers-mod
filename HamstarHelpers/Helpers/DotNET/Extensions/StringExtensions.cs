using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Extensions for strings.
	/// </summary>
	public static class StringExtensions {
		/// <summary>
		/// Safely truncates an input string.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string Trunc( this string value, int maxLength ) {
			return value?.Substring( 0, Math.Min( value.Length, maxLength ) );
		}
	}


	/// <summary>
	/// Extensions for Vector2 to output shorter strings.
	/// </summary>
	public static class Vector2StringExtensions {
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToShortString( this Vector2 value ) {
			return "{"+value.X.ToString("N0")+", "+value.Y.ToString("N0")+"}";
		}
	}
}
