using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.LoadHooks {
	public partial class CustomLoadHooks {
		public static int CountValidatedHooks( CustomLoadHookValidator validator ) {
			var mymod = ModHelpersMod.Instance;

			lock( validator ) {
				if( !mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					return 0;
				}
				return mymod.CustomLoadHooks.Hooks.Count;
			}
		}


		////////////////

		public static bool IsHookValidated( CustomLoadHookValidator validator ) {
			lock( validator ) {
				return ModHelpersMod.Instance.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}
		}

		////////////////

		public static void AddHook( CustomLoadHookValidator validator, Func<T, bool> func ) {
			var mymod = ModHelpersMod.Instance;
			bool conditionsMet;
			T args;

			lock( validator ) {
				conditionsMet = mymod.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}

			if( conditionsMet ) {
				lock( validator ) {
					args = (T)mymod.CustomLoadHooks.HookArgs[validator];
				}

				if( !func( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					mymod.CustomLoadHooks.Hooks[validator] = new List<Func<T, bool>>();
				}
				mymod.CustomLoadHooks.Hooks[validator].Add( U => func( (T)U ) );
			}
		}

		public static void AddHook( CustomLoadHookValidator validator, Func<bool> action ) {
			CustomLoadHooks.AddHook( validator, _ => action() );
		}


		////////////////

		public static void TriggerHook( CustomLoadHookValidator validator, object validatorKey, T args ) {
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
				mymod.CustomLoadHooks.HookConditionsMet.Add( validator );
				mymod.CustomLoadHooks.HookArgs[validator] = args;
				isValidated = mymod.CustomLoadHooks.Hooks.ContainsKey( validator );
			}

			if( isValidated ) {
				IList<Func<T, bool>> funcList;
				int count;

				lock( validator.MyLock ) {
					funcList = mymod.CustomLoadHooks.Hooks[validator];
					count = funcList.Count;
				}

				for( int i = 0; i < funcList.Count; i++ ) {
					lock( validator.MyLock ) {
						isEach = funcList[i]( args );
					}

					if( !isEach ) {
						funcList.RemoveAt( i );
						i--;
					}
				}
			}
		}

		public static void TriggerHook( CustomLoadHookValidator validator, object validatorKey ) {
			CustomLoadHooks.TriggerHook( validator, validatorKey, default(T) );
		}


		public static void UntriggerHook( CustomLoadHookValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
				throw new HamstarException( "Validation failed." );
			}

			lock( validator ) {
				mymod.CustomLoadHooks.HookConditionsMet.Remove( validator );
				mymod.CustomLoadHooks.HookArgs.Remove( validator );
			}
		}


		////////////////

		public static void ClearHook( CustomLoadHookValidator validator, object validatorKey ) {
			var mymod = ModHelpersMod.Instance;

			if( validator.ValidatorKey != validatorKey ) {
				throw new HamstarException( "Validation failed." );
			}

			lock( validator ) {
				mymod.CustomLoadHooks.HookConditionsMet.Remove( validator );
				mymod.CustomLoadHooks.HookArgs.Remove( validator );

				if( mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					mymod.CustomLoadHooks.Hooks.Remove( validator );
				}
			}
		}
	}
}
