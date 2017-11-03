using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;

namespace HamstarHelpers.Utilities.UI {
	public class UIWebUrl : UIElement {
		public readonly static Color Default = new Color( 32, 32, 255 );


		////////////////

		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }


		////////////////

		public UIWebUrl( string label, string url, float scale=0.85f, bool large=false ) : base() {
			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = new Color( 32, 32, 255 );
			this.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
Main.NewText( "support" );
			};

			this.LineElem = new UIText( new String('_', label.Length/2), scale, large );
			this.LineElem.TextColor = new Color( 32, 32, 255 );
			
			this.Append( this.TextElem );
			this.Append( this.LineElem );
		}
	}
}
