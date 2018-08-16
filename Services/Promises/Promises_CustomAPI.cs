using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Promises {
	sealed public class PromiseValidator {
		internal object ValidatorKey;


		public PromiseValidator( object validator_key ) {
			this.ValidatorKey = validator_key;
		}
	}



	abstract public class PromiseArguments { }




	public partial class Promises {
		public static int CountValidatedPromises( PromiseValidator validator ) {
			var mymod = ModHelpersMod.Instance;

			lock( validator ) {
				if( !mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
					return 0;
				}
				return mymod.Promises.ValidatedPromise.Count;
			}
		}

		////////////////

		public static void AddValidatedPromise<T>( PromiseValidator validator, Func<T, bool> action ) where T : PromiseArguments {
			var mymod = ModHelpersMod.Instance;
			bool conditions_met;
			T args;
			
			lock( validator ) {
				conditions_met = mymod.Promises.ValidatedPromiseConditionsMet.Contains( validator );
			}

			if( conditions_met ) {
				lock( validator ) {
					args = (T)mymod.Promises.ValidatedPromiseArgs[ validator ];
				}

				if( !action( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
					mymod.Promises.ValidatedPromise[validator] = new List<Func<PromiseArguments, bool>>();
				}
				mymod.Promises.ValidatedPromise[validator].Add( U => action( (T)U ) );
			}
		}
		
		public static void AddValidatedPromise( PromiseValidator validator, Func<bool> action ) {
			Promises.AddValidatedPromise<PromiseArguments>( validator, _ => action() );
		}


		////////////////

		public static void TriggerValidatedPromise( PromiseValidator validator, object validator_key, PromiseArguments args ) {
			var mymod = ModHelpersMod.Instance;
			bool is_validated, is_each;

			if( validator.ValidatorKey != validator_key ) {
				throw new Exception( "Validation failed." );
			}

			lock( validator ) {
				mymod.Promises.ValidatedPromiseConditionsMet.Add( validator );
				mymod.Promises.ValidatedPromiseArgs[ validator ] = args;
				is_validated = mymod.Promises.ValidatedPromise.ContainsKey( validator );
			}

			if( is_validated ) {
				IList<Func<PromiseArguments, bool>> func_list = mymod.Promises.ValidatedPromise[ validator ];
				int count;

				lock( validator ) {
					count = func_list.Count;
				}

				for( int i = 0; i < func_list.Count; i++ ) {
					lock( validator ) {
						is_each = func_list[ i ]( args );
					}

					if( !is_each ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}
		
		public static void TriggerValidatedPromise( PromiseValidator validator, object validator_key ) {
			Promises.TriggerValidatedPromise( validator, validator_key, null );
		}


		////////////////

		public static void ClearValidatedPromise( PromiseValidator validator, object validator_key ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validator_key ) {
				throw new Exception( "Validation failed." );
			}

			lock( validator ) {
				mymod.Promises.ValidatedPromiseConditionsMet.Remove( validator );
				mymod.Promises.ValidatedPromiseArgs.Remove( validator );

				if( mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
					mymod.Promises.ValidatedPromise.Remove( validator );
				}
			}
		}
	}
}
