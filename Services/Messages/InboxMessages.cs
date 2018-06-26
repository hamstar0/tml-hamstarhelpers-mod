using HamstarHelpers.DebugHelpers;
using HamstarHelpers.MiscHelpers;
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
		public static void SetMessage( string which, string msg, bool force_unread, Action<bool> on_run=null ) {
			Promises.Promises.AddPostWorldLoadOncePromise( () => {
				InboxMessages inbox = HamstarHelpersMod.Instance.Inbox.Messages;
				int idx = inbox.Order.IndexOf( which );

				inbox.Messages[which] = msg;
				inbox.MessageActions[which] = on_run;

				if( idx >= 0 ) {
					if( force_unread ) {
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
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox.Messages;

			return inbox.Messages.Count - inbox.Current;
		}


		public static string DequeueMessage() {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox.Messages;
			
			if( inbox.Current >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ inbox.Current++ ];
			string msg = inbox.Messages[ which ];

			inbox.MessageActions[which]?.Invoke( true );

			return msg;
		}


		public static string GetMessageAt( int idx, out bool is_unread ) {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox.Messages;
			is_unread = false;

			if( idx < 0 || idx >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ idx ];
			string msg = inbox.Messages[ which ];

			is_unread = idx >= inbox.Current;

			inbox.MessageActions[which]?.Invoke( is_unread );

			return msg;
		}


		public static string ReadMessage( string which ) {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox.Messages;

			int idx = inbox.Order.IndexOf( which );
			if( idx == -1 ) { return null; }

			string msg = inbox.Messages[ which ];
			bool is_unread = idx >= inbox.Current;

			inbox.MessageActions[which]?.Invoke( is_unread );

			if( is_unread ) {
				if( inbox.Current != idx ) {
					inbox.Order.RemoveAt( idx );
					inbox.Order.Insert( inbox.Current, which );
				}
				inbox.Current++;
			}
			
			return msg;
		}



		////////////////

		private IDictionary<string, string> Messages { get { return this.Data.Messages; } }
		private IDictionary<string, Action<bool>> MessageActions { get { return this.Data.MessageActions; } }
		private List<string> Order { get { return this.Data.Order; } }
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
			var data = DataFileHelpers.LoadJson<InboxMessageData>( HamstarHelpersMod.Instance, "Inbox", out success );

			if( success ) {
				this.Data = data;

				foreach( string msg_name in this.Data.Messages.Keys ) {
					this.Data.MessageActions[ msg_name ] = null;
				}
			}
			return success;
		}

		internal void SaveToFile() {
			DataFileHelpers.SaveAsJson<InboxMessageData>( HamstarHelpersMod.Instance, "Inbox", this.Data );
		}
	}
}
