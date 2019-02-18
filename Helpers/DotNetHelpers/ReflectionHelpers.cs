using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false, Inherited = true )]
	public class NullableAttribute : Attribute { }





	public partial class ReflectionHelpers {
		public readonly static BindingFlags MostAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;



		////////////////

		public static Type GetTypeFromMod( Mod mod, string typeName ) {
			return mod.GetType().Assembly.GetType( typeName );
		}

		public static IList<Type> GetTypesFromAssembly( string assemblyName, string typeName ) {
			var typeList = new List<Type>();

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Func<Assembly, IEnumerable<Type>> selectMany = delegate ( Assembly a ) {
					try {
						return a.GetTypes();
					} catch {
						return new List<Type>();
					}
				};

				foreach( var ass in assemblies ) {
					if( ass.GetName().Name != assemblyName ) { continue; }

					foreach( Type t in selectMany( ass ) ) {
						if( t.Name == typeName ) {
							typeList.Add( t );
						}
					}
					break;
				}

			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}

			return typeList;
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
