using HamstarHelpers.Classes.ModTagDefinitions;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public void LayoutCategoryButtons() {
			float top = this.PositionY - 2;
			float x = this.PositionXCenterOffset;
			float y = top;

			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.SetMenuSpacePosition( x, y );

				y += UICategoryMenuButton.ButtonHeight;
				if( y >= (UIModTagsInterface.CategoryPanelHeight + top - 2) ) {
					y = top;
					x += UICategoryMenuButton.ButtonWidth - 2;
				}
			}
		}

		public void LayoutTagButtonsByCategory() {
			float x, y;
			UITagMenuButton tagButton;
			float top = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
			float maxY = UIModTagsInterface.TagsPanelHeight + top - UIResetTagsMenuButton.ButtonHeight - 4;
			IReadOnlyList<ModTagDefinition> tags = this.Manager.MyTags;

			IEnumerable<IGrouping<string, ModTagDefinition>> groups = tags.GroupBy( tagDef => tagDef.Category );

			foreach( IGrouping<string, ModTagDefinition> group in groups ) {
				x = this.PositionXCenterOffset;
				y = top;

				foreach( ModTagDefinition tagDef in group ) {
					if( !this.TagButtons.TryGetValue( tagDef.Tag, out tagButton ) ) {
						LogHelpers.AlertOnce( "Missing tag button "+tagDef.Tag );
						continue;
					}

					tagButton.SetMenuSpacePosition( x, y );

					tagButton.PutAway();
					if( group.Key == this.CurrentCategory ) {
						tagButton.TakeOut();
					}

					y += UITagMenuButton.ButtonHeight;
					if( y >= maxY ) {
						y = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
						x += UITagMenuButton.ButtonWidth - 2;
					}
				}
			}
		}

		public void LayoutTagButtonsByOnState() {
			float top = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
			float x = this.PositionXCenterOffset;
			float y = top;
			float maxY = UIModTagsInterface.TagsPanelHeight + top - UIResetTagsMenuButton.ButtonHeight - 4;
			
			foreach( UITagMenuButton tagButton in this.TagButtons.Values ) {
				if( tagButton.TagState != 1 ) {
					continue;
				}

				tagButton.SetMenuSpacePosition( x, y );
				tagButton.TakeOut();

				y += UITagMenuButton.ButtonHeight;
				if( y >= maxY ) {
					y = top;
					x += UITagMenuButton.ButtonWidth;
				}
			}
		}


		////////////////

		public Vector2 GetTagControlsTopLeftPositionOffset() {
			float x = this.PositionXCenterOffset;
			float y = this.PositionY
				+ UIModTagsInterface.CategoryPanelHeight
				+ UIModTagsInterface.TagsPanelHeight
				+ 4
				- UIResetTagsMenuButton.ButtonHeight;

			return new Vector2( x, y );
		}

		public Rectangle GetCategoryPanelRectangle() {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			int wid = UIModTagsInterface.PanelWidth;
			return new Rectangle( x, y, wid, UIModTagsInterface.CategoryPanelHeight );
		}

		public Rectangle GetTagsPanelRectangle() {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			int wid = UIModTagsInterface.PanelWidth;
			return new Rectangle( x, y + UIModTagsInterface.CategoryPanelHeight + 2, wid, UIModTagsInterface.TagsPanelHeight );
		}
	}
}
