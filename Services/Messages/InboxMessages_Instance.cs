using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Misc;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Messages {
	/// <summary>
	/// This service gives a way for mods to post persistent, non-obtrusive, in-game messages to players that can be
	/// re-read freely.
	/// </summary>
	public partial class InboxMessages {
		private InboxMessageData Data = new InboxMessageData();

		////////////////

		private IDictionary<string, string> Messages => this.Data.Messages;
		private IDictionary<string, Action<bool>> MessageActions => this.Data.MessageActions;
		private List<string> Order => this.Data.Order;
		internal int Current {
			get { return this.Data.Current; }
			set { this.Data.Current = value; }
		}



		////////////////

		internal InboxMessages() {
			this.Current = 0;
			
			LoadHooks.LoadHooks.AddWorldLoadEachHook( () => {
				bool success = this.LoadFromFile();
			} );
		}
		
		//~InboxMessages() {

		internal void OnWorldExit() {
			this.SaveToFile();
		}


		////////////////

		internal bool LoadFromFile() {
			var data = ModCustomDataFileHelpers.LoadJson<InboxMessageData>( ModHelpersMod.Instance, "Inbox" );
			if( data == null ) {
				return false;
			}

			this.Data = data;

			foreach( string msgName in this.Data.Messages.Keys ) {
				this.Data.MessageActions[msgName] = null;
			}

			return true;
		}

		internal void SaveToFile() {
			if( this.Data != null ) {
				ModCustomDataFileHelpers.SaveAsJson<InboxMessageData>( ModHelpersMod.Instance, "Inbox", true, this.Data );
			}
		}
	}
}
