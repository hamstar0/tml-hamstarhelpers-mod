using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NetHelpers;
using HamstarHelpers.Services.Promises;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	class ModInfoPromiseArguments : PromiseArguments {
		public bool Found;
		public IDictionary<string, BasicModInfoEntry> Info;
	}




	class BasicModInfoEntry {
		public string DisplayName;
		public Version Version;
		public string Description;
		public string Homepage;
	}




	class GetModInfo {
		public static string ModInfoUrl => "https://script.google.com/macros/s/AKfycbwtUsafWtIun_9_gO1o2dI6Tgqin09U7jWk4LPS/exec";
		//"://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5";

		
		////////////////

		private readonly static object MyLock = new object();

		internal readonly static object PromiseValidatorKey;
		public readonly static PromiseValidator ModVersionPromiseValidator;


		////////////////

		static GetModInfo() {
			GetModInfo.PromiseValidatorKey = new object();
			GetModInfo.ModVersionPromiseValidator = new PromiseValidator( GetModInfo.PromiseValidatorKey );
		}

		

		////////////////

		/*public static void GetLatestKnownVersionAsync( Mod mod, Action<Version> onSuccess, Action<string> onFail ) {
			Action check = delegate () {
				var mymod = ModHelpersMod.Instance;

				try {
					if( mymod.GetModVersion.ModVersions.ContainsKey( mod.Name ) ) {
						onSuccess( mymod.GetModVersion.ModVersions[mod.Name] );
					} else {
						onFail( "GetLatestKnownVersion - Unrecognized mod " + mod.Name + " (not found on mod browser)" );
					}
				} catch( Exception e ) {
					onFail( e.ToString() );
				}
			};

			GetModVersion.CacheAllModVersionsAsync( check );
		}*/


		internal static void CacheAllModVersionsAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModInfo.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new ModInfoPromiseArguments {
						Found = false
					};
					
					GetModInfo.RetrieveAllModVersionsAsync( ( info, found ) => {
						args.Info = info;
						args.Found = found;

						Promises.TriggerValidatedPromise( GetModInfo.ModVersionPromiseValidator, GetModInfo.PromiseValidatorKey, args );
					} );
				}
			} );

			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}



		private static void RetrieveAllModVersionsAsync( Action<IDictionary<string, Tuple<string, Version>>, bool> onSuccess ) {
			Func<string, Tuple<IDictionary<string, Tuple<string, Version>>, bool>> onResponse = ( string output ) => {
				bool found = false;
				IDictionary<string, Tuple<string, Version>> modVersions = new Dictionary<string, Tuple<string, Version>>();

				JObject respJson = JObject.Parse( output );

				if( respJson.Count > 0 ) {
					JToken modListToken = respJson.SelectToken( "modlist" );
					if( modListToken == null ) {
						throw new NullReferenceException( "No modlist" );
					}

					JToken[] modList = modListToken.ToArray();

					foreach( JToken modEntry in modList ) {
						JToken modNameToken = modEntry.SelectToken( "name" );
						JToken modDisplaynameToken = modEntry.SelectToken( "displayname" );
						JToken modVersRawToken = modEntry.SelectToken( "version" );
						JToken hasDescRawToken = modEntry.SelectToken( "has_description" );
						JToken homepageRawToken = modEntry.SelectToken( "homepage" );

						if( modNameToken == null || modVersRawToken == null || modDisplaynameToken == null || hasDescRawToken == null
								|| homepageRawToken == null ) {
							continue;
						}

						string modName = modNameToken.ToObject<string>();
						string modDisplayName = modDisplaynameToken.ToObject<string>();
						string modVersRaw = modVersRawToken.ToObject<string>();

						Version modVers = Version.Parse( modVersRaw.Substring( 1 ) );

						modVersions[ modName ] = Tuple.Create( modDisplayName, modVers );
					}

					found = true;
				}
				
				return Tuple.Create( modVersions, found );
			};

			Action<Exception, string> onFail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + (output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( (output ?? "") + " - " + e.Message );
				} else {
					LogHelpers.Alert( (output ?? "") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, Tuple<string, Version>>, bool> onCompletion = ( responseVal, success ) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, Tuple<string, Version>>();
				}

				onSuccess( responseVal, success );
			};

			NetHelpers.MakeGetRequestAsync( GetModInfo.ModInfoUrl, onResponse, onFail, onCompletion );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModVersions ) {
				GetModInfo.CacheAllModVersionsAsync();
				/*GetModVersion.CacheAllModVersionsAsync( () => {
					LogHelpers.Log( "Mod versions successfully retrieved and cached." );
				} );*/
			}
		}
	}
}
