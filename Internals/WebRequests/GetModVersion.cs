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
	class ModVersionPromiseArguments : PromiseArguments {
		public bool Found;
		public IDictionary<string, Tuple<string, Version>> Info;
	}




	class GetModVersion {
		public static string ModVersionUrl => "https://script.google.com/macros/s/AKfycbwtUsafWtIun_9_gO1o2dI6Tgqin09U7jWk4LPS/exec";
		//"://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5";

		
		////////////////

		private readonly static object MyLock = new object();

		internal readonly static object PromiseValidatorKey;
		public readonly static PromiseValidator ModVersionPromiseValidator;

		////////////////

		static GetModVersion() {
			GetModVersion.PromiseValidatorKey = new object();
			GetModVersion.ModVersionPromiseValidator = new PromiseValidator( GetModVersion.PromiseValidatorKey );
		}




		////////////////

		/*public static void GetLatestKnownVersionAsync( Mod mod, Action<Version> on_success, Action<string> on_fail ) {
			Action check = delegate () {
				var mymod = ModHelpersMod.Instance;

				try {
					if( mymod.GetModVersion.ModVersions.ContainsKey( mod.Name ) ) {
						on_success( mymod.GetModVersion.ModVersions[mod.Name] );
					} else {
						on_fail( "GetLatestKnownVersion - Unrecognized mod " + mod.Name + " (not found on mod browser)" );
					}
				} catch( Exception e ) {
					on_fail( e.ToString() );
				}
			};

			GetModVersion.CacheAllModVersionsAsync( check );
		}*/


		internal static void CacheAllModVersionsAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModVersion.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new ModVersionPromiseArguments {
						Found = false
					};
					
					GetModVersion.RetrieveAllModVersionsAsync( ( info, found ) => {
						args.Info = info;
						args.Found = found;

						Promises.TriggerValidatedPromise( GetModVersion.ModVersionPromiseValidator, GetModVersion.PromiseValidatorKey, args );
					} );
				}
			} );

			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}



		private static void RetrieveAllModVersionsAsync( Action<IDictionary<string, Tuple<string, Version>>, bool> on_success ) {
			Func<string, Tuple<IDictionary<string, Tuple<string, Version>>, bool>> on_response = ( string output ) => {
				bool found = false;
				IDictionary<string, Tuple<string, Version>> mod_versions = new Dictionary<string, Tuple<string, Version>>();

				JObject resp_json = JObject.Parse( output );

				if( resp_json.Count > 0 ) {
					JToken mod_list_token = resp_json.SelectToken( "modlist" );
					if( mod_list_token == null ) {
						throw new NullReferenceException( "No modlist" );
					}

					JToken[] mod_list = mod_list_token.ToArray();

					foreach( JToken mod_entry in mod_list ) {
						JToken mod_name_token = mod_entry.SelectToken( "name" );
						JToken mod_displayname_token = mod_entry.SelectToken( "displayname" );
						JToken mod_vers_raw_token = mod_entry.SelectToken( "version" );
						
						if( mod_name_token == null || mod_vers_raw_token == null || mod_displayname_token == null ) {
							continue;
						}

						string mod_name = mod_name_token.ToObject<string>();
						string mod_displayname = mod_displayname_token.ToObject<string>();
						string mod_vers_raw = mod_vers_raw_token.ToObject<string>();

						Version mod_vers = Version.Parse( mod_vers_raw.Substring( 1 ) );

						mod_versions[ mod_name ] = Tuple.Create( mod_displayname, mod_vers );
					}

					found = true;
				}
				
				return Tuple.Create( mod_versions, found );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Log( "ModHelpers.ModVersionGet.RetrieveLatestKnownVersions - Bad JSON: " +
						(output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Log( "ModHelpers.ModVersionGet.RetrieveLatestKnownVersions - " + (output ?? "") + " - " + e.Message );
				} else {
					LogHelpers.Log( "ModHelpers.ModVersionGet.RetrieveLatestKnownVersions - " + (output ?? "") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, Tuple<string, Version>>, bool> on_completion = ( response_val, success ) => {
				if( response_val == null ) {
					response_val = new Dictionary<string, Tuple<string, Version>>();
				}

				on_success( response_val, success );
			};

			NetHelpers.MakeGetRequestAsync( GetModVersion.ModVersionUrl, on_response, on_fail, on_completion );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModVersions ) {
				GetModVersion.CacheAllModVersionsAsync();
				/*GetModVersion.CacheAllModVersionsAsync( () => {
					LogHelpers.Log( "Mod versions successfully retrieved and cached." );
				} );*/
			}
		}
	}
}
