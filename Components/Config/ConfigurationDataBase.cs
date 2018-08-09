using Newtonsoft.Json;


namespace HamstarHelpers.Components.Config {
	public class ConfigurationDataBase {
		public static string RelativePath { get { return "Mod Configs"; } }

		
		public virtual void OnLoad( bool success ) { }
		public virtual void OnSave() { }

		public ConfigurationDataBase Clone() {
			return (ConfigurationDataBase)this.MemberwiseClone();
		}

		public override string ToString() {
			return JsonConvert.SerializeObject( this );
		}
	}
}
