using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.PromisedHooks;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	public class BasicModInfo {
		public string DisplayName { get; private set; }
		public IEnumerable<string> Authors { get; private set; }
		public Version Version { get; private set; }
		public string Description { get; private set; }
		public string Homepage { get; private set; }

		public bool IsBadMod { get; internal set; }


		public BasicModInfo( string displayName, IEnumerable<string> authors, Version version, string description, string homepage ) {
			this.DisplayName = displayName;
			this.Authors = authors;
			this.Version = version;
			this.Description = description;
			this.Homepage = homepage;
		}
	}




	/** <summary>Assorted static "helper" functions pertaining to mod identification.</summary> */
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


		////////////////

		public static bool IsLoadedModProperlyPresented( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				LogHelpers.Alert( "Invalid mod "+modName );
				return false;
			}

			IDictionary<string, Services.Tml.BuildPropertiesViewer> modInfos = ModListHelpers.GetLoadedModNamesWithBuildProps();
			if( !modInfos.ContainsKey(modName) ) {
				LogHelpers.Alert( "Missing mod "+modName );
				return false;
			}

			var modInfo = new BasicModInfo( mod.DisplayName,
				modInfos[modName].Author.Split(',').SafeSelect( a=>a.Trim() ),
				mod.Version, modInfos[modName].Description, modInfos[modName].Homepage
			);

			return ModIdentityHelpers.IsProperlyPresented( modInfo );
		}


		public static void IsListModProperlyPresented( string modName, Action<bool> callback ) {
			PromisedHooks.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.ModInfoListPromiseValidator, ( args ) => {
				if( args.Found && args.ModInfo.ContainsKey( modName ) ) {
					BasicModInfo modInfo = args.ModInfo[modName];

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

		public static bool IsProperlyPresented( BasicModInfo modInfo ) {
			if( modInfo.Description != null && modInfo.Description.Length < 16 ) { return false; }
			if( modInfo.Homepage.Length < 10 ) { return false; }

			if( modInfo.Description.Contains("Learn how to mod with tModLoader by exploring the source code for this mod.") ) { return false; }
			if( modInfo.Description.Contains("Modify this file with a description of your mod.") ) { return false; }

			string homepage = modInfo.Homepage.ToLower();

			//if( homepage.Contains( "discord.gg/" ) ) { return false; }

			// Go away, url shorteners
			foreach( string url in WebHelpers.UrlShortenerList ) {
				if( homepage.Contains("/"+url+"/") ) { return false; }
				if( homepage.Contains("."+url+"/") ) { return false; }
			}
			
			return true;
		}
	}
}
