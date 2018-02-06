using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.Utilities.Errors {
	public class HamstarException : Exception {
		public HamstarException( string msg ) : base( msg ) {
			LogHelpers.Log( "EXCEPTION - " + msg );
		}
	}
}
