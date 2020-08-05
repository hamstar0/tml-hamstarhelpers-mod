using System.Collections.Generic;


namespace HamstarHelpers.Classes.DataStructures.BTree {
	/// <summary></summary>
	/// <typeparam name="TK"></typeparam>
	/// <typeparam name="TP"></typeparam>
	public class BTreeNode<TK, TP>  {
        private int Degree;



		/// <summary></summary>
		public List<BTreeNode<TK, TP>> Children { get; set; }

		/// <summary></summary>
		public List<BTreeEntry<TK, TP>> Entries { get; set; }

		/// <summary></summary>
		public bool IsLeaf {
			get {
				return this.Children.Count == 0;
			}
		}

		/// <summary></summary>
		public bool HasReachedMaxEntries {
			get {
				return this.Entries.Count == ( 2 * this.Degree ) - 1;
			}
		}

		/// <summary></summary>
		public bool HasReachedMinEntries {
			get {
				return this.Entries.Count == this.Degree - 1;
			}
		}



		/// <summary></summary>
		/// <param name="degree"></param>
		public BTreeNode( int degree ) {
			this.Degree = degree;
			this.Children = new List<BTreeNode<TK, TP>>( degree );
			this.Entries = new List<BTreeEntry<TK, TP>>( degree );
		}
	}
}
