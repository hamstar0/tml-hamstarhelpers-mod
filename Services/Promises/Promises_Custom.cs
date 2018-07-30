using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Promises {
	public class PromiseTrigger {
		internal object Validator;

		public PromiseTrigger( object validator ) {
			this.Validator = validator;
		}
	}




	public partial class Promises {
		public static void AddCustomPromise( string name, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.NamedCustomPromiseConditionsMet.Contains(name) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.Promises.CustomPromise.ContainsKey(name) ) {
				mymod.Promises.CustomPromise[ name ] = new List<Func<bool>>();
			}
			mymod.Promises.CustomPromise[ name ].Add( action );
		}


		public static void TriggerCustomPromise( string name ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.NamedCustomPromiseConditionsMet.Add( name );

			if( mymod.Promises.CustomPromise.ContainsKey(name) ) {
				var func_list = mymod.Promises.CustomPromise[ name ];

				for( int i=0; i<func_list.Count; i++ ) {
					if( !func_list[i]() ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}


		public static void ClearCustomPromise( string name ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.NamedCustomPromiseConditionsMet.Remove( name );

			if( mymod.Promises.CustomPromise.ContainsKey( name ) ) {
				mymod.Promises.CustomPromise.Remove( name );
			}
		}


		////////////////


		public static void AddCustomPromiseForObject( object obj, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.ObjectCustomPromiseConditionsMet.Contains( obj ) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.Promises.CustomObjectPromise.ContainsKey( obj ) ) {
				mymod.Promises.CustomObjectPromise[ obj ] = new List<Func<bool>>();
			}
			mymod.Promises.CustomObjectPromise[ obj ].Add( action );
		}


		public static void TriggerCustomPromiseForObject( PromiseTrigger obj, object validator ) {
			var mymod = HamstarHelpersMod.Instance;

			if( obj.Validator != validator ) {
				throw new Exception( "Validation failed." );
			}

			mymod.Promises.ObjectCustomPromiseConditionsMet.Add( obj );

			if( mymod.Promises.CustomObjectPromise.ContainsKey( obj ) ) {
				var func_list = mymod.Promises.CustomObjectPromise[ obj ];

				for( int i = 0; i < func_list.Count; i++ ) {
					if( !func_list[i]() ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}


		public static void ClearCustomPromiseForObject( object obj ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.ObjectCustomPromiseConditionsMet.Remove( obj );

			if( mymod.Promises.CustomObjectPromise.ContainsKey( obj ) ) {
				mymod.Promises.CustomObjectPromise.Remove( obj );
			}
		}
	}
}
