using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Tml;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public string GetModDescriptionFromActiveMod( string mod_name ) {
			Mod mod = ModLoader.GetMod( mod_name );
			if( mod == null ) {
				return null;
			}

			var build_edit = BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			string description = (string)build_edit.GetField( "description" );

			return string.IsNullOrEmpty( description ) ? "" : description;
		}


		public string GetModDescriptionFromUI( string mod_name, ref string err ) {
			UIPanel msg_box;
			if( this.MyUI == null || !ReflectionHelpers.GetField<UIPanel>( this.MyUI, "modInfo", out msg_box ) ) {
				err = "No modInfo field.";
				return "";
			}

			string mod_desc;
			if( !ReflectionHelpers.GetField<string>( msg_box, "text", BindingFlags.NonPublic | BindingFlags.Instance, out mod_desc ) ) {
				err = "No modInfo.text field.";
				return "";
			}

			return mod_desc;
		}
	}
}
