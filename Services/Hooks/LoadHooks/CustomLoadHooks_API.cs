using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.LoadHooks {
	/// <summary>
	/// Allows defining custom load hooks. Like the preset load hooks, these will activate when conditions are triggered
	/// for hooks that are later added.
	/// </summary>
	public partial class CustomLoadHooks {
		/// <summary>
		/// Counts the total number of hooks of a given validator.
		/// </summary>
		/// <param name="validator"></param>
		/// <returns></returns>
		public static int CountHooks( CustomLoadHookValidator validator ) {
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
		/// <param name="validator"></param>
		/// <returns></returns>
		public static bool AreConditionsMet( CustomLoadHookValidator validator ) {
			lock( validator ) {
				return ModHelpersMod.Instance.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}
		}

		////////////////

		public static void AddHook( CustomLoadHookValidator validator, Func<object, bool> func ) {
			var mymod = ModHelpersMod.Instance;
			bool conditionsMet;
			object args;

			lock( validator ) {
				conditionsMet = mymod.CustomLoadHooks.HookConditionsMet.Contains( validator );
			}

			if( conditionsMet ) {
				lock( validator ) {
					args = mymod.CustomLoadHooks.HookArgs[ validator ];
				}

				if( validator.ArgType == null && args != null ) {
					throw new HamstarException( "Invalid argument type: `null` expected, found " + args.GetType().Name );
				}
				if( validator.ArgType != args.GetType() ) {
					throw new HamstarException( "Invalid argument type: Expected "
						+ validator.ArgType.Name + ", found "
						+ args.GetType().Name );
				}

				if( !func( args ) ) {
					return;
				}
			}

			lock( validator ) {
				if( !mymod.CustomLoadHooks.Hooks.ContainsKey( validator ) ) {
					mymod.CustomLoadHooks.Hooks[validator] = new List<Func<object, bool>>();
				}

				mymod.CustomLoadHooks.Hooks[validator].Add( arg => func( arg ) );
			}
		}

		public static void AddHook( CustomLoadHookValidator validator, Func<bool> action ) {
			CustomLoadHooks.AddHook( validator, _ => action() );
		}


		////////////////

		public static void TriggerHook( CustomLoadHookValidator validator, object validatorKey, object args ) {
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
			if( validator.ArgType == null && args != null ) {
				throw new HamstarException( "Invalid argument type: `null` expected, found "+args.GetType().Name );
			}
			if( validator.ArgType != args.GetType() ) {
				throw new HamstarException( "Invalid argument type: Expected "
					+ validator.ArgType.Name + ", found "
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

		public static void TriggerHook( CustomLoadHookValidator validator, object validatorKey ) {
			CustomLoadHooks.TriggerHook( validator, validatorKey, null );
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
