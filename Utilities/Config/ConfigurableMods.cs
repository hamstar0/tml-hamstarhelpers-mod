using HamstarHelpers.TmlHelpers;


namespace HamstarHelpers.Utilities.Config {
	public interface ConfigurableMod : ExtendedModData {
		JsonConfig<ConfigurationDataBase> Config { get; }
	}
}
