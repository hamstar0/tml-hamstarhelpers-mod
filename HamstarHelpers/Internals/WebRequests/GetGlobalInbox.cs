using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using System.Threading;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	class GlobalInboxLoadHookArguments {
		public bool Found;
		internal IDictionary<string, string> Inbox = null;


		////////////////

		internal void SetGlobalInbox( IDictionary<string, string> messages ) {
			this.Inbox = messages;
		}
	}



	partial class GetGlobalInbox {
		private readonly static object MyLock = new object();

		internal readonly static object GlobalInboxReceivedHookValidatorKey;
		public readonly static CustomLoadHookValidator<GlobalInboxLoadHookArguments> GlobalInboxReceivedHookValidator;

		////////////////

		public static string GlobalInboxUrl => "http://hamstar.pw/hamstarhelpers/global_inbox/";

		
		////////////////

		static GetGlobalInbox() {
			GetGlobalInbox.GlobalInboxReceivedHookValidatorKey = new object();
			GetGlobalInbox.GlobalInboxReceivedHookValidator = new CustomLoadHookValidator<GlobalInboxLoadHookArguments>( GetModTags.TagsReceivedHookValidatorKey );
		}
		
		

		////////////////
		
		private static void CacheAllGlobalInboxAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				lock( GetGlobalInbox.MyLock ) {
					var mymod = ModHelpersMod.Instance;
					var args = new GlobalInboxLoadHookArguments {
						Found = false
					};

					GetGlobalInbox.RetrieveGlobalInboxAsync( ( success, globalInbox ) => {
						try {
							if( success ) {
								args.SetGlobalInbox( globalInbox );
							}
							args.Found = success;

							CustomLoadHooks.TriggerHook( GetGlobalInbox.GlobalInboxReceivedHookValidator, GetGlobalInbox.GlobalInboxReceivedHookValidatorKey, args );
						} catch( Exception e ) {
							LogHelpers.Alert( e.ToString() );
						}
					} );
				}
			} );
		}



		////////////////
		
		internal void OnPostSetupContent() {	//TODO
			GetGlobalInbox.CacheAllGlobalInboxAsync();
		}
	}
}
