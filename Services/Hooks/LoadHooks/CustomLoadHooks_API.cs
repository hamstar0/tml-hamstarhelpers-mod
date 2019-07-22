using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	public partial class CustomLoadHooks {
		/// <summary>
		/// Counts the total number of hooks of a given validator.
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator">Hook's argument type.</param>
		/// <returns></returns>
		public static int CountHooks<T>( CustomLoadHookValidator<T> validator ) {
			var mymod = ModHelpersMod.Instance;

			lock( validator ) {
				if( !mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					return 0;
				}
				return mymod.CustomLoadHooks.Hooks.Count;
			}
		}


		////////////////

		/// <summary>
		/// Tests if conditions are met for a given hook type (by its validator).
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator"></param>
		/// <returns></returns>
		public static bool AreConditionsMet<T>( CustomLoadHookValidator<T> validator ) {
			lock( validator ) {
				return ModHelpersMod.Instance.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}
		}

		////////////////

		/// <summary>
		/// Add a custom hook.
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator">Validator used to identify the hook and validate what can trigger it.</param>
		/// <param name="func">The hook. Accepts an object containing its arguments (type validated). Returns `true`
		/// if it's meant to be repeatably called (when the conditions clear up and then later come to pass).</param>
		public static void AddHook<T>( CustomLoadHookValidator<T> validator, Func<T, bool> func ) {
			var mymod = ModHelpersMod.Instance;
			bool conditionsMet;
			Type argType = typeof( T );
			T args;

			lock( validator ) {
				conditionsMet = mymod.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}

			if( conditionsMet ) {
				lock( validator ) {
					args = (T)mymod.CustomLoadHooks.HookArgs[validator];
				}

				if( args != null ) {
					if( argType != args.GetType() ) {
						throw new HamstarException( "Invalid argument type: Expected "
							+ argType.Name + ", found "
							+ args.GetType().Name );
					}
				} else {
					throw new HamstarException( "Invalid argument type: "+argType.Name+" expected, found `null`" );
				}

				if( !func( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					mymod.CustomLoadHooks.Hooks[validator] = new List<Func<object, bool>>();
				}

				mymod.CustomLoadHooks.Hooks[validator].Add( arg => func( (T)arg ) );
			}
		}


		////////////////

		/// <summary>
		/// Triggers all of a given hook. A `validatorKey` must be provided matching the `validator`'s key (loosely
		/// guards against mis-triggering hooks).
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator">Validator used to identify the hook and validate what can trigger it.</param>
		/// <param name="validatorKey">A key that must match the `validator`'s own key.</param>
		/// <param name="args">Arguments passed to each hook. Must match the `validator`'s expected generic type.</param>
		public static void TriggerHook<T>( CustomLoadHookValidator<T> validator, object validatorKey, T args ) {
			var mymod = ModHelpersMod.Instance;
			Type argType = typeof( T );
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
			if( args == null ) {
				throw new HamstarException( "Invalid argument type: Expected "
					+ argType.Name + ", found `null`" );
			}
			if( argType != args.GetType() ) {
				throw new HamstarException( "Invalid argument type: Expected "
					+ argType.Name + ", found "
					+ args.GetType().Name );
			}

			lock( validator.MyLock ) {
				mymod.CustomLoadHooks.HookConditionsMet.Add( validator );
				mymod.CustomLoadHooks.HookArgs[validator] = args;
				isValidated = mymod.CustomLoadHooks.Hooks.ContainsKey( validator );
			}

			if( isValidated ) {
				IList<Func<object, bool>> funcList;
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

		/// <summary>
		/// Triggers all of a given hook. A `validatorKey` must be provided matching the `validator`'s key (loosely
		/// guards against mis-triggering hooks).
		/// </summary>
		/// <param name="validator">Validator used to identify the hook and validate what can trigger it.</param>
		/// <param name="validatorKey">A key that must match the `validator`'s own key.</param>
		public static void TriggerHook( CustomLoadHookValidator<object> validator, object validatorKey ) {
			CustomLoadHooks.TriggerHook<object>( validator, validatorKey, new object() );
		}


		/// <summary>
		/// Untriggers all of a given hook, allowing for reuse. A `validatorKey` must be provided matching the
		/// `validator`'s key (loosely guards against mis-triggering hooks).
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator">Validator used to identify the hook and validate what can trigger it.</param>
		/// <param name="validatorKey">A key that must match the `validator`'s own key.</param>
		public static void UntriggerHook<T>( CustomLoadHookValidator<T> validator, object validatorKey ) {
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

		/// <summary>
		/// Clears all of a given hook. A `validatorKey` must be provided matching the `validator`'s key (loosely guards
		/// against mis-triggering hooks).
		/// </summary>
		/// <typeparam name="T">Hook's argument type.</typeparam>
		/// <param name="validator">Validator used to identify the hook and validate what can trigger it.</param>
		/// <param name="validatorKey">A key that must match the `validator`'s own key.</param>
		public static void ClearHook<T>( CustomLoadHookValidator<T> validator, object validatorKey ) {
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
