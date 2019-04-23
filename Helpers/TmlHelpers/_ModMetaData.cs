using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public class ModMetaDataManager {
		[Obsolete( "use `ModFeaturesHelpers.DetectGithub(Mod)`", true )]
		public static bool DetectGithub( Mod mod ) {
			return ModFeaturesHelpers.DetectGithub( mod );
		}

		[Obsolete( "use `ModFeaturesHelpers.DetectConfig(Mod)`", true )]
		public static bool DetectConfig( Mod mod ) {
			return ModFeaturesHelpers.DetectConfig( mod );
		}

		[Obsolete( "use `ModFeaturesHelpers.DetectConfigDefaultsReset(Mod)`", true )]
		public static bool DetectConfigDefaultsReset( Mod mod ) {
			return ModFeaturesHelpers.DetectConfigDefaultsReset( mod );
		}


		////////////////

		[Obsolete( "use `ModFeaturesHelpers.HasGithub(Mod)`", true )]
		public static bool HasGithub( Mod mod ) {
			return ModFeaturesHelpers.HasGithub( mod );
		}
		[Obsolete( "use `ModFeaturesHelpers.HasConfig(Mod)`", true )]
		public static bool HasConfig( Mod mod ) {
			return ModFeaturesHelpers.HasConfig( mod );
		}
		[Obsolete( "use `ModFeaturesHelpers.HasConfigDefaultsReset(Mod)`", true )]
		public static bool HasConfigDefaultsReset( Mod mod ) {
			return ModFeaturesHelpers.HasConfigDefaultsReset( mod );
		}

		////////////////

		[Obsolete( "use `ModFeaturesHelpers.GetConfigRelativePath(Mod)`", true )]
		public static string GetConfigRelativePath( Mod mod ) {
			return ModFeaturesHelpers.GetConfigRelativePath( mod );
		}

		[Obsolete( "use `ModFeaturesHelpers.ReloadConfigFromFile(Mod)`", true )]
		public static void ReloadConfigFromFile( Mod mod ) {
			ModFeaturesHelpers.ReloadConfigFromFile( mod );
		}

		[Obsolete( "use `ModFeaturesHelpers.ResetDefaultsConfig(Mod)`", true )]
		public static void ResetDefaultsConfig( Mod mod ) {
			ModFeaturesHelpers.ResetDefaultsConfig( mod );
		}

		////////////////

		[Obsolete( "use `ModFeaturesHelpers.GetGithubUserName(Mod)`", true )]
		public static string GetGithubUserName( Mod mod ) {
			return ModFeaturesHelpers.GetGithubUserName( mod );
		}

		[Obsolete( "use `ModFeaturesHelpers.GetGithubProjectName(Mod)`", true )]
		public static string GetGithubProjectName( Mod mod ) {
			return ModFeaturesHelpers.GetGithubProjectName( mod );
		}
	}
}
