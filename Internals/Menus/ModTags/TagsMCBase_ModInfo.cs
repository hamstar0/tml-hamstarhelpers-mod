using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Tml;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public string GetModDescriptionFromActiveMod( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				return null;
			}

			var buildEdit = BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			string description = (string)buildEdit.GetField( "description" );

			return string.IsNullOrEmpty( description ) ? "" : description;
		}


		public string GetModDescriptionFromUI( string modName, ref string err ) {
			UIPanel msgBox;
			if( this.MyUI == null || !ReflectionHelpers.GetField<UIPanel>( this.MyUI, "modInfo", out msgBox ) ) {
				err = "No modInfo field.";
				return "";
			}

			string modDesc;
			if( !ReflectionHelpers.GetField<string>( msgBox, "text", BindingFlags.NonPublic | BindingFlags.Instance, out modDesc ) ) {
				err = "No modInfo.text field.";
				return "";
			}

			return modDesc;
		}
	}
}
