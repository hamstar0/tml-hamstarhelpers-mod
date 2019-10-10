using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Classes.DataStructures.BTree {
	public partial class BTree<TK, TP> where TK : IComparable<TK> {
		/// <summary>
		/// Searches for all items indicated as matches by a checker function.
		/// </summary>
		/// <param name="check">Accepts a key as a parmeter, returns a value between 0-1 to indicate a match.</param>
		/// <param name="key"></param>
		/// <returns></returns>
		public IEnumerable<BTreeEntry<TK, TP>> SearchWhile( Func<TK, float> check, TK key ) {
			IList<(float, BTreeEntry<TK, TP>)> list = new List<(float, BTreeEntry<TK, TP>)>();
			this.SearchWhileInternal( check, this.Root, key, ref list );

			return list.Select( t => t.Item2 );
		}


		////////////////

		private bool SearchWhileInternal(
				Func<TK, float> check,
				BTreeNode<TK, TP> node,
				TK key,
				ref IList<(float, BTreeEntry<TK, TP>)> entries ) {
			int i = node.Entries.TakeWhile( entry => key.CompareTo( entry.Key ) > 0 ).Count();

			if( i < node.Entries.Count && node.Entries[i].Key.CompareTo( key ) == 0 ) {
				BTreeEntry<TK, TP> entry = node.Entries[i];
				float entryVal = check( entry.Key );

				if( entryVal >= -1 && entryVal <= 1 ) {
					entries.Add( (entryVal, entry) );
					return true;
				} else {
					return false;
				}
			}

			return node.IsLeaf ? false : this.SearchWhileInternal(
				check,
				node.Children[i],
				key,
				ref entries );
		}
	}
}
