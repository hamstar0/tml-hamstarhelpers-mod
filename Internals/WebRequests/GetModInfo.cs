using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	class BasicModInfoDatabase : Dictionary<string, BasicModInfo>,
		IDictionary<string, BasicModInfo>, ICollection<KeyValuePair<string, BasicModInfo>>, IDictionary, ICollection,
		IReadOnlyDictionary<string, BasicModInfo>, IReadOnlyCollection<KeyValuePair<string, BasicModInfo>>,
		IEnumerable<KeyValuePair<string, BasicModInfo>>, IEnumerable, ISerializable, IDeserializationCallback { }
	class BadModsDatabase : Dictionary<string, int>,
		IDictionary<string, int>, ICollection<KeyValuePair<string, int>>, IDictionary, ICollection, IReadOnlyDictionary<string, int>,
		IReadOnlyCollection<KeyValuePair<string, int>>, IEnumerable<KeyValuePair<string, int>>, IEnumerable, ISerializable,
		IDeserializationCallback { }




	/// @private
	class ModInfoListLoadHookArguments {
		public bool Found;
		public BasicModInfoDatabase ModInfo;

		public ModInfoListLoadHookArguments() {
			this.Found = false;
			this.ModInfo = new BasicModInfoDatabase();
		}
	}




	partial class GetModInfo {
		private readonly static object MyLock = new object();

		internal readonly static object LoadHookValidatorKey;
		public readonly static CustomLoadHookValidator<ModInfoListLoadHookArguments> ModInfoListLoadHookValidator;
		public readonly static CustomLoadHookValidator<ModInfoListLoadHookArguments> BadModsListLoadHookValidator;

		////

		public static string ModInfoUrl => "https://script.google.com/macros/s/AKfycbwtUsafWtIun_9_gO1o2dI6Tgqin09U7jWk4LPS/exec";
		public static string BadModsUrl => ModInfoUrl + "?route=bad_mods";


		////////////////

		static GetModInfo() {
			GetModInfo.LoadHookValidatorKey = new object();
			GetModInfo.ModInfoListLoadHookValidator = new CustomLoadHookValidator<ModInfoListLoadHookArguments>(
				GetModInfo.LoadHookValidatorKey
			);
			GetModInfo.BadModsListLoadHookValidator = new CustomLoadHookValidator<ModInfoListLoadHookArguments>(
				GetModInfo.LoadHookValidatorKey
			);
		}



		////////////////

		private static bool _IsCachedFailsafe = false;

		internal static void CacheAllModInfoAsyncSafe() {
			if( GetModInfo._IsCachedFailsafe ) { return; }
			GetModInfo._IsCachedFailsafe = true;

			bool isLocalCachedFailsafe = false;	// wtf

			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModInfo.MyLock ) {
					if( isLocalCachedFailsafe ) { return; }
					isLocalCachedFailsafe = true;

					GetModInfo.CacheAllModInfoAsync();
				}
			} );

			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}

		private static void CacheAllModInfoAsync() {
			var mymod = ModHelpersMod.Instance;
			var modInfoArgs = new ModInfoListLoadHookArguments();

			GetModInfo.RetrieveAllModInfoAsync( ( found, modInfo ) => {
				modInfoArgs.ModInfo = modInfo;
				modInfoArgs.Found = found;

				Timers.SetTimer( "CacheAllModInfoAsyncFailsafe", 2, true, () => {
					if( GetModInfo.ModInfoListLoadHookValidator == null ) { return true; }

					CustomLoadHooks.TriggerHook(
						GetModInfo.ModInfoListLoadHookValidator,
						GetModInfo.LoadHookValidatorKey,
						modInfoArgs
					);
					return false;
				} );
			} );

			CustomLoadHooks.AddHook( GetModInfo.ModInfoListLoadHookValidator, ( modInfoArgs2 ) => {
				Thread.Sleep( 2000 );

				if( modInfoArgs2.Found ) {
					GetModInfo.RetrieveBadModsAsync( ( found, badMods ) => {
						if( found ) {
							GetModInfo.RegisterBadMods( modInfoArgs2, badMods );
						}
						
						CustomLoadHooks.TriggerHook(
							GetModInfo.BadModsListLoadHookValidator,
							GetModInfo.LoadHookValidatorKey,
							modInfoArgs2
						);
					} );
				}

				return true;
			} );
		}

		private static void RegisterBadMods( ModInfoListLoadHookArguments modInfoArgs, BadModsDatabase badMods ) {
			foreach( var kv in modInfoArgs.ModInfo ) {
				string modName = kv.Key;
				BasicModInfo modInfo = kv.Value;

				modInfo.IsBadMod = badMods.ContainsKey(modName);
			}
		}



		////////////////
		
		internal void OnPostModsLoad() {
			if( !ModHelpersMod.Config.DisableModMenuUpdates ) {
				GetModInfo.CacheAllModInfoAsyncSafe();
				/*GetModVersion.CacheAllModVersionsAsync( () => {
					LogHelpers.Log( "Mod versions successfully retrieved and cached." );
				} );*/
			}
		}
	}
}
