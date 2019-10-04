using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	public class MethodDefinition {
		private string MethodName;
		private Delegate Wrapper;
		private ParameterInfo[] ParamInfo;



		////////////////

		public MethodDefinition( MethodInfo method ) {
			this.MethodName = method.Name;
			this.Wrapper = method.CreateDelegate( typeof(Func<object[], object>) );

			this.ParamInfo = method.GetParameters();
		}

		////////////////

		public object Invoke( object methodContext, params object[] args ) {
			if( args.Length != this.ParamInfo.Length ) {
				throw new ModHelpersException( "Mismatched input argument quantity. (for call " + this.MethodName + ")" );
			}

			for( int i = 0; i < this.ParamInfo.Length; i++ ) {
				Type paramType = this.ParamInfo[i].ParameterType;
				Type argType = args[i].GetType();

				if( args[i] == null ) {
					if( !paramType.IsClass || this.ParamInfo[i].GetCustomAttribute<NullableAttribute>() == null ) {
						throw new ModHelpersException( "Invalid param " + this.ParamInfo[i].Name + " (#" + i + "): Expected " + paramType.Name + ", found null (for call " + this.MethodName + ")" );
					}
				} else if( argType.Name != paramType.Name && !argType.IsSubclassOf( paramType ) ) {
					throw new ModHelpersException( "Invalid param " + this.ParamInfo[i].Name + " (#" + i + "): Expected " + paramType.Name + ", found " + argType.Name + " (for call " + this.MethodName + ")" );
				}
			}

			return this.Wrapper.DynamicInvoke( args );
		}
	}




	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers {
		/// <summary>
		/// Invokes a method, first validating the supplied parameters for type consistency.
		/// </summary>
		/// <param name="method">Reflected method to invoke.</param>
		/// <param name="methodContext">Object instance containing of the given method. Use `null` for static methods.</param>
		/// <param name="args">Parameters to pass to the given method.</param>
		/// <returns>Results from invoking the method.</returns>
		public static object SafeCall( MethodInfo method, object methodContext, object[] args ) {
			var paramInfos = method.GetParameters();

			if( args.Length != paramInfos.Length ) {
				throw new ModHelpersException( "Mismatched input argument quantity. (for call " + method.Name + ")" );
			}

			for( int i = 0; i < paramInfos.Length; i++ ) {
				Type paramType = paramInfos[i].ParameterType;
				Type argType = args[i].GetType();

				if( args[i] == null ) {
					if( !paramType.IsClass || paramInfos[i].GetCustomAttribute<NullableAttribute>() == null ) {
						throw new ModHelpersException( "Invalid param " + paramInfos[i].Name + " (#" + i + "): Expected " + paramType.Name + ", found null (for call " + method.Name + ")" );
					}
				} else if( argType.Name != paramType.Name && !argType.IsSubclassOf( paramType ) ) {
					throw new ModHelpersException( "Invalid param " + paramInfos[i].Name + " (#" + i + "): Expected " + paramType.Name + ", found " + argType.Name + " (for call " + method.Name + ")" );
				}
			}

			return method.Invoke( methodContext, args );
		}


		////////////////

		/// <summary>
		/// Invokes a method of a given class.
		/// </summary>
		/// <typeparam name="T">Invoked method's return value type.</typeparam>
		/// <param name="instance">Class instance of class of method.</param>
		/// <param name="methodName">Method's name.</param>
		/// <param name="args">Method's arguments (will be validated before invoking).</param>
		/// <param name="returnVal">Return value of method.</param>
		/// <returns>`true` if method found and invoked successfully.</returns>
		public static bool RunMethod<T>( Object instance, string methodName, object[] args, out T returnVal ) {
			if( instance == null ) {
				returnVal = default( T );
				return false;
			}
			return ReflectionHelpers.RunMethod<T>( instance.GetType(), instance, methodName, args, out returnVal );
		}

		/// <summary>
		/// Invokes a method of a given class. May invoke static methods if the given `instance` parameter is `null`.
		/// </summary>
		/// <typeparam name="T">Invoked method's return value type.</typeparam>
		/// <param name="classType">Class type of method.</param>
		/// <param name="instance">Class instance of class of method. Use `null` for static methods.</param>
		/// <param name="methodName">Method's name.</param>
		/// <param name="args">Method's arguments (will be validated before invoking).</param>
		/// <param name="returnVal">Return value of method.</param>
		/// <returns>`true` if method found and invoked successfully.</returns>
		public static bool RunMethod<T>( Type classType, Object instance, string methodName, object[] args, out T returnVal ) {
			var mymod = ModHelpersMod.Instance;
			var rh = mymod.ReflectionHelpers;
			MethodDefinition methodDef = rh.GetCachedMethodDefinition( instance, methodName );
			if( methodDef == null ) {
				returnVal = default(T);
				return false;
			}

			returnVal = (T)methodDef.Invoke( args );
			return true;

			/*Type[] paramTypes = args?.SafeSelect( o => o.GetType() ).ToArray()
				?? new Type[] { };

			MethodInfo method = classType.GetMethod( methodName, ReflectionHelpers.MostAccess, null, paramTypes, null );
			if( method == null ) {
				if( classType.BaseType != null ) {
					return ReflectionHelpers.RunMethod<T>( classType.BaseType, instance, methodName, args, out returnVal );
				}

				returnVal = default( T );
				return false;
			}

			returnVal = (T)ReflectionHelpers.SafeCall( method, instance, args );
			return true;*/
		}
	}
}
