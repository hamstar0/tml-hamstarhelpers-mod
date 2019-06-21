using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/** @private */
	partial class ModHelpersMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-hamstarhelpers-mod";


		////////////////

		public static string ConfigFileRelativePath {
			get { return JsonConfig<HamstarHelpersConfigData>.ConfigSubfolder + Path.DirectorySeparatorChar + HamstarHelpersConfigData.ConfigFileName; }
		}

		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reload configs outside of single player." );
			}
			if( ModHelpersMod.Instance != null ) {
				if( !ModHelpersMod.Instance.ConfigJson.LoadFile() ) {
					ModHelpersMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reset to default configs outside of single player." );
			}

			var configData = new HamstarHelpersConfigData();
			//configData.SetDefaults();

			ModHelpersMod.Instance.ConfigJson.SetData( configData );
			ModHelpersMod.Instance.ConfigJson.SaveFile();
		}
	}
}
