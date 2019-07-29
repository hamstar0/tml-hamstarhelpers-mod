using HamstarHelpers.Services.Errors;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.ModHelpers {
	/// <summary>
	/// Assorted static "helper" functions pertaining to Mod Helpers control panel features.
	/// </summary>
	public partial class ModFeaturesHelpers {
		private static PropertyInfo GetGithubUserNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubUserName", BindingFlags.Static | BindingFlags.Public );
		}

		private static PropertyInfo GetGitubProjectNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubProjectName", BindingFlags.Static | BindingFlags.Public );
		}
		
		////

		private static bool DetectGithub( Mod mod ) {
			if( ModFeaturesHelpers.GetGithubUserNameProp( mod ) == null ) { return false; }
			if( ModFeaturesHelpers.GetGitubProjectNameProp( mod ) == null ) { return false; }
			return true;
		}


		////////////////

		/// <summary>
		/// Indicates if a given `Mod` references a github repo via. `GithubProjectName` and `GithubUserName` static properties.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static bool HasGithub( Mod mod ) {
			var self = ModHelpersMod.Instance.ModFeaturesHelpers;
			return self.GithubMods.ContainsKey( mod.Name );
		}

		////////////////

		/// <summary>
		/// Gets a mod's github user name, if defined.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetGithubUserName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModFeaturesHelpers;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitUserProp = ModFeaturesHelpers.GetGithubUserNameProp( mod );
			return (string)gitUserProp.GetValue( null );
		}

		/// <summary>
		/// Gets a mod's github project (source code) name, if defined.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetGithubProjectName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModFeaturesHelpers;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitProjProp = ModFeaturesHelpers.GetGitubProjectNameProp( mod );
			return (string)gitProjProp.GetValue( null );
		}
	}
}
