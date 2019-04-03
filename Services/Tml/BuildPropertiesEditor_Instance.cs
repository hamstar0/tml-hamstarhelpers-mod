using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.Tml {
	public partial class BuildPropertiesEditor {
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
				var modRefsRaw = (object)this.GetField( "modReferences" );
				int length;

				if( !ReflectionHelpers.Get( modRefsRaw, "Length", out length ) ) {
					//throw new HamstarException( "!ModHelpers.BuildPropertiesEditor.ModReferences - Invalid modReferences" );
					throw new HamstarException( "Invalid modReferences" );
				}
				
				var dict = new Dictionary<string, Version>( length );
				string name;
				Version vers;
				
				for( int i=0; i<length; i++ ) {
					object modRef;
					if( !ReflectionHelpers.RunMethod( modRefsRaw, "GetValue", new object[] { i }, out modRef ) ) {
						//throw new HamstarException( "!ModHelpers.BuildPropertiesEditor.ModReferences - Invalid modReference array value "+i );
						throw new HamstarException( "Invalid modReference array value " + i );
					}

					if( !ReflectionHelpers.Get( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionHelpers.Get( modRef, "target", out vers ) ) { continue; }
					dict[ name ] = vers;
				}
				return dict;
			}
		}

		public IDictionary<string, Version> WeakReferences {
			get {
				var modRefsRaw = (object)this.GetField( "modReferences" );
				int length;

				if( !ReflectionHelpers.Get( modRefsRaw, "Length", out length ) ) {
					//throw new HamstarException( "!ModHelpers.BuildPropertiesEditor.WeakReferences - Invalid modReferences" );
					throw new HamstarException( "Invalid modReferences" );
				}

				var dict = new Dictionary<string, Version>( length );
				string name;
				Version vers;

				for( int i = 0; i < length; i++ ) {
					object modRef;
					if( !ReflectionHelpers.RunMethod( modRefsRaw, "GetValue", new object[] { i }, out modRef ) ) {
						//throw new HamstarException( "!ModHelpers.BuildPropertiesEditor.WeakReferences - Invalid modReference array value " + i );
						throw new HamstarException( "Invalid modReference array value " + i );
					}

					if( !ReflectionHelpers.Get( modRef, "mod", out name ) ) { continue; }
					if( !ReflectionHelpers.Get( modRef, "target", out vers ) ) { continue; }
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
