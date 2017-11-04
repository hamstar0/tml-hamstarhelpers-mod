using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;


namespace HamstarHelpers.Utilities.UI {
	public class UIWebUrl : UIElement {
		public static Color DefaultColor = new Color( 32, 32, 255 );
		
		
		
		////////////////

		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }
		public string Url { get; private set; }


		////////////////

		public UIWebUrl( string label, string url, float scale=0.85f, bool large=false ) : base() {
			this.Url = url;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = UIWebUrl.DefaultColor;
			this.Append( this.TextElem );
				
			float underscore_len = Main.fontMouseText.MeasureString("_").X * scale;
			CalculatedStyle dim = this.TextElem.GetDimensions();
			int line_len = Math.Max( 1, (int)(dim.Width / underscore_len) + 1 );

			this.LineElem = new UIText( new String('_', line_len), scale, large );
			this.LineElem.TextColor = UIWebUrl.DefaultColor;
			this.Append( this.LineElem );

			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );

			this.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				try {
					System.Diagnostics.Process.Start( this.Url );
				} catch( Exception e ) {
					Main.NewText( e.Message );
				}
			};
		}


		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.TextElem.IsMouseHovering ) {
				sb.DrawString( Main.fontMouseText, this.Url, new Vector2( Main.mouseX + 24, Main.mouseY ), Color.White );
			}
		}
	}
}
