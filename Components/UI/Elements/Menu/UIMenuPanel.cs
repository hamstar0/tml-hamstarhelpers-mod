﻿using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Components.UI.Elements.Menu {
	/// <summary>
	/// Defines a main menu info panel UI element.
	/// </summary>
	internal class UIMenuPanel : UIPanel {
		protected float XCenterOffset;
		protected float YPos;

		////////////////

		public UITheme Theme { get; protected set; }



		////////////////

		public UIMenuPanel( UITheme theme, float width, float height, float xCenterOffset, float y ) {
			this.Theme = theme;

			this.Width.Set( width, 0f );
			this.Height.Set( height, 0f );

			this.XCenterOffset = xCenterOffset;
			this.YPos = y;

			this.RecalculatePos();
			this.RefreshTheme();
		}


		////////////////

		public virtual void RecalculatePos() {
			this.Left.Set( ((float)Main.screenWidth / 2f) + this.XCenterOffset, 0f );
			this.Top.Set( this.YPos, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}


		////////////////

		public virtual void RefreshTheme() {
			this.Theme.ApplyPanel( this );
		}
	}
}
