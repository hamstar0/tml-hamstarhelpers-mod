using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public class ModIdentityHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();



		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( ModIdentityHelpers.ModIds.ContainsKey( mod ) ) {
				return ModIdentityHelpers.ModIds[mod];
			}
			ModIdentityHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return ModIdentityHelpers.ModIds[mod];
		}


		public static IDictionary<Mod, Version> FindDependencyModMajorVersionMismatches( Mod mod ) {
			Services.Tml.BuildPropertiesEditor buildEditor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			IDictionary<string, Version> modRefs = buildEditor.ModReferences;
			var badModDeps = new Dictionary<Mod, Version>();

			foreach( var kv in modRefs ) {
				Mod depMod = ModLoader.GetMod( kv.Key );
				if( depMod == null ) { continue; }

				if( depMod.Version.Major != kv.Value.Major ) {
					badModDeps[depMod] = kv.Value;
				}
			}

			return badModDeps;
		}

		////////////////

		public static string FormatBadDependencyModList( Mod mod ) {
			IDictionary<Mod, Version> badDepMods = ModIdentityHelpers.FindDependencyModMajorVersionMismatches( mod );

			if( badDepMods.Count != 0 ) {
				IEnumerable<string> badDepModsList = badDepMods.SafeSelect(
					kv => kv.Key.DisplayName + " (needs " + kv.Value.ToString() + ", is " + kv.Key.Version.ToString() + ")"
				);
				return mod.DisplayName + " (" + mod.Name + ") is out of date with its dependency mod(s): " + string.Join( ", \n", badDepModsList );
			}
			return null;
		}


		////////////////

		public static bool IsLoadedModProperlyPresented( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				LogHelpers.Alert( "Invalid mod "+modName );
				return false;
			}

			IDictionary<string, Services.Tml.BuildPropertiesEditor> modInfos = ModListHelpers.GetLoadedModNamesWithBuildProps();
			if( !modInfos.ContainsKey(modName) ) {
				LogHelpers.Alert( "Missing mod "+modName );
				return false;
			}

			var modInfo = new BasicModInfoEntry( mod.DisplayName,
				modInfos[modName].Author.Split(',').SafeSelect( a=>a.Trim() ),
				mod.Version, modInfos[modName].Description, modInfos[modName].Homepage
			);

			return ModIdentityHelpers.IsProperlyPresented( modInfo );
		}


		public static void IsListModProperlyPresented( string modName, Action<bool> callback ) {
			Promises.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.ModInfoListPromiseValidator, ( args ) => {
				if( args.Found && args.ModInfo.ContainsKey( modName ) ) {
					BasicModInfoEntry modInfo = args.ModInfo[modName];

					bool isProper = ModIdentityHelpers.IsProperlyPresented( modInfo );

					callback( isProper );
				} else {
					if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
						LogHelpers.Log( "Error retrieving mod data for '" + modName + "'" ); //+ "': " + reason );
					}
				}
				return false;
			} );
		}


		////

		public static bool IsProperlyPresented( BasicModInfoEntry modInfo ) {
			if( modInfo.Description != null && modInfo.Description.Length < 16 ) { return false; }
			if( modInfo.Homepage.Length < 10 ) { return false; }

			if( modInfo.Description.Contains("Learn how to mod with tModLoader by exploring the source code for this mod.") ) { return false; }
			if( modInfo.Description.Contains("Modify this file with a description of your mod.") ) { return false; }

			string homepage = modInfo.Homepage.ToLower();

			//if( homepage.Contains( "discord.gg/" ) ) { return false; }

			// Go away, url shorteners
			foreach( string url in WebHelpers.UrlShorteners ) {
				if( homepage.Contains("/"+url+"/") ) { return false; }
				if( homepage.Contains("."+url+"/") ) { return false; }
			}
			
			return true;
		}
	}
}
