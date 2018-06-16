using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.Utilities.Errors {
	[Obsolete("use Components.Errors.HamstarException", true)]
	public class HamstarException : Exception {
		public HamstarException( string msg ) : base( msg ) {
			var msg_count = HamstarHelpersMod.Instance.ExceptionMngr.MsgCount;
			int count = 0;

			if( msg_count.TryGetValue(msg, out count) ) {
				if( count > 10 && (Math.Log10(count) % 1) != 0 ) {
					return;
				}
			} else {
				msg_count[ msg ] = 0;
			}
			msg_count[ msg ]++;

			LogHelpers.Log( "EXCEPTION ("+count+") - " + msg );
		}
	}
}
