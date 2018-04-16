using HamstarHelpers.DebugHelpers;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.TmlHelpers {
	[System.Obsolete( "use TmlHelpers.BuildPropertiesEditor", true )]
	public class BuildPropertiesInterface {
		public static BuildPropertiesInterface GetBuildPropertiesForModFile( TmodFile modfile ) {
			var editor = BuildPropertiesEditor.GetBuildPropertiesForModFile( modfile );
			return new BuildPropertiesInterface( editor );
		}


		////////////////

		private BuildPropertiesEditor Editor;


		////////////////

		private BuildPropertiesInterface( BuildPropertiesEditor editor ) {
			this.Editor = editor;
		}

		public object GetField( string prop_name ) {
			return this.Editor.GetField( prop_name );
		}
	}
}
