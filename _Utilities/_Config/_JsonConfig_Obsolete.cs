using System;


namespace HamstarHelpers.Utilities.Config {
	public partial class JsonConfig<T> : JsonConfig {
		[System.Obsolete( "use JsonConfig.ConfigSubfolder", true )]
		public static string RelativePath { get { return JsonConfig.ConfigSubfolder; } }
	}
}
