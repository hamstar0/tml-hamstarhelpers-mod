using HamstarHelpers.TmlHelpers;


namespace HamstarHelpers.Utilities.Config {
	public interface ConfigurableMod {
		JsonConfig<ConfigurationDataBase> Config { get; }
	}
}
