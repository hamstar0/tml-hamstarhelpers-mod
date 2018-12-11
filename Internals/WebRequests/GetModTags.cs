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

		internal void SetTagMods( IDictionary<string, ISet<string>> modTags ) {
			this.ModTags = modTags;
			this.TagMods = new Dictionary<string, ISet<string>>();

			foreach( var kv in modTags ) {
				string modName = kv.Key;
				ISet<string> tags = kv.Value;

				foreach( string tag in tags ) {
					if( string.IsNullOrEmpty( tag ) ) { continue; }

					if( !this.TagMods.ContainsKey( tag ) ) {
						this.TagMods[tag] = new HashSet<string>();
					}
					this.TagMods[tag].Add( modName );
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

		public static string ModTagsUrl => "https://script.google.com/macros/s/AKfycbwakEvF9DDYGup34DJJjcxPd0MUApNpl2GalZgr/exec";
			//"http://hamstar.pw/hamstarhelpers/mod_info/";	<- express

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
					
					GetModTags.RetrieveAllTagModsAsync( ( modTags, found ) => {
						if( found ) {
							args.SetTagMods( modTags );
						}
						args.Found = found;
						
						Promises.TriggerValidatedPromise( GetModTags.TagsReceivedPromiseValidator, GetModTags.PromiseValidatorKey, args );
					} );
				}
			} );
		}



		private static void RetrieveAllTagModsAsync( Action<IDictionary<string, ISet<string>>, bool> onCompletion ) {
			Func<string, Tuple<IDictionary<string, ISet<string>>, bool>> onGetResponse = ( string output ) => {
				bool found = false;
				IDictionary<string, ISet<string>> modTagSet = new Dictionary<string, ISet<string>>();

				JObject respJson = JObject.Parse( output );

				if( respJson.Count > 0 ) {
					JToken tagListToken = respJson.SelectToken( "modlist" );
					if( tagListToken == null ) {
						throw new NullReferenceException( "No modlist: " + string.Join(",", respJson.Properties()) );
					}

					JToken[] tagList = tagListToken.ToArray();

					foreach( JToken tagEntry in tagList ) {
						JToken modNameToken = tagEntry.SelectToken( "ModName" );
						JToken modTagsToken = tagEntry.SelectToken( "ModTags" );
						if( modNameToken == null || modTagsToken == null ) {
							continue;
						}

						string modName = modNameToken.ToObject<string>();
						string modTagsRaw = modTagsToken.ToObject<string>();
						string[] modTags = modTagsRaw.Split( ',' );

						modTagSet[ modName ] = new HashSet<string>( modTags );
					}
					found = true;
				}
				
				return Tuple.Create( modTagSet, found );
			};

			Action<Exception, string> onGetFail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - Bad JSON: " +
						(output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "..." ) + " - " + e.Message );
				} else {
					LogHelpers.Log( "ModHelpers.ModTagsGet.RetrieveAllModTagsAsync - " + (output ?? "...") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, ISet<string>>, bool> onGetCompletion = ( responseVal, found ) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, ISet<string>>();
				}

				onCompletion( responseVal, found );
			};

			NetHelpers.MakeGetRequestAsync( GetModTags.ModTagsUrl, onGetResponse, onGetFail, onGetCompletion );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModTags ) {
				GetModTags.CacheAllModTagsAsync();
			}
		}
	}
}
