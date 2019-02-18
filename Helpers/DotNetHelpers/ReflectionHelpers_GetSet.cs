using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		public static bool Get<T>( Object instance, string propOrFieldName, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, ReflectionHelpers.MostAccess, out val );
			}
			return true;
		}

		public static bool Get<T>( Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, flags, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, flags, out val );
			}
			return true;
		}

		public static bool Get<T>( Type objType, Object instance, string propOrFieldName, out T val ) {
			if( !ReflectionHelpers.GetField<T>( objType, instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, ReflectionHelpers.MostAccess, out val );
			}
			return true;
		}

		public static bool Get<T>( Type objType, Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( objType, instance, propOrFieldName, flags, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, flags, out val );
			}
			return true;
		}


		////////////////
		
		public static bool Set( Object instance, string propOrFieldName, object value ) {
			if( !ReflectionHelpers.SetField(instance, propOrFieldName, value) ) {
				return ReflectionHelpers.SetProperty( instance, propOrFieldName, value );
			}
			return true;
		}
	}
}
