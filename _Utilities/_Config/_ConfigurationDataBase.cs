using Newtonsoft.Json;
using System;


namespace HamstarHelpers.Utilities.Config {
	[Obsolete( "use Components.Config.ConfigurationDataBase", true )]
	public class ConfigurationDataBase {
		[Obsolete( "use Components.Config.ConfigurationDataBase", true )]
		public static string RelativePath { get { return "Mod Configs"; } }

		
		public virtual void OnLoad( bool success ) { }
		public virtual void OnSave() { }

		public override string ToString() {
			return JsonConvert.SerializeObject( this );
		}
	}
}
