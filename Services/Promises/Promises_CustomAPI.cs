using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Promises {
	sealed public class PromiseValidator {
		internal object MyLock = new object();
		internal object ValidatorKey;


		public PromiseValidator( object validatorKey ) {
			this.ValidatorKey = validatorKey;
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

		public static bool IsPromiseValidated( PromiseValidator validator ) {
			lock( validator ) {
				return ModHelpersMod.Instance.Promises.ValidatedPromiseConditionsMet.Contains( validator );
			}
		}
		
		////////////////

		public static void AddValidatedPromise<T>( PromiseValidator validator, Func<T, bool> func ) where T : PromiseArguments {
			var mymod = ModHelpersMod.Instance;
			bool conditionsMet;
			T args;
			
			lock( validator ) {
				conditionsMet = mymod.Promises.ValidatedPromiseConditionsMet.Contains( validator );
			}

			if( conditionsMet ) {
				lock( validator ) {
					args = (T)mymod.Promises.ValidatedPromiseArgs[ validator ];
				}

				if( !func( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.Promises.ValidatedPromise.ContainsKey( validator ) ) {
					mymod.Promises.ValidatedPromise[validator] = new List<Func<PromiseArguments, bool>>();
				}
				mymod.Promises.ValidatedPromise[validator].Add( U => func( (T)U ) );
			}
		}
		
		public static void AddValidatedPromise( PromiseValidator validator, Func<bool> action ) {
			Promises.AddValidatedPromise<PromiseArguments>( validator, _ => action() );
		}


		////////////////

		public static void TriggerValidatedPromise( PromiseValidator validator, object validatorKey, PromiseArguments args ) {
			var mymod = ModHelpersMod.Instance;
			bool isValidated, isEach;

			if( validator.ValidatorKey != validatorKey ) {
				throw new Exception( "Validation failed." );
			}

			lock( validator.MyLock ) {
				mymod.Promises.ValidatedPromiseConditionsMet.Add( validator );
				mymod.Promises.ValidatedPromiseArgs[ validator ] = args;
				isValidated = mymod.Promises.ValidatedPromise.ContainsKey( validator );
			}

			if( isValidated ) {
				IList<Func<PromiseArguments, bool>> funcList;
				int count;

				lock( validator.MyLock ) {
					funcList = mymod.Promises.ValidatedPromise[ validator ];
					count = funcList.Count;
				}

				for( int i = 0; i < funcList.Count; i++ ) {
					lock( validator.MyLock ) {
						isEach = funcList[ i ]( args );
					}

					if( !isEach ) {
						funcList.RemoveAt( i );
						i--;
					}
				}
			}
		}
		
		public static void TriggerValidatedPromise( PromiseValidator validator, object validatorKey ) {
			Promises.TriggerValidatedPromise( validator, validatorKey, null );
		}


		public static void UntriggerValidatedPromise( PromiseValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
				throw new Exception( "Validation failed." );
			}

			lock( validator ) {
				mymod.Promises.ValidatedPromiseConditionsMet.Remove( validator );
				mymod.Promises.ValidatedPromiseArgs.Remove( validator );
			}
		}


		////////////////

		public static void ClearValidatedPromise( PromiseValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
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
