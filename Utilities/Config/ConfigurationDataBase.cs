namespace HamstarHelpers.Utilities.Config {
	public class ConfigurationDataBase {
		public static string RelativePath { get { return "Mod Configs"; } }


		public virtual void OnLoad( bool success ) { }
		public virtual void OnSave() { }
	}
}
