using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.TML;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public static int PanelWidth { get; private set; } = 192;
		public static int CategoryPanelHeight { get; private set; } = 128;
		public static int TagsPanelHeight { get; private set; } = 368;

		public static int CategoryButtonWidth { get; private set; } = 160;
		public static int CategoryButtonHeight { get; private set; } = 32;



		////////////////

		protected readonly IDictionary<string, UIMenuButton> CategoryButtons = new Dictionary<string, UIMenuButton>();
		protected readonly IDictionary<string, UIModTagMenuButton> TagButtons = new Dictionary<string, UIModTagMenuButton>();

		////////////////

		protected int PositionXCenterOffset;
		protected int PositionY;

		protected ModTagsManager Manager;

		protected UIModTagsResetMenuButton ResetButton;



		////////////////

		public UIModTagsInterface( UITheme theme, ModTagsManager manager, bool canExcludeTags )
				: base( theme, true ) {
			this.Manager = manager;

			this.PositionXCenterOffset = -400 + 4;
			this.PositionY = 64 + 4;
			this.InitializeControls( manager.MyTags, canExcludeTags );

			this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		public void SetCurrentMod( string modName, ISet<string> tags ) {
			bool hasNetTags = tags.Count > 0;

			foreach( var kv in this.TagButtons ) {
				string tagName = kv.Key;
				UIModTagMenuButton button = kv.Value;
				bool hasTag = tags.Contains( tagName );

				if( !hasNetTags ) {
					button.Enable();
				}

				if( tagName == "Low Effort" ) {
					if( hasTag ) {
						button.SetTagState( 1 );
					} else {
						BuildPropertiesViewer viewer = BuildPropertiesViewer.GetBuildPropertiesForActiveMod( modName );
						string desc = viewer.Description ?? "";

						if( viewer == null || string.IsNullOrEmpty( desc ) ) {
							if( !ModMenuHelpers.GetModDescriptionFromCurrentMenuUI( out desc ) ) {
								desc = "";
							}
						}

						if( desc.Contains( "Modify this file with a description of your mod." ) ) {
							button.SetTagState( 1 );
						}
					}
				} else {
					button.SetTagState( hasTag ? 1 : 0 );
				}
			}
		}


		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( kv.Value.TagState == state ) {
					tags.Add( kv.Key );
				}
			}
			return tags;
		}

		
		public override void Recalculate() {
			float x = ((float)Main.screenWidth * 0.5f) - this.PositionXCenterOffset;

			this.Top.Set( this.PositionY, 0f );
			this.Left.Set( x, 0f );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			var rect1 = new Rectangle( x, y, UIModTagsInterface.PanelWidth, UIModTagsInterface.CategoryPanelHeight );
			var rect2 = new Rectangle( x, y+132, UIModTagsInterface.PanelWidth, UIModTagsInterface.TagsPanelHeight );

			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect1, 2 );
			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect2, 2 );

			base.Draw( sb );
		}
	}
}
