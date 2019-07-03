using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Misc;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Messages {
	public partial class InboxMessages {
		private InboxMessageData Data = new InboxMessageData();

		////////////////

		private IDictionary<string, string> Messages => this.Data.Messages;
		private IDictionary<string, Action<bool>> MessageActions => this.Data.MessageActions;
		private List<string> Order => this.Data.Order;
		public int Current {
			get { return this.Data.Current; }
			set { this.Data.Current = value; }
		}



		////////////////

		internal InboxMessages() {
			this.Current = 0;
			
			PromisedHooks.PromisedHooks.AddWorldLoadEachPromise( () => {
				bool success = this.LoadFromFile();
			} );
		}
		
		//~InboxMessages() {

		internal void OnWorldExit() {
			this.SaveToFile();
		}


		////////////////

		internal bool LoadFromFile() {
			bool success;
			var data = DataFileHelpers.LoadJson<InboxMessageData>( ModHelpersMod.Instance, "Inbox", out success );

			if( success ) {
				if( data != null ) {
					this.Data = data;

					foreach( string msgName in this.Data.Messages.Keys ) {
						this.Data.MessageActions[msgName] = null;
					}
				} else {
					success = false;
				}
			}
			return success;
		}

		internal void SaveToFile() {
			if( this.Data != null ) {
				DataFileHelpers.SaveAsJson<InboxMessageData>( ModHelpersMod.Instance, "Inbox", this.Data );
			}
		}
	}
}
