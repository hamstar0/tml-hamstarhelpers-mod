using System;


namespace HamstarHelpers.Classes.DataStructures.BTree {
	/// <summary></summary>
	/// <typeparam name="TK"></typeparam>
	/// <typeparam name="TP"></typeparam>
	public class BTreeEntry<TK, TP> : IEquatable<BTreeEntry<TK, TP>> {
		/// <summary></summary>
		public TK Key { get; set; }

		/// <summary></summary>
		public TP Pointer { get; set; }



		/// <summary></summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals( BTreeEntry<TK, TP> other ) {
			return this.Key.Equals( other.Key ) && this.Pointer.Equals( other.Pointer );
		}
	}
}
