using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Promises {
	public class PromiseValidator {
		internal object ValidatorKey;


		public PromiseValidator( object validator_key ) {
			this.ValidatorKey = validator_key;
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


		public static void AddCustomValidatedPromise( PromiseValidator validator, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.ObjectCustomPromiseConditionsMet.Contains( validator ) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.Promises.CustomObjectPromise.ContainsKey( validator ) ) {
				mymod.Promises.CustomObjectPromise[ validator ] = new List<Func<bool>>();
			}
			mymod.Promises.CustomObjectPromise[ validator ].Add( action );
		}


		public static void TriggerCustomValidatedPromise( PromiseValidator validator, object validator_key ) {
			var mymod = HamstarHelpersMod.Instance;

			if( validator.ValidatorKey != validator_key ) {
				throw new Exception( "Validation failed." );
			}

			mymod.Promises.ObjectCustomPromiseConditionsMet.Add( validator );

			if( mymod.Promises.CustomObjectPromise.ContainsKey( validator ) ) {
				var func_list = mymod.Promises.CustomObjectPromise[ validator ];

				for( int i = 0; i < func_list.Count; i++ ) {
					if( !func_list[i]() ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}


		public static void ClearCustomValidatedPromise( PromiseValidator validator ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.ObjectCustomPromiseConditionsMet.Remove( validator );

			if( mymod.Promises.CustomObjectPromise.ContainsKey( validator ) ) {
				mymod.Promises.CustomObjectPromise.Remove( validator );
			}
		}
	}
}
