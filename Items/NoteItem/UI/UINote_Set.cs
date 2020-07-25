using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Items.NoteItem.UI {
	/// <summary></summary>
	public partial class UINote : UIElement {
		/// <summary></summary>
		/// <param name="text"></param>
		public void SetTitle( string text ) {
			this.TitleElem.SetText( text, 1f, true );
			this.TitleElem.Recalculate();

			CalculatedStyle dim = this.TitleElem.GetDimensions();
			this.TitleElem.Left.Set( dim.Width * -0.5f, 0.5f );

			this.Recalculate();
		}

		/// <summary></summary>
		/// <param name="pages"></param>
		public void SetPages( string[] pages ) {
			if( pages.Length == 0 ) {
				throw new ModHelpersException( "Cannot load empty note." );
			}

			this.Pages = pages;
			this.GoToPage( 0 );
		}

		////

		/// <summary></summary>
		/// <param name="text"></param>
		protected void SetBody( string text ) {
			this.BodyElem.SetText( text, 1f, false );
			this.BodyElem.Recalculate();

			CalculatedStyle dim = this.BodyElem.GetDimensions();
			this.BodyElem.Left.Set( dim.Width * -0.5f, 0.5f );

			this.Recalculate();
		}


		////////////////

		/// <summary></summary>
		/// <param name="pageNum"></param>
		public void GoToPage( int pageNum ) {
			this.CurrentPage = (int)MathHelper.Clamp( pageNum, 0, this.Pages.Length - 1 );

			this.SetBody( this.Pages[this.CurrentPage] );

			this.PageNumElem.SetText(
				(this.CurrentPage+1)+" / "+this.Pages.Length,
				1f,
				false
			);
			this.PageNumElem.Recalculate();

			CalculatedStyle pageDim = this.PageNumElem.GetDimensions();
			this.PageNumElem.Left.Set( pageDim.Width * -0.5f, 0.5f );

			if( this.CurrentPage == 0 ) {
				this.PrevPageElem.TextColor = Color.Gray;
			} else {
				this.PrevPageElem.TextColor = Color.White;
			}

			if( this.CurrentPage >= this.Pages.Length - 1 ) {
				this.NextPageElem.TextColor = Color.Gray;
			} else {
				this.NextPageElem.TextColor = Color.White;
			}
		}
	}
}
