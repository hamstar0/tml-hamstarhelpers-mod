using HamstarHelpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static partial class ItemFinderHelpers {
		[System.Obsolete( "use ItemFinderHelpers.FindIndexOfFirstOfItemInCollection", true )]
		public static int FindFirstOfItemInCollection( Item[] collection, ISet<int> item_types ) {
			return ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( collection, item_types );
		}
		
		[System.Obsolete( "use ItemFinderHelpers.FindIndexOfEach", true )]
		public static SortedSet<int> FindIndexOfEachItemInCollection( Item[] collection, ISet<int> item_types ) {
			return (SortedSet<int>)ItemFinderHelpers.FindIndexOfEach( collection, item_types );
		}
	}
}
