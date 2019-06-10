﻿using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Tml;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public string GetModDataFromActiveMod( string modName, string fieldName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				return null;
			}

			var buildEdit = BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			string data = (string)buildEdit.GetField( fieldName );

			return string.IsNullOrEmpty( data ) ? "" : data;
		}


		public string GetModDescriptionFromUI( string modName, ref string err ) {
			UIPanel msgBox;
			if( this.MyUI == null || !ReflectionHelpers.Get( this.MyUI, "modInfo", out msgBox ) ) {
				err = "No modInfo field.";
				return "";
			}

			string modDesc;
			if( !ReflectionHelpers.Get( msgBox, "text", out modDesc ) ) {
				err = "No modInfo.text field.";
				return "";
			}

			return modDesc;
		}
	}
}
