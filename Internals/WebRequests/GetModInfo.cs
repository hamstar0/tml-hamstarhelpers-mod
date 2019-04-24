using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	class ModInfoListPromiseArguments : PromiseArguments {
		public bool Found;
		public IDictionary<string, BasicModInfoEntry> ModInfo;

		public ModInfoListPromiseArguments() {
			this.Found = false;
			this.ModInfo = new Dictionary<string, BasicModInfoEntry>();
		}
	}




	public class BasicModInfoEntry {
		public string DisplayName { get; private set; }
		public IEnumerable<string> Authors { get; private set; }
		public Version Version { get; private set; }
		public string Description { get; private set; }
		public string Homepage { get; private set; }

		public bool IsBadMod { get; internal set; }


		public BasicModInfoEntry( string displayName, IEnumerable<string> authors, Version version, string description, string homepage ) {
			this.DisplayName = displayName;
			this.Authors = authors;
			this.Version = version;
			this.Description = description;
			this.Homepage = homepage;
		}
	}




	partial class GetModInfo {
		private readonly static object MyLock = new object();

		internal readonly static object PromiseValidatorKey;
		public readonly static PromiseValidator ModInfoListPromiseValidator;
		public readonly static PromiseValidator BadModsListPromiseValidator;
		
		////

		public static string ModInfoUrl => "https://script.google.com/macros/s/AKfycbyddPh79y6SwgYqOG-P7Pw0iCbi2tBgVCRvUfP9fJT_xrbHFzo/exec";
		//v11: "https://script.google.com/macros/s/AKfycbwtUsafWtIun_9_gO1o2dI6Tgqin09U7jWk4LPS/exec";
		public static string BadModsUrl => ModInfoUrl + "?bad_mods";


		////////////////

		static GetModInfo() {
			GetModInfo.PromiseValidatorKey = new object();
			GetModInfo.ModInfoListPromiseValidator = new PromiseValidator( GetModInfo.PromiseValidatorKey );
			GetModInfo.BadModsListPromiseValidator = new PromiseValidator( GetModInfo.PromiseValidatorKey );
		}



		////////////////

		internal static void CacheAllModInfoAsyncSafe() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModInfo.MyLock ) {
					GetModInfo.CacheAllModInfoAsync();
				}
			} );

			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}

		private static void CacheAllModInfoAsync() {
			var mymod = ModHelpersMod.Instance;
			var modInfoArgs = new ModInfoListPromiseArguments();

			GetModInfo.RetrieveAllModInfoAsync( ( modInfo, found ) => {
				modInfoArgs.ModInfo = modInfo;
				modInfoArgs.Found = found;

				Promises.TriggerValidatedPromise( GetModInfo.ModInfoListPromiseValidator, GetModInfo.PromiseValidatorKey, modInfoArgs );
			} );

			Promises.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.ModInfoListPromiseValidator, ( modInfoArgs2 ) => {
				if( modInfoArgs2.Found ) {
					GetModInfo.RetrieveBadModsAsync( ( badMods, found ) => {
						if( found ) {
							GetModInfo.RegisterBadMods( modInfoArgs2, badMods );
						}
						
						Promises.TriggerValidatedPromise( GetModInfo.BadModsListPromiseValidator, GetModInfo.PromiseValidatorKey, modInfoArgs2 );
					} );
				}

				return true;
			} );
		}

		private static void RegisterBadMods( ModInfoListPromiseArguments modInfoArgs, IDictionary<string, bool> badMods ) {
			foreach( var kv in modInfoArgs.ModInfo ) {
				string modName = kv.Key;
				BasicModInfoEntry modInfo = kv.Value;

				modInfo.IsBadMod = badMods.ContainsKey(modName) && badMods[modName];
			}
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModVersions ) {
				GetModInfo.CacheAllModInfoAsyncSafe();
				/*GetModVersion.CacheAllModVersionsAsync( () => {
					LogHelpers.Log( "Mod versions successfully retrieved and cached." );
				} );*/
			}
		}
	}
}
