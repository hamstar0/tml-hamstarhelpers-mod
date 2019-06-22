using Newtonsoft.Json;
using System;


namespace HamstarHelpers.Components.Config {
	/**
	 * <summary>Base class expected for config files (optional). Implements handy hooks and may eventually add ways to make more useful config handling in the future.</summary>
	 */
	[Obsolete("use ModConfig", false)]
	public class ConfigurationDataBase {
		/**
		 * <summary>Specifies the folder (under %DOCUMENTS%/My Games/Terraria/ModLoader) where all configs are saved.</summary>
		 */
		public static string RelativePath => "Mod Configs";



		////////////////

		public ConfigurationDataBase() { }

		public ConfigurationDataBase Clone() {
			return (ConfigurationDataBase)this.MemberwiseClone();
		}

		////
		
		/**
		 * <summary>Runs when the config is loaded from file or created anew.</summary>
		 * <param name="success">Is set to `true` when load from file succeeds.</param>
		 */
		public virtual void OnLoad( bool success ) { }
		/**
		 * <summary>Runs when the config is saved to file.</summary>
		 */
		public virtual void OnSave() { }


		////////////////
		
		public override string ToString() {
			return JsonConvert.SerializeObject( this );
		}
	}
}
