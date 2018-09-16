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
	class ModTagsPromiseArguments : PromiseArguments {
		public bool Found;
		internal IDictionary<string, ISet<string>> TagMods = null;
		internal IDictionary<string, ISet<string>> ModTags = null;


		////////////////

		internal void SetTagMods( IDictionary<string, ISet<string>> tagmods ) {
			this.TagMods = tagmods;
			this.ModTags = new Dictionary<string, ISet<string>>();

			foreach( var kv in tagmods ) {
				string tagname = kv.Key;
				ISet<string> modnames = kv.Value;

				foreach( string modname in modnames ) {
					if( !this.ModTags.ContainsKey( modname ) ) {
						this.ModTags[modname] = new HashSet<string>();
					}
					this.ModTags[modname].Add( tagname );
				}
			}
			LogHelpers.Log( "tag mods: " + string.Join( ",", tagmods.Select( kv => kv.Key + ":" + kv.Value ) ) );
		}
	}




	class GetModTags {
		private readonly static object MyLock = new object();

		internal readonly static object PromiseValidatorKey;
		public readonly static PromiseValidator TagsReceivedPromiseValidator;

		////////////////

		public static string ModTagsUrl => "https://script.google.com/macros/s/AKfycbwSuU6XJ6uSh0RNWjkzJUmVW6wRkNighHlfKGPf4pUdcu0J2tys/exec";

		////////////////

		static GetModTags() {
			GetModTags.PromiseValidatorKey = new object();
			GetModTags.TagsReceivedPromiseValidator = new PromiseValidator( GetModTags.PromiseValidatorKey );
		}




		
		////////////////
		
		internal static void CacheAllModTagsAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModTags.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new ModTagsPromiseArguments {
						Found = false
					};

					GetModTags.RetrieveAllTagModsAsync( ( tags, found ) => {
						if( found ) {
							args.SetTagMods( tags );
						}
						args.Found = found;

						Promises.TriggerValidatedPromise( GetModTags.TagsReceivedPromiseValidator, GetModTags.PromiseValidatorKey, args );
					} );
				}
			} );
		}



		private static void RetrieveAllTagModsAsync( Action<IDictionary<string, ISet<string>>, bool> on_success ) {
			Func<string, Tuple<IDictionary<string, ISet<string>>, bool>> on_response = ( string output ) => {
				bool found = false;
				IDictionary<string, ISet<string>> tag_mod_set = new Dictionary<string, ISet<string>>();

				JObject resp_json = JObject.Parse( output );

				if( resp_json.Count > 0 ) {
					JToken mod_list_token = resp_json.SelectToken( "modlist" );
					if( mod_list_token == null ) {
						throw new NullReferenceException( "No modlist" );
					}

					JToken[] mod_list = mod_list_token.ToArray();

					foreach( JToken mod_entry in mod_list ) {
						JToken tag_name_token = mod_entry.SelectToken( "name" );
						JToken tag_mods_raw_token = mod_entry.SelectToken( "tags" );
						if( tag_name_token == null || tag_mods_raw_token == null ) {
							continue;
						}

						string tag_name = tag_name_token.ToObject<string>();
						string tag_mods_raw = tag_mods_raw_token.ToObject<string>();
						string[] tag_mods = tag_mods_raw.Substring( 1 ).Split( ',' );

						tag_mod_set[ tag_name ] = new HashSet<string>( tag_mods );
					}
					found = true;
				}
				
				return Tuple.Create( tag_mod_set, found );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - Bad JSON: " +
						(output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "") + " - " + e.Message );
				} else {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, ISet<string>>, bool> on_completion = ( response_val, success ) => {
				if( response_val == null ) {
					response_val = new Dictionary<string, ISet<string>>();
				}

				on_success( response_val, success );
			};

			NetHelpers.MakeGetRequestAsync( GetModTags.ModTagsUrl, on_response, on_fail, on_completion );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModTags ) {
				GetModTags.CacheAllModTagsAsync();
			}
		}
	}
}
