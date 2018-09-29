using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();


		////////////////

		public static string GetModNameFromUI( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( ui_localmod_field == null ) {
				LogHelpers.Log( "No 'localMod' field in " + ui_type );
				return null;
			}
			
			object localmod = ui_localmod_field.GetValue( ui );
			Type localmod_type = localmod.GetType();
			FieldInfo localmod_modfile_field = localmod_type.GetField( "modFile", BindingFlags.Public | BindingFlags.Instance );
			if( localmod_modfile_field == null ) {
				LogHelpers.Log( "No 'modFile' field in " + localmod_type );
				return null;
			}

			var modfile = (TmodFile)localmod_modfile_field.GetValue( localmod );
			if( modfile == null ) {
				LogHelpers.Log( "Empty 'mod' field" );
				return null;
			}

			return modfile.name;
		}


		public static void Initialize() {
			new ModInfoTagsMenuContext();
		}



		////////////////
		
		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;
		public UIInfoDisplay InfoDisplay;

		public string ModName = "";

		////////////////

		public override string UIName => "UIModInfo";
		public override string ContextName => "Mod Info";



		////////////////

		private ModInfoTagsMenuContext() {
			this.InitializeBase();
			this.InitializeTagButtons( false );
			this.InitializeContext();
			this.InitializeInfoDisplay();
			this.InitializeButtons();
			this.InitializeHoverText();
		}


		////////////////

		public override void OnTagStateChange( UITagButton tag_button ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
		}
	}
}
