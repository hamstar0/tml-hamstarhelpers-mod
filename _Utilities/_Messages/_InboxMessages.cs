using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.Utilities.Messages {
	[Obsolete( "use Services.Messages.InboxMessages", true )]
	public class InboxMessages {
		public static void SetMessage( string which, string msg, bool force_unread, Action<bool> on_run=null ) {
			Services.Messages.InboxMessages.SetMessage( which, msg, force_unread, on_run );
		}
		public static int CountUnreadMessages() {
			return Services.Messages.InboxMessages.CountUnreadMessages();
		}
		public static string DequeueMessage() {
			return Services.Messages.InboxMessages.DequeueMessage();
		}
		public static string GetMessageAt( int idx, out bool is_unread ) {
			return Services.Messages.InboxMessages.GetMessageAt( idx, out is_unread );
		}
		public static string ReadMessage( string which ) {
			return Services.Messages.InboxMessages.ReadMessage( which );
		}
	}
}
