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
		public static int CountValidatedPromises( PromiseValidator validator ) {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
				return 0;
			}
			return mymod.Promises.ValidatedPromise.Count;
		}

		public static void AddValidatedPromise( PromiseValidator validator, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.ValidatedPromiseConditionsMet.Contains( validator ) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
				mymod.Promises.ValidatedPromise[ validator ] = new List<Func<bool>>();
			}
			mymod.Promises.ValidatedPromise[ validator ].Add( action );
		}


		public static void TriggerValidatedPromise( PromiseValidator validator, object validator_key ) {
			var mymod = HamstarHelpersMod.Instance;

			if( validator.ValidatorKey != validator_key ) {
				throw new Exception( "Validation failed." );
			}

			mymod.Promises.ValidatedPromiseConditionsMet.Add( validator );

			if( mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
				var func_list = mymod.Promises.ValidatedPromise[ validator ];

				for( int i = 0; i < func_list.Count; i++ ) {
					if( !func_list[i]() ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}


		public static void ClearValidatedPromise( PromiseValidator validator ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.ValidatedPromiseConditionsMet.Remove( validator );

			if( mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
				mymod.Promises.ValidatedPromise.Remove( validator );
			}
		}
	}
}
