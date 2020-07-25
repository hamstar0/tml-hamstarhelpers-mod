using System;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Items.NoteItem.UI {
	/// <summary></summary>
	public partial class UINote : UIElement {
		/// @private
		public override void OnInitialize() {
			CalculatedStyle dim;

			this.TitleElem = new UIText( this.TitleText, 1f, true );
			this.TitleElem.Left.Set( 0f, 0.5f );
			this.TitleElem.Top.Set( 128f, 0f );
			this.Append( this.TitleElem );

			this.BodyElem = new UIText( this.BodyText, 1f, false );
			this.BodyElem.Left.Set( 0f, 0.5f );
			this.BodyElem.Top.Set( 208f, 0f );
			this.Append( this.BodyElem );
			
			//

			this.PrevPageElem = new UIText( "Prev Page", 1f, false );
			this.PrevPageElem.Recalculate();
			dim = this.PrevPageElem.GetDimensions();
			this.PrevPageElem.Left.Set( -128f - (dim.Width * 0.5f), 0.5f );
			this.PrevPageElem.Top.Set( 176f, 0f );
			this.Append( this.PrevPageElem );

			this.NextPageElem = new UIText( "Next Page", 1f, false );
			this.NextPageElem.Recalculate();
			dim = this.NextPageElem.GetDimensions();
			this.NextPageElem.Left.Set( 128f - (dim.Width * 0.5f), 0.5f );
			this.NextPageElem.Top.Set( 176f, 0f );
			this.Append( this.NextPageElem );

			this.PageNumElem = new UIText( this.CurrentPage+" / "+(this.Pages.Length-1), 1f, false );
			this.PageNumElem.Recalculate();
			dim = this.NextPageElem.GetDimensions();
			this.PageNumElem.Left.Set( dim.Width * -0.5f, 0.5f );
			this.PageNumElem.Top.Set( 176f, 0f );
			this.Append( this.PageNumElem );

			//

			this.PrevPageElem.OnClick += ( _, __ ) => {
				Main.LocalPlayer.mouseInterface = true;
				this.GoToPage( this.CurrentPage - 1 );
			};
			this.NextPageElem.OnClick += ( _, __ ) => {
				Main.LocalPlayer.mouseInterface = true;
				this.GoToPage( this.CurrentPage + 1 );
			};

			//

			this.SetTitle( this.TitleText );
			this.GoToPage( this.CurrentPage );
		}
	}
}
