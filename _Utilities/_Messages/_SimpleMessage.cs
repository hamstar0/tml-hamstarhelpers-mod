using System;


namespace HamstarHelpers.Utilities.Messages {
	[Obsolete( "use Services.Messages.SimpleMessage", true )]
	public static class SimpleMessage {
		public static int MessageDuration {
			get { return Services.Messages.SimpleMessage.MessageDuration; }
			set { Services.Messages.SimpleMessage.MessageDuration = value; }
		}
		public static string Message {
			get { return Services.Messages.SimpleMessage.Message; }
			set { Services.Messages.SimpleMessage.Message = value; }
		}
		public static string SubMessage {
			get { return Services.Messages.SimpleMessage.SubMessage; }
			set { Services.Messages.SimpleMessage.SubMessage = value; }
		}
		
		public static void PostMessage( string msg, string submsg, int duration ) {
			Services.Messages.SimpleMessage.PostMessage( msg, submsg, duration );
		}
	}
}
