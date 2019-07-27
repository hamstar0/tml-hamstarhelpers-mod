using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Messages.Inbox {
	class InboxMessageData {
		public IDictionary<string, string> Messages = new Dictionary<string, string>();
		internal IDictionary<string, Action<bool>> MessageActions = new Dictionary<string, Action<bool>>();
		public List<string> Order = new List<string>();
		public int Current = 0;
	}




	/// <summary>
	/// This service gives a way for mods to post persistent, non-obtrusive, in-game messages to players that can be
	/// re-read freely.
	/// </summary>
	public partial class InboxMessages {
		/// <summary>
		/// Creates an inbox message. New unread messages will be visible by an inventory screen icon until opened.
		/// Past messages can be viewed.
		/// </summary>
		/// <param name="which">Identifier of a given message. Overrides messages with the given identifier.</param>
		/// <param name="msg">Message body. Plain text only, for now.</param>
		/// <param name="forceUnread">If the message has been read, this will force it to be "unread" again.</param>
		/// <param name="onRun">Code to activate when a given message is read. Parameter `true` if message is unread.</param>
		public static void SetMessage( string which, string msg, bool forceUnread, Action<bool> onRun=null ) {
			LoadHooks.AddPostWorldLoadOnceHook( () => {
				InboxMessages inbox = ModHelpersMod.Instance.Inbox?.Messages;
				if( inbox == null ) {
					LogHelpers.Warn( "Inbox or Inbox.Messages is null" );
					return;
				}

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


		/// <summary>
		/// Indicates total unread messages.
		/// </summary>
		/// <returns></returns>
		public static int CountUnreadMessages() {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox?.Messages;
			if( inbox == null || inbox.Messages == null ) {
				return 0;
			}

			return inbox.Messages.Count - inbox.Current;
		}


		/// <summary>
		/// "Reads" latest message. Will trigger message's `onRun` function, if any.
		/// </summary>
		/// <returns></returns>
		public static string DequeueMessage() {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;
			
			if( inbox.Current >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ inbox.Current++ ];
			string msg;

			if( inbox.Messages.TryGetValue( which, out msg ) ) {
				inbox.MessageActions[ which ]?.Invoke( true );
			}

			return msg;
		}


		/// <summary>
		/// Retrieves a given message by it's order position. Does not "read" the message.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="msg">The message. Returns `null` if no message found.</param>
		/// <returns>`true` if unread.</returns>
		public static bool GetMessageAt( int pos, out string msg ) {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;
			bool isUnread = false;

			if( pos < 0 || pos >= inbox.Order.Count ) {
				msg = null;
				return false;
			}

			string which = inbox.Order[ pos ];

			if( inbox.Messages.TryGetValue( which, out msg ) ) {
				isUnread = pos >= inbox.Current;
				inbox.MessageActions[ which ]?.Invoke( isUnread );
			}

			return isUnread;
		}


		/// <summary>
		/// Reads a given message.
		/// </summary>
		/// <param name="which">Identifier of message to read.</param>
		/// <param name="msg">The message. Returns `null` if no message found.</param>
		/// <returns>`true` if unread.</returns>
		public static bool ReadMessage( string which, out string msg ) {
			InboxMessages inbox = ModHelpersMod.Instance.Inbox.Messages;

			int idx = inbox.Order.IndexOf( which );
			if( idx == -1 ) {
				msg = null;
				return false;
			}
			
			if( !inbox.Messages.TryGetValue( which, out msg ) ) {
				msg = null;
				return false;
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

			return isUnread;
		}
	}
}
