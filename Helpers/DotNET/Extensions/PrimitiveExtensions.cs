using System;


namespace HamstarHelpers.Helpers.DotNET.Extensions {
	/// <summary>
	/// Extensions for general types.
	/// </summary>
	public static class PrimitiveExtensions {
		/// <summary>
		/// Clamps most primitive value types.
		/// Credit to absoluteAquarian
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static void Clamp<T>( this ref T value, T min, T max ) where T : struct, IComparable<T> {
			value = value.CompareTo( max ) > 0
				? max
				: ( value.CompareTo( min ) < 0
					? min
					: value );
		}
	}
}
