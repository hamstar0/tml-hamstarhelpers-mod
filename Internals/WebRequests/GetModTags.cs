using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	class ModTagsDatabase : Dictionary<string, ISet<string>>,
		IDictionary<string, ISet<string>>, ICollection<KeyValuePair<string, ISet<string>>>, IDictionary, ICollection,
		IReadOnlyDictionary<string, ISet<string>>, IReadOnlyCollection<KeyValuePair<string, ISet<string>>>,
		IEnumerable<KeyValuePair<string, ISet<string>>>, IEnumerable, ISerializable, IDeserializationCallback { }





	/// @private
	class ModTagsLoadHookArguments {
		public bool Found;
		internal IDictionary<string, ISet<string>> ModTags = null;
		internal IDictionary<string, ISet<string>> TagMods = null;


		////////////////

		internal void SetTagMods( ModTagsDatabase modTags ) {
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

		internal readonly static object TagsReceivedHookValidatorKey;
		public readonly static CustomLoadHookValidator<ModTagsLoadHookArguments> TagsReceivedHookValidator;

		////////////////

		public static string ModTagsUrl => "https://script.google.com/macros/s/AKfycbwakEvF9DDYGup34DJJjcxPd0MUApNpl2GalZgr/exec";
			//"http://hamstar.pw/hamstarhelpers/mod_info/";	<- express

		
		////////////////

		static GetModTags() {
			GetModTags.TagsReceivedHookValidatorKey = new object();
			GetModTags.TagsReceivedHookValidator = new CustomLoadHookValidator<ModTagsLoadHookArguments>( GetModTags.TagsReceivedHookValidatorKey );
		}
		
		

		////////////////
		
		private static void CacheAllModTagsAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetModTags.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new ModTagsLoadHookArguments {
						Found = false
					};
					
					GetModTags.RetrieveAllModTagsAsync( ( success, modTags ) => {
						try {
							if( success ) {
								args.SetTagMods( modTags );
							}
							args.Found = success;

							CustomLoadHooks.TriggerHook( GetModTags.TagsReceivedHookValidator, GetModTags.TagsReceivedHookValidatorKey, args );
						} catch( Exception e ) {
							LogHelpers.Alert( e.ToString() );
						}
					} );
				}
			} );
		}



		////////////////
		
		internal void OnPostModsLoad() {
			if( !ModHelpersMod.Instance.Config.DisableModTags ) {
				GetModTags.CacheAllModTagsAsync();
			}
		}
	}
}
