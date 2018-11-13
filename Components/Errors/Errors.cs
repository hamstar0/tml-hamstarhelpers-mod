using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.Errors {
	class HamstarExceptionManager {
		internal readonly IDictionary<string, int> MsgCount = new Dictionary<string, int>();
	}




	public class HamstarException : Exception {
		public HamstarException( string msg ) : base( msg ) {
			this.Initialize( msg );
		}

		public HamstarException( string msg, Exception inner ) : base( msg, inner ) {
			this.Initialize( msg );
		}


		////////////////

		private void Initialize( string msg ) {
			var msg_count = ModHelpersMod.Instance.ExceptionMngr.MsgCount;
			int count = 0;

			if( msg_count.TryGetValue( msg, out count ) ) {
				if( count > 10 && (Math.Log10( count ) % 1) != 0 ) {
					return;
				}
			} else {
				msg_count[msg] = 0;
			}
			msg_count[msg]++;

			if( this.InnerException != null ) {
				LogHelpers.Log( "EXCEPTION (" + count + ") - " + msg + " | " + this.InnerException.Message );
			} else {
				LogHelpers.Log( "EXCEPTION (" + count + ") - " + msg );
			}
		}
	}
}
