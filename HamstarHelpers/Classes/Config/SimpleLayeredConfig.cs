using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.DotNET.Reflection;


namespace HamstarHelpers.Classes.Config {
	/// <summary>
	/// Allows programmatically storing config field/property values that are not saved to the .json file, and do not affect existing user
	/// settings. In short, you can now use your configs for data API use.
	/// </summary>
	abstract public class SimpleLayeredConfig : ModConfig {
		private IDictionary<string, object> Overrides = new ConcurrentDictionary<string, object>();



		////////////////

		/// <summary>
		/// Gets a given config field by its string name. Recommend using `nameof` to catch mismatched fields/properties at compile time.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propName"></param>
		/// <returns></returns>
		public T Get<T>( string propName ) {
			if( !this.Overrides.TryGetValue( propName, out object val ) ) {
				return this.GetBase<T>( propName );
			}

			if( val.GetType() != typeof( T ) ) {
				throw new ModHelpersException( "Invalid type (" + typeof( T ).Name + ") of property " + propName + "." );
			}

			return (T)val;
		}
		
		/// <summary>
		/// Gets a given config field by its string name. Recommend using `nameof` to catch mismatched fields/properties at compile time.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propName"></param>
		/// <param name="isOverride">Reports if the field/property has been overridden.</param>
		/// <returns></returns>
		public T Get<T>( string propName, out bool isOverride ) {
			if( !this.Overrides.TryGetValue( propName, out object val ) ) {
				isOverride = false;
				return this.GetBase<T>( propName );
			}

			if( val.GetType() != typeof( T ) ) {
				throw new ModHelpersException( "Invalid type (" + typeof( T ).Name + ") of property " + propName + "." );
			}

			isOverride = true;
			return (T)val;
		}

		////

		private T GetBase<T>( string propName ) {
			if( !ReflectionLibraries.Get( this, propName, out T myval ) ) {
				throw new ModHelpersException( "Invalid property " + propName + " of type " + typeof( T ).Name );
			}

			return myval;
		}


		////////////////

		/// <summary>
		/// Sets a field or property to have a new (layered) value. Preserves the underlying ModConfig user data by storing into a separate
		/// layer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propName"></param>
		/// <param name="value"></param>
		public void SetLayered<T>( string propName, T value ) {
			if( !ReflectionLibraries.Get( this, propName, out T _ ) ) {
				throw new ModHelpersException( "Invalid property " + propName + " of type " + typeof( T ).Name );
			}
			this.Overrides[propName] = value;
		}
	}
}
