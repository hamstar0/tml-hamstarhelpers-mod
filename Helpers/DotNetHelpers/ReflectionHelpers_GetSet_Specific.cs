using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		public static bool GetField<T>( Object instance, string fieldName, out T fieldVal ) {
			fieldVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( fieldName, ReflectionHelpers.MostAccess );
			if( field == null ) { return false; }
			
			fieldVal = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Object instance, string fieldName, BindingFlags flags, out T fieldVal ) {
			fieldVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( fieldName, flags );
			if( field == null ) { return false; }

			fieldVal = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Type objType, Object instance, string fieldName, out T fieldVal ) {
			fieldVal = default( T );

			FieldInfo field = objType.GetField( fieldName, ReflectionHelpers.MostAccess );
			if( field == null ) { return false; }

			fieldVal = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Type objType, Object instance, string fieldName, BindingFlags flags, out T fieldVal ) {
			fieldVal = default( T );

			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }

			fieldVal = (T)field.GetValue( instance );
			return true;
		}

		////

		public static bool SetField( Object instance, string fieldName, object value ) {
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			FieldInfo field = objType.GetField( fieldName, ReflectionHelpers.MostAccess );
			if( field == null ) { return false; }
			
			field.SetValue( instance, value );
			return true;
		}

		public static bool SetField( Object instance, string fieldName, BindingFlags flags, object value ) {
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }
			
			field.SetValue( instance, value );
			return true;
		}

		public static bool SetField( Type objType, Object instance, string fieldName, object value ) {
			if( instance == null ) { return false; }

			FieldInfo field = objType.GetField( fieldName, ReflectionHelpers.MostAccess );
			if( field == null ) { return false; }

			field.SetValue( instance, value );
			return true;
		}

		public static bool SetField( Type objType, Object instance, string fieldName, BindingFlags flags, object value ) {
			if( instance == null ) { return false; }

			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }

			field.SetValue( instance, value );
			return true;
		}


		////////////////

		public static bool GetProperty<T>( Object instance, string propName, out T propVal ) {
			propVal = default( T );
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			PropertyInfo prop = objType.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Object instance, string propName, BindingFlags flags, out T propVal ) {
			propVal = default( T );
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			PropertyInfo prop = objType.GetProperty( propName, flags );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Type objType, Object instance, string propName, out T propVal ) {
			propVal = default( T );

			PropertyInfo prop = objType.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Type objType, Object instance, string propName, BindingFlags flags, out T propVal ) {
			propVal = default( T );

			PropertyInfo prop = objType.GetProperty( propName, flags );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		////

		public static bool SetProperty( Object instance, string propName, object value ) {
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }
			
			prop.SetValue( instance, value );
			return true;
		}
	}
}
