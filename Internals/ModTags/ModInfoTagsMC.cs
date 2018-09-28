using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Dialogs;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
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
		
		public UISubmitUpdateButton SubUpButton;

		public string ModName = "";

		////////////////

		protected override string UIName => "UIModInfo";
		protected override string ContextName => "Mod Info";



		////////////////

		private ModInfoTagsMenuContext() {
			this.InitializeBase();
			this.InitializeTagButtons( false );
			this.InitializeContext();
			this.InitializeSubUpButton();
			this.InitializeHoverText();
		}


		////////////////

		public override void OnTagStateChange( UIModTagButton tag_button ) {
			this.SubUpButton.UpdateEnableState();
		}


		////////////////

		internal void SubmitTags() {
			if( this.ModName == "" ) {
				throw new Exception( "Invalid mod name." );
			}

			var myui = this.MyUI;

			Action<string> on_success = delegate ( string output ) {
				var prompt = new UIPromptPanel( UITheme.Vanilla, 600, 100, output, ()=>{} );

				myui.Append( prompt );
				myui.Recalculate();

				MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Submit + Update", _=>{}, ui => {
					prompt.Remove();
					this.SubUpButton.Lock();
					ui.Recalculate();
				} );

				ErrorLogger.Log( "Mod info submit result: " + output );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				var prompt = new UIPromptPanel( UITheme.Vanilla, 600, 100, "Error: "+output, ()=>{} );

				myui.Append( prompt );
				myui.Recalculate();

				MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Submit + Update", _ => { }, ui => {
					prompt.Remove();
					this.SubUpButton.Unlock();
					ui.Recalculate();
				} );

				Main.NewText( "Mod info submit error: " + e.Message, Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			PostModInfo.SubmitModInfo( this.ModName, this.GetTagsOfState(1), on_success, on_fail );

			this.SubUpButton.Lock();
		}
	}
}
