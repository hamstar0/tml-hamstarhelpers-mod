using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.LoadHooks {
	sealed public class CustomLoadHookValidator {
		internal object MyLock = new object();
		internal object ValidatorKey;


		public CustomLoadHookValidator( object validatorKey ) {
			this.ValidatorKey = validatorKey;
		}
	}



	abstract public class CustomLoadHookArguments { }




	public partial class LoadHooks {
		public static int CountValidatedHooks( CustomLoadHookValidator validator ) {
			var mymod = ModHelpersMod.Instance;

			lock( validator ) {
				if( !mymod.LoadHooks.CustomHooks.ContainsKey( validator ) ) {
					return 0;
				}
				return mymod.LoadHooks.CustomHooks.Count;
			}
		}


		////////////////

		public static bool IsHookValidated( CustomLoadHookValidator validator ) {
			lock( validator ) {
				return ModHelpersMod.Instance.LoadHooks.CustomHookConditionsMet.Contains( validator );
			}
		}
		
		////////////////

		public static void AddCustomHook<T>( CustomLoadHookValidator validator, Func<T, bool> func )
					where T : CustomLoadHookArguments {
			var mymod = ModHelpersMod.Instance;
			bool conditionsMet;
			T args;
			
			lock( validator ) {
				conditionsMet = mymod.LoadHooks.CustomHookConditionsMet.Contains( validator );
			}

			if( conditionsMet ) {
				lock( validator ) {
					args = (T)mymod.LoadHooks.CustomHookArgs[ validator ];
				}

				if( !func( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.LoadHooks.CustomHooks.ContainsKey( validator ) ) {
					mymod.LoadHooks.CustomHooks[validator] = new List<Func<CustomLoadHookArguments, bool>>();
				}
				mymod.LoadHooks.CustomHooks[validator].Add( U => func( (T)U ) );
			}
		}
		
		public static void AddCustomHook( CustomLoadHookValidator validator, Func<bool> action ) {
			LoadHooks.AddCustomHook<CustomLoadHookArguments>( validator, _ => action() );
		}


		////////////////

		public static void TriggerCustomHook( CustomLoadHookValidator validator, object validatorKey, CustomLoadHookArguments args ) {
			var mymod = ModHelpersMod.Instance;
			bool isValidated, isEach;

			if( mymod.LoadHooks == null ) {
				throw new HamstarException( "Custom hooks not loaded." );
			}
			if( validatorKey == null ) {
				throw new HamstarException( "No validation key specified." );
			}
			if( validator.ValidatorKey != validatorKey ) {
				throw new HamstarException( "Validation failed." );
			}

			lock( validator.MyLock ) {
				mymod.LoadHooks.CustomHookConditionsMet.Add( validator );
				mymod.LoadHooks.CustomHookArgs[ validator ] = args;
				isValidated = mymod.LoadHooks.CustomHooks.ContainsKey( validator );
			}

			if( isValidated ) {
				IList<Func<CustomLoadHookArguments, bool>> funcList;
				int count;

				lock( validator.MyLock ) {
					funcList = mymod.LoadHooks.CustomHooks[ validator ];
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
		
		public static void TriggerCustomHook( CustomLoadHookValidator validator, object validatorKey ) {
			LoadHooks.TriggerCustomHook( validator, validatorKey, null );
		}


		public static void UntriggerCustomHook( CustomLoadHookValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
				throw new HamstarException( "Validation failed." );
			}

			lock( validator ) {
				mymod.LoadHooks.CustomHookConditionsMet.Remove( validator );
				mymod.LoadHooks.CustomHookArgs.Remove( validator );
			}
		}


		////////////////

		public static void ClearCustomHook( CustomLoadHookValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
				throw new HamstarException( "Validation failed." );
			}

			lock( validator ) {
				mymod.LoadHooks.CustomHookConditionsMet.Remove( validator );
				mymod.LoadHooks.CustomHookArgs.Remove( validator );

				if( mymod.LoadHooks.CustomHooks.ContainsKey( validator ) ) {
					mymod.LoadHooks.CustomHooks.Remove( validator );
				}
			}
		}
	}
}
