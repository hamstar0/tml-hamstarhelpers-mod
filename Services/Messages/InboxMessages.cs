using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Messages {
	class InboxMessageData {
		public IDictionary<string, string> Messages = new Dictionary<string, string>();
		internal IDictionary<string, Action<bool>> MessageActions = new Dictionary<string, Action<bool>>();
		public List<string> Order = new List<string>();
		public int Current = 0;
	}




	public class InboxMessages {
		public static void SetMessage( string which, string msg, bool forceUnread, Action<bool> onRun=null ) {
			Promises.Promises.AddPostWorldLoadOncePromise( () => {
				InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;
				int idx = inbox.Order.IndexOf( which );

				inbox.Messages[ which ] = msg;
				inbox.MessageActions[ which ] = onRun;

				if( idx >= 0 ) {
					if( forceUnread ) {
						if( idx < inbox.Current ) {
							inbox.Current--;
						}

						inbox.Order.Remove( which );
						inbox.Order.Add( which );
					}
				} else {
					inbox.Order.Add( which );
				}
//LogHelpers.Log("which:"+which+", curr:"+inbox.Current+", pos:"+inbox.Order.IndexOf( which )+", forced:"+force_unread);
			} );
		}


		public static int CountUnreadMessages() {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;

			return inbox.Messages.Count - inbox.Current;
		}


		public static string DequeueMessage() {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;
			
			if( inbox.Current >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ inbox.Current++ ];
			string msg = null;

			if( inbox.Messages.TryGetValue( which, out msg ) ) {
				inbox.MessageActions[ which ]?.Invoke( true );
			}

			return msg;
		}


		public static string GetMessageAt( int idx, out bool isUnread ) {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;
			isUnread = false;

			if( idx < 0 || idx >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ idx ];
			string msg = null;

			if( inbox.Messages.TryGetValue( which, out msg ) ) {
				isUnread = idx >= inbox.Current;
				inbox.MessageActions[ which ]?.Invoke( isUnread );
			}

			return msg;
		}


		public static string ReadMessage( string which ) {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;

			int idx = inbox.Order.IndexOf( which );
			if( idx == -1 ) { return null; }
			
			string msg = null;

			if( !inbox.Messages.TryGetValue( which, out msg ) ) {
				return null;
			}

			bool isUnread = idx >= inbox.Current;

			inbox.MessageActions[ which ]?.Invoke( isUnread );

			if( isUnread ) {
				if( inbox.Current != idx ) {
					inbox.Order.RemoveAt( idx );
					inbox.Order.Insert( inbox.Current, which );
				}
				inbox.Current++;
			}
			
			return msg;
		}



		////////////////

		private IDictionary<string, string> Messages => this.Data.Messages;
		private IDictionary<string, Action<bool>> MessageActions => this.Data.MessageActions;
		private List<string> Order => this.Data.Order;
		public int Current {
			get { return this.Data.Current; }
			set { this.Data.Current = value; }
		}

		private InboxMessageData Data = new InboxMessageData();


		////////////////

		internal InboxMessages() {
			this.Current = 0;
			
			Promises.Promises.AddWorldLoadEachPromise( () => {
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
				this.Data = data;

				foreach( string msgName in this.Data.Messages.Keys ) {
					this.Data.MessageActions[ msgName ] = null;
				}
			}
			return success;
		}

		internal void SaveToFile() {
			DataFileHelpers.SaveAsJson<InboxMessageData>( ModHelpersMod.Instance, "Inbox", this.Data );
		}
	}
}
