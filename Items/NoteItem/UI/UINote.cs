using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Items.NoteItem.UI {
	/// <summary></summary>
	public partial class UINote : UIElement {
		/// <summary></summary>
		protected UIText TitleElem;
		/// <summary></summary>
		protected UIText BodyElem;

		/// <summary></summary>
		protected UIText PrevPageElem;
		/// <summary></summary>
		protected UIText NextPageElem;
		/// <summary></summary>
		protected UIText PageNumElem;


		////////////////

		/// <summary></summary>
		public string TitleText { get; private set; }
		/// <summary></summary>
		public string BodyText { get; private set; }

		/// <summary></summary>
		public int CurrentPage { get; private set; }
		/// <summary></summary>
		public string[] Pages { get; private set; }



		////////////////

		/// <summary></summary>
		/// <param name="title"></param>
		/// <param name="pages"></param>
		public UINote( string title, string[] pages ) : base() {
			if( pages.Length == 0 ) {
				throw new ModHelpersException( "Cannot load empty note." );
			}

			this.Width.Set( 0f, 1f );
			this.Height.Set( 0f, 1f );

			this.TitleText = title;
			this.BodyText = pages[0];
			this.Pages = pages;
			this.CurrentPage = 0;
		}


		////////////////

		/// @private
		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( this.PrevPageElem.ContainsPoint(Main.MouseScreen) || this.NextPageElem.ContainsPoint(Main.MouseScreen) ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}
}
