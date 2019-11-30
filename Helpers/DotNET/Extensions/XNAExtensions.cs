using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Assorted static extension "helper" functions pertaining to XNA data structures.
	/// </summary>
	public static class XNAExtensions {
		/// <summary>
		/// Converts a value tuple to a Vector2.
		/// </summary>
		/// <param name="tuple"></param>
		/// <returns></returns>
		public static Vector2 ToVector2( this (int x, int y) tuple ) {
			return new Vector2( tuple.x, tuple.y );
		}

		/// <summary>
		/// Converts a value tuple to a Vector2.
		/// </summary>
		/// <param name="tuple"></param>
		/// <returns></returns>
		public static Vector2 ToVector2( this (float x, float y) tuple ) {
			return new Vector2( tuple.x, tuple.y );
		}

		/// <summary>
		/// Converts a Point to a Vector2.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Vector2 ToVector2( this Point pt ) {
			return new Vector2( pt.X, pt.Y );
		}

		////////////////

		/// <summary>
		/// Converts a Vector2 to a value tuple.
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static (float X, float Y) ToTuple( this Vector2 vec ) {
			return (vec.X, vec.Y);
		}

		/// <summary>
		/// Converts a Point to a value tuple.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static (float X, float Y) ToTuple( this Point pt ) {
			return (pt.X, pt.Y);
		}
	}
}
