using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	/** <summary>Assorted static "helper" functions for alleviating tedious "boilerplate" code.</summary> */
	public class ModBoilerplateHelpers {
		public static object HandleModCall( Type apiClassType, params object[] args ) {
			if( args == null || args.Length == 0 ) { throw new HamstarException( "Undefined call." ); }

			string callType = args[0] as string;
			if( callType == null ) {
				LogHelpers.Alert( "Invalid call binding: " + args[0] );
				return null;
			}

			MethodInfo methodInfo = apiClassType.GetMethod( callType );
			if( methodInfo == null ) {
				var argsList = args.SafeSelect( a => a.GetType().Name + ": " + a == null ? "null" : a.ToString() );
				string argsListStr = string.Join( ", ", argsList );

				LogHelpers.Alert( apiClassType.Name+" has no Call binding for " + callType + " with args: "+argsListStr );
				return null;
			}

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			try {
				return ReflectionHelpers.SafeCall( methodInfo, null, newArgs );
			} catch( Exception e ) {
				throw new HamstarException( apiClassType.Name+" failed to execute Call binding " +callType, e );
			}
		}
	}
}
