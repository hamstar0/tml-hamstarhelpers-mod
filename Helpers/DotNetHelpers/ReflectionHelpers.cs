using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		public static Type GetClassTypeFrom( string assembleName, string className ) {
			return typeof( ModLoader ).Assembly.GetType( className );
		}


		public static IEnumerable<Type> GetAllAvailableSubTypes( Type parentType ) {
			var subclasses = new List<Type>();

			foreach( var mod in ModLoader.LoadedMods ) {
				Type[] myTypes = null;
				try {
					myTypes = mod.GetType().Assembly.GetTypes();
				} catch {
					continue;
				}

				foreach( var myType in myTypes ) {
					if( myType == null || !myType.IsSubclassOf( parentType ) || myType.IsAbstract ) { continue; }
					subclasses.Add( myType );
				}
			}

			return subclasses;
		}

		/*public static IEnumerable<Type> GetAllAvailableSubTypes2( Type parentType ) {
			var modTypes = ModLoader.LoadedMods.Where( mod => mod != null ).Select( mod => mod.GetType() );
			var assemblies = modTypes.Select( modType => modType.Assembly );
			var allSubclasses = assemblies.SelectMany( assembly => assembly.GetTypes() ).Where( myType => myType != null );
			var subclasses = allSubclasses.Where( myType => myType.IsSubclassOf( parentType ) && !myType.IsAbstract );
			return subclasses;
		}

		public static IEnumerable<Type> GetAllAvailableSubTypes3( Type parentType ) {
			var modTypes = ModLoader.LoadedMods.Where( mod => mod != null ).Select( mod => mod.GetType() );
			var assemblies = modTypes.Select( modType => modType.Assembly );
			var subclasses = from assembly in assemblies
							 from type in assembly.GetTypes()
							 where type.IsSubclassOf( parentType ) && !type.IsAbstract
							 select type;
			return subclasses;
		}*/


		////////////////

		public static bool Get<T>( Object instance, string propOrFieldName, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, out val );
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
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, out val );
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

		public static bool GetField<T>( Object instance, string fieldName, out T fieldVal ) {
			fieldVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( fieldName );
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

			FieldInfo field = objType.GetField( fieldName );
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
			FieldInfo field = objType.GetField( fieldName );
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

			FieldInfo field = objType.GetField( fieldName );
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
			PropertyInfo prop = objType.GetProperty( propName );
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

			PropertyInfo prop = objType.GetProperty( propName );
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
			FieldInfo field = objtype.GetField( propName );
			PropertyInfo prop = objtype.GetProperty( propName );
			if( prop == null ) { return false; }
			
			prop.SetValue( instance, value );
			return true;
		}


		////////////////
		
		public static bool RunMethod<T>( Object instance, string methodName, object[] args, out T returnVal ) {
			returnVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			MethodInfo method = objtype.GetMethod( methodName );
			if( method == null ) { return false; }

			method.Invoke( instance, args );
			return true;
		}

		public static bool RunMethod<T>( Object instance, string methodName, BindingFlags flags, object[] args, out T returnVal ) {
			returnVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			MethodInfo method = objtype.GetMethod( methodName, flags );
			if( method == null ) { return false; }

			method.Invoke( instance, args );
			return true;
		}
	}
}
