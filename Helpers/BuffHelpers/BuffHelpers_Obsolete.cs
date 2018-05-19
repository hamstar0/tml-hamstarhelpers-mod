using System.Collections.Generic;


namespace HamstarHelpers.BuffHelpers {
	public partial class BuffHelpers {
		[System.Obsolete( "use BuffIdentityHelpers.NamesToIds", true )]
		public static IReadOnlyDictionary<string, int> BuffIdsByName { get {
			return BuffIdentityHelpers.NamesToIds;
		} }
	}
}
