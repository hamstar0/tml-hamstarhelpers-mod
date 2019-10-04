using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers {
		public static ReflectionHelpers Instance => ModHelpersMod.Instance?.ReflectionHelpers;



		////////////////

		private IDictionary<string, Type> TypeMap = new ConcurrentDictionary<string, Type>();
		private IDictionary<string, Assembly> AssMap = new ConcurrentDictionary<string, Assembly>();
		private IDictionary<string, IDictionary<string, IList<Type>>> AssClassTypeMap
			= new ConcurrentDictionary<string, IDictionary<string, IList<Type>>>();
		private IDictionary<string, IDictionary<string, MemberInfo>> MemberMap
			= new ConcurrentDictionary<string, IDictionary<string, MemberInfo>>();
		private IDictionary<string, IDictionary<string, IDictionary<int, MethodDefinition>>> MethodDefMap
			= new ConcurrentDictionary<string, IDictionary<string, IDictionary<int, MethodDefinition>>>();



		////////////////
		
		internal ReflectionHelpers() { }


		////////////////
		
		internal MemberInfo GetCachedMemberInfo( Type classType, string memberName ) {
			string className = classType.FullName;

			if( !this.MemberMap.ContainsKey( className ) ) {
				this.MemberMap[className] = new Dictionary<string, MemberInfo>();
			}

			if( !this.MemberMap[className].ContainsKey( memberName ) ) {
				MemberInfo result = (MemberInfo)classType.GetField( memberName, ReflectionHelpers.MostAccess );
				if( result == null ) {
					result = (MemberInfo)classType.GetProperty( memberName, ReflectionHelpers.MostAccess );
				}
				//if( result == null ) {
				//	result = (MemberInfo)classType.GetMethod( memberName, ReflectionHelpers.MostAccess );
				//}

				this.MemberMap[className][memberName] = result;
			}

			return this.MemberMap[className][memberName];
		}

		internal MethodDefinition GetCachedMethodDefinition( Object instance, string methodName ) {
			Type classType = instance.GetType();
			string className = classType.FullName;

			if( !this.MethodDefMap.ContainsKey( className ) ) {
				this.MethodDefMap[className] = new Dictionary<string, IDictionary<int, MethodDefinition>>();
			}

			if( !this.MethodDefMap[className].ContainsKey( methodName ) ) {
				this.MethodDefMap[className][methodName] = new Dictionary<int, MethodDefinition>();
			}

			int code = instance?.GetHashCode() ?? 0;
			if( !this.MethodDefMap[className][methodName].ContainsKey( code ) ) {
				MethodInfo method = classType.GetMethod( methodName, ReflectionHelpers.MostAccess );

				if( method == null ) {
					this.MethodDefMap[className][methodName][code] = null;
				} else {
					this.MethodDefMap[className][methodName][code] = new MethodDefinition( method );
				}
			}

			return this.MethodDefMap[className][methodName][code];
		}
	}
}
