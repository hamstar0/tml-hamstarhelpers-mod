using System;
using System.Collections.Generic;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Internals.ClassOverrides {
	class ClassOverrides {
		private IDictionary<string, (Type OverrideType, object OverrideValue)> OverridenMembers
			= new Dictionary<string, (Type OverrideType, object OverrideValue)>();



		////////////////

		public bool TryGet<T>( string key, out T value ) {
			if( !this.OverridenMembers.ContainsKey(key) ) {
				value = default(T);
				return false;
			}

			Type expectedType = this.OverridenMembers[key].OverrideType;
			if( expectedType != typeof(T) ) {
				throw new ModHelpersException( "Mistmatched types: Found "+typeof(T).Name+", expected "+expectedType.Name );
			}

			value = (T)this.OverridenMembers[key].OverrideValue;
			return true;
		}


		public void Set<T>( string key, T value ) {
			this.OverridenMembers[key] = (typeof(T), value);
		}

		public bool Unset( string key ) {
			return this.OverridenMembers.Remove( key );
		}

		public void Clear() {
			this.OverridenMembers.Clear();
		}


		////////////////

		public bool ApplyOverrides( object obj ) {
			foreach( (string memberName, (Type overType, object overVal)) in this.OverridenMembers ) {
				if( !ReflectionHelpers.Set(obj, memberName, overVal) ) {
					return false;
				}
			}
			return true;
		}
	}
}
