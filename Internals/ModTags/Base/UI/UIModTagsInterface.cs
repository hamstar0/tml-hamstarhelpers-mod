using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public static int PanelWidth { get; private set; } = 204;
		public static int CategoryPanelHeight { get; private set; } = 82;
		public static int TagsPanelHeight { get; private set; } = 416;



		////////////////

		public string CurrentCategory { get; protected set; }

		protected IDictionary<string, UICategoryMenuButton> CategoryButtons;
		protected IDictionary<string, UITagMenuButton> TagButtons;


		////////////////

		protected int PositionXCenterOffset;
		protected int PositionY;

		protected ModTagsManager Manager;

		protected UIResetTagsMenuButton ResetButton;



		////////////////

		public UIModTagsInterface( UITheme theme, ModTagsManager manager )
				: base( theme, true ) {
			this.Manager = manager;

			this.PositionXCenterOffset = -402;
			this.PositionY = 42;
			this.InitializeControls();

			this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		public void OnTagStateChange( string tagName, int state ) {
			this.RefreshButtonEnableStates();
		}


		////////////////

		public ISet<string> GetTagsWithGivenState( int state, string category=null ) {
			ISet<string> tags = new HashSet<string>();

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				if( category != null && this.Manager.MyTagMap[tagName].Category != category ) {
					continue;
				}

				if( tagButton.TagState == state ) {
					tags.Add( tagName );
				}
			}
			return tags;
		}


		////////////////

		public override void Recalculate() {
			float x = ((float)Main.screenWidth * 0.5f) + this.PositionXCenterOffset;

			this.Top.Set( this.PositionY, 0f );
			this.Left.Set( x, 0f );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			Rectangle rect1 = this.GetCategoryPanelRectangle();
			Rectangle rect2 = this.GetTagsPanelRectangle();

			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect1, 2 );
			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect2, 2 );
		}
	}
}
