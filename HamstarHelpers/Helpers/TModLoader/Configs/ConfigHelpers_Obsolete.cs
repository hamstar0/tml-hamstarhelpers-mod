using HamstarHelpers.Classes.Errors;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.TModLoader.Configs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to configs (ModConfig, primarily).
	/// </summary>
	public partial class ConfigHelpers {
		/*
		/// <summary>
		/// Combines one ModConfig instance into another, prioritizing non-default values of the latter. Any collections settings are
		/// merged.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="to"></param>
		/// <param name="fro"></param>
		/// <returns></returns>
		public static bool MergeConfigsAndTheirCollections<T>( T to, T fro ) where T : ModConfig {
			T template = (T)Activator.CreateInstance(
				typeof( T ),
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);
			if( template == null ) {
				return false;
			}

			object toVal, froVal, tempVal;

			foreach( MemberInfo memb in typeof( T ).GetMembers() ) {
				if( !ReflectionHelpers.Get( to, memb.Name, out toVal ) ) {
					return false;
				}
				if( !ReflectionHelpers.Get( fro, memb.Name, out froVal ) ) {
					return false;
				}
				if( !ReflectionHelpers.Get( template, memb.Name, out tempVal ) ) {
					return false;
				}

				if( froVal != tempVal ) {
					froVal = ConfigHelpers.CombineConfigValues( toVal, froVal );

					ReflectionHelpers.Set( to, memb.Name, fro );
				}
			}

			return true;
		}

		////

		private static object CombineConfigValues( object a, object b ) {
			if( a.GetType() != b.GetType() ) {
				throw new ModHelpersException( "Mismatched config value types." );
			}
			if( a == null ) {
				return b;
			}
			if( b == null ) {
				return a;
			}
			if( !(b is ICollection) ) {
				return b;
			}

			IEnumerable<object> rawCombinedCollection = ( (ICollection)a ).Cast<object>();
			rawCombinedCollection = rawCombinedCollection.Concat( ( (ICollection)b ).Cast<object>() );
			if( rawCombinedCollection.Count() == 0 ) {
				return b;
			}

			Type collectionType = b.GetType();
			Type collectionGenericType = rawCombinedCollection.First().GetType();

			MethodInfo castMethod = rawCombinedCollection.GetType().GetMethod( "Cast" );
			castMethod = castMethod.MakeGenericMethod( collectionGenericType );

			var typedCombinedCollection = (IEnumerable)castMethod.Invoke( rawCombinedCollection, new object[] { } );

			return Activator.CreateInstance( collectionType, new object[] { typedCombinedCollection } );
		}*/


		/*public static ModConfig LoadConfig( Type modConfigType ) {
			var config = Activator.CreateInstance( modConfigType ) as ModConfig;
			if( config == null ) {
				throw new TypeInitializationException( modConfigType.FullName, new Exception( "Not a ModConfig subclass." ) );
			}

			object _;
			ReflectionHelpers.RunMethod( modConfigType, config, "Load", new object[] { config }, out _ );

			return config;
		}

		public static void ResetConfig( ModConfig config ) {
			object _;
			ReflectionHelpers.RunMethod( config.GetType(), config, "Reset", new object[] { config }, out _ );
		}

		public static void SaveConfig( ModConfig config ) {
			object _;
			ReflectionHelpers.RunMethod( config.GetType(), config, "Save", new object[] { config }, out _ );
		}


		////////////////

		public static T GetValue( ModConfig config, string fieldOrPropertyName ) {

		}*/
	}
}
