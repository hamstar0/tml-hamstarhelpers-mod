using System;
using HamstarHelpers.Libraries.Debug;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>Implements a scrollbar designed to hide like it's parent element.</summary>
	public class UIHideableScrollbar : UIScrollbar {
		/// <summary></summary>
		public static bool IsScrollbarHidden( int innerAreaHeight, UIElement container ) {
			int listHeight = (int)container.Height.Pixels - 8;
			
			return innerAreaHeight < listHeight;
		}



		////////////////

		private UIList List;

		/// <summary></summary>
		public bool IsHidden;



		////////////////

		/// <summary></summary>
		public UIHideableScrollbar( UIList list, bool isHidden ) {
			this.List = list;
			this.IsHidden = isHidden;
		}

		////////////////
		
		/// @private
		public override void Draw( SpriteBatch sb ) {
			this.IsHidden = UIHideableScrollbar.IsScrollbarHidden(
				(int)this.List.Height.Pixels,
				this.List.Parent
			);

			if( !this.IsHidden ) {
				base.Draw( sb );
			}
		}
	}
}
