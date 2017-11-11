using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use UIHelpers.UI.UIWebUrl", true )]
	public class UIWebUrl : UIElement {
		[System.Obsolete( "use UIHelpers.UI.UIWebUrl.DefaultColor", true )]
		public static Color DefaultColor {
			get { return UIHelpers.Elements.UIWebUrl.DefaultColor; }
			set { UIHelpers.Elements.UIWebUrl.DefaultColor = value; }
		}

		[System.Obsolete( "use UIHelpers.UI.UIWebUrl.DefaultLitColor", true )]
		public static Color DefaultLitColor {
			get { return UIHelpers.Elements.UIWebUrl.DefaultLitColor; }
			set { UIHelpers.Elements.UIWebUrl.DefaultLitColor = value; }
		}

		[System.Obsolete( "use UIHelpers.UI.UIWebUrl.DefaultVisitColor", true )]
		public static Color DefaultVisitColor {
			get { return UIHelpers.Elements.UIWebUrl.DefaultVisitColor; }
			set { UIHelpers.Elements.UIWebUrl.DefaultVisitColor = value; }
		}



		////////////////

		private UIHelpers.Elements.UIWebUrl TrueElement;

		public UIText TextElem { get { return this.TrueElement.TextElem; } }
		public UIText LineElem { get { return this.TrueElement.LineElem; } }

		public string Url { get { return this.TrueElement.Url; } }
		public bool WillDrawOwnHoverUrl { get { return this.TrueElement.WillDrawOwnHoverUrl; } }


		////////////////

		public UIWebUrl( string label, string url, bool hover_url=true, float scale=0.85f, bool large=false ) : base() {
			this.TrueElement = new UIHelpers.Elements.UIWebUrl( label, url, hover_url, scale, large );
		}


		////////////////
		
		public void DrawHoverEffects( SpriteBatch sb ) {
			this.TrueElement.DrawHoverEffects( sb );
		}
	}
}
