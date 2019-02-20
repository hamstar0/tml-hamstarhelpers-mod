using Newtonsoft.Json;
using System;


namespace HamstarHelpers.Components.Config {
	public class ConfigurationDataBase {
		public static string RelativePath => "Mod Configs";



		////////////////

		public ConfigurationDataBase() { }

		public ConfigurationDataBase Clone() {
			return (ConfigurationDataBase)this.MemberwiseClone();
		}

		////
		
		public virtual void OnLoad( bool success ) { }
		public virtual void OnSave() { }


		////////////////
		
		public override string ToString() {
			return JsonConvert.SerializeObject( this );
		}
	}
}
