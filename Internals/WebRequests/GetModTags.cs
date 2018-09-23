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
		internal IDictionary<string, ISet<string>> ModTags = null;
		internal IDictionary<string, ISet<string>> TagMods = null;


		////////////////

		internal void SetTagMods( IDictionary<string, ISet<string>> mod_tags ) {
			this.ModTags = mod_tags;
			this.TagMods = new Dictionary<string, ISet<string>>();

			foreach( var kv in mod_tags ) {
				string mod_name = kv.Key;
				ISet<string> tags = kv.Value;

				foreach( string tag in tags ) {
					if( !this.TagMods.ContainsKey( tag ) ) {
						this.TagMods[tag] = new HashSet<string>();
					}
					this.TagMods[tag].Add( mod_name );
				}
			}
//LogHelpers.Log( "tag mods: " + string.Join( ",", tagmods.Select( kv => kv.Key + ":" + kv.Value ) ) );
		}
	}




	class GetModTags {
		private readonly static object MyLock = new object();

		internal readonly static object PromiseValidatorKey;
		public readonly static PromiseValidator TagsReceivedPromiseValidator;

		////////////////

		public static string ModTagsUrl => "http://hamstar.pw/hamstarhelpers/mod_info/";
			//"https://script.google.com/macros/s/AKfycbwSuU6XJ6uSh0RNWjkzJUmVW6wRkNighHlfKGPf4pUdcu0J2tys/exec";

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
					
					GetModTags.RetrieveAllTagModsAsync( ( mod_tags, found ) => {
						if( found ) {
							args.SetTagMods( mod_tags );
						}
						args.Found = found;
						
						Promises.TriggerValidatedPromise( GetModTags.TagsReceivedPromiseValidator, GetModTags.PromiseValidatorKey, args );
					} );
				}
			} );
		}



		private static void RetrieveAllTagModsAsync( Action<IDictionary<string, ISet<string>>, bool> on_completion ) {
			Func<string, Tuple<IDictionary<string, ISet<string>>, bool>> on_get_response = ( string output ) => {
				bool found = false;
				IDictionary<string, ISet<string>> mod_tag_set = new Dictionary<string, ISet<string>>();

				JObject resp_json = JObject.Parse( output );

				if( resp_json.Count > 0 ) {
					JToken tag_list_token = resp_json.SelectToken( "modlist" );
					if( tag_list_token == null ) {
						throw new NullReferenceException( "No modlist: " + string.Join(",", resp_json.Properties()) );
					}

					JToken[] tag_list = tag_list_token.ToArray();

					foreach( JToken tag_entry in tag_list ) {
						JToken mod_name_token = tag_entry.SelectToken( "ModName" );
						JToken mod_tags_token = tag_entry.SelectToken( "ModTags" );
						if( mod_name_token == null || mod_tags_token == null ) {
							continue;
						}

						string mod_name = mod_name_token.ToObject<string>();
						string mod_tags_raw = mod_tags_token.ToObject<string>();
						string[] mod_tags = mod_tags_raw.Split( ',' );

						mod_tag_set[ mod_name ] = new HashSet<string>( mod_tags );
					}
					found = true;
				}
				
				return Tuple.Create( mod_tag_set, found );
			};

			Action<Exception, string> on_get_fail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - Bad JSON: " +
						(output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "..." ) + " - " + e.Message );
				} else {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "...") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, ISet<string>>, bool> on_get_completion = ( response_val, found ) => {
				if( response_val == null ) {
					response_val = new Dictionary<string, ISet<string>>();
				}

				on_completion( response_val, found );
			};

			NetHelpers.MakeGetRequestAsync( GetModTags.ModTagsUrl, on_get_response, on_get_fail, on_get_completion );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModTags ) {
				GetModTags.CacheAllModTagsAsync();
			}
		}
	}
}
