using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Services.Tml {
	public class BuildPropertiesEditor {
		private static Type GetBuildPropertiesClassType() {
			//IEnumerable<Type> bpClassTypes;

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
					foreach( Type t in selectMany( ass ) ) {
						if( t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties" ) {
							return t;
						}
					}
				}
				//bpClassTypes = from t in assemblies.SelectMany( selectMany )
				//				 where t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties"
				//				 select t;
			} catch( Exception e ) {
				LogHelpers.Log( "BuildPropertiesEditor.GetBuildPropertiesForModFile - " + e.ToString() );
				return (Type)null;
			}

			//if( bpClassTypes.Count() == 0 ) {
			//	return (Type)null;
			//}

			//return bpClassTypes.FirstOrDefault();
			return null;
		}


		public static BuildPropertiesEditor GetBuildPropertiesForModFile( TmodFile modfile ) {
			Type buildPropType = BuildPropertiesEditor.GetBuildPropertiesClassType();
			if( buildPropType == null ) { return (BuildPropertiesEditor)null; }

			MethodInfo method = buildPropType.GetMethod( "ReadModFile", BindingFlags.NonPublic | BindingFlags.Static );
			if( method == null ) { return (BuildPropertiesEditor)null; }

			object buildProps = method.Invoke( null, new object[] { modfile } );
			if( buildProps == null ) {
				LogHelpers.Log( "BuildProperties has changed." );
				return (BuildPropertiesEditor)null;
			}

			return new BuildPropertiesEditor( buildProps );
		}



		////////////////

		public string[] DllReferences => (string[])this.GetField( "dllReferences" );
		//public string[] ModReferences => ((object[])this.GetField( "modReferences" )).Select( m=>m.ToString() ).ToArray();
		//public string[] WeakReferences => ((object[])this.GetField( "weakReferences" ) ).Select( m=>m.ToString() ).ToArray();
		public string[] SortAfter => (string[])this.GetField( "sortAfter" );
		public string[] SortBefore => (string[])this.GetField( "sortBefore" );
		public string[] BuildIgnores => (string[])this.GetField( "buildIgnores" );
		public string Author => (string)this.GetField( "author" );
		public Version Version => (Version)this.GetField( "version" );
		public string DisplayName => (string)this.GetField( "displayName" );
		public bool NoCompile => (bool)this.GetField( "noCompile" );
		public bool HideCode => (bool)this.GetField( "hideCode" );
		public bool HideResources => (bool)this.GetField( "hideResources" );
		public bool IncludeSource => (bool)this.GetField( "includeSource" );
		public bool IncludePDB => (bool)this.GetField( "includePDB" );
		public bool EditAndContinue => (bool)this.GetField( "editAndContinue" );
		public bool Beta => (bool)this.GetField( "beta" );
		public int LanguageVersion => (int)this.GetField( "languageVersion" );
		public string Homepage => (string)this.GetField( "homepage" );
		public string Description => (string)this.GetField( "description" );
		public ModSide Side => (ModSide)this.GetField( "side" );

		public IDictionary<string, Version> ModReferences {
			get {
				var modRefs = (object[])this.GetField( "modReferences" );
				var dict = new Dictionary<string, Version>( modRefs.Length );
				string name;
				Version vers;
				
				foreach( var modRef in modRefs ) {
					if( !ReflectionHelpers.GetField( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionHelpers.GetField( modRef, "target", out vers ) ) { continue; }
					dict[name] = vers;
				}
				return dict;
			}
		}
		public IDictionary<string, Version> WeakReferences {
			get {
				var modRefs = (object[])this.GetField( "weakReferences" );
				var dict = new Dictionary<string, Version>( modRefs.Length );
				string name;
				Version vers;

				foreach( var modRef in modRefs ) {
					if( !ReflectionHelpers.GetField( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionHelpers.GetField( modRef, "target", out vers ) ) { continue; }
					dict[name] = vers;
				}
				return dict;
			}
		}

		////////////////

		internal object BuildProps;


		////////////////

		internal BuildPropertiesEditor( object buildProps ) {
			this.BuildProps = buildProps;
		}

		////////////////

		public object GetField( string propName ) {
			Type modPropsType = this.BuildProps.GetType();
			FieldInfo fieldInfo = modPropsType.GetField( propName, BindingFlags.NonPublic | BindingFlags.Instance );
			if( fieldInfo == null ) { return null; }

			return fieldInfo.GetValue( this.BuildProps );
		}
	}
}
