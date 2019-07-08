using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.PromisedHooks;
using System;
using System.Collections.Generic;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
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



	
	partial class GetModTags {
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
		
		private static void CacheAllModTagsAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModTags.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new ModTagsPromiseArguments {
						Found = false
					};
					
					GetModTags.RetrieveAllModTagsAsync( ( modTags, found ) => {
						try {
							if( found ) {
								args.SetTagMods( modTags );
							}
							args.Found = found;

							PromisedHooks.TriggerValidatedPromise( GetModTags.TagsReceivedPromiseValidator, GetModTags.PromiseValidatorKey, args );
						} catch( Exception e ) {
							LogHelpers.Alert( e.ToString() );
						}
					} );
				}
			} );
		}



		////////////////
		
		internal void OnPostSetupContent() {
			if( ModHelpersMod.Instance.Config.IsCheckingModTags ) {
				GetModTags.CacheAllModTagsAsync();
			}
		}
	}
}
