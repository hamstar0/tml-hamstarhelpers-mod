using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	[Obsolete( "use DotNetHelpers.Reflection.ReflectionHelpers", true )]
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false, Inherited = true )]
	public class NullableAttribute : Attribute { }




	[Obsolete("use DotNetHelpers.Reflection.ReflectionHelpers", true)]
	public partial class ReflectionHelpers {
		public static Reflection.ReflectionHelpers Instance => ModHelpersMod.Instance.ReflectionHelpers;
		public readonly static BindingFlags MostAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;



		////////////////

		public static IList<Type> GetTypesFromAssembly( string assemblyName, string typeName ) {
			return Reflection.ReflectionHelpers.GetTypesFromAssembly( assemblyName, typeName );
		}
		
		public static IEnumerable<Type> GetAllAvailableSubTypesFromMods( Type parentType ) {
			return Reflection.ReflectionHelpers.GetAllAvailableSubTypesFromMods( parentType );
		}

		public static IEnumerable<Type> GetAllAvailableSubTypesFromAssemblies( IEnumerable<Assembly> asses, Type parentType ) {
			return Reflection.ReflectionHelpers.GetAllAvailableSubTypesFromAssemblies( asses, parentType );
		}

		public static Type GetClassFromAssembly( string assemblyName, string namespaceAndClassName ) {
			return Reflection.ReflectionHelpers.GetClassFromAssembly( assemblyName, namespaceAndClassName );
		}


		////////////////
		
		public static bool Get<T>( Object instance, string fieldOrPropName, out T result ) {
			return Reflection.ReflectionHelpers.Get<T>( instance, fieldOrPropName, out result );
		}

		public static bool Get<T>( Type objType, Object instance, string fieldOrPropName, out T result ) {
			return Reflection.ReflectionHelpers.Get<T>( objType, instance, fieldOrPropName, out result );
		}

		public static bool Set( Object instance, string fieldOrPropName, object value ) {
			return Reflection.ReflectionHelpers.Set( instance, fieldOrPropName, value );
		}

		public static bool Set( Type objType, Object instance, string fieldOrPropName, object newValue ) {
			return Reflection.ReflectionHelpers.Set( objType, instance, fieldOrPropName, newValue );
		}


		////////////////

		public static object SafeCall( MethodInfo method, object methodContext, object[] args ) {
			return Reflection.ReflectionHelpers.SafeCall( method, methodContext, args );
		}

		public static bool RunMethod<T>( Object instance, string methodName, object[] args, out T returnVal ) {
			return Reflection.ReflectionHelpers.RunMethod<T>( instance, methodName, args, out returnVal );
		}

		public static bool RunMethod<T>( Object instance, string methodName, BindingFlags flags, object[] args, out T returnVal ) {
			return Reflection.ReflectionHelpers.RunMethod<T>( instance, methodName, flags, args, out returnVal );
		}


		////////////////

	}
}
